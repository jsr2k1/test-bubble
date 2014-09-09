using UnityEngine;
using System.Collections;

public class Flip : MonoBehaviour {


	void Awake()
	{
		//The scene of the template its setted on the 0 0 0 but somehow its fliped when you want to work with the 2d view
		//of unity so to solve this we make a superParent object fliped 180º on the y axis but then there are some effects and things
		//That doesnt work so we are gonna do this script that on awake turns the parent object to the 0 y axis
		this.transform.eulerAngles = new Vector3(0,0,0);
	}

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
