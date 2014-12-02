using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class AudioManager : MonoBehaviour
{
	public AudioClip[] audioClips;
	public AudioClip UIClick;
	
	Toggle musicToggle;
	Toggle soundsToggle;
	int oldLevel=0;
	int lastArcadeClip=0;

	//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

	void Awake()
	{
		DontDestroyOnLoad(gameObject);
	}
	
	//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
	
	void OnLevelWasLoaded(int level)
	{
		GetToggleComponents();
		SetToggleValues();
		SetAudio(level);
		PlayAudio(level);
		oldLevel=level;
	}
	
	//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
	
	void GetToggleComponents()
	{
		musicToggle = GameObject.Find("LeftToggle_Music").GetComponent<Toggle>();
		soundsToggle = GameObject.Find("CenterToggle_Sounds").GetComponent<Toggle>();
	}

	//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

	void SetToggleValues()
	{
		//Music
		int i = PlayerPrefs.GetInt("Music");
		if(i > 0){
			musicToggle.isOn = true;
		}else{
			musicToggle.isOn = false;
		}
		//Sounds
		int j = PlayerPrefs.GetInt("Sounds");
		if(j > 0){
			soundsToggle.isOn = true;
		}else{
			soundsToggle.isOn = false;
		}
	}
	
	//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
	//Establece para cada escena el clip de audio que le corresponde
	void SetAudio(int level)
	{
		//En la pantalla principal reproducimos el clip 0
		if(level==1 && (oldLevel==0 || oldLevel==4)){
			audio.clip = audioClips[0];
		}
		//En la pantalla de los mundos continuamos reproduciendo el clip 0
		else if(level==2 && oldLevel==3){
			audio.clip = audioClips[0];
		}
		//En el modo MUNDOS reproducimos el clip 1 en los mundos pares y el clip 2 en los impares
		else if(level==3){
			if(LevelManager.levelNo%2==0){
				audio.clip = audioClips[1];
			}else{
				audio.clip = audioClips[2];
			}
		}//En el modo ARCADE reproducimos el clip 1	y el clip 2 alternados
		else if(level==4){
			if(lastArcadeClip==0){
				audio.clip = audioClips[1];
			}else{
				audio.clip = audioClips[2];
			}
			lastArcadeClip = (lastArcadeClip+1)%2;
		}
	}
	
	//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
	
	public void PlayAudio()
	{
		PlayAudio(Application.loadedLevel);
	}
	
	//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
	
	void PlayAudio(int level)
	{
		int i = PlayerPrefs.GetInt("Music");
		if(i > 0){
			if((level==2 && oldLevel==1) || level==1 && oldLevel==2){
				//No hacemos nada
			}else{
				audio.Play();
			}
		}
	}
	
	//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
	
	public void StopAudio()
	{
		audio.Stop();
	}
}



