using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class BackButtonMgr : MonoBehaviour
{
	public GameObject goQuit;
	public GameObject goLives;
	public GameObject goSettings;
	public GameObject goShopLives;
	public GameObject goShopCoins;
	public GameObject goWin;
	public GameObject goLose;
	public GameObject goInfoLevel;
	public GameObject goQuitArcade;
	
	PopUpMgr quitPopUp;
	PopUpMgr livesPopUp;
	PopUpMgr settingsPopUp;
	PopUpMgr shopLivesPopUp;
	PopUpMgr shopCoinsPopUp;
	PopUpMgr winPopUp;
	PopUpMgr losePopUp;
	PopUpMgr infoLevelPopUp;
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
		if(goShopLives!=null){
			shopLivesPopUp = goShopLives.GetComponent<PopUpMgr>();
		}
		if(goShopCoins!=null){
			shopCoinsPopUp = goShopCoins.GetComponent<PopUpMgr>();
		}
		if(goWin!=null){
			winPopUp = goWin.GetComponent<PopUpMgr>();
		}
		if(goLose!=null){
			losePopUp = goLose.GetComponent<PopUpMgr>();
		}
		if(goInfoLevel!=null){
			infoLevelPopUp = goInfoLevel.GetComponent<PopUpMgr>();
		}
		if(goQuitArcade!=null){
			quitArcadePopUp = goQuitArcade.GetComponent<PopUpMgr>();
		}		
	}
	
	/////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

	public void BackButtonPressed()
	{
		//MENU
		if(Application.loadedLevel==1){
			settingsPopUp.HidePopUp();
			quitPopUp.ShowPopUp();
		}
		//WORLDS
		else if(Application.loadedLevel==2){
			Application.LoadLevel(1); //Go to Menu Scene
		}
		//GAME-WORLDS
		else if(Application.loadedLevel==3){
			settingsPopUp.HidePopUp();
			livesPopUp.ShowPopUp();
		}
		//GAME-ARCADE
		else if(Application.loadedLevel==4){
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
				if(Application.loadedLevel==1){
					quitPopUp.ShowPopUp();
				}
				//WORLDS
				else if(Application.loadedLevel==2){
					if(shopLivesPopUp.bShow){
						shopLivesPopUp.HidePopUp();
					}
					else if(shopCoinsPopUp.bShow){
						shopCoinsPopUp.HidePopUp();
					}
					else if(infoLevelPopUp.bShow){
						infoLevelPopUp.HidePopUp();
					}
					else{
						Application.LoadLevel(1); //Go to Menu Scene
					}
				}
				//GAME-WORLDS
				else if(Application.loadedLevel==3){
					if(!winPopUp.bShow && !losePopUp.bShow){
						livesPopUp.ShowPopUp();
					}
				}
				//GAME-ARCADE
				else if(Application.loadedLevel==4){
					quitArcadePopUp.ShowPopUp();
				}
			}
		}
	}
}
