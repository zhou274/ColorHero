using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;


namespace ROFLPlay.Localization
{
	public class LocalizedText : MonoBehaviour
	{

		public string key;

		void Start ()
		{
			TextMeshProUGUI text = GetComponent<TextMeshProUGUI> ();

			if (text != null) {
				if (key != null && !key.Equals (string.Empty)) {
					text.text = LocalizationManager.instance.GetLocalizedValue (key);
				}
			}
		}

	}
}

