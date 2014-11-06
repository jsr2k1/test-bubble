using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class BackButtonMgr : MonoBehaviour
{
	AdBanner banner;
	public Image quitPop;
	public Image thisPop;
	public Image livesPop;

	public void BackButtonPressed()
	{
		if(PlayerPrefs.GetInt("Sounds")==1){
			audio.Play();
		}
		if(Application.loadedLevel==3)//Worlds game scene
		{
			thisPop.GetComponent<PopUpMgr>().HidePopUp(false);
			livesPop.GetComponent<PopUpMgr>().ShowPopUp();
		}
		else if(Application.loadedLevel==4)//Arcade
		{
			Application.LoadLevel(1); //Go to Menu Scene
		}
		else if(Application.loadedLevel==2)//World Scene
		{
			Application.LoadLevel(1); //Go to Menu Scene
		}
		else{
			thisPop.GetComponent<PopUpMgr>().HidePopUp();
			quitPop.GetComponent<PopUpMgr>().ShowPopUp();

		}
	}
	
	/////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

	void Update()
	{
		if(Input.GetKeyDown(KeyCode.Escape)){
			BackButtonPressed();
		}
	}
}
