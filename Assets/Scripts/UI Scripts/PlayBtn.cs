using UnityEngine;
using System.Collections;

public class PlayBtn : MonoBehaviour {

	public Texture texture;
	public Texture overtexture;

	private Vector3 initialScreenPos;

	void OnMouseEnter(){

		renderer.material.mainTexture = overtexture;
		//this.renderer.material.mainTexture = texture;
	}

	void OnMouseDown(){
		initialScreenPos = new Vector3 (Input.mousePosition.x, Input.mousePosition.y, Input.mousePosition.z);
	}

	void OnMouseExit(){
		
		renderer.material.mainTexture = texture;

	}


	void OnMouseUp() 
	{        
		Vector3 finalScreenPos = new Vector3 (Input.mousePosition.x, Input.mousePosition.y, Input.mousePosition.z);

		float d = Mathf.Sqrt (Mathf.Pow((initialScreenPos.x-finalScreenPos.x), 2f) + Mathf.Pow((initialScreenPos.y-finalScreenPos.y), 2f));
		if (d < 10f)
			//Button action

		Application.LoadLevel(2);
	}
}
