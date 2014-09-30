// -----------------------------------------------
// Documentation: http://www.reign-studios.net/docs/unity-plugin/
// -----------------------------------------------

using UnityEngine;
using System.Collections;
using Reign;

public class SystemEventsDemo : MonoBehaviour
{
	void Start ()
	{
		Debug.Log("NOTE: Run on Win8 device to test!");

		// Handle Win8 Snapping
		SystemEvents.Win8Event += SystemEvents_Win8Event;
	}
	
	void SystemEvents_Win8Event(Win8EventTypes type, object data)
	{
		Debug.Log(type);
		if (type == Win8EventTypes.SizeChanged)
		{
			var values = (int[])data;
			Debug.Log(string.Format("Width: {0} Height: {1}", values[0], values[1]));
		}
	}

	void Update()
	{
		// NOTE: If you are getting unity activity pause timeout issues on Android, call "ApplicationEx.Quit();"
		// There seems to be what may be a memeory leak in Unity4.3+
		// Until this is fixed I recomend trying to calling this quit method on Android.
		// (It will save your player prefs and use "System.exit(0)" instead of "finish()" on Android)
		// If you have a better work-around, email support, Thanks.
		if (Input.GetKeyUp(KeyCode.Escape)) ApplicationEx.Quit();// NOTE: Unity 4.5 does not need this
	}
}
