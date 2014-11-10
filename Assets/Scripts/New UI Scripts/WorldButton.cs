﻿using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class WorldButton : MonoBehaviour
{
	public int numberBalls;
	public int ActualWorld;
	public int refScore;
	public Text numTextPrefab;
	public Image star1;
	public Image star2;
	public Image star3;
	public GameObject ProfileImg;
	public Sprite bubbleFilled;
	Button button;
	AudioSource audioSource;
	public Image livesPop;

	//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
	
	void Awake()
	{
		audioSource = GameObject.Find("1").GetComponent<AudioSource>();
		
		button = GetComponent<Button>();

		if(gameObject.name == "1" || PlayerPrefs.HasKey("STARS_" +(int.Parse(gameObject.name) - 1))){
			button.interactable = true;
			GetComponent<Image>().sprite = bubbleFilled;
		} else{
			button.interactable = false;
		}

		Text currentText = Instantiate(numTextPrefab) as Text;
		currentText.text = gameObject.name;
		currentText.transform.position = transform.position;
		currentText.transform.SetParent(transform);
		currentText.name = "label_" + gameObject.name;
		currentText.transform.localScale = new Vector3(0.2f, 0.2f, 1.0f);

		Image stars;

		if(PlayerPrefs.GetInt("STARS_" + gameObject.name) == 3){
			stars = Instantiate(star3) as Image;

		} else if(PlayerPrefs.GetInt("STARS_" + gameObject.name) == 2){
			stars = Instantiate(star2) as Image;

		} else if(PlayerPrefs.GetInt("STARS_" + gameObject.name) == 1){
			stars = Instantiate(star1) as Image;

		} else{
			stars = null;
		}

		if(stars != null){
			stars.transform.SetParent(transform);
			stars.transform.localPosition = new Vector3(0, 40, 0);
			stars.transform.localScale = new Vector3(0.8f, 0.8f, 1.0f);
		}
	}
	
	//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
	
	void Update()
	{
		if(gameObject.name ==(PlayerPrefs.GetInt("Level")+1).ToString()){
			ProfileImg.transform.position = gameObject.transform.position + new Vector3(0.0f, 60.0f, 0.0f);
		}
	}
	
	//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

	public void ButtonPressed()
	{
		if(audioSource && PlayerPrefs.GetInt("Sounds")==1){
			audioSource.Play();
		}
		int nLives=PlayerPrefs.GetInt("Lives");
		if (nLives > 0) {
			PlayerPrefs.SetString ("GameType", "Normal");
			LevelManager.patternType = PatternType.TextLevel;
			LevelParser.instance.LoadTextLevel (int.Parse (name), ActualWorld);

			LevelManager.NumberOfBalls = numberBalls;
			LevelManager.ReferenceScore = refScore;
			LevelManager.rowAddingInterval = 1;
			LevelManager.levelNo = int.Parse (name);

			PlayerPrefs.SetInt ("bPlaying", 1);
			Application.LoadLevel (3);
		} else {
			livesPop.GetComponent<PopUpMgr>().ShowPopUp();
		}
	}
}



