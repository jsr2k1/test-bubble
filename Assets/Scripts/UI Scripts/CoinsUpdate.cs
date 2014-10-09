using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class CoinsUpdate : MonoBehaviour
{
	Text text;

	void Awake()
	{
		text = GetComponent<Text>();	
	}
	
	void Update()
	{
		text.text = PlayerPrefs.GetInt("Coins").ToString();
	}
}
