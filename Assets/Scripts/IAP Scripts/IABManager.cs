using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Prime31;
//using GameAnalyticsSDK;
//using com.adjust.sdk;

public class IABManager : MonoBehaviour
{

	//TODO: Joel: Set android public key

#if UNITY_ANDROID || UNITY_IPHONE
	string item1 = "xsmall";
	string item2 = "small";
	string item3 = "medium";
	string item4 = "big";
	//string item5 = "extrabig";
#endif
#if UNITY_ANDROID
	string androidPublicKey = "";
#endif

	public static IABManager instance;
	public Dictionary<string,string> dictPrices;
	
	////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

	void Awake()
	{
		instance = this;
	}
	
	////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
	//En Android hay que hacer el init(), esperar a que termine y entonces hacer el QueryInventory
	//En iOS hacemos el RequestProductData directamente
	void Start()
	{
		DontDestroyOnLoad(gameObject);
		dictPrices = new Dictionary<string, string>();

#if UNITY_ANDROID
		GoogleIAB.init(androidPublicKey);
#elif UNITY_IPHONE
		//Request product data
		var androidSkus = new string[] { item1, item2, item3, item4 };
		var iosProductIds = new string[] { item1, item2, item3, item4 };
		IAP.requestProductData( iosProductIds, androidSkus, productList =>
		{
			Debug.Log("IABManager: Product list received" );
			Utils.logObject(productList);
			/*
			foreach(IAPProduct product in productList){
				string s = product.currencyCode;
				dictPrices.Add(product.productId, product.currencyCode+" "+product.price);
				Debug.Log(product.productId + ": " + product.currencyCode+" "+product.price);
			}*/
		});
#endif		
	}
	
	////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
#if UNITY_ANDROID
	public void CallAndroidQueryInventory()
	{
		var androidSkus = new string[] { item1, item2, item3, item4 };
		GoogleIAB.queryInventory(androidSkus);
	}
#endif	
	////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
	
	public void PurchaseSomething(string productId)
	{
#if UNITY_EDITOR
		DoPurchase(productId);
#else
		if(dictPrices.ContainsKey(productId)){
			IAP.purchaseConsumableProduct(productId,(didSucceed, error) =>
			{
				Debug.Log("purchasing product " + productId + " result: " + didSucceed);
				
				if(didSucceed){
					DoPurchase(productId);
				}else{
					Debug.Log("purchase error: " + error);
				}
			});
		}
#endif
	}
	
	////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
	
	void DoPurchase(string productId)
	{
		//#if UNITY_ANDROID || UNITY_IPHONE
		//string currentlvl = (PlayerPrefs.GetInt ("Level") + 1).ToString ();
		//#endif

		if(productId == "xsmall") {
			int coins = PlayerPrefs.GetInt("Coins");
			coins = coins + 200;
			CoinsManager.instance.SetCoins(coins);
			
			//AdjustEvent adjustEvent = new AdjustEvent("v00myc");
			//adjustEvent.setRevenue(0.99, "EUR");
			//Adjust.trackEvent(adjustEvent);

			#if UNITY_ANDROID
				//GameAnalytics.NewBusinessEventGooglePlay("USD", 199, "PurchaseXSmall", "itemId", currentlvl, null, null);
			#elif UNITY_IPHONE
				//GameAnalytics.NewBusinessEventIOSAutoFetchReceipt ("USD", 199, "PurchaseXSmall", "itemId", currentlvl);
			#endif
		}
		if(productId == "small") {
			int coins = PlayerPrefs.GetInt("Coins");
			coins = coins + 500;
			CoinsManager.instance.SetCoins(coins);
			
			//AdjustEvent adjustEvent = new AdjustEvent("x088of");
			//adjustEvent.setRevenue(2.99, "EUR");
			//Adjust.trackEvent(adjustEvent);

			#if UNITY_ANDROID
				//GameAnalytics.NewBusinessEventGooglePlay("USD", 399, "PurchaseSmall", "itemId", currentlvl, null, null);
			#elif UNITY_IPHONE
				//GameAnalytics.NewBusinessEventIOSAutoFetchReceipt ("USD", 399, "PurchaseBig", "itemId", currentlvl);
			#endif
		}
		if(productId == "medium") {
			int coins = PlayerPrefs.GetInt("Coins");
			coins = coins + 1500;
			CoinsManager.instance.SetCoins(coins);
			
			//AdjustEvent adjustEvent = new AdjustEvent("4hz7lk");
			//adjustEvent.setRevenue(4.99, "EUR");
			//Adjust.trackEvent(adjustEvent);

			#if UNITY_ANDROID
				//GameAnalytics.NewBusinessEventGooglePlay("USD", 899, "PurchaseMedium", "itemId", currentlvl, null, null);
			#elif UNITY_IPHONE
				//GameAnalytics.NewBusinessEventIOSAutoFetchReceipt ("USD", 899, "PurchaseBig", "itemId", currentlvl);
			#endif
		}
		if(productId == "big") {
			int coins = PlayerPrefs.GetInt("Coins");
			coins = coins + 5000;
			CoinsManager.instance.SetCoins(coins);
			
			//AdjustEvent adjustEvent = new AdjustEvent("mh1aku");
			//adjustEvent.setRevenue(9.99, "EUR");
			//Adjust.trackEvent(adjustEvent);

			#if UNITY_ANDROID
				//GameAnalytics.NewBusinessEventGooglePlay("USD", 1999, "PurchaseBig", "itemId", currentlvl, null, null);
			#elif UNITY_IPHONE
				//GameAnalytics.NewBusinessEventIOSAutoFetchReceipt ("USD", 1999, "PurchaseBig", "itemId", currentlvl);
			#endif
		}/*
		if(productId == "extrabig") {
			int coins = PlayerPrefs.GetInt("Coins");
			coins = coins + 5000;
			CoinsManager.instance.SetCoins(coins);
			
			AdjustEvent adjustEvent = new AdjustEvent("9ok2mj");
			adjustEvent.setRevenue(19.99, "EUR");
			//Adjust.trackEvent(adjustEvent);

			#if UNITY_ANDROID
				//GameAnalytics.NewBusinessEventGooglePlay("USD", 1999, "PurchaseExtraBig", "itemId", currentlvl, null, null);
			#elif UNITY_IPHONE
				//GameAnalytics.NewBusinessEventIOSAutoFetchReceipt ("USD", 1999, "PurchaseExtraBig", "itemId", currentlvl);
			#endif
		}*/
			
		//ParseManager.instance.SaveCurrentData();
		////Adjust.trackEvent(new AdjustEvent ("80jv5o"));
	}
}



