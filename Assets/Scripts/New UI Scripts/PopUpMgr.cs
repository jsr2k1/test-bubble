using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class PopUpMgr : MonoBehaviour
{
	Animator anim;
	bool bExitPause=false;
	bool bShow=false;
	public Image ImgBlack;

	////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

	void Awake()
	{
		anim = GetComponent<Animator>();
	}

	////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

	public void ShowPopUp()
	{
		if(!bShow){
			anim.SetTrigger("ShowPopUp");
			ImgBlack.GetComponent<Animator>().SetTrigger("ShowPopUp");
			bShow=true;
			if(LevelManager.instance!=null){
				LevelManager.instance.pauseCtrlForced(GameState.Pause);
			}
		}
	}

	////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

	public void HidePopUp(bool bChangeState=true)
	{
		if(bShow){
			anim.SetTrigger("HidePopUp");
			ImgBlack.GetComponent<Animator>().SetTrigger("HidePopUp");
			bShow=false;
			if(bChangeState){
				bExitPause=true;
			}
		}
	}

	////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

	public void GoToWorlds()
	{
		Application.LoadLevel(2);
	}

	public void GoToMainMenu()
	{
		Application.LoadLevel(1);
	}

	public void RetryArcadeLevel()
	{
		Application.LoadLevel(4);
	}

	public void RetryLevel()
	{
		Application.LoadLevel(3);
	}

	////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

	void Update()
	{
		//Nos esperamos un frame antes de poner el juego en playmode para que no de problemas con el Update del InputScript
		if(bExitPause){
			if(LevelManager.instance!=null){
				LevelManager.instance.pauseCtrlForced(GameState.Start);
			}
			bExitPause=false;
		}
	}
}
