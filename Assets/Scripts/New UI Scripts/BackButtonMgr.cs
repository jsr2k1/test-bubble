using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class BackButtonMgr : MonoBehaviour
{
	AdBanner banner;
	public Image quitPop;
	public Image thisPop;
	public Image livesPop;

	public void BackButtonPressed()
	{
		if(PlayerPrefs.GetInt("Sounds")==1)
			audio.Play();
		
		//MENU
		if(Application.loadedLevel==1){
			thisPop.GetComponent<PopUpMgr>().HidePopUp();
			quitPop.GetComponent<PopUpMgr>().ShowPopUp();
		}
		//WORLDS
		else if(Application.loadedLevel==2){
			Application.LoadLevel(1); //Go to Menu Scene
		}
		//GAME-WORLDS
		else if(Application.loadedLevel==3){
			thisPop.GetComponent<PopUpMgr>().HidePopUp();
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
			//MENU
			if(Application.loadedLevel==1){
				quitPop.GetComponent<PopUpMgr>().ShowPopUp();
			}
			//WORLDS
			else if(Application.loadedLevel==2){
				Application.LoadLevel(1); //Go to Menu Scene
			}
			//GAME-WORLDS
			else if(Application.loadedLevel==3){
				livesPop.GetComponent<PopUpMgr>().ShowPopUp();
			}
			//GAME-ARCADE
			else if(Application.loadedLevel==4){
				Application.LoadLevel(1); //Go to Menu Scene
			}
		}
	}
}
