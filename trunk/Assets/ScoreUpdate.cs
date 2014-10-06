using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class ScoreUpdate : MonoBehaviour {

	public Text finalscore;

	// Update is called once per frame
	void Update () {
		finalscore.text = GameObject.Find("Score Text").GetComponent<TextMesh>().text;
	}
}
