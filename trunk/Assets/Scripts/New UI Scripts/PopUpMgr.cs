using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class PopUpMgr : MonoBehaviour
{
	public Image ImgBlack;
	Animator anim;
	bool bExitPause=false;
	bool bShow=false;


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
			if(audio && PlayerPrefs.GetInt("Sounds")==1){
				audio.Play();
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
			if(PlayerPrefs.GetInt("Sounds")==1){
				audio.Play();
			}
		}

	}

	////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

	public void GoToWorlds()
	{
		if(PlayerPrefs.GetInt("Sounds")==1){
			audio.Play();
		}
		PlayerPrefs.SetInt("bPlaying", 0);
		Application.LoadLevel(2);
	}
	
	////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

	public void GoToMainMenu()
	{
		Application.LoadLevel(1);
	}
	
	////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

	public void RetryArcadeLevel()
	{
		if(PlayerPrefs.GetInt("Sounds")==1){
			audio.Play();
		}
		Application.LoadLevel(4);
	}
	
	////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

	public void RetryLevel()
	{
		if(PlayerPrefs.GetInt("Sounds")==1){
			audio.Play();
		}
		if(LivesManager.lives>0){
			PlayerPrefs.SetInt("bPlaying", 1);
			Application.LoadLevel(3);
		}else{
			Application.LoadLevel(2);
		}
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
