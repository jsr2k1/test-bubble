using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using UnityEngine.EventSystems;

public class ToggleController : MonoBehaviour, IPointerClickHandler
{
	////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
	//Añadimos esta funcion para que haga click al pulsar sobre uno de los toggles.
	//Para poder llamar a esta funcion hemos tenido que hacer que la classe 'ToggleController' implemente la interfaz 'IPointerClickHandler'.
	//No podemos hacer el Play del click en las funciones SetMusic y SetSounds porque entonces tambien sonaria cuando establecemos el valor
	//de los toggles segun la informacion de PlayerPrefs desde el AudioManager
	public void OnPointerClick(PointerEventData data)
	{
		int i = PlayerPrefs.GetInt("Sounds");
		if(i>0){
			audio.Play();
		}
	}
	
	////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
	
	public void SetMusic(bool b)
	{
		if(AudioManager.instance==null){
			return;
		}
		if(b){
			PlayerPrefs.SetInt("Music", 1);
			AudioManager.instance.PlayAudio();
		}else{
			PlayerPrefs.SetInt("Music", 0);
			AudioManager.instance.StopAudio();
		}
	}
	
	////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
	
	public void SetSounds(bool b)
	{
		if(b){
			PlayerPrefs.SetInt("Sounds", 1);
		}else{
			PlayerPrefs.SetInt("Sounds", 0);
		}
	}
}
