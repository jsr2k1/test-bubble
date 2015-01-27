using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class AudioManager : MonoBehaviour
{
	public static AudioManager instance;
	
	public AudioClip[] audioClips;
	public AudioClip UIClick;
	public AudioClip UIClickBooster;
	
	Toggle musicToggle;
	Toggle soundsToggle;
	string oldLevel="";
	int lastArcadeClip=0;
	
	public AudioSource shootingSound;
	public AudioSource hookSound;
	public AudioSource wallCollisionSound;
	public AudioSource burstSound;
	
	public bool bMusicOn;
	public bool bSoundsOn;

	//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

	void Awake()
	{
		DontDestroyOnLoad(gameObject);
		instance = this;
	}
	
	//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
	
	void Start()
	{
		if(PlayerPrefs.GetInt("Music")==1){
			bMusicOn=true;
		}
		if(PlayerPrefs.GetInt("Sounds")==1){
			bSoundsOn=true;
		}
	}
	
	//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
	
	void OnLevelWasLoaded(int level)
	{
		if(level>2){
			GetToggleComponents();
			SetToggleValues();
			SetAudio(Application.loadedLevelName);
			PlayAudio(Application.loadedLevelName);
			//oldLevel=level;
			oldLevel=Application.loadedLevelName;
		}
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
		musicToggle.isOn = AudioManager.instance.bMusicOn;
		soundsToggle.isOn = AudioManager.instance.bSoundsOn;
	}
	
	//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
	//Establece para cada escena el clip de audio que le corresponde
	void SetAudio(string level)
	{
		//En la pantalla principal reproducimos el clip 0
		if(level=="03 Menu" && (oldLevel=="02 Splash Managers" || oldLevel=="06 Arcade Game Scene")){
			audio.clip = audioClips[0];
		}
		//En la pantalla de los mundos continuamos reproduciendo el clip 0
		else if(level=="04 World Menu" && oldLevel=="05 Game Scene"){
			audio.clip = audioClips[0];
		}
		//En el modo MUNDOS reproducimos el clip 1 en los mundos pares y el clip 2 en los impares
		else if(level=="05 Game Scene"){
			if(LevelManager.levelNo%2==0){
				audio.clip = audioClips[1];
			}else{
				audio.clip = audioClips[2];
			}
		}//En el modo ARCADE reproducimos el clip 1	y el clip 2 alternados
		else if(level=="06 Arcade Game Scene"){
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
		PlayAudio(Application.loadedLevelName);
	}
	
	//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
	
	void PlayAudio(string level)
	{
		if(AudioManager.instance.bMusicOn){
			if((level=="03 Menu" && oldLevel=="04 World Menu") || (level=="04 World Menu" && oldLevel=="03 Menu")){
				//No hacemos nada
			}else{
				audio.Play();
			}
		}
	}
	
	//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
	
	public void PlayFxSound(AudioSource audiosource)
	{
		if(AudioManager.instance.bSoundsOn && !audiosource.isPlaying){
			audiosource.Play();
		}
	}
	
	//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
	
	public void StopAudio()
	{
		audio.Stop();
	}
}



