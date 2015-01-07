using UnityEngine;
using System.Collections;

public class script_FPS : MonoBehaviour
{
	public float updateInterval = 1.0f;

	float accum = 0; // FPS accumulated over the interval
	int frames = 0; // Frames drawn over the interval
	float timeleft; // Left time for current interval
	string sFPS, sMinFPS, sMaxFPS;
	float minFPS=99999999999;
	float maxFPS=0;
	
	////////////////////////////////////////////////////////////////////////////////////////////////////////////

	void Start()
	{
		timeleft = updateInterval;
	}
	
	////////////////////////////////////////////////////////////////////////////////////////////////////////////

	void Update()
	{
		timeleft -= Time.deltaTime;
		accum += Time.timeScale / Time.deltaTime;
		++frames;

		if (timeleft <= 0.0)
		{
			float fps = accum / frames;
			sFPS = System.String.Format("{0:F2} FPS", fps);

			minFPS = Mathf.Min(minFPS, fps);
			maxFPS = Mathf.Max(maxFPS, fps);
			
			sMinFPS = System.String.Format("{0:F2} FPS", minFPS);
			sMaxFPS = System.String.Format("{0:F2} FPS", maxFPS);

			timeleft = updateInterval;
			accum = 0.0F;
			frames = 0;
		}
		//reset all values
		if(Input.GetKeyDown(KeyCode.R)){
			minFPS=999999;
			maxFPS=0;
		}
	}
	
	////////////////////////////////////////////////////////////////////////////////////////////////////////////
	
	void OnGUI()
	{
		GUI.Label(new Rect(20,20,200,50), "FPS: "+sFPS);
		GUI.Label(new Rect(20,40,200,50), "Min: "+sMinFPS);
		GUI.Label(new Rect(20,60,200,50), "Max: "+sMaxFPS);
	}
}

