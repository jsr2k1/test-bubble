using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class ButtonsInfoLives : MonoBehaviour {

	public Image infoLivesPop;

	public void exitlvl(){
		PlayerPrefs.SetInt("bPlaying", 0);
		LivesManager.lives--;
		Application.LoadLevel(2);
	}

	
	public void continuelvl(){
		infoLivesPop.GetComponent<PopUpMgr>().HidePopUp();
	}

}
