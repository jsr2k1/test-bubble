// -----------------------------------------------
// Documentation: http://www.reign-studios.net/docs/unity-plugin/
// -----------------------------------------------

using UnityEngine;
using System.Collections;
using Reign;

public class MessageBoxDemo : MonoBehaviour
{
	void OnGUI()
	{
		// Simple OK message box
		if (GUI.Button(new Rect(0, 0, 256, 64), "Show OK MessageBox")) MessageBoxManager.Show("Yahoo", "Hello World!");

		// OK/Cancel message box
		if (GUI.Button(new Rect(Screen.width-256, 0, 256, 64), "Show OK/Cancel MessageBox")) MessageBoxManager.Show("Yahoo", "Are you Awesome!?", MessageBoxTypes.OkCancel, callback);
	}

	private void callback(MessageBoxResult result)
	{
		Debug.Log(result);
		if (result == MessageBoxResult.Ok) Debug.Log("+1 for you!");
		else if (result == MessageBoxResult.Cancel) Debug.Log("How sad...");
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
