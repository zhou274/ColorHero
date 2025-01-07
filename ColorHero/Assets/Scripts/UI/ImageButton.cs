using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

using ColorHero;

namespace ColorHero {
	
	public class ImageButton : TextButton {

		Image image;

		// Use this for initialization
		protected override void Awake () {
			base.Awake ();

			image = GetComponent<Image> ();
		}

		// Update is called once per frame
		public void Initialize (StageManager.CellType type, Color imageColor, string text, Color textColor, bool right) {
			base.Initialize (type, text, textColor, right);

			if (image != null) {
				image.color = imageColor;
			}
		}
	}
}


