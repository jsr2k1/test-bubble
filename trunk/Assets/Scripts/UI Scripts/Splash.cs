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
		if (PlayerPrefs.HasKey("Multicolor Ball")==false) {
			PlayerPrefs.SetInt("Multicolor Ball", 3);
		}
		if (PlayerPrefs.HasKey("Fire Ball")==false) {
			PlayerPrefs.SetInt("Fire Ball", 3);
		}
		if (PlayerPrefs.HasKey("Bomb Ball")==false) {
			PlayerPrefs.SetInt("Bomb Ball", 3);
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
		if (!PlayerPrefs.HasKey ("Lifes")){
			PlayerPrefs.SetInt ("Lifes", 5);
		}

		PlayerPrefs.SetInt ("Coins", 0);
	}

	IEnumerator DisplayScene(){
		yield return new WaitForSeconds(timer);
		Application.LoadLevel(1);
	}


}


