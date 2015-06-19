using UnityEngine;
using System;
using System.Collections;

class ScreenShotMovie : MonoBehaviour
{
	public string folder = "VIDEO";
	public int frameRate = 24;
	public bool bRec = false;
	string dir;
	
	//////////////////////////////////////////////////////////////////////////////////////////////////////////////////

	void Awake()
	{
		DontDestroyOnLoad(transform.gameObject);
	}

	////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

	void Start ()
	{
	    //Time.captureFramerate = frameRate;
		dir ="C:\\"+folder+"\\";
	}
	
	//////////////////////////////////////////////////////////////////////////////////////////////////////////////////
	
	void Update ()
	{
		if(Input.GetKeyDown(KeyCode.K)){
			bRec=!bRec;
		}
		if(bRec){
			Time.captureFramerate = frameRate;
		    string name = String.Format("{0}/{1:D04} shot.png", dir, Time.frameCount );
		    Application.CaptureScreenshot(name);
		}
	}
}
	