using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

using ColorHero;

namespace ColorHero
{

	public class AdjustGrid : MonoBehaviour
	{

		private GridLayoutGroup grid;

		void Awake ()
		{
			grid = GetComponent<GridLayoutGroup> ();
		}

		// Use this for initialization
		void Start ()
		{
			int buttonWidth = Screen.width / 3;
			grid.cellSize = new Vector2 (buttonWidth, buttonWidth / 3);
		}

	}

}
