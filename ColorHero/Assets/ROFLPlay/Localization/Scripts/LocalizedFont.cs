using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


namespace ROFLPlay.Localization
{
	public class LocalizedFont : MonoBehaviour
	{

		// Use this for initialization
		void Start ()
		{
			Font localizedFont = LocalizationManager.instance.enFont;

			if (LocalizationManager.instance.IsChinese()) {
				localizedFont = LocalizationManager.instance.cnFont;
			}

			Text[] texts = gameObject.GetComponentsInChildren<Text> (true);
			for (int i = 0; i < texts.Length; i++) {
				texts [i].font = localizedFont;
			}
		}
	}
}

