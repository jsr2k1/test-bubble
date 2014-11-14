using UnityEngine;
using System.Collections;

public class LoadLevelFromPop : MonoBehaviour
{
	public PopUpMgr popUpMgr;
	
	public void loadlevel()
	{
		popUpMgr.HidePopUp();
		Application.LoadLevel(3);
	}
}
