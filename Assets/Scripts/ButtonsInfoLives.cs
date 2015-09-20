using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using GameAnalyticsSDK;

public class ButtonsInfoLives : MonoBehaviour
{
	public Image infoLivesPop;

	////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////v

	public void exitlvl()
	{
		//infoLivesPop.GetComponent<PopUpMgr>().HidePopUp();
		GameAnalytics.NewProgressionEvent (GA_Progression.GAProgressionStatus.GAProgressionStatusFail, LevelManager.levelNo.ToString());
		//PlayerPrefs.SetInt("bPlaying", 0);
		//LivesManager.lives--;
		//ParseManager.instance.SaveCurrentData();
		Application.LoadLevel("04 World Menu");
	}
	
	////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////v

	public void continuelvl()
	{
		infoLivesPop.GetComponent<PopUpMgr>().HidePopUp();
	}
}
