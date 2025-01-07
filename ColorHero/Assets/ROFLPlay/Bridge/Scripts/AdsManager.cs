using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Runtime.InteropServices;


namespace ROFLPlay.Bridge
{
	
	public class AdsManager : MonoBehaviour
	{
		public static AdsManager instance;

		AdsResponse adsResponse;

		#if !UNITY_EDITOR && UNITY_IOS

		[DllImport ("__Internal")]    
		private static extern bool initAds();

		[DllImport ("__Internal")]    
		private static extern bool isInterstitialAdReady();

		[DllImport ("__Internal")]    
		private static extern void showInterstitialAd();

		[DllImport ("__Internal")]    
		private static extern bool isRewardedVideoAdReady();

		[DllImport ("__Internal")]    
		private static extern void showRewardedVideoAd();

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
			initAds();
			#elif !UNITY_EDITOR && UNITY_ANDROID
			AndroidJavaClass jc = new AndroidJavaClass("com.unity3d.player.UnityPlayer");
			if(jc != null) {
			AndroidJavaObject jo = jc.GetStatic<AndroidJavaObject>("currentActivity");
			if(jo != null) {
			jo.Call("initAds");
			}
			}
			#endif
		}

		public bool ShowInterstitialAd (AdsResponse response)
		{
			#if !UNITY_EDITOR && UNITY_IOS
			if(isInterstitialAdReady()) {
			adsResponse = response;
			showInterstitialAd();
			return true;
			}
			#elif !UNITY_EDITOR && UNITY_ANDROID
			AndroidJavaClass jc = new AndroidJavaClass("com.unity3d.player.UnityPlayer");
			if(jc != null) {
			AndroidJavaObject jo = jc.GetStatic<AndroidJavaObject>("currentActivity");
			if(jo != null && jo.Call<bool>("isInterstitialAdReady")) {
			adsResponse = response;
			jo.Call("showInterstitialAd");
			return true;
			}
			}
			#endif	

			return false;
		}

		public bool ShowRewardedVideoAd (AdsResponse response)
		{
			#if !UNITY_EDITOR && UNITY_IOS
			if(isRewardedVideoAdReady()) {
			adsResponse = response;
			showRewardedVideoAd();
			return true;
			}
			#elif !UNITY_EDITOR && UNITY_ANDROID
			AndroidJavaClass jc = new AndroidJavaClass("com.unity3d.player.UnityPlayer");
			AndroidJavaObject jo = jc.GetStatic<AndroidJavaObject>("currentActivity");
			if(jo.Call<bool>("isRewardedVideoAdReady")) {
			adsResponse = response;
			jo.Call("showRewardedVideoAd");
			return true;
			}
			#endif	

			return false;
		}

		public void OnInterstitialAdDismissed (string message)
		{
			if (adsResponse != null) {
				adsResponse.OnInterstitialAdDismissed ();
			}
		}

		public void OnRewardedVideoAdDidRewardUser (string message)
		{
			if (adsResponse != null) {
				adsResponse.OnRewardedVideoAdDidRewardUser ();
			}
		}

		public void OnRewardedVideoAdDismissed (string message)
		{
			if (adsResponse != null) {
				adsResponse.OnRewardedVideoAdDismissed ();
			}
		}

	}
}


