using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

using ROFLPlay.DiffuseX;

public class SampleScript : MonoBehaviour, DiffuseResponse
{
	public Text text;

	int counter;

	void Awake()
	{
		counter = 0;
	}

	void Start()
	{
		DiffuseManager.instance.Diffuse (this);
	}

	public void Diffuse ()
	{
		bool ret = DiffuseManager.instance.Diffuse (this);

		string msg = "[SampleButtonScript.Diffuse] function called, counter is " + counter + ", result is " + ret;

		Debug.Log (msg);

		text.text = msg;

		counter++;
	}

	public void ShowThumb ()
	{
		DiffuseManager.instance.ShowThumb (ThumbPosition.BOTTOM_RIGHT);
	}

	public void HideThumb ()
	{
		DiffuseManager.instance.HideThumb ();
	}

	public void OnDiffuseResponse (ResponseCode code)
	{
		Debug.Log ("[SampleButtonScript] diffuse response code is " + code);
	}
}
