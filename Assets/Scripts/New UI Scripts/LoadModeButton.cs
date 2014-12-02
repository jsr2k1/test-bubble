﻿using UnityEngine;
using System.Collections;

public class LoadModeButton : MonoBehaviour
{
	public void LoadWorldMode()
	{
		Adjust.trackEvent("1b5w81");
		
		Application.LoadLevel(2);
	}

	////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

	public void LoadArcadeMode()
	{
		Adjust.trackEvent("xr59vt");
		
		PlayerPrefs.SetString("GameType", "Arcade");

		int levelNo = 1;
		int val = levelNo % 4;
		
		LevelManager.patternType = (PatternType)(levelNo / 4);
		LevelManager.totalNoOfRows = 10 + val * 2;
		LevelManager.minimumNumberOfRows = 3 + val;
		LevelManager.rowAddingInterval = 8;
		LevelManager.levelNo = levelNo;
		
		Application.LoadLevel (4);
	}
}
