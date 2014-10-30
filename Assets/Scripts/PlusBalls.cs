using UnityEngine;
using System.Collections;

public class PlusBalls : MonoBehaviour
{
	public string BallString;
	public GameObject text;
	public GameObject button;

	// Use this for initialization
	void Start ()
	{
		if (PlayerPrefs.GetInt (BallString) > 0) {
			button.SetActive (false);
			text.SetActive (true);
		} else {
			button.SetActive (true);
			text.SetActive (false);
		}	
	}
	
	// Update is called once per frame
	void Update ()
	{
		if (PlayerPrefs.GetInt (BallString) > 0) {
			button.SetActive (false);
			text.SetActive (true);
		} else {
			button.SetActive (true);
			text.SetActive (false);
		}
	}
}
