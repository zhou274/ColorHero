namespace ROFLPlay.Bridge
{
	public interface AdsResponse
	{

		void OnInterstitialAdDismissed ();

		void OnRewardedVideoAdDidRewardUser ();

		void OnRewardedVideoAdDismissed ();

	}
}
