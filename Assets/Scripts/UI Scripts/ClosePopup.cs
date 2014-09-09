using UnityEngine;
using System.Collections;

public class ClosePopup : MonoBehaviour {
	
	public Texture texture;
	public Texture overtexture;
	
	private Vector3 initialScreenPos;

	public SettingsBtn Script;

	public GameObject parent;
	
	void OnMouseEnter(){
		// textura over
		renderer.material.mainTexture = overtexture;
	}
	
	void OnMouseDown(){
		initialScreenPos = new Vector3 (Input.mousePosition.x, Input.mousePosition.y, Input.mousePosition.z);
	}
	
	void OnMouseExit(){
		// textura normal
		renderer.material.mainTexture = texture;
		
	}
	
	
	void OnMouseUp() 
	{        
		Vector3 finalScreenPos = new Vector3 (Input.mousePosition.x, Input.mousePosition.y, Input.mousePosition.z);

		//print (Mathf.Sqrt((Mathf.Pow(initialScreenPos.x-finalScreenPos.x), 2f) + (Mathf.Pow(initialScreenPos.y-finalScreenPos.y), 2f)));

		float d = Mathf.Sqrt (Mathf.Pow((initialScreenPos.x-finalScreenPos.x), 2f) + Mathf.Pow((initialScreenPos.y-finalScreenPos.y), 2f));


		if ( d < 10f){
			//Button action
			iTween.MoveTo (parent, iTween.Hash ("y", 20));
			Script.setAppears(false);
	}
}
}

