using UnityEngine;
using System.Collections;

public class Audio : MonoBehaviour {


	void Awake() {
		DontDestroyOnLoad(transform.gameObject);
	}
}
