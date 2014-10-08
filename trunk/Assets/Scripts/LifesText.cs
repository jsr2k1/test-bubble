using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class LifesText : MonoBehaviour {
	
	public Text lifes;

	// Update is called once per frame
	void Update () {
		lifes.text = "Lifes: " + PlayerPrefs.GetInt ("Lifes");
	}
}
