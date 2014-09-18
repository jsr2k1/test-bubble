using UnityEngine;
using System.Collections;

public class BackButtonMgr : MonoBehaviour
{
	public void BackButtonPressed()
	{
		if(Application.loadedLevel>1){
			Application.LoadLevel(Application.loadedLevel-1);
		}else{
			Application.Quit();
		}
	}
}
