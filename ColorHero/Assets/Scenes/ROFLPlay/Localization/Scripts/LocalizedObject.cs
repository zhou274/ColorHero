using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


namespace ROFLPlay.Localization
{
	public class LocalizedObject : MonoBehaviour
	{

		public GameObject enObject;
		public GameObject cnObject;

		// Use this for initialization
		void Start ()
		{
			if (LocalizationManager.instance.IsChinese()) {
				cnObject.SetActive (true);
				enObject.SetActive (false);
			} else {
				cnObject.SetActive (false);
				enObject.SetActive (true);
			}
		}
	}
}

