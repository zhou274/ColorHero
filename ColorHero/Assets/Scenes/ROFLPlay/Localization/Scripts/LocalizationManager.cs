using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.IO;

namespace ROFLPlay.Localization
{
	
	public class LocalizationManager : MonoBehaviour
	{
		public static LocalizationManager instance;

		public bool forceChinese;

		public Font cnFont;
		public Font enFont;

		private Dictionary<string, string> localizedText;
		private bool isReady = false;

		private const string missingTextString = "";
		private const string localizationChineseSimplified = "localization_zhCN.json";
		private const string localizationOthers = "localization_enUS.json";


		// Use this for initialization
		void Awake ()
		{
			if (instance == null) {
				instance = this;
			} else if (instance != this) {
				Destroy (gameObject);
			}

			// if (IsChinese()) {
				StartCoroutine (LoadLocalizedText (localizationChineseSimplified));
			// } else {
			// 	StartCoroutine (LoadLocalizedText (localizationOthers));
			// }

			DontDestroyOnLoad (gameObject);
		}

		public bool IsChinese()
		{
			if (forceChinese || Application.systemLanguage == SystemLanguage.ChineseSimplified) {
				return true;
			} else {
				return false;
			}
		}

		public IEnumerator LoadLocalizedText (string fileName)
		{
			localizedText = new Dictionary<string, string> ();
			string filePath = Path.Combine (Application.streamingAssetsPath, fileName);
			string fileData = string.Empty;

			if (filePath.Contains ("://")) {
				WWW www = new WWW (filePath);
				yield return www;
				fileData = www.text;
			} else if (File.Exists (filePath)) {
				fileData = File.ReadAllText (filePath);
			} else {
				Debug.LogError ("Cannot find file!");
			}

			if (!fileData.Equals (string.Empty)) {
				LocalizationData loadedData = JsonUtility.FromJson<LocalizationData> (fileData);

				for (int i = 0; i < loadedData.items.Length; i++) {
					localizedText.Add (loadedData.items [i].key, loadedData.items [i].value);   
				}
				Debug.Log ("Data loaded, dictionary contains: " + localizedText.Count + " entries");

				isReady = true;
			}
		}

		public Font GetLocalizedFont()
		{
			if (IsChinese ()) {
				return cnFont;
			} else {
				return enFont;
			}
		}

		public string GetLocalizedValue (string key)
		{
			string result = missingTextString;
			if (localizedText.ContainsKey (key)) {
				result = localizedText [key];
			}

			return result;
		}

		public bool IsReady ()
		{
			return isReady;
		}

	}
}

