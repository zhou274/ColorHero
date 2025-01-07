using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Audio;

using ColorHero;

namespace ColorHero
{
	public class AudioManager : MonoBehaviour
	{

		public AudioMixer audioMixer;
		public string musicMixerParameter;
		public string sfxMixerParameter;

		public string musicPrefsKey;
		[HideInInspector] public float musicValue;
		public float musicDefaultValue;

		public string sfxPrefsKey;
		[HideInInspector] public float sfxValue;
		public float sfxDefaultValue;



		void Start ()
		{
			RestorePrefs ();
		}

		private void RestorePrefs ()
		{
			musicValue = PlayerPrefs.GetFloat (musicPrefsKey, musicDefaultValue);
			audioMixer.SetFloat (musicMixerParameter, musicValue);

			sfxValue = PlayerPrefs.GetFloat (sfxPrefsKey, sfxDefaultValue);
			audioMixer.SetFloat (sfxMixerParameter, sfxValue);
		}

		public void UpdateMusic (float value)
		{
			musicValue = value;
			audioMixer.SetFloat (musicMixerParameter, musicValue);

			PlayerPrefs.SetFloat (musicPrefsKey, musicValue);
			PlayerPrefs.Save ();
		}

		public void UpdateSFX (float value)
		{
			sfxValue = value;
			audioMixer.SetFloat (sfxMixerParameter, sfxValue);

			PlayerPrefs.SetFloat (sfxPrefsKey, sfxValue);
			PlayerPrefs.Save ();
		}
	}

}

