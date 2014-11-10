using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class BackButtonMgr : MonoBehaviour
{
	AdBanner banner;
	public Image quitPop;
	public Image thisPop;
	public Image livesPop;
	bool backPressed=false;

	public void BackButtonPressed()
	{
		if(PlayerPrefs.GetInt("Sounds")==1)
			audio.Play();
		
		//MENU
		if(Application.loadedLevel==1){
			if(!backPressed){
				thisPop.GetComponent<PopUpMgr>().HidePopUp();
			}
			quitPop.GetComponent<PopUpMgr>().ShowPopUp();
		}
		//WORLDS
		else if(Application.loadedLevel==2){
			Application.LoadLevel(1); //Go to Menu Scene
		}
		//GAME-WORLDS
		else if(Application.loadedLevel==3){
			if(!backPressed){
				thisPop.GetComponent<PopUpMgr>().HidePopUp(false);
			}
			livesPop.GetComponent<PopUpMgr>().ShowPopUp();
		}
		//GAME-ARCADE
		else if(Application.loadedLevel==4){
			Application.LoadLevel(1); //Go to Menu Scene
		}
	}
	
	/////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

	void Update()
	{
		if(Input.GetKeyDown(KeyCode.Escape)){
			backPressed=true;
			BackButtonPressed();
			backPressed=false;
		}
	}
}
