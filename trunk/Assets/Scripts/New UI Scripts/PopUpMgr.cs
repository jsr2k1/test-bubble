using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class PopUpMgr : MonoBehaviour
{
	Animator anim;
	public bool bShow=false;
	public GameObject buttonNext;
	
	public AudioSource sound_stars_1;
	public AudioSource sound_stars_2;
	public AudioSource sound_stars_3;
	
	public static GameObject currentPopUpObj=null;

	public Text scoreText;
	
	////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

	void Awake()
	{
		anim = GetComponent<Animator>();
	}

	////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

	public void ShowPopUp()
	{
		if(!bShow){
			if(name== "SettingsPopUp" || name=="RankingPopUpArcade"){
				if(InGameScriptRefrences.strikerManager!=null &&!InGameScriptRefrences.strikerManager.bStartDone){
					return;
				}
			}
			if(name=="LosePopUpArcade" || name=="WinPopUp"){
				scoreText.text = LevelManager.score.ToString();
			}
			currentPopUpObj=gameObject;
			StartCoroutine(ChangeState(false));
			if(name!="WinPopUp" && name!="LosePopUp"){
				ImageBlack.popUpActionImageBlack = ImageBlack.PopUpAction.OnShow;
			}
			anim.SetTrigger("ShowPopUp");
			bShow=true;
			
			if(audio && AudioManager.instance.bSoundsOn){
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
				StartCoroutine(ChangeState(true));
				ImageBlack.popUpActionImageBlack = ImageBlack.PopUpAction.OnHide;
			}
			anim.SetTrigger("HidePopUp");
			bShow=false;
		}
	}
	
	////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
	//Nos esperamos un frame antes de cambiar de estado para que no de problemas con el InputScript
	IEnumerator ChangeState(bool bShow)
	{
		yield return new WaitForEndOfFrame();
		
		if(LevelManager.instance!=null){
			if(bShow){
				LevelManager.instance.pauseCtrlForced(GameState.Start);
			}else{
				LevelManager.instance.pauseCtrlForced(GameState.Pause);
			}
		}
	}
	
	////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
	//Esta funcion se ejecuta mediante un evento en la animacion de ShowPopUp para que se oculte automaticamente
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
			if(AudioManager.instance.bMusicOn){
				AudioManager.instance.PlayAudio();
			}
			buttonNext.SetActive(true);
		}
		//Siempre se ejecutara este evento -> guardamos los datos
		LevelManager.instance.SaveGameData();
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
			if(AudioManager.instance.bMusicOn){
				AudioManager.instance.PlayAudio();
			}
			buttonNext.SetActive(true);
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
			if(AudioManager.instance.bMusicOn){
				AudioManager.instance.PlayAudio();
			}
			buttonNext.SetActive(true);
		}
	}
	
	////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
	//Esta funcion salta con un evento en la animacion para volver a reproducir el audio
	//Solo se ejecuta cuando el usuario consigue 3 estrellas (animacion completa)
	public void PlayGameMusic()
	{
		if(name!="WinPopUp" && name!="LosePopUp"){
			return;
		}
		if(AudioManager.instance.bMusicOn){
			AudioManager.instance.PlayAudio();
		}
		if(buttonNext!=null){
			buttonNext.SetActive(true);
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
			if(AudioManager.instance.bSoundsOn){
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
			if(AudioManager.instance.bSoundsOn){
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
			if(AudioManager.instance.bSoundsOn){
				sound_stars_3.Play();
			}
		}
	}

	////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

	public void GoToWorlds()
	{
		PlayerPrefs.SetInt("bPlaying", 0);
		Application.LoadLevel("04 World Menu");
	}
	
	////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

	public void GoToMainMenu()
	{
		Application.LoadLevel("03 Menu");
	}
	
	////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

	public void RetryArcadeLevel()
	{
		Application.LoadLevel("06 Arcade Game Scene");
	}
	
	////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

	public void RetryLevel()
	{
		if(LivesManager.lives>0){
			PlayerPrefs.SetInt("bPlaying", 1);
			Application.LoadLevel("05 Game Scene");
		}else{
			Application.LoadLevel("04 World Menu");
		}
	}
}




