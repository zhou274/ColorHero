using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.UI;

using ROFLPlay.Http;

namespace ROFLPlay.DiffuseX
{
	public class DiffuseManager : MonoBehaviour, WWWResponse, DiffuseResponse
	{

		public static DiffuseManager instance;

		public string androidMarketUrl = "";
		public string iOSMarketUrl = "";

		public string configFileName = "branch.json";
		public string localConfigFilePath = "ROFLPlay/DiffuseX/diffusex.json";

		public string moreGameUrl = "http://www.roflplay.com";

		public PanelController aboutUsPanel;
		public LikePanelController likePanel;
		public PanelController interstitialPanel;
		public Image interstitialImage;

		public DiffuseThumbManager thumbManager;

		[HideInInspector] public int nextDiffuseAppIndex;

		DiffuseData diffuseData;
		DiffuseApp currentDiffuseApp;
		DiffuseResponse diffuseResponse;

		WWWManager wwwManager;

		int diffuseCounter;
		int canShowAboutUs;
		int hasShowAboutUs;

		private const string HOST = "https://roflplay-ae54c.firebaseapp.com/";
		private const string ANDROID_TAG = "android";
		private const string IOS_TAG = "iOS";

		private const string ABOUT_US_URL = "http://www.facebook.com/ROFLPlay";

		private const int REQUEST_TYPE_JSON = 0;
		private const int REQUEST_TYPE_IMAGE = 1;

		private const string PLAYERPREFS_SHOWABOUTUS_KEY = "__diffusex__aboutus__";
		private const string PLAYERPREFS_RATE_KEY = "__diffusex__rate__";
		private const int PLAYERPREFS_VALUE_FALSE = 0;
		private const int PLAYERPREFS_VALUE_TRUE = 1;

		private const float ANIMATION_DURATION = 0.667f;


		void Awake ()
		{
			if (instance == null) {
				instance = this;
			} else if (instance != this) {
				Destroy (gameObject);
			}

			wwwManager = gameObject.GetComponent<WWWManager> ();

			canShowAboutUs = PlayerPrefs.GetInt (PLAYERPREFS_SHOWABOUTUS_KEY, PLAYERPREFS_VALUE_FALSE);
			PlayerPrefs.SetInt (PLAYERPREFS_SHOWABOUTUS_KEY, PLAYERPREFS_VALUE_TRUE);

			hasShowAboutUs = PLAYERPREFS_VALUE_FALSE;

			diffuseCounter = 0;

			nextDiffuseAppIndex = 0;

			DontDestroyOnLoad (gameObject);
		}

		void Start ()
		{
			WWWRequest request = new WWWRequest ();

			#if UNITY_IOS
			request.url = HOST + IOS_TAG + "/" + configFileName;
			#elif UNITY_ANDROID
			request.url = HOST + ANDROID_TAG + "/" + configFileName;
			#endif

//			Debug.Log ("config url is [" + request.url + "]");

			wwwManager.ClearQueue ();

			request.requestType = REQUEST_TYPE_JSON;
			request.requestTime = 1;
			request.response = this;
			wwwManager.AddQueue (request);

			wwwManager.StartQueue ();

			currentDiffuseApp = null;
		}

		public bool Diffuse (DiffuseResponse response, int odds = 5)
		{
			if (canShowAboutUs == PLAYERPREFS_VALUE_TRUE) {
				if (hasShowAboutUs == PLAYERPREFS_VALUE_FALSE) {
					hasShowAboutUs = PLAYERPREFS_VALUE_TRUE;
					ShowAboutUs (response);

					return true;
				}
			}

			int n = ++diffuseCounter % odds;

			if (n == 0) {
				int f = (int)(diffuseCounter / odds);

				if (f % odds == 2) {
					int like = PlayerPrefs.GetInt (PLAYERPREFS_RATE_KEY, PLAYERPREFS_VALUE_FALSE);

					if (like == PLAYERPREFS_VALUE_FALSE) {
						ShowLikeUs (response);
						return true;
					}
				}

				DiffuseApp app = FindAvailableDiffuseApp (ref nextDiffuseAppIndex);

				return ShowInterstitial (response, app);
			}

			return false;
		}

