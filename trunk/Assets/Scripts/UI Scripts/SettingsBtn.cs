using UnityEngine;
using System.Collections;

public class SettingsBtn : MonoBehaviour {
	private float time = 1000f;
	public bool appears = false;

	public GameObject parent;

	float VolumeMusic;
	//public AudioSource MusicSource;

	void start() {
			VolumeMusic = PlayerPrefs.GetFloat ("VolumeMusic");
			//MusicSource.volume = VolumeMusic; 
	}

	// Use this for initialization
	void OnMouseUp () {

		//iTween.MoveBy(gameObject, iTween.Hash("x", 2, "easeType", "easeInOutExpo", "loopType", "pingPong", "delay", .1));
		if (appears) {
			iTween.MoveTo (parent, iTween.Hash("y", 20, "time", 1));
			appears=false;
			/*PlayerPrefs.SetFloat("VolumeMusic", 1f);
			VolumeMusic = PlayerPrefs.GetFloat ("VolumeMusic");
			MusicSource.volume = VolumeMusic; 
			gameObject.renderer.material.mainTexture = null;*/
		} else {
			iTween.MoveTo (parent, iTween.Hash("y", 0, "time", 1));
			appears=true;
			/*PlayerPrefs.SetFloat("VolumeMusic", 0f);
			VolumeMusic = PlayerPrefs.GetFloat ("VolumeMusic");
			MusicSource.volume = VolumeMusic; */
		}
	}

	internal void setAppears(bool a){
		appears = a;
	}

	public bool getAppears(){
		return appears;
	}

}
