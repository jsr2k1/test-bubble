using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class PurchaseSpecialBall : MonoBehaviour
{
	public Text NumberBallText;
	public Text CoinsText;
	public string BallString;
	public int price;

	private int quantity;
	private int coins;

	PopUpMgr ShopBoostersPopUp;
	PopUpMgr ShopCoinsPopUp;
	bool bShowCoinsPopUp=false;

	//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

	void Awake()
	{
		ShopBoostersPopUp = GameObject.Find("ShopBoostersPopUp").GetComponent<PopUpMgr>();
		ShopCoinsPopUp = GameObject.Find("ShopCoinsPopUp").GetComponent<PopUpMgr>();
	}

	//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
	/*
	void Update()
	{
		//Nos esperamos un frame antes de mostrar el popup ya que PopUpMgr se espera un frame para evitar problemas con el Update del InputScript
		if(bShowCoinsPopUp){
			ShopCoinsPopUp.ShowPopUp();
			bShowCoinsPopUp=false;
		}
	}
	*/
	//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

	public void PurchaseBall()
	{
		//Comprar un booster y restar dinero disponible
		if(PlayerPrefs.GetInt("Coins") >= price){
			quantity = PlayerPrefs.GetInt(BallString) + 1;
			coins = PlayerPrefs.GetInt("Coins") - price;

			PlayerPrefs.SetInt(BallString, quantity);
			PlayerPrefs.SetInt("Coins", coins);

			NumberBallText.text = PlayerPrefs.GetInt(BallString).ToString();
			CoinsText.text = PlayerPrefs.GetInt("Coins").ToString();
		}
		//Si no hay suficiente dinero, abrir el popup para comprar monedas
		else{
			ShopBoostersPopUp.HidePopUp(false);
			//bShowCoinsPopUp=true;
			ShopCoinsPopUp.ShowPopUp();
		}
	}
}
