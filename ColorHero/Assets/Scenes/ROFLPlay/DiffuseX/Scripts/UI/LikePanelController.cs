using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

using ROFLPlay.Localization;

namespace ROFLPlay.DiffuseX
{
	public class LikePanelController : PanelController
	{
		public enum LikeStep
		{
			Like,
			Advise,
			Rate
		}

		LikeStep step;

		public Text messageText;
		public Text confirmButtonText;
		public Text cancelButtonText;

		private const string LIKE_TEXT_KEY = "LikeText";
		private const string LIKE_CONFIRM_KEY = "LikeConfirm";
		private const string LIKE_CANCEL_KEY = "LikeCancel";
		private const string ADVISE_TEXT_KEY = "AdviseText";
		private const string ADVISE_CONFIRM_KEY = "AdviseConfirm";
		private const string ADVISE_CANCEL_KEY = "AdviseCancel";
		private const string RATE_TEXT_KEY = "RateText";
		private const string RATE_CONFIRM_KEY = "RateConfirm";
		private const string RATE_CANCEL_KEY = "RateCancel";



		public override void Initialize (DiffuseResponse response)
		{
			base.Initialize (response);

			UpdateStep (LikeStep.Like);
		}

		protected override void LeaveEnded ()
		{
			if (step == LikeStep.Like) {
				if (responseCode == ResponseCode.LIKE_CONFIRM) {
					UpdateStep (LikeStep.Rate);
				} else if (responseCode == ResponseCode.LIKE_CANCEL) {
					UpdateStep (LikeStep.Advise);
				} else {
					Debug.Log ("LikePanelController.LeaveEnded() Error ResponseCode in step Like");
					return;
				}

				animator.SetTrigger ("Enter");
				Initialize ();
			} else {
				base.LeaveEnded ();
			}
		}

		private void UpdateStep(LikeStep step)
		{
			this.step = step;

			if (step == LikeStep.Like) {
				confirmCode = ResponseCode.LIKE_CONFIRM;
				cancelCode = ResponseCode.LIKE_CANCEL;

				messageText.text = LocalizationManager.instance.GetLocalizedValue (LIKE_TEXT_KEY);
				confirmButtonText.text = LocalizationManager.instance.GetLocalizedValue (LIKE_CONFIRM_KEY);
				cancelButtonText.text = LocalizationManager.instance.GetLocalizedValue (LIKE_CANCEL_KEY);
			} else if (step == LikeStep.Advise) {
				confirmCode = ResponseCode.ADVISE_CONFIRM;
				cancelCode = ResponseCode.ADVISE_CANCEL;

				messageText.text = LocalizationManager.instance.GetLocalizedValue (ADVISE_TEXT_KEY);
				confirmButtonText.text = LocalizationManager.instance.GetLocalizedValue (ADVISE_CONFIRM_KEY);
				cancelButtonText.text = LocalizationManager.instance.GetLocalizedValue (ADVISE_CANCEL_KEY);
			} else {
				confirmCode = ResponseCode.RATE_CONFIRM;
				cancelCode = ResponseCode.RATE_CANCEL;

				messageText.text = LocalizationManager.instance.GetLocalizedValue (RATE_TEXT_KEY);
				confirmButtonText.text = LocalizationManager.instance.GetLocalizedValue (RATE_CONFIRM_KEY);
				cancelButtonText.text = LocalizationManager.instance.GetLocalizedValue (RATE_CANCEL_KEY);
			}
		}
	}

}

