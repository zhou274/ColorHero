using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

using ColorHero;

namespace ColorHero {
	
	public class HorizontalSpaceControl : MonoBehaviour {

		// Use this for initialization
		void Start () {
			HorizontalLayoutGroup horizontalLayout = GetComponent<HorizontalLayoutGroup> ();

			if (horizontalLayout != null) {
				horizontalLayout.spacing = Screen.width / 20;
			}
		}
	}

}

