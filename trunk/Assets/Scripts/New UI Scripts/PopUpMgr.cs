using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class PopUpMgr : MonoBehaviour
{
	public Image ImgBlack;
	Animator anim;
	public bool bShow=false;

	public enum PopUpAction{
		On,
		Off,
		Skip
	}
	static PopUpAction popUpActionChangeState = PopUpAction.Skip;
	
	public AudioSource sound_stars_1;
	public AudioSource sound_stars_2;
	public AudioSource sound_stars_3;
		
	////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

	void Awake()
	{
		anim = GetComponent<Animator>();
	}

	////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

	public void ShowPopUp()
	{
		if(!bShow){
			popUpActionChangeState = PopUpAction.Off;
			ImageBlack.popUpActionImageBlack = PopUpAction.On;
			anim.SetTrigger("ShowPopUp");
			bShow=true;
			
			if(audio && PlayerPrefs.GetInt("Sounds")==1){
				audio.Play();
			}
		}
	}

	////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

	public void HidePopUp()
	{
		if(bShow){
			popUpActionChangeState = PopUpAction.On;
			ImageBlack.popUpActionImageBlack = PopUpAction.Off;
			anim.SetTrigger("HidePopUp");
			bShow=false;
			
			if(PlayerPrefs.GetInt("Sounds")==1){
				audio.Play();
			}
		}
	}
	
	////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
	//Esta funcion salta con un evento en la animacion para parar la animacion que aparezcan 0 estrellas
	public void SetNumStars0()
	{
		if(name!="WinPopUp"){
			return;
		}
		if(LevelManager.score < 0.3 * LevelManager.ReferenceScore){
			anim.speed=0;
		}
	}
	
	////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
	//Esta funcion salta con un evento en la animacion para parar la animacion que aparezca 1 estrella
	public void SetNumStars1()
	{
		if(name!="WinPopUp"){
			return;
		}
		if(LevelManager.score < 0.6 * LevelManager.ReferenceScore){
			anim.speed=0;
		}
	}
	
	////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
	//Esta funcion salta con un evento en la animacion para parar la animacion que aparezcan 2 estrellas
	public void SetNumStars2()
	{
		if(name!="WinPopUp"){
			return;
		}
		if(LevelManager.score < LevelManager.ReferenceScore){
			anim.speed=0;
		}
	}
	
	////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
	//Esta funcion salta con un evento en la animacion para ejecutar un audio
	public void PlayStars1()
	{
		if(name!="WinPopUp"){
			return;
		}
		sound_stars_1.Play();
	}
	
	////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
	//Esta funcion salta con un evento en la animacion para ejecutar un audio
	public void PlayStars2()
	{
		if(name!="WinPopUp"){
			return;
		}
		sound_stars_2.Play();
	}
	
	////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
	//Esta funcion salta con un evento en la animacion para ejecutar un audio
	public void PlayStars3()
	{
		if(name!="WinPopUp"){
			return;
		}
		sound_stars_3.Play();
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
	//Nos esperamos un frame antes de poner el juego en playmode para que no de problemas con el Update del InputScript
	void Update()
	{
		//Play mode
		if(LevelManager.instance!=null){
			if(popUpActionChangeState==PopUpAction.On){
				LevelManager.instance.pauseCtrlForced(GameState.Start);
				popUpActionChangeState = PopUpAction.Skip;
			}
			else if(popUpActionChangeState==PopUpAction.Off){
				LevelManager.instance.pauseCtrlForced(GameState.Pause);
				popUpActionChangeState = PopUpAction.Skip;
			}
		}
		/*
		//Imagen negra de fondo
		if(popUpActionImageBlack==PopUpAction.On && ImgBlack!=null){
			ImgBlack.GetComponent<Animator>().SetTrigger("ShowPopUp");
			popUpActionImageBlack=PopUpAction.Skip;
		}
		else if(popUpActionImageBlack==PopUpAction.Off && ImgBlack!=null){
			ImgBlack.GetComponent<Animator>().SetTrigger("HidePopUp");
			popUpActionImageBlack=PopUpAction.Skip;
		}*/
	}
}




