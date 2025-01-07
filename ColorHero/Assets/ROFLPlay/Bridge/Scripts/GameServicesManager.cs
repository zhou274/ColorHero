using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Runtime.InteropServices;


namespace ROFLPlay.Bridge
{
	
	public class GameServicesManager : MonoBehaviour
	{
		public static GameServicesManager instance;

		public string iOSLeaderboardID;
		public string androidLeaderboardID;

		#if !UNITY_EDITOR && UNITY_IOS

		[DllImport ("__Internal")] 
		private static extern void initGameServices();

		[DllImport ("__Internal")] 
		private static extern void openURL(string url);

		[DllImport ("__Internal")]
		private static extern void showLeaderboard(string url);

		[DllImport ("__Internal")]    
		private static extern void submitScore(int score, string url);

		#endif

		void Awake ()
		{
			if (instance == null) {
				instance = this;
			} else if (instance != this) {
				Destroy (gameObject);
			}
		}

		public void Initialize()
		{
			#if !UNITY_EDITOR && UNITY_IOS
			initGameServices();
			#elif !UNITY_EDITOR && UNITY_ANDROID
			AndroidJavaClass jc = new AndroidJavaClass("com.unity3d.player.UnityPlayer");
			if(jc != null) {
			AndroidJavaObject jo = jc.GetStatic<AndroidJavaObject>("currentActivity");
			if(jo != null) {
			jo.Call("initGameServices");
			}
			}
			#endif
		}

		public void OpenURL(string url)
		{
			#if !UNITY_EDITOR && UNITY_IOS
			openURL(url);
			#elif !UNITY_EDITOR && UNITY_ANDROID
			AndroidJavaClass jc = new AndroidJavaClass("com.unity3d.player.UnityPlayer");
			if(jc != null) {
			AndroidJavaObject jo = jc.GetStatic<AndroidJavaObject>("currentActivity");
			if(jo != null) {
			jo.Call("openURL", url);
			}
			}
			#endif
		}

		public void ShowLeaderboard()
		{
			#if !UNITY_EDITOR && UNITY_IOS
			showLeaderboard(iOSLeaderboardID);
			#elif !UNITY_EDITOR && UNITY_ANDROID
			AndroidJavaClass jc = new AndroidJavaClass("com.unity3d.player.UnityPlayer");
			if(jc != null) {
			AndroidJavaObject jo = jc.GetStatic<AndroidJavaObject>("currentActivity");
			if(jo != null) {
			jo.Call("showLeaderboard", androidLeaderboardID);
			}
			}
			#endif
		}

		public void SubmitScore(int score)
		{
			#if !UNITY_EDITOR && UNITY_IOS
			submitScore(score, iOSLeaderboardID);
			#elif !UNITY_EDITOR && UNITY_ANDROID
			AndroidJavaClass jc = new AndroidJavaClass("com.unity3d.player.UnityPlayer");
			if(jc != null) {
			AndroidJavaObject jo = jc.GetStatic<AndroidJavaObject>("currentActivity");
			if(jo != null) {
			jo.Call("submitScore", score, androidLeaderboardID);
			}
			}
			#endif
		}

	}
}


