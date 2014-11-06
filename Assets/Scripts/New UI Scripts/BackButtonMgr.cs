using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class BackButtonMgr : MonoBehaviour
{
	AdBanner banner;
	public Image quitPop;

	public void BackButtonPressed()
	{
		if(PlayerPrefs.GetInt("Sounds")==1){
			audio.Play();
		}
		if(Application.loadedLevel==3)//Worlds game scene
		{
			LivesManager.lives--;
			PlayerPrefs.SetInt("bPlaying", 0);
			Application.LoadLevel(2); //Go to World Scene
		}
		else if(Application.loadedLevel==4)//Arcade
		{
			Application.LoadLevel(1); //Go to Menu Scene
		}
		else if(Application.loadedLevel==2)//World Scene
		{
			Application.LoadLevel(1); //Go to Menu Scene
		}
		else{
			quitPop.GetComponent<PopUpMgr>().ShowPopUp();
		}
	}
}
