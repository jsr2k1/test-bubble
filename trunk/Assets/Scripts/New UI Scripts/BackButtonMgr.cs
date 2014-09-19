using UnityEngine;
using System.Collections;

public class BackButtonMgr : MonoBehaviour
{
	public void BackButtonPressed()
	{
		if(Application.loadedLevel==3)//Game Scene
		{
			if(PlayerPrefs.GetString("GameType") == "Arcade"){
				Application.LoadLevel(1); //Go to Menu Scene
			}else{
				Application.LoadLevel(2); //Go to World Scene
			}
		}
		else if(Application.loadedLevel==2)//World Scene
		{
			Application.LoadLevel(1); //Go to Menu Scene
		}
		else{
			Application.Quit();
		}
	}
}
