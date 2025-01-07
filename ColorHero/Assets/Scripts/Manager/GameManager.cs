using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using ROFLPlay.Localization;
using ROFLPlay.DiffuseX;

namespace ColorHero
{
	public class GameManager : MonoBehaviour
	{
		public static GameManager instance;

		public GameObject mainPanel;
		public GameObject settingPanel;
		public GameObject tutorialPanel;
		public GameObject stagePanel;

		private const string HAS_TUTORIAL_SHOWN_KEY = "ColorHero_Has_Tutorial_Shown";
		private const int TUTORIAL_HAS_NOT_SHOWN = 0;
		private const int TUTORIAL_HAS_SHOWN = 1;

		void Awake ()
		{
			if (instance == null) {
				instance = this;
			} else if (instance != this) {
				Destroy (gameObject);
			}
		}

		IEnumerator Start()
		{	
			while (!LocalizationManager.instance.IsReady()) {
				yield return null;
			}

			mainPanel.SetActive (true);

			DiffuseManager.instance.Diffuse (null);

			ROFLPlay.Bridge.AdsManager.instance.Initialize ();

			ROFLPlay.Bridge.GameServicesManager.instance.Initialize ();
		}
	
		public void StartGame()
		{
			if (!HasTutorialShown ()) {
				StartTutorial ();
			} else {
				StartStage (1);
			}
		}

		public void LeaveGame()
		{
			mainPanel.SetActive (true);
		}

		private bool HasTutorialShown()
		{
			int rest = PlayerPrefs.GetInt (HAS_TUTORIAL_SHOWN_KEY, TUTORIAL_HAS_NOT_SHOWN);

			return rest == TUTORIAL_HAS_SHOWN;
		}

		public void StartTutorial()
		{
			tutorialPanel.SetActive (true);
		}

		public void EndTutorial()
		{
			PlayerPrefs.SetInt (HAS_TUTORIAL_SHOWN_KEY, TUTORIAL_HAS_SHOWN);

			StartStage (1);
		}

		private void StartStage(int stage)
		{
			stagePanel.SetActive (true);
		}



		public void OpenSetting()
		{
			settingPanel.SetActive(true);
		}

	}
}
