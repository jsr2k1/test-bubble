using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class PurchaseMoreBalls : MonoBehaviour
{
	public Image shopPop;
	public Image thisPop;
	public Text multiBall;
	public NumberBallsManager ballsManager;
	public SpecialBallButton multiBallButton;
	
	////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

	public void purchaseExtraBalls()
	{
		int coins = PlayerPrefs.GetInt("Coins");

		if(coins >= 160){
			coins = coins - 160;
			PlayerPrefs.SetInt("Coins", coins);

			LevelManager.currentBalls = 5;
			ballsManager.setBallsLeft(5);
			PlayerPrefs.SetInt("Multicolor Ball", PlayerPrefs.GetInt("Multicolor Ball") + 1);
			multiBall.text = PlayerPrefs.GetInt("Multicolor Ball").ToString();

			if(InGameScriptRefrences.strikerManager.currentStrikerObject!=null){
				if(InGameScriptRefrences.strikerManager.currentStrikerObject.GetComponent<PlayingObject>().isConnected==false){
					Destroy(InGameScriptRefrences.strikerManager.currentStrikerObject);
				}
			}
			InGameScriptRefrences.strikerManager.GenerateNextStriker();
			InGameScriptRefrences.strikerManager.GenerateStriker();
			InGameScriptRefrences.strikerManager.FixStrikerPosition();

			thisPop.GetComponent<PopUpMgr>().HidePopUp();

		}else{
			thisPop.GetComponent<PopUpMgr>().HidePopUp();
			shopPop.GetComponent<PopUpMgr>().ShowPopUp();
		}
	}
	
	////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

	public void changePop()
	{
		if(LevelManager.currentBalls == 0){
			shopPop.GetComponent<PopUpMgr>().HidePopUp();
			thisPop.GetComponent<PopUpMgr>().ShowPopUp();
		}
	}
}
