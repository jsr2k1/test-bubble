using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class PopUpMgr : MonoBehaviour
{
	public Image ImgBlack;
	Animator anim;
	public bool bShow=false;

	public enum PopUpAction{
		OnShow,
		OnHide,
		Idle
	}
	public static PopUpAction popUpActionChangeState = PopUpAction.Idle;
	
	public AudioSource sound_stars_1;
	public AudioSource sound_stars_2;
	public AudioSource sound_stars_3;
	
	public static GameObject currentPopUpObj=null;
	
	////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

	void Awake()
	{
		anim = GetComponent<Animator>();
	}

	////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

	public void ShowPopUp()
	{
		if(!bShow){
			currentPopUpObj=gameObject;
			popUpActionChangeState = PopUpAction.OnHide;
			if(name!="WinPopUp" && name!="LosePopUp"){
				ImageBlack.popUpActionImageBlack = PopUpAction.OnShow;
			}
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
			currentPopUpObj=null;
			if(name!="WinCharacterPopUp" && name!="LoseCharacterPopUp"){
				popUpActionChangeState = PopUpAction.OnShow;
				ImageBlack.popUpActionImageBlack = PopUpAction.OnHide;
			}
			anim.SetTrigger("HidePopUp");
			bShow=false;
		}
	}
	
	////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
	//Esta funcion se ejecuta mediante un evento en la animacion de ShowPopUp y solo se tiene en cuenta para el PopUpWinCharacter
	public void HidePopUpWinLoseCharacter()
	{
		if(name=="WinCharacterPopUp" || name=="LoseCharacterPopUp"){
			HidePopUp();
		}
	}
	
	////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
	//Esta funcion se ejecuta mediante un evento en la animacion de HidePopUp y solo se tiene en cuenta para el WinCharacterPopUp
	public void ShowPopUpWin()
	{
		if(name=="WinCharacterPopUp"){
			GameObject.Find("WinPopUp").GetComponent<PopUpMgr>().ShowPopUp();
		}
	}
	
	////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
	//Esta funcion se ejecuta mediante un evento en la animacion de HidePopUp y solo se tiene en cuenta para el LoseCharacterPopUp
	public void ShowPopUpLose()
	{
		if(name=="LoseCharacterPopUp"){
			GameObject.Find("LosePopUp").GetComponent<PopUpMgr>().ShowPopUp();
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
			if(PlayerPrefs.GetInt("Music")==1){
				AudioManager.instance.PlayAudio();
			}
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
			if(PlayerPrefs.GetInt("Music")==1){
				AudioManager.instance.PlayAudio();
			}
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
			if(PlayerPrefs.GetInt("Music")==1){
				AudioManager.instance.PlayAudio();
			}
		}
	}
	
	////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
	//Esta funcion salta con un evento en la animacion para parar la animacion que aparezcan 2 estrellas
	public void PlayGameMusic()
	{
		if(name!="WinPopUp" && name!="LosePopUp"){
			return;
		}
		if(PlayerPrefs.GetInt("Music")==1){
			AudioManager.instance.PlayAudio();
		}
	}
	
	////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
	//Esta funcion salta con un evento en la animacion para ejecutar un audio
	public void PlayStars1()
	{
		if(name!="WinPopUp"){
			return;
		}
		if(LevelManager.score > 0.3 * LevelManager.ReferenceScore){
			if(PlayerPrefs.GetInt("Sounds")==1){
				sound_stars_1.Play();
			}
		}
	}
	
	////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
	//Esta funcion salta con un evento en la animacion para ejecutar un audio
	public void PlayStars2()
	{
		if(name!="WinPopUp"){
			return;
		}
		if(LevelManager.score > 0.6 * LevelManager.ReferenceScore){
			if(PlayerPrefs.GetInt("Sounds")==1){
				sound_stars_2.Play();
			}
		}
	}
	
	////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
	//Esta funcion salta con un evento en la animacion para ejecutar un audio
	public void PlayStars3()
	{
		if(name!="WinPopUp"){
			return;
		}
		if(LevelManager.score > LevelManager.ReferenceScore){
			if(PlayerPrefs.GetInt("Sounds")==1){
				sound_stars_3.Play();
			}
		}
	}

	////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

	public void GoToWorlds()
	{
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
		Application.LoadLevel(4);
	}
	
	////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

	public void RetryLevel()
	{
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
			if(popUpActionChangeState==PopUpAction.OnShow){
				LevelManager.instance.pauseCtrlForced(GameState.Start);
				popUpActionChangeState = PopUpAction.Idle;
			}
			else if(popUpActionChangeState==PopUpAction.OnHide){
				LevelManager.instance.pauseCtrlForced(GameState.Pause);
				popUpActionChangeState = PopUpAction.Idle;
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




