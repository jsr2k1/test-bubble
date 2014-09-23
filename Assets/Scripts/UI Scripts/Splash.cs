using UnityEngine;
using System.Collections;

public class Splash : MonoBehaviour {

	public float timer;

	void Start () {

		StartCoroutine ("DisplayScene");

		if (PlayerPrefs.HasKey("Music")==false) {
			PlayerPrefs.SetInt("Music", 1);
		}
		if (PlayerPrefs.HasKey("Sound")==false) {
			PlayerPrefs.SetFloat("Sound", 1f);
		}
		if (PlayerPrefs.HasKey("Multicolor ball")==false) {
			PlayerPrefs.SetInt("Multicolor ball", 3);
		}
		if (PlayerPrefs.HasKey("Fire ball")==false) {
			PlayerPrefs.SetInt("Fire ball", 3);
		}
		if (PlayerPrefs.HasKey("Bomb ball")==false) {
			PlayerPrefs.SetInt("Bomb ball", 3);
		}
		if (PlayerPrefs.HasKey("Coins")==false) {
			PlayerPrefs.SetInt("Coins", 500);
		}
		if (!PlayerPrefs.HasKey ("Level")){
			PlayerPrefs.SetInt ("Level", 0);
		}
		if (!PlayerPrefs.HasKey ("World")){
			PlayerPrefs.SetInt ("World", 1);
		}
	}

	IEnumerator DisplayScene(){
		yield return new WaitForSeconds(timer);
		Application.LoadLevel(1);
	}


}


