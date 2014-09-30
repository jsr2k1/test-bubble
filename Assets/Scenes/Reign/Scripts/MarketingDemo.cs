// -----------------------------------------------
// Documentation: http://www.reign-studios.net/docs/unity-plugin/
// -----------------------------------------------

using UnityEngine;
using System.Collections.Generic;
using Reign;

public class MarketingDemo : MonoBehaviour
{
	void OnGUI()
	{
		float scale = new Vector2(Screen.width, Screen.height).magnitude / new Vector2(1280, 720).magnitude;

		if (GUI.Button(new Rect((Screen.width/2)-(64), (Screen.height/2)-(32*scale)+(64*scale), 128, 64*scale), "Review App"))
		{
			var desc = new MarketingDesc();
			desc.Editor_URL = "http://reign-studios.net/";// Editor: Any full URL
			desc.Win8_PackageFamilyName = "41631Reign-Studios.CosmicPong_2wv2wvs0mpzqp";// Win8: This is the "Package family name" that can be found in your "Package.appxmanifest".
			// WP8: Do nothing...
			desc.iOS_AppID = "547246359";// iOS: Pass in your AppID "xxxxxxxxx"
			desc.BB10_AppID = "49146902";// BB10: You pass in your AppID "xxxxxxxx".

			desc.Android_MarketingStore = MarketingStores.Amazon;
			desc.Android_GooglePlay_BundleID = "com.ReignStudios.CosmicPong";// Android GooglePlay: Pass in your bundle ID "com.Company.AppName"
			desc.Android_Amazon_BundleID = "com.reignstudios.cosmicpong";// Android Amazon: Pass in your bundle ID "com.Company.AppName"
			
			MarketingManager.OpenStoreForReview(desc);
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
