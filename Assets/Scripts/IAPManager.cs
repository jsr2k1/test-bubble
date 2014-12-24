using UnityEngine;
using System.Collections;
using Reign;
using UnityEngine.UI;

public class IAPManager : MonoBehaviour
{
	private bool waiting;
	private string[] restoreInAppStatusText;
	//private string formatedPriceText;
	//public Text coinstext;

	static bool bStarted=false;
	
	#if SAMSUNG
	private string item1 = "xxxxxxxxxxx1";
	private string item2 = "xxxxxxxxxxx2";
	private string item3 = "xxxxxxxxxxx3";
	#else
	private string item1 = "xsmall";
	private string item2 = "small";
	private string item3 = "medium";
	private string item4 = "big";
	private string item5 = "extrabig";
	#endif
	
	void Start()
	{
		if(bStarted){
			return;
		}

		//DontDestroyOnLoad(gameObject);// Make sure the start method never gets called more then once.
		
		// InApp-Purchases - NOTE: you can set different "In App IDs" for each platform.
		var inAppIDs = new InAppPurchaseID[5];
		inAppIDs [0] = new InAppPurchaseID(item1, 0.99m, "$", InAppPurchaseTypes.Consumable);
		inAppIDs [1] = new InAppPurchaseID(item2, 0.99m, "$", InAppPurchaseTypes.Consumable);
		inAppIDs [2] = new InAppPurchaseID(item3, 0.99m, "$", InAppPurchaseTypes.Consumable);
		inAppIDs [3] = new InAppPurchaseID(item4, 0.99m, "$", InAppPurchaseTypes.Consumable);
		inAppIDs [4] = new InAppPurchaseID(item5, 0.99m, "$", InAppPurchaseTypes.Consumable);
		
		restoreInAppStatusText = new string[inAppIDs.Length];
		var desc = new InAppPurchaseDesc();
		
		// Global
		desc.Testing = true;
		
		// Editor
		desc.Editor_InAppIDs = inAppIDs;
		
		// Win8
		desc.Win8_InAppPurchaseAPI = InAppPurchaseAPIs.MicrosoftStore;
		desc.Win8_MicrosoftStore_InAppIDs = inAppIDs;
		
		// WP8
		desc.WP8_InAppPurchaseAPI = InAppPurchaseAPIs.MicrosoftStore;
		desc.WP8_MicrosoftStore_InAppIDs = inAppIDs;
		
		// BB10
		desc.BB10_InAppPurchaseAPI = InAppPurchaseAPIs.BlackBerryWorld;
		desc.BB10_BlackBerryWorld_InAppIDs = inAppIDs;
		
		// iOS
		desc.iOS_InAppPurchaseAPI = InAppPurchaseAPIs.AppleStore;
		desc.iOS_AppleStore_InAppIDs = inAppIDs;
		desc.iOS_AppleStore_SharedSecretKey = "e60ebb2d723149f4ab400404b28aad43";// NOTE: Must set SharedSecretKey, even for Testing!
		
		// Android
		// Choose for either GooglePlay or Amazon.
		// NOTE: Use "player settings" to define compiler directives.
		#if AMAZON
		desc.Android_InAppPurchaseAPI = InAppPurchaseAPIs.Amazon;
		#elif SAMSUNG
		desc.Android_InAppPurchaseAPI = InAppPurchaseAPIs.Samsung;
		#else
		desc.Android_InAppPurchaseAPI = InAppPurchaseAPIs.GooglePlay;
		#endif
		
		desc.Android_GooglePlay_InAppIDs = inAppIDs;
		desc.Android_GooglePlay_Base64Key = "MIIBIjANBgkqhkiG9w0BAQEFAAOCAQ8AMIIBCgKCAQEA6CkjzMnNCuwRhqpOelNnvdJkQ6xr3E2I++ubNNOk4GeBx99Fh0wZJZQ6mHB+2b4sD55+rHU2OUavNtM7b8Cu7En4Hkeac1bi4LWf9DiL7OTHz3o6atH9T0gZPewtZb+rkYuXP1GURs+Vt/aKOnAhgOjUsU++KW3rvevnvUMF5hDl3O1XsybepEldJ4aSPvful5NJiIVf3qkiP1jTGcdFTnjznOeGaI9bRmTOrnZIrSsfo5FKcX9hROrolGOy01Oa706yi6xHd6Et0TKtPPbiF8KNB4JUGku/4Uwc37o1osNDVjEp55tyLXP9W4QgXtNNE7tBswvRvGgcMTIcCKUemwIDAQAB";// NOTE: Must set Base64Key for GooglePlay in Apps to work, even for Testing!
		desc.Android_Amazon_InAppIDs = inAppIDs;
		desc.Android_Samsung_InAppIDs = inAppIDs;
		desc.Android_Samsung_ItemGroupID = "";

		// init
		InAppPurchaseManager.Init(desc, createdCallback);

		bStarted=true;
	}
	
	private void createdCallback(bool succeeded)
	{
		//Debug.Log("InAppPurchaseManager: " + succeeded);
		InAppPurchaseManager.MainInAppAPI.AwardInterruptedPurchases(awardInterruptedPurchases);
	}
	
