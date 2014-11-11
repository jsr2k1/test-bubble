using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class NumLives : MonoBehaviour {

	public Image este;
	public Sprite x1;
	public Sprite x2;
	public Sprite x3;
	public Sprite x4;
	public Sprite x5;

	// Use this for initialization
	void Start () {
		if (LivesManager.lives == 0) {
				este.GetComponent<Image> ().sprite = x5;
		} else if (LivesManager.lives == 1) {
				este.GetComponent<Image> ().sprite = x4;
		} else if (LivesManager.lives == 2) {
				este.GetComponent<Image> ().sprite = x3;
		} else if (LivesManager.lives == 3) {
				este.GetComponent<Image> ().sprite = x2;
		} else if (LivesManager.lives == 4) {
				este.GetComponent<Image> ().sprite = x1;
		} else {
				este.GetComponent<Image> ().enabled = false;
		}
	}
	
	// Update is called once per frame
	void Update () {
		if (LivesManager.lives == 0) {
			este.GetComponent<Image> ().sprite = x5;
		} else if (LivesManager.lives == 1) {
			este.GetComponent<Image> ().sprite = x4;
		} else if (LivesManager.lives == 2) {
			este.GetComponent<Image> ().sprite = x3;
		} else if (LivesManager.lives == 3) {
			este.GetComponent<Image> ().sprite = x2;
		} else if (LivesManager.lives == 4) {
			este.GetComponent<Image> ().sprite = x1;
		} else {
			este.GetComponent<Image> ().enabled = false;
		}
	}
}
