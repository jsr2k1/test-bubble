using UnityEngine;
using System.Collections;

public class SpecialBallCount : MonoBehaviour {

	public TextMesh counter;
	public string BallString;
	
	void Start () {
		counter.text = PlayerPrefs.GetInt (BallString).ToString ();
	}

	void Update () {
		counter.text = PlayerPrefs.GetInt (BallString).ToString ();
	}

}
