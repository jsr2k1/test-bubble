using UnityEngine;
using System.Collections;

public class ShopButton : MonoBehaviour {

	private float time = 1000f;
	public bool appear = false;
	
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
		if (appear) {
			iTween.MoveTo (parent, iTween.Hash("x", -13));
			appear=false;
			/*PlayerPrefs.SetFloat("VolumeMusic", 1f);
			VolumeMusic = PlayerPrefs.GetFloat ("VolumeMusic");
			MusicSource.volume = VolumeMusic; 
			gameObject.renderer.material.mainTexture = null;*/
		} else {
			iTween.MoveTo (parent, iTween.Hash("x", 0));
			appear=true;
			/*PlayerPrefs.SetFloat("VolumeMusic", 0f);
			VolumeMusic = PlayerPrefs.GetFloat ("VolumeMusic");
			MusicSource.volume = VolumeMusic; */
		}
	}
	
	internal void setAppears(bool a){
		appear = a;
	}
	
	public bool getAppears(){
		return appear;
	}
	
}