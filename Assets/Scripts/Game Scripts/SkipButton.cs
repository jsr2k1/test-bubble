using UnityEngine;
using System.Collections;

public class SkipButton : MonoBehaviour 
{

	// Use this for initialization
	void Start () {
	
	}
	
	
	void OnMouseDown () 
    {
        Striker.instance.Skip();
	
	}
}
