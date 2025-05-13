using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


namespace ROFLPlay.DiffuseX
{
	public class DiffuseThumbManager : MonoBehaviour, DiffuseResponse {

		public class ThumbAnchors
		{
			public float min;
			public float max;

			public ThumbAnchors (float min, float max)
			{
				this.min = min;
				this.max = max;
			}
		}

		private ThumbAnchors START = new ThumbAnchors (0.02f, 0.22f);
		private ThumbAnchors CENTER = new ThumbAnchors (0.4f, 0.6f);
		private ThumbAnchors END = new ThumbAnchors (0.78f, 0.98f);

		[HideInInspector] public ThumbPosition thumbPosition;
		[HideInInspector] public DiffuseApp diffuseApp;

		Image image;
		RectTransform rectTransform;
		Vector3 startPosition, endPosition, destinationPosition;

		Vector3 moveVelocity;

		bool animating;

		const float OFFSET = 30f;
		const float DAMP_TIME = 0.4f;

		void Awake()
		{
			image = GetComponent<Image> ();
			rectTransform = GetComponent<RectTransform> ();
		}

		// Use this for initialization
		void OnEnable () {

			switch (thumbPosition) {
			case ThumbPosition.BOTTOM_LEFT:
				{
					rectTransform.anchorMin = new Vector2(START.min, START.min);
					rectTransform.anchorMax = new Vector2(START.max, START.max);
				}
				break;
			case ThumbPosition.BOTTOM_CENTER:
				{
					rectTransform.anchorMin = new Vector2(CENTER.min, START.min);
					rectTransform.anchorMax = new Vector2(CENTER.max, START.max);
				}
				break;
			case ThumbPosition.BOTTOM_RIGHT:
				{
					rectTransform.anchorMin = new Vector2(END.min, START.min);
					rectTransform.anchorMax = new Vector2(END.max, START.max);
				}
				break;
			case ThumbPosition.CENTER_LEFT:
				{
					rectTransform.anchorMin = new Vector2(START.min, CENTER.min);
					rectTransform.anchorMax = new Vector2(START.max, CENTER.max);
				}
				break;
			case ThumbPosition.CENTER:
				{
					rectTransform.anchorMin = new Vector2(CENTER.min, CENTER.min);
					rectTransform.anchorMax = new Vector2(CENTER.max, CENTER.max);
				}
				break;
			case ThumbPosition.CENTER_RIGHT:
				{
					rectTransform.anchorMin = new Vector2(END.min, CENTER.min);
					rectTransform.anchorMax = new Vector2(END.max, CENTER.max);
				}
				break;
			case ThumbPosition.TOP_LEFT:
				{
					rectTransform.anchorMin = new Vector2(START.min, END.min);
					rectTransform.anchorMax = new Vector2(START.max, END.max);
				}
				break;
			case ThumbPosition.TOP_CENTER:
				{
					rectTransform.anchorMin = new Vector2(CENTER.min, END.min);
					rectTransform.anchorMax = new Vector2(CENTER.max, END.max);
				}
				break;
			case ThumbPosition.TOP_RIGHT:
				{
					rectTransform.anchorMin = new Vector2(END.min, END.min);
					rectTransform.anchorMax = new Vector2(END.max, END.max);
				}
				break;
			}

			SetSpriteWithDiffuseApp (diffuseApp);

			rectTransform.anchoredPosition = Vector3.zero;
			startPosition = rectTransform.position;

			if (rectTransform.anchorMax.y >= 0.5f) {
				endPosition = startPosition + new Vector3 (0f, -OFFSET);
			} else {
				endPosition = startPosition + new Vector3 (0f, OFFSET);
			}

			destinationPosition = endPosition;

			animating = true;
		}

		// Update is called once per frame
		void Update () {
			if (!animating) {
				return;
			}

			if (Vector3.Distance (rectTransform.position, endPosition) <= 0.1f) {
				destinationPosition = startPosition;
			} else if(Vector3.Distance (rectTransform.position, startPosition) <= 0.1f) {
				destinationPosition = endPosition;
			}

			rectTransform.position = Vector3.SmoothDamp (rectTransform.position, destinationPosition, ref moveVelocity, DAMP_TIME);
		}

		public void SetSpriteWithDiffuseApp(DiffuseApp app)
		{
			if (app != null) {
				image.sprite = DiffuseManager.instance.LoadSpriteFromPersistent (app.icon_url);
			} else {
				gameObject.SetActive (false);
			}
		}

		public void ThumbClicked()
		{
			animating = false;

			DiffuseManager.instance.ShowInterstitial (this, diffuseApp);
		}

		public void OnDiffuseResponse(ROFLPlay.DiffuseX.ResponseCode code)
		{
			animating = true;

			diffuseApp = DiffuseManager.instance.FindAvailableDiffuseApp (ref DiffuseManager.instance.nextDiffuseAppIndex);

			SetSpriteWithDiffuseApp (diffuseApp);
		}
	}
}


