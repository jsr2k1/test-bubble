using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using GameAnalyticsSDK;

public class quitgame : MonoBehaviour
{
	public Image PopUpquit;

	public void Quit()
	{
		if(Application.loadedLevelName == "03 Menu"){
			Application.Quit();
		}else{
			GameAnalytics.NewProgressionEvent (GA_Progression.GAProgressionStatus.GAProgressionStatusFail, "Arcade");
			Application.LoadLevel("03 Menu");
		}
	}
	
	public void Continue()
	{
		PopUpquit.GetComponent<PopUpMgr> ().HidePopUp ();
	}
	

}
