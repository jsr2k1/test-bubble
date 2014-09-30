// -----------------------------------------------
// Documentation: http://www.reign-studios.net/docs/unity-plugin/
// -----------------------------------------------

using UnityEngine;
using System.Collections;
using Reign;

public class InterstitialAdDemo : MonoBehaviour
{
	private static InterstitialAd ad;

	void Start()
	{
		DontDestroyOnLoad(gameObject);// Make sure the start method never gets called more then once.

		var desc = new InterstitialAdDesc();

		// Global
		desc.Testing = true;
		desc.EventCallback = eventCallback;

		// WP8
		desc.WP8_AdAPI = InterstitialAdAPIs.AdMob;
		desc.WP8_AdMob_UnitID = "ca-app-pub-5295031539813538/9735508408";// NOTE: Must set event for testing
			
		// iOS
		desc.iOS_AdAPI = InterstitialAdAPIs.AdMob;
		desc.iOS_AdMob_UnitID = "ca-app-pub-5295031539813538/9735508408";// NOTE: Must set event for testing
			
		// Android
		desc.Android_AdAPI = InterstitialAdAPIs.AdMob;
		desc.Android_AdMob_UnitID = "ca-app-pub-5295031539813538/9735508408";// NOTE: Must set event for testing

		// create ad
		ad = InterstitialAdManager.CreateAd(desc, createdCallback);
	}

	private void createdCallback(bool success)
	{
		Debug.Log(success);
		if (!success) Debug.LogError("Failed to create InterstitialAd!");
	}

	private void eventCallback(InterstitialAdEvents adEvent, string eventMessage)
	{
		Debug.Log(adEvent);
		if (adEvent == InterstitialAdEvents.Error) Debug.LogError(eventMessage);
		if (adEvent == InterstitialAdEvents.Cached) ad.Show();
	}

	void OnGUI()
	{
		GUI.matrix = Matrix4x4.identity;
		GUI.color = Color.white;

		if (GUI.Button(new Rect(0, 0, 128, 64), "Show Ad"))
		{
			ad.Cache();
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
