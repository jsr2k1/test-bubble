using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class ButtonsInfoLives : MonoBehaviour
{
	public Image infoLivesPop;

	////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////v

	public void exitlvl()
	{
		infoLivesPop.GetComponent<PopUpMgr>().HidePopUp();
		PlayerPrefs.SetInt("bPlaying", 0);
		LivesManager.lives--;
		ParseManager.instance.SaveCurrentData();
		Application.LoadLevel(2);
	}
	
	////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////v

	public void continuelvl()
	{
		infoLivesPop.GetComponent<PopUpMgr>().HidePopUp();
	}
}
