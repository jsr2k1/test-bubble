using UnityEngine;
using System.Collections;

public class LoadModeButton : MonoBehaviour
{
	public void LoadWorldMode()
	{
		Adjust.trackEvent("1b5w81");
		GA.API.Design.NewEvent("LoadWorldMode");
		
		Application.LoadLevel("04 World Menu");
	}

	////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

	public void LoadArcadeMode()
	{
		Adjust.trackEvent("xr59vt");
		GA.API.Design.NewEvent("LoadArcadeMode");
		
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
