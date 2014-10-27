using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class AudioManagerMusic : MonoBehaviour
{
	public AudioClip[] audioClips;

	//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

	public void PlayAudio()
	{
		if(Application.loadedLevel!=3){
			audio.clip = audioClips[0];
		}
		else if(LevelManager.levelNo%2==0){
			audio.clip = audioClips[1];
		}else{
			audio.clip = audioClips[2];
		}
		audio.Play();
	}
	
	//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
	
	public void StopAudio()
	{
		audio.Stop();
	}
}