		public void ShowThumb (ThumbPosition position)
		{
			DiffuseApp app = FindAvailableDiffuseApp (ref nextDiffuseAppIndex);

			if (!DiffuseAppAvailable(app)) {
				Debug.Log ("[ShowThumb] can not find an available app.");
				return;
			}

			thumbManager.thumbPosition = position;
			thumbManager.diffuseApp = app;
			thumbManager.gameObject.SetActive (true);
		}

		public void HideThumb()
		{
			thumbManager.gameObject.SetActive (false);
		}

		public void OpenMarketPage()
		{
			#if !UNITY_EDITOR && UNITY_IOS
			OpenURL (iOSMarketUrl);
			#elif !UNITY_EDITOR && UNITY_ANDROID
			OpenURL (androidMarketUrl);
			#endif
		}

		public void MoreGame()
		{
			OpenURL (moreGameUrl);
		}

		public void OnDiffuseResponse (ResponseCode code)
		{
			switch (code) {
			case ResponseCode.ABOUTUS_CONFIRM:
				OpenURL (ABOUT_US_URL);
				break;
			case ResponseCode.ABOUTUS_CANCEL:
				break;
			case ResponseCode.LIKE_CONFIRM:
			case ResponseCode.LIKE_CANCEL:
				return;
			case ResponseCode.RATE_CONFIRM:
				PlayerPrefs.SetInt (PLAYERPREFS_RATE_KEY, PLAYERPREFS_VALUE_TRUE);
				OpenMarketPage ();
				break;
			case ResponseCode.RATE_CANCEL:
				break;
			case ResponseCode.ADVISE_CONFIRM:
				PlayerPrefs.SetInt (PLAYERPREFS_RATE_KEY, PLAYERPREFS_VALUE_TRUE);
				OpenMarketPage ();
				break;
			case ResponseCode.ADVISE_CANCEL:
				break;
			case ResponseCode.INTERSTITIAL_CONFIRM:
				{
					OpenURL (currentDiffuseApp.market_url);
				}
				break;
			case ResponseCode.INTERSTITIAL_CANCEL:
				break;
			default:
				break;
			}

			if (diffuseResponse != null) {
				diffuseResponse.OnDiffuseResponse (code);
			}
		}

		public void OnWWWResponse (WWWRequest request, UnityEngine.WWW www)
		{
			switch (request.requestType) {
			case REQUEST_TYPE_JSON:
				{
					if (www == null) {
						Debug.Log ("[DiffuseManager.OnResponse] WWW error, request url is [" + request.url + "]");

						LoadConfigFromHistory ();
						return;
					}

					string json = www.text;

					if (ParseJson (json)) {
						WriteConfigToHistory (json);
					} else {
						LoadConfigFromHistory ();
					}
				}
				break;
			case REQUEST_TYPE_IMAGE:
				{
					if (www != null) {
						string fileName = GetFileName (request.url);

						if (!fileName.Equals (string.Empty)) {
							byte[] bytes = www.bytes;

							WriteTextureToPersistent (fileName, bytes);
						}
					}
				}
				break;
			default:
				break;
			}
		}

		void ShowAboutUs (DiffuseResponse response)
		{
			diffuseResponse = response;

			aboutUsPanel.Initialize (this);
		}

		void ShowLikeUs (DiffuseResponse response)
		{
			diffuseResponse = response;

			likePanel.Initialize (this);
		}

		public bool ShowInterstitial (DiffuseResponse response, DiffuseApp diffuseApp)
		{
			if (!DiffuseAppAvailable (diffuseApp)) {
				Debug.Log ("[ShowInterstitial] diffuse app is not available.");
				return false;
			}

			diffuseResponse = response;

			currentDiffuseApp = diffuseApp;

			Sprite sprite = LoadSpriteFromPersistent (diffuseApp.image_url);

			interstitialImage.sprite = sprite;

			interstitialPanel.Initialize (this);

			return true;
		}

