using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class Translate : MonoBehaviour {

	string id;

	// Use this for initialization
	void Start () {

		id = gameObject.GetComponent<Text> ().text;

		if (LanguageManager.dict.ContainsKey (id)) {
			string[] res = LanguageManager.dict [id];
			gameObject.GetComponent<Text> ().text = res [LanguageManager.lang];
		} else {
			Debug.Log ("No se encuentra la key en el diccionario: " + id);
		}
	}

}
