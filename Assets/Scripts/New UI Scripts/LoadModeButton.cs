using UnityEngine;
using System.Collections;
//using GameAnalyticsSDK;
//using com.adjust.sdk;

public class LoadModeButton : MonoBehaviour
{
	public void LoadWorldMode()
	{
		//Adjust.trackEvent(new AdjustEvent("1b5w81"));

		//GameAnalytics.NewDesignEvent("LoadWorldMode");
		
		Application.LoadLevel("04 World Menu");
	}

	////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

	public void LoadArcadeMode()
	{
		//Adjust.trackEvent(new AdjustEvent("xr59vt"));

		//GameAnalytics.NewDesignEvent("LoadArcadeMode");
		
		LevelManager.GameType = LevelManager.GameTypes.ARCADE;

		int levelNo = 1;
		int val = levelNo % 4;
		
		LevelManager.patternType = (PatternType)(levelNo / 4);
		LevelManager.totalNoOfRows = 10 + val * 2;
		LevelManager.minimumNumberOfRows = 3 + val;
		LevelManager.rowAddingInterval = 8;
		LevelManager.levelNo = levelNo;
		
		Application.LoadLevel("06 Arcade Game Scene");
	}
}
