using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class PurchaseSpecialBall : MonoBehaviour
{
	public Text NumberBallText;
	public string BallString;
	public Text textPrice;
	public int numBalls;

	public PopUpMgr ShopBoostersPopUp;
	PopUpMgr ShopCoinsPopUp;
	
	//bool bShowCoinsPopUp=false;
	
	//Creamos un evento para poder saber cuando se ha comprado un booster
	public delegate void SpecialBallLaunched();
	public static event SpecialBallLaunched OnSpecialBallBuyed;

	//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

	void Awake()
	{
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
		int price = int.Parse(textPrice.text);
		
		//Comprar un booster y restar dinero disponible
		if(PlayerPrefs.GetInt("Coins") >= price)
		{
			int quantity = PlayerPrefs.GetInt(BallString) + numBalls;
			int coins = PlayerPrefs.GetInt("Coins") - price;

			PlayerPrefs.SetInt(BallString, quantity);
			CoinsManager.instance.SetCoins(coins);

			NumberBallText.text = PlayerPrefs.GetInt(BallString).ToString();
			
			if(OnSpecialBallBuyed!=null){
				OnSpecialBallBuyed();
			}
		}//Si no hay suficiente dinero, abrir el popup para comprar monedas
		else{
			ShopBoostersPopUp.HidePopUp();
			//bShowCoinsPopUp=true;
			ShopCoinsPopUp.ShowPopUp();
		}
	}
}
