using UnityEngine;
using System.Collections;

public class PlaySoundButton : MonoBehaviour
{
	AudioSource audioSource;
	AudioManager audioManager;
	
	//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
	
	void Awake()
	{
		audioSource = gameObject.AddComponent<AudioSource>();
		audioSource.loop = false;
		audioSource.clip = GameObject.Find("AudioManager").GetComponent<AudioManager>().UIClick;
	}
	
	//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
	
	public void PlayUIClick()
	{
		if(audioSource && PlayerPrefs.GetInt("Sounds")==1){
			audioSource.Play();
		}
	}
}
