using UnityEngine;
using System.Collections;

public class BackButton : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	
	void OnMouseDown () 
    {        
        //SoundFxManager.instance.buttonClickSound.Play();
		SoundFxManager.instance.Play(SoundFxManager.instance.buttonClickSound);
        Application.LoadLevel(2);
	
	}
}
