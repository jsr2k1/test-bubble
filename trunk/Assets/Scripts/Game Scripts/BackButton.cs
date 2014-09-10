using UnityEngine;
using System.Collections;

public class BackButton : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	
	void OnMouseDown () 
    {        
        SoundFxManager.instance.buttonClickSound.Play();
        Application.LoadLevel(2);
	
	}
}
