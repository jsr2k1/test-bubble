using UnityEngine;
using System.Collections;

public class script_FPS : MonoBehaviour
{
	public float updateInterval = 1.0f;

	float accum = 0; // FPS accumulated over the interval
	int frames = 0; // Frames drawn over the interval
	float timeleft; // Left time for current interval
	string sFPS;
	
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

			timeleft = updateInterval;
			accum = 0.0F;
			frames = 0;
		}
	}
	
	////////////////////////////////////////////////////////////////////////////////////////////////////////////
	
	void OnGUI()
	{
		GUI.Label(new Rect(20,20,200,50), sFPS);
	}
}