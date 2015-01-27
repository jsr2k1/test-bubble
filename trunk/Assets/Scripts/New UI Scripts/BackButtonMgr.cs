using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class BackButtonMgr : MonoBehaviour
{
	public GameObject goQuit;
	public GameObject goLives;
	public GameObject goSettings;
	public GameObject goQuitArcade;
	
	PopUpMgr quitPopUp;
	PopUpMgr livesPopUp;
	PopUpMgr settingsPopUp;
	PopUpMgr quitArcadePopUp;
	
	/////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
	
	void Awake()
	{
		if(goQuit!=null){
			quitPopUp = goQuit.GetComponent<PopUpMgr>();
		}
		if(goLives!=null){
			livesPopUp = goLives.GetComponent<PopUpMgr>();
		}
		if(goSettings!=null){
			settingsPopUp = goSettings.GetComponent<PopUpMgr>();
		}
		if(goQuitArcade!=null){
			quitArcadePopUp = goQuitArcade.GetComponent<PopUpMgr>();
		}
	}
	
	/////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

	public void BackButtonPressed()
	{
		//MENU
		if(Application.loadedLevel==2){
			settingsPopUp.HidePopUp();
			quitPopUp.ShowPopUp();
		}
		//WORLDS
		else if(Application.loadedLevel==3){
			Application.LoadLevel(2); //Go to Menu Scene
		}
		//GAME-WORLDS
		else if(Application.loadedLevel==4){
			settingsPopUp.HidePopUp();
			livesPopUp.ShowPopUp();
		}
		//GAME-ARCADE
		else if(Application.loadedLevel==5){
			settingsPopUp.HidePopUp();
			quitArcadePopUp.ShowPopUp();
		}
	}
	
	/////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

	void Update()
	{
		if(Input.GetKeyDown(KeyCode.Escape))
		{
			if(settingsPopUp.bShow){
				settingsPopUp.HidePopUp();
			}else{
				//MENU
				if(Application.loadedLevel==2){
					quitPopUp.ShowPopUp();
				}
				//WORLDS
				else if(Application.loadedLevel==3){
					if(PopUpMgr.currentPopUpObj!=null){
						PopUpMgr.currentPopUpObj.GetComponent<PopUpMgr>().HidePopUp();
					}
					else{
						Application.LoadLevel(2); //Go to Menu Scene
					}
				}
				//GAME-WORLDS
				else if(Application.loadedLevel==4){
					if(LevelManager.instance.totalNumberOfRowsLeft > 0 || !InGameScriptRefrences.strikerManager.bStartDone){
						return;
					}
					if(PopUpMgr.currentPopUpObj!=null){
						string s = PopUpMgr.currentPopUpObj.name;
						if(s!="MoreBubblesPopup" && s!="WinPopUp" && s!="LosePopUp"){
							PopUpMgr.currentPopUpObj.GetComponent<PopUpMgr>().HidePopUp();
						}
					}else{
						livesPopUp.ShowPopUp();
					}
				}
				//GAME-ARCADE
				else if(Application.loadedLevel==5){
					quitArcadePopUp.ShowPopUp();
				}
			}
		}
	}
}
