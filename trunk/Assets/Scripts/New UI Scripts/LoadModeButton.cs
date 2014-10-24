using UnityEngine;
using System.Collections;

public class LoadModeButton : MonoBehaviour
{
	public void LoadWorldMode()
	{
		Application.LoadLevel(2);
	}

	////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

	public void LoadArcadeMode()
	{
		int levelNo = 1;

		PlayerPrefs.SetString("GameType", "Arcade");
		
		int val = levelNo % 4;
		
		LevelManager.patternType = (PatternType)(levelNo / 4);
		
		LevelManager.totalNoOfRows = 10 + val * 2;
		
		LevelManager.minimumNumberOfRows = 3 + val;
		//LevelManager.rowAddingInterval = 9 - val;
		LevelManager.rowAddingInterval = 8;
		
		LevelManager.levelNo = levelNo;
		Application.LoadLevel (4);
	}
}
