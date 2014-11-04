using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class HighscoreStart : MonoBehaviour {

	public Text highscore;

	// Use this for initialization
	void Start () {
		highscore.text = PlayerPrefs.GetInt ("Highscore").ToString ();
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
