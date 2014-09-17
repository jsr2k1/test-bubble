using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class AudioManager : MonoBehaviour
{
	public Toggle musicToggle;
	public Toggle soundsToggle;
	GameObject audioManagerMusic;

	//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

	void Awake()
	{
		audioManagerMusic = GameObject.Find("AudioManagerMusic");

		//Music
		int i = PlayerPrefs.GetInt("Music");
		if(i > 0){
			if(!audioManagerMusic.audio.isPlaying){
				audioManagerMusic.audio.Play();
			}
			musicToggle.isOn = true;
		}else{
			if(audioManagerMusic.audio.isPlaying){
				audioManagerMusic.audio.Stop();
			}
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

	void Update()
	{
		SetToggles();
	}

	//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

	void SetToggles()
	{
		if(musicToggle==null && GameObject.Find("LeftToggle_Music")!=null){
			musicToggle = GameObject.Find("LeftToggle_Music").GetComponent<Toggle>();
		}
		if(soundsToggle==null && GameObject.Find("CenterToggle_Sounds")!=null){
			soundsToggle = GameObject.Find("CenterToggle_Sounds").GetComponent<Toggle>();
		}
	}

	//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

	public void SetMusic(bool b)
	{
		if(b){
			PlayerPrefs.SetInt("Music", 1);
			audioManagerMusic.audio.Play();
		}else{
			PlayerPrefs.SetInt("Music", 0);
			audioManagerMusic.audio.Stop();
		}
	}

	//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

	public void SetSounds(bool b)
	{
		if(b){
			PlayerPrefs.SetInt("Sounds", 1);
		}else{
			PlayerPrefs.SetInt("Sounds", 0);
		}
	}
}



