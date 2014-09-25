using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class BallCounterWorldMenu : MonoBehaviour {

	public Text counter;
	public string BallString;

	// Use this for initialization
	void Start () {
		counter.text = PlayerPrefs.GetInt (BallString).ToString ();
	}
	
	// Update is called once per frame
	void Update () {
		counter.text = PlayerPrefs.GetInt (BallString).ToString ();
	}
}