		public DiffuseApp FindAvailableDiffuseApp (ref int index)
		{
			if (diffuseData == null || diffuseData.apps == null) {
				return null;
			}

			for (int i = 0; i < diffuseData.apps.Length; i++) {
				if (index >= diffuseData.apps.Length) {
					index = 0;
				}

				DiffuseApp app = diffuseData.apps [index];
				if (DiffuseAppAvailable (app)) {
					index++;

					return app;
				}

				index++;
			}

			Debug.Log ("[FindAvailableDiffuseApp] none app is available.");

			return null;
		}

		bool DiffuseAppAvailable (DiffuseApp diffuseApp)
		{
			return diffuseApp != null && FileDownloaded (diffuseApp.icon_url) && FileDownloaded (diffuseApp.image_url);
		}

		public Sprite LoadSpriteFromPersistent (string path)
		{
			Texture2D texture = new Texture2D (2, 2);

			texture.LoadImage (LoadTextureFromPersistent (path));

			Sprite sprite = Sprite.Create (texture, new Rect (0, 0, texture.width, texture.height), new Vector2 (0.5f, 0.5f));

			return sprite;
		}
			
		void OpenURL (string url)
		{
			ROFLPlay.Bridge.GameServicesManager.instance.OpenURL (url);
		}


		void WriteConfigToHistory (string json)
		{
			string configPath = Application.persistentDataPath + "/" + configFileName;

			File.WriteAllText (configPath, json);
		}


		void LoadConfigFromHistory ()
		{
			string configPath = Application.persistentDataPath + "/" + configFileName;

			if (!File.Exists (configPath)) {
				LoadConfigFromLocalFile ();
				return;
			}

			string json = File.ReadAllText (configPath);

			if (!ParseJson (json)) {
				LoadConfigFromLocalFile ();
			}
		}

		void LoadConfigFromLocalFile ()
		{
			string configPath = Application.dataPath + "/" + localConfigFilePath;

			if (!File.Exists (configPath)) {
				return;
			}

			string json = File.ReadAllText (configPath);

			ParseJson (json);
		}


		void WriteTextureToPersistent (string fileName, byte[] bytes)
		{
			string filePath = Application.persistentDataPath + "/" + fileName;

			File.WriteAllBytes (filePath, bytes);
		}


		byte[] LoadTextureFromPersistent (string url)
		{
			string fileName = GetFileName (url);
			string filePath = Application.persistentDataPath + "/" + fileName;

			return File.ReadAllBytes (filePath);
		}


		bool ParseJson (string json)
		{
			if (json == null || json.Equals (string.Empty)) {
				return false;
			}

			DiffuseData data = JsonUtility.FromJson<DiffuseData> (json);

			if (data == null || data.apps == null) {
				Debug.Log ("[DiffuseManager.ParseJson] parse json string error, json body is [" + json + "]");

				return false;
			}

			wwwManager.ClearQueue ();

			for (int i = 0; i < data.apps.Length; i++) {
				DownloadFile (data.apps [i].icon_url);
				DownloadFile (data.apps [i].image_url);
			}

			wwwManager.StartQueue ();

			diffuseData = data;
			nextDiffuseAppIndex = 0;

			return true;
		}


		void DownloadFile (string url)
		{
			if (FileDownloaded (url)) {
				return;
			}

			WWWRequest request = new WWWRequest ();
			request.url = url;
			request.requestType = REQUEST_TYPE_IMAGE;
			request.requestTime = 5;
			request.response = this;

			wwwManager.AddQueue (request);
		}


		bool FileDownloaded (string url)
		{
			string fileName = GetFileName (url);

			if (fileName.Equals (string.Empty)) {
				Debug.Log ("[FileDownloaded] fileName is EMPTY, provided parameter url is [" + url + "]");
				return false;
			}

			string filePath = Application.persistentDataPath + "/" + fileName;

			return File.Exists (filePath);
		}


		string GetFileName (string url)
		{
			if (url == null) {
				return "";
			}

			int pos = url.LastIndexOf ("/");

			if (pos != -1 && pos < url.Length - 1) {
				return url.Substring (pos + 1);
			}

			return "";
		}
	}

}
