using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class ButtonsInfoLives : MonoBehaviour
{
	public Image infoLivesPop;
	
	//Creamos un evento para saber el momento en el que se ha abandonado un nivel para guardarlo en el Parse
	public delegate void ExitLevel();
	public static event ExitLevel OnExitLevel;

	////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////v

	public void exitlvl()
	{
		infoLivesPop.GetComponent<PopUpMgr>().HidePopUp();
		PlayerPrefs.SetInt("bPlaying", 0);
		LivesManager.lives--;
		if(OnExitLevel!=null){
			OnExitLevel();
		}
		Application.LoadLevel(2);
	}
	
	////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////v

	public void continuelvl()
	{
		infoLivesPop.GetComponent<PopUpMgr>().HidePopUp();
	}
}
