using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

using ColorHero;

namespace ColorHero
{
	public class AudioUIController : MonoBehaviour
	{

		public AudioManager audioManager;

		public Slider musicSlider;

		public Slider sfxSlider;


		void OnEnable ()
		{
			if (musicSlider != null) {
				musicSlider.value = audioManager.musicValue;
			}

			if (sfxSlider != null) {
				sfxSlider.value = audioManager.sfxValue;
			}
		}
	}

}

