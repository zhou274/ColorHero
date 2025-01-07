using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

using ColorHero;

namespace ColorHero
{
	public class TutorialManager : MonoBehaviour {

		public Button continueButton;
		public GameObject winPanel;
		public GameObject continueText;

		public GameObject textStep1;
		public GameObject textStep2;
		public GameObject borderStep1;
		public GameObject borderStep2;
		public GameObject rectStep1;
		public GameObject rectStep2;

		private int step = 0;

		void OnEnable()
		{
			step = 0;

			ResetAll ();

			ContinueTutorial ();
		}

		public void ContinueTutorial()
		{
			switch (step) {
			case 0:
				ResetAll ();
				textStep1.SetActive (true);
				step = 1;
				break;
			case 1:
				ResetAll ();
				continueButton.interactable = false;
				continueText.SetActive (false);
				textStep2.SetActive (true);
				step = 2;
				break;
			case 2:
				ResetAll ();
				winPanel.SetActive (true);
				step = 10;
				break;
			case 10:
				ResetAll ();
				borderStep1.SetActive (true);
				step = 11;
				break;
			case 11:
				ResetAll ();
				continueButton.interactable = false;
				continueText.SetActive (false);
				borderStep2.SetActive (true);
				step = 12;
				break;
			case 12:
				ResetAll ();
				winPanel.SetActive (true);
				step = 20;
				break;
			case 20:
				ResetAll ();
				rectStep1.SetActive (true);
				step = 21;
				break;
			case 21:
				ResetAll ();
				continueButton.interactable = false;
				continueText.SetActive (false);
				rectStep2.SetActive (true);
				step = 22;
				break;
			case 22:
				ResetAll ();
				winPanel.SetActive (true);
				step = 30;
				break;
			case 30:
				EndTutorial ();
				break;
			default:
				break;
			}
		}

		public void EndTutorial()
		{
			gameObject.SetActive (false);

			GameManager.instance.EndTutorial ();
		}

		private void ResetAll()
		{
			textStep1.SetActive (false);
			textStep2.SetActive (false);
			borderStep1.SetActive (false);
			borderStep2.SetActive (false);
			rectStep1.SetActive (false);
			rectStep2.SetActive (false);

			continueButton.interactable = true;
			continueText.SetActive (true);
			winPanel.SetActive (false);
		}

	}
}


