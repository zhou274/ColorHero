using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

using ColorHero;
using TMPro;

namespace ColorHero {
	
	public class TextButton : MonoBehaviour {

		StageManager.CellType cellType;

		TextMeshProUGUI buttonLabel;

		Animator animator;

		CanvasGroup canvasGroup;

		bool isRight, isAnimating;

		bool interactable;

		const float RIGHT_ANIMATION_DURATION = 0.667f;
		const float WRONG_ANIMATION_DURATION = 1.0f;


		protected virtual void Awake()
		{
			animator = GetComponent<Animator> ();
			buttonLabel = GetComponentInChildren<TextMeshProUGUI> ();
			canvasGroup = GetComponent<CanvasGroup> ();

			interactable = false;
			canvasGroup.alpha = 0f;
		}

		public void Initialize(StageManager.CellType type, string text, Color color, bool right)
		{
			cellType = type;

			if (buttonLabel != null) {
				//buttonLabel.font = ROFLPlay.Localization.LocalizationManager.instance.GetLocalizedFont ();
				buttonLabel.text = text;
				buttonLabel.color = color;
			}

			isRight = right;
		}

		public void SetVisible()
		{
			interactable = true;
			canvasGroup.alpha = 1f;
		}

		public void Submit()
		{
			if (!StageManager.instance.interactable || !interactable) {
				return;
			}

			interactable = false;

			if (animator != null) {
				isAnimating = true;
				if (isRight) {
					animator.SetTrigger ("Right");
					Invoke ("RightAnimEnded", RIGHT_ANIMATION_DURATION);
				} else {
					animator.SetTrigger ("Wrong");
					Invoke ("WrongAnimEnded", WRONG_ANIMATION_DURATION);
				}
			}

			StageManager.instance.SubmitCell (cellType, isRight);
		}

		public bool IsAnimating()
		{
			return isAnimating;
		}

		void WrongAnimEnded()
		{
			isAnimating = false;

			interactable = true;
		}

		void RightAnimEnded()
		{
			isAnimating = false;

			canvasGroup.alpha = 0f;
		}
	}

}

