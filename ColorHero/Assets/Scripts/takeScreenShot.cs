using UnityEngine;
using System.Collections;

public class takeScreenShot : MonoBehaviour
{
    public int width, height;

    // Use this for initialization
    void Start()
    {
        #if !UNITY_EDITOR
            this.enabled = false;
        #endif
                // DontDestroyOnLoad(gameObject);
        // InvokeRepeating("screenshot", 5, 5);
    }

    // string resolution;
    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.G))
        {
            screenshot();
        }
    }

    public void screenshot()
    {
        Screen.SetResolution(width, height, false);
        string resolution = "" + Screen.width + "X" + Screen.height;
        ScreenCapture.CaptureScreenshot("ScreenShot-" + resolution + "-" + PlayerPrefs.GetInt("number", 0) + ".png");
        PlayerPrefs.SetInt("number", PlayerPrefs.GetInt("number", 0) + 1);
        //Debug.Log ("takenShot with " + resolution);
    }

}
