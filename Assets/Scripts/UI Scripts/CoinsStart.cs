using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class CoinsStart : MonoBehaviour {
	
	public Text coins;
	
	void Start () {
		//PlayerPrefs.SetInt ("Coins", 0);
		coins.text = "Coins: " + PlayerPrefs.GetInt ("Coins").ToString();
	}

}
