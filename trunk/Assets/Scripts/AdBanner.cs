// -----------------------------------------------
// Documentation: http://www.reign-studios.net/docs/unity-plugin/
// -----------------------------------------------

using UnityEngine;
using System.Collections;
using Reign;

public class AdBanner : MonoBehaviour
{
	private static Ad ad;
	private string adStatus;
	public bool bDebug=false;
	
	//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
	
	void Start()
	{
		//DontDestroyOnLoad(gameObject);// Make sure the start method never gets called more then once.
		adStatus = "none";
		
		// Ads - NOTE: You can pass in multiple "AdDesc" objects if you want more then one ad.
		var desc = new AdDesc();
		// global settings
		desc.Testing = false;// NOTE: To test ads on iOS, you must enable them in iTunes Connect.
		desc.Visible = true;
		desc.EventCallback = eventCallback;
		
		// Editor
		desc.Editor_AdAPI = AdAPIs.EditorTestAd;
		desc.Editor_AdGravity = AdGravity.BottomCenter;
		desc.Editor_AdWidth = 256;
		desc.Editor_AdHeight = 32;
		//desc.Editor_AdGUIOverrideEnabled = true;
		
		desc.Editor_MillennialMediaAdvertising_APID = "";
		desc.Editor_MillennialMediaAdvertising_AdGravity = AdGravity.BottomCenter;
		desc.Editor_GuiAdScale = 1;
		//desc.Editor_MillennialMediaAdvertising_RefreshRate = 120,

		// iOS settings
		desc.iOS_AdAPI = AdAPIs.iAd;
		//desc.iOS_AdAPI = AdAPIs.AdMob;
		desc.iOS_iAd_AdGravity = AdGravity.BottomCenter;
		
		desc.iOS_AdMob_AdGravity = AdGravity.BottomCenter;
		desc.iOS_AdMob_UnitID = "ca-app-pub-4996019706488907/6665040478";// NOTE: You can use legacy (PublisherID) too, You MUST have this even for Testing!
		desc.iOS_AdMob_AdSize = iOS_AdMob_AdSize.SmartBannerPortrait;
		//desc.iOS_AdMob_AdSize = iOS_AdMob_AdSize.Banner_320x50;
		
		// Android settings
		#if AMAZON
		desc.Android_AdAPI = AdAPIs.AmazonAds;// Choose between AdMob or AmazonAds
		#else
		desc.Android_AdAPI = AdAPIs.AdMob;// Choose between AdMob or AmazonAds
		#endif
		
		desc.Android_AdMob_UnitID = "ca-app-pub-4996019706488907/5332751276";// NOTE: You can use legacy (PublisherID) too, You MUST have this even for Testing!
		desc.Android_AdMob_AdGravity = AdGravity.BottomCenter;
		desc.Android_AdMob_AdSize = Android_AdMob_AdSize.SmartBanner;
		
		desc.Android_AmazonAds_ApplicationKey = "";
		desc.Android_AmazonAds_AdSize = Android_AmazonAds_AdSize.Wide_320x50;
		desc.Android_AmazonAds_AdGravity = AdGravity.BottomCenter;
		//desc.Android_AmazonAds_RefreshRate = 120;

		// create ad
		ad = AdManager.CreateAd(desc, adCreatedCallback);
		
		// show ad
		//ad.Visible = true;
		//ad.Draw();
	}
	
	//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
	
	private void adCreatedCallback(bool succeeded)
	{
		adStatus = succeeded ? "Ads Succeded" : "Ads Failed";
		if(bDebug){
			Debug.Log("adCreatedCallback: "+adStatus);
		}
	}
	
	//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
	
	private void eventCallback(AdEvents adEvent, string eventMessage)
	{
		if(bDebug){
			Debug.Log("eventCallback: "+adEvent);
		}
		// NOTE: On BB10 these events never get called!
		switch (adEvent)
		{
			case AdEvents.Refreshed: adStatus = "Refreshed"; break;
			case AdEvents.Clicked: adStatus = "Clicked"; break;
			case AdEvents.Error: adStatus = "Error: " + eventMessage; break;
		}
	}

	//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
	
	void OnGUI()
	{
		if(bDebug){
			GUI.matrix = Matrix4x4.identity;
			GUI.color = Color.blue;
			
			GUI.Label(new Rect(0, Screen.height/2, 256, 64), "Ad status: " + adStatus);
			
			// Manual Refresh does not work on (Apple iAd) or (BB10 Ads).
			if (GUI.Button(new Rect(0, 0, 128, 64), "Manual Refresh")) ad.Refresh();
			
			// Show / Hide Ad
			if (GUI.Button(new Rect(0, 128+8, 128, 64), "Show / Hide")) ad.Visible = !ad.Visible;
			
			// You can also manually draw GUI based Ads if you want to control GUI sort order
			//ad.Draw();
		}
	}
	
	//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

	void Update()
	{
		// NOTE: If you are getting unity activity pause timeout issues on Android, call "ApplicationEx.Quit();"
		// There seems to be what may be a memeory leak in Unity4.3+
		// Until this is fixed I recomend trying to calling this quit method on Android.
		// (It will save your player prefs and use "System.exit(0)" instead of "finish()" on Android)
		// If you have a better work-around, email support, Thanks.
		//if (Input.GetKeyUp(KeyCode.Escape)) ApplicationEx.Quit();// NOTE: Unity 4.5 does not need this
	}
	
	//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

	void OnDestroy()
	{
		ad.Visible = false;
	}
}



