using UnityEngine;
using System.Collections;

public class PlaySoundButton : MonoBehaviour
{
	AudioSource audioSource;
	
	//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
	
	void Awake()
	{
		audioSource = gameObject.AddComponent<AudioSource>();
		audioSource.loop = false;
		audioSource.clip = AudioManager.instance.UIClick;
		audioSource.playOnAwake = false;
	}
	
	//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
	
	public void PlayUIClick()
	{
		if(audioSource && PlayerPrefs.GetInt("Sounds")==1){
			audioSource.Play();
		}
	}
}
