﻿using UnityEngine;
using System.Collections;

public class Splash : MonoBehaviour
{
	public float timer;
	public static Sprite spriteIMG;
	public bool bDeleteAllKeys; //debug
	public int initial_lives;

	void Awake()
	{
		if(bDeleteAllKeys){
			PlayerPrefs.DeleteAll();
		}

		if(!PlayerPrefs.HasKey("Lives")){
			PlayerPrefs.SetInt("Lives", initial_lives);
		}
		if(PlayerPrefs.HasKey("Music")==false){
			PlayerPrefs.SetInt("Music", 1);
		}
		if(PlayerPrefs.HasKey("Sounds")==false){
			PlayerPrefs.SetInt("Sounds", 1);
		}
		if(PlayerPrefs.HasKey("Multicolor Ball")==false){
			PlayerPrefs.SetInt("Multicolor Ball", 3);
		}
		if(PlayerPrefs.HasKey("Fire Ball")==false){
			PlayerPrefs.SetInt("Fire Ball", 3);
		}
		if(PlayerPrefs.HasKey("Bomb Ball")==false){
			PlayerPrefs.SetInt("Bomb Ball", 3);
		}
		if(PlayerPrefs.HasKey("Coins")==false){
			PlayerPrefs.SetInt("Coins", 500);
		}
		if(!PlayerPrefs.HasKey("Level")){
			PlayerPrefs.SetInt("Level", 0);
		}
		if(!PlayerPrefs.HasKey("World")){
			PlayerPrefs.SetInt("World", 1);
		}

		if(!PlayerPrefs.HasKey("Highscore")){
			PlayerPrefs.SetInt("Highscore", 0);
		}

		FB.Init(OnInitComplete);
		
		StartCoroutine("DisplayScene");
	}
	
	////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

	IEnumerator DisplayScene()
	{
		yield return new WaitForSeconds(timer);
		Application.LoadLevel(1);
	}
	
	////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

	private void OnInitComplete()
	{
		Debug.Log("FB.Init completed: Is user logged in? " + FB.IsLoggedIn);
	}
}



