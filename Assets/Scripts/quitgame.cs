using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class quitgame : MonoBehaviour
{
	public Image PopUpquit;

	public void Quit()
	{
		if(Application.loadedLevelName == "03 Menu"){
			Application.Quit();
		}else{
			Application.LoadLevel("03 Menu");
		}
	}
	
	public void Continue()
	{
		PopUpquit.GetComponent<PopUpMgr> ().HidePopUp ();
	}
	

}
