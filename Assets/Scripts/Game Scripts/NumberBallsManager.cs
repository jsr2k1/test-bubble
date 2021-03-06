﻿using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class NumberBallsManager : MonoBehaviour
{
	Text ballsText;

	//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

	void Awake()
	{
		ballsText = GetComponent<Text>();
	}

	//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

	void Start()
	{
		if(LevelManager.GameType == LevelManager.GameTypes.ARCADE){
			ballsText.text = "";
		}
	}

	//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

	public void setBallsLeft(int ballsLeft)
	{
		if(ballsLeft < 0){
			ballsText.text = "0";
		}else{ 
			ballsText.text = ballsLeft.ToString();
		}
	}
}
