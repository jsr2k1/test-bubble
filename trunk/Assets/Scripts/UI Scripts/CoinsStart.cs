using UnityEngine;
using System.Collections;

public class CoinsStart : MonoBehaviour {
	
	public TextMesh coins;
	
	void Start () {
		coins.text = "Coins: " + PlayerPrefs.GetInt ("Coins").ToString();
	}

}
