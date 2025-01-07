using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace ROFLPlay.DiffuseX
{
	public class PanelController : MonoBehaviour
	{
		public RectTransform panelRectTransform;

		public ResponseCode confirmCode;
		public ResponseCode cancelCode;

		protected Animator animator;
		protected ResponseCode responseCode;
		protected bool isLeaving;

		DiffuseResponse response;

		const float ANIMATION_DURATION = 0.667f;


		void Awake ()
		{
			animator = gameObject.GetComponent<Animator> ();
		}

		public virtual void Initialize (DiffuseResponse response)
		{
			this.response = response;

			gameObject.SetActive (true);

			Initialize ();
		}

		protected void Initialize()
		{
			isLeaving = false;
		}

		public void Confirm ()
		{
			if (isLeaving) {
				return;
			}

			responseCode = confirmCode;

			LeaveBegan ();
		}

		public void Cancel ()
		{
			if (isLeaving) {
				return;
			}

			responseCode = cancelCode;

			LeaveBegan ();
		}

		protected void LeaveBegan ()
		{
			isLeaving = true;

			animator.SetTrigger ("Leave");
			Invoke ("LeaveEnded", ANIMATION_DURATION);
		}

		protected virtual void LeaveEnded ()
		{
			gameObject.SetActive (false);

			if (response != null) {
				response.OnDiffuseResponse (responseCode);
			}
		}
	}

}
