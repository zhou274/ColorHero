using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace ROFLPlay.DiffuseX
{
	public class AnchorAdapter : MonoBehaviour
	{
		
		public Vector2 portraitAnchorMin = new Vector2 (0.05f, 0.3f);
		public Vector2 portraitAnchorMax = new Vector2 (0.95f, 0.75f);
		public Vector2 landscapeAnchorMin = new Vector2 (0.2f, 0.05f);
		public Vector2 landscapeAnchorMax = new Vector2 (0.8f, 0.95f);

		void Awake ()
		{
			RectTransform rectTransform = GetComponent<RectTransform> ();

			if (rectTransform != null) {
				if (Screen.width > Screen.height) {
					rectTransform.anchorMin = landscapeAnchorMin;
					rectTransform.anchorMax = landscapeAnchorMax;
				} else {
					rectTransform.anchorMin = portraitAnchorMin;
					rectTransform.anchorMax = portraitAnchorMax;
				}
			}
		}
	
	}
}