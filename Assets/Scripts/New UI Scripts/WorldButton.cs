using UnityEngine;
using System.Collections;

public class WorldButton : MonoBehaviour {

	public int numberBalls;
	public int ActualWorld;

	public void ButtonPressed()
	{
		PlayerPrefs.SetString("GameType", "Normal");
		LevelManager.patternType = PatternType.TextLevel;
		LevelParser.instance.LoadTextLevel(int.Parse(name), ActualWorld);
		
		LevelManager.NumberOfBalls = numberBalls;
		LevelManager.rowAddingInterval = 1;
		LevelManager.levelNo = int.Parse(name);
		Application.LoadLevel(3);
	
	}
	

}
