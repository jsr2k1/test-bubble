using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class quitgame : MonoBehaviour {
	
	public Image PopUpquit;

	public void Quit(){
		if (Application.loadedLevel == 2) {
			Application.Quit ();
		} else {
			Application.LoadLevel(2);
		}
	}
	
	public void Continue(){
		PopUpquit.GetComponent<PopUpMgr> ().HidePopUp ();
	}
	

}
