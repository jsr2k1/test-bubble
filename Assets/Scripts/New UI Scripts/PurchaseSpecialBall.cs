using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class PurchaseSpecialBall : MonoBehaviour {

	public Text NumberBallText;
	public Text CoinsText;
	public string BallString;
	public int price;
	private int quantity;
	private int coins;



	public void PurchaseBall () {

		if (PlayerPrefs.GetInt ("Coins") >= price) {
			
						quantity = PlayerPrefs.GetInt (BallString) + 1;
						coins = PlayerPrefs.GetInt ("Coins") - price;

						PlayerPrefs.SetInt (BallString, quantity);
						PlayerPrefs.SetInt ("Coins", coins);
			
						NumberBallText.text = PlayerPrefs.GetInt (BallString).ToString ();
						CoinsText.text = PlayerPrefs.GetInt ("Coins").ToString ();

				} else {
					
				}
	
	}

}
