using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class quitgame : MonoBehaviour {

	public Image PopUpquit;

	public void Quit(){
		Application.Quit ();
	}

	public void Continue(){
		PopUpquit.GetComponent<PopUpMgr> ().HidePopUp ();
	}
}
