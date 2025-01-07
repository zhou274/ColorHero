using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

using ColorHero;

namespace ColorHero
{
	public class SFXPlayer : MonoBehaviour
	{

		public AudioClip buttonAudioClip;
		public float rightPitchStart;
		public float rightPitchStep;
		public float wrongPitch;

		public AudioClip winAudioClip;

		public AudioClip loseAudioClip;

		AudioSource audioSource;

		void Awake ()
		{
			audioSource = GetComponent<AudioSource> ();
		}

		// Use this for initialization
		public void Play ()
		{
			audioSource.clip = buttonAudioClip;
			audioSource.pitch = 1f;
			audioSource.Play ();
		}

		public void PlayRightWithStep (int step)
		{
			audioSource.clip = buttonAudioClip;
			audioSource.pitch = rightPitchStart + step * rightPitchStep;
			audioSource.Play ();
		}

		public void PlayWrong ()
		{
			audioSource.clip = buttonAudioClip;
			audioSource.pitch = wrongPitch;
			audioSource.Play ();
		}

		public void PlayWin ()
		{
			audioSource.clip = winAudioClip;
			audioSource.pitch = 1f;
			audioSource.Play ();
		}

		public void PlayLose ()
		{
			audioSource.clip = loseAudioClip;
			audioSource.pitch = 1f;
			audioSource.Play ();
		}
	}
}


