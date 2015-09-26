using UnityEngine;
using System.Collections;
using UnityEngine.UI;
//using com.adjust.sdk;

public class PurchaseLives : MonoBehaviour
{
	PopUpMgr LivesPopUp;
	PopUpMgr ShopCoinsPopUp;
	public Text coinstext;
	
	////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

	void Awake()
	{
		LivesPopUp = GameObject.Find("ShopLivesPopUp").GetComponent<PopUpMgr>();
		ShopCoinsPopUp = GameObject.Find("ShopCoinsPopUp").GetComponent<PopUpMgr>();
	}
	
	////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

	public void purchaseLives()
	{
		if(LivesManager.lives < 5 && PlayerPrefs.GetInt("Coins") >= 100)
		{
			LivesManager.lives = 5;
			CoinsManager.instance.SetCoins(PlayerPrefs.GetInt("Coins") - 100);
			coinstext.text = PlayerPrefs.GetInt("Coins").ToString();
			LivesPopUp.HidePopUp();
			//ParseManager.instance.SaveCurrentData();
			//Adjust.trackEvent(new AdjustEvent("ydzqno"));
		}
		else if(LivesManager.lives < 5 && PlayerPrefs.GetInt("Coins") < 100)
		{
			LivesPopUp.HidePopUp();
			ShopCoinsPopUp.ShowPopUp();
		}
	}
}
