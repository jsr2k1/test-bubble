using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Prime31;

public class IABManager : MonoBehaviour
{
	string item1 = "xsmall";
	string item2 = "small";
	string item3 = "medium";
	string item4 = "big";
	string item5 = "extrabig";
#if UNITY_ANDROID
	string androidPublicKey = "MIIBIjANBgkqhkiG9w0BAQEFAAOCAQ8AMIIBCgKCAQEA6CkjzMnNCuwRhqpOelNnvdJkQ6xr3E2I++ubNNOk4GeBx99Fh0wZJZQ6mHB+2b4sD55+rHU2OUavNtM7b8Cu7En4Hkeac1bi4LWf9DiL7OTHz3o6atH9T0gZPewtZb+rkYuXP1GURs+Vt/aKOnAhgOjUsU++KW3rvevnvUMF5hDl3O1XsybepEldJ4aSPvful5NJiIVf3qkiP1jTGcdFTnjznOeGaI9bRmTOrnZIrSsfo5FKcX9hROrolGOy01Oa706yi6xHd6Et0TKtPPbiF8KNB4JUGku/4Uwc37o1osNDVjEp55tyLXP9W4QgXtNNE7tBswvRvGgcMTIcCKUemwIDAQAB";
#endif

	public static IABManager instance;
	public Dictionary<string,string> dictPrices;
	
	////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

	void Awake()
	{
		instance = this;
	}

	////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

	void OnEnable()
	{
#if UNITY_ANDROID
		GoogleIABManager.billingSupportedEvent += billingSupportedEvent;
		GoogleIABManager.billingNotSupportedEvent += billingNotSupportedEvent;
		GoogleIABManager.queryInventorySucceededEvent += queryInventorySucceededEvent;
		GoogleIABManager.queryInventoryFailedEvent += queryInventoryFailedEvent;
#endif		
	}
	
	////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
	
	void OnDisable()
	{
#if UNITY_ANDROID
		GoogleIABManager.billingSupportedEvent -= billingSupportedEvent;
		GoogleIABManager.billingNotSupportedEvent -= billingNotSupportedEvent;
		GoogleIABManager.queryInventorySucceededEvent -= queryInventorySucceededEvent;
		GoogleIABManager.queryInventoryFailedEvent -= queryInventoryFailedEvent;
#endif
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
		var androidSkus = new string[] { item1, item2, item3, item4, item5 };
		var iosProductIds = new string[] { item1, item2, item3, item4, item5 };
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
	void billingSupportedEvent()
	{
		Debug.Log("billingSupportedEvent");

		var androidSkus = new string[] { item1, item2, item3, item4, item5 };
		GoogleIAB.queryInventory(androidSkus);
	}
	
	////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
	
	void billingNotSupportedEvent(string error)
	{
		Debug.Log("billingNotSupportedEvent: " + error);
	}
	
	////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
	
	void queryInventorySucceededEvent( List<GooglePurchase> purchases, List<GoogleSkuInfo> skus )
	{
		foreach(GoogleSkuInfo sku in skus){
			if(!dictPrices.ContainsKey(sku.productId)){
				dictPrices.Add(sku.productId, sku.price);
			}
			//Debug.Log(sku.productId + ":" + sku.price);
		}
	}
	
	////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
	
	void queryInventoryFailedEvent(string error)
	{
		Debug.Log( "queryInventoryFailedEvent: " + error );
	}
#endif	
	////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
	
	public void PurchaseSomething(string productId)
	{
#if UNITY_EDITOR
		DoPurchase(productId);
#else
		if(dictPrices.ContainsKey(productId)){
			IAP.purchaseConsumableProduct(productId, (didSucceed, error) =>
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
		if(productId == "xsmall") {
			int coins = PlayerPrefs.GetInt("Coins");
			coins = coins + 100;
			CoinsManager.instance.SetCoins(coins);
			Adjust.trackRevenue(99, "v00myc");
			GA.API.Business.NewEvent("PurchaseXSmall", dictPrices.ContainsKey(item1) ? dictPrices[item1] : "none", 99);
		}
		if(productId == "small") {
			int coins = PlayerPrefs.GetInt("Coins");
			coins = coins + 400;
			CoinsManager.instance.SetCoins(coins);
			Adjust.trackRevenue(299, "x088of");
			GA.API.Business.NewEvent("PurchaseSmall", dictPrices.ContainsKey(item2) ? dictPrices[item2] : "none", 299);
		}
		if(productId == "medium") {
			int coins = PlayerPrefs.GetInt("Coins");
			coins = coins + 800;
			CoinsManager.instance.SetCoins(coins);
			Adjust.trackRevenue(499, "4hz7lk");
			GA.API.Business.NewEvent("PurchaseMedium", dictPrices.ContainsKey(item3) ? dictPrices[item3] : "none", 499);
		}
		if(productId == "big") {
			int coins = PlayerPrefs.GetInt("Coins");
			coins = coins + 2000;
			CoinsManager.instance.SetCoins(coins);
			Adjust.trackRevenue(999, "mh1aku");
			GA.API.Business.NewEvent("PurchaseBig", dictPrices.ContainsKey(item4) ? dictPrices[item4] : "none", 999);
		}
		if(productId == "extrabig") {
			int coins = PlayerPrefs.GetInt("Coins");
			coins = coins + 5000;
			CoinsManager.instance.SetCoins(coins);
			Adjust.trackRevenue(1999, "9ok2mj");
			GA.API.Business.NewEvent("PurchaseExtraBig", dictPrices.ContainsKey(item5) ? dictPrices[item5] : "none", 1999);
		}
		
		ParseManager.instance.SaveCurrentData();
		Adjust.trackEvent("80jv5o");
	}
}