	private void awardInterruptedPurchases(string inAppID, bool succeeded)
	{
		int appIndex = InAppPurchaseManager.MainInAppAPI.GetAppIndexForAppID(inAppID);
		if(appIndex != -1) {
			restoreInAppStatusText [appIndex] = "Restore Status: " + inAppID + ": " + succeeded + " Index: " + appIndex;
			Debug.Log(restoreInAppStatusText [appIndex]);
		}
	}
	/*
		void OnGUI()
		{
				float scale = new Vector2(Screen.width, Screen.height).magnitude / new Vector2(1280, 720).magnitude;
		
				// Buy
				if(!waiting && GUI.Button(new Rect(0, 0, 148, 64 * scale), "Buy NonConsumable")) {
						waiting = true;
						// NOTE: You can pass in a "InAppID string value" or an "index" value.
						InAppPurchaseManager.MainInAppAPI.Buy(item1, buyAppCallback);
				}
		
				if(!waiting && GUI.Button(new Rect(0, 64 * scale, 148, 64 * scale), "Buy Consumable")) {
						waiting = true;
						// NOTE: You can pass in a "InAppID string value" or an "index" value.
						InAppPurchaseManager.MainInAppAPI.Buy(item3, buyAppCallback);
				}
		
				// Restore
				if(!waiting && GUI.Button(new Rect(0, 128 * scale, 148, 64 * scale), "Restore Apps")) {
						waiting = true;
						InAppPurchaseManager.MainInAppAPI.Restore(restoreAppsCallback);
				} else {
						for(int i = 0; i != restoreInAppStatusText.Length; ++i) {
								GUI.Label(new Rect(Screen.width - 256, 64 * i, 256, 64), restoreInAppStatusText [i]);
						}
				}
		
				// Get price information
				if(!waiting && GUI.Button(new Rect(148 + 16, 0, 148, 64 * scale), "Get Price Info")) {
						waiting = true;
						InAppPurchaseManager.MainInAppAPI.GetProductInfo(productInfoCallback);
				} else if(formatedPriceText != null) {
						GUI.Label(new Rect(148 * 2 + 16 + 8, 0, 128, 32), formatedPriceText);
				}
		}
		*/
	public void PurchaseSomething(string item)
	{
		if(!waiting) {
			waiting = true;
			InAppPurchaseManager.MainInAppAPI.Buy(item, buyAppCallback);
		}
	}
	
	private void productInfoCallback(InAppPurchaseInfo[] priceInfos, bool succeeded)
	{
		waiting = false;
		if(succeeded) {
			//foreach(var info in priceInfos) {
			//if(info.ID == item1)
			//	formatedPriceText = info.FormattedPrice;
			//Debug.Log(string.Format("ID: {0} Price: {1}", info.ID, info.FormattedPrice));
			//}
		}
	}
	
	void buyAppCallback(string inAppID, bool succeeded)
	{
		waiting = false;
		int appIndex = InAppPurchaseManager.MainInAppAPI.GetAppIndexForAppID(inAppID);
		//MessageBoxManager.Show("App Buy Status", inAppID + " Success: " + succeeded + " Index: " + appIndex);
		if(appIndex != -1)
			restoreInAppStatusText [appIndex] = "Restore Status: " + inAppID + ": " + succeeded + " Index: " + appIndex;
		if(succeeded) {
			if(inAppID == "xsmall") {
				int coins = PlayerPrefs.GetInt("Coins");
				coins = coins + 100;
				PlayerPrefs.SetInt("Coins", coins);	
				//coinstext.text = PlayerPrefs.GetInt("Coins").ToString();
			}
			if(inAppID == "small") {
				int coins = PlayerPrefs.GetInt("Coins");
				coins = coins + 400;
				PlayerPrefs.SetInt("Coins", coins);	
				//coinstext.text = PlayerPrefs.GetInt("Coins").ToString();
			}
						
			if(inAppID == "medium") {
				int coins = PlayerPrefs.GetInt("Coins");
				coins = coins + 800;
				PlayerPrefs.SetInt("Coins", coins);	
				//coinstext.text = PlayerPrefs.GetInt("Coins").ToString();
			}
						
			if(inAppID == "big") {
				int coins = PlayerPrefs.GetInt("Coins");
				coins = coins + 2000;
				PlayerPrefs.SetInt("Coins", coins);	
				//coinstext.text = PlayerPrefs.GetInt("Coins").ToString();
			}
						
			if(inAppID == "extrabig") {
				int coins = PlayerPrefs.GetInt("Coins");
				coins = coins + 5000;
				PlayerPrefs.SetInt("Coins", coins);	
				//coinstext.text = PlayerPrefs.GetInt("Coins").ToString();
			}
		}
	}
	
	void restoreAppsCallback(string inAppID, bool succeeded)
	{
		waiting = false;
		int appIndex = InAppPurchaseManager.MainInAppAPI.GetAppIndexForAppID(inAppID);
		if(appIndex != -1) {
			restoreInAppStatusText [appIndex] = "Restore Status: " + inAppID + ": " + succeeded + " Index: " + appIndex;
			Debug.Log(restoreInAppStatusText [appIndex]);
		}
	}
	
	void Update()
	{
		// NOTE: If you are getting unity activity pause timeout issues on Android, call "ApplicationEx.Quit();"
		// There seems to be what may be a memeory leak in Unity4.3+
		// Until this is fixed I recomend trying to calling this quit method on Android.
		//(It will save your player prefs and use "System.exit(0)" instead of "finish()" on Android)
		// If you have a better work-around, email support, Thanks.
		//if(Input.GetKeyUp(KeyCode.Escape))
		//	ApplicationEx.Quit();// NOTE: Unity 4.5 does not need this
	}
}

