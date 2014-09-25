using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class IAPbutton : MonoBehaviour
{

		public Text CoinsText;
		public int coins;

		public void PurchaseCoins ()
		{

				PlayerPrefs.SetInt ("Coins", PlayerPrefs.GetInt ("Coins") + coins);
		
				CoinsText.text = "Coins: " + PlayerPrefs.GetInt ("Coins").ToString ();
		}

}
