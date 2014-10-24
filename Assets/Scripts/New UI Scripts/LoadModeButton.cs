using UnityEngine;
using System.Collections;

public class LoadModeButton : MonoBehaviour
{
	public void LoadWorldMode()
	{
		if(PlayerPrefs.GetInt("Sounds")==1){
			audio.Play();
		}
		Application.LoadLevel(2);
	}

	////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

	public void LoadArcadeMode()
	{
		PlayerPrefs.SetString("GameType", "Arcade");
		
		if(PlayerPrefs.GetInt("Sounds")==1){
			audio.Play();
		}
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
