using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using ColorHero;

namespace ColorHero
{

	public class MainPanel : MonoBehaviour
	{
		Animator animator;

		private bool isLeaving;
		private int enterTimes;

		private const float leaveAnimDuration = 0.667f;

		void Awake ()
		{
			animator = GetComponent<Animator> ();
			enterTimes = 0;
		}

		void OnEnable()
		{
			isLeaving = false;
			enterTimes++;

//			if (enterTimes > 1) {
//				ROFLPlay.DiffuseX.DiffuseManager.instance.ShowThumb (ROFLPlay.DiffuseX.ThumbPosition.CENTER_LEFT);
//			}
		}

		public void OnTapToStart()
		{
			if (isLeaving) {
				return;
			}

			Leave ();

			Invoke ("StartGame", leaveAnimDuration);
		}

		public void OnSettingSelected()
		{
			if (isLeaving) {
				return;
			}

			Leave ();

			Invoke ("OpenSetting", leaveAnimDuration);
		}

		public void OnLeaderboardSelected()
		{
			ROFLPlay.Bridge.GameServicesManager.instance.ShowLeaderboard ();
		}

		public void OnLikeSelected()
		{
			ROFLPlay.DiffuseX.DiffuseManager.instance.OpenMarketPage ();
		}

		public void OnMoreGameSelected()
		{
			ROFLPlay.DiffuseX.DiffuseManager.instance.MoreGame ();
		}

		private void Leave ()
		{
			isLeaving = true;

			ROFLPlay.DiffuseX.DiffuseManager.instance.HideThumb ();

			if (animator != null) {
				animator.SetTrigger ("Leave");
			}
		}

		private void StartGame ()
		{
			gameObject.SetActive (false);

			GameManager.instance.StartGame ();
		}

		private void OpenSetting ()
		{
			gameObject.SetActive (false);

			GameManager.instance.OpenSetting ();
		}
	}
}