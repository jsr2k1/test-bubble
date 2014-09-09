using UnityEngine;
using System.Collections;

public class ClosePopupSprite : MonoBehaviour {
	
	public Texture texture;
	public Texture overtexture;
	
	private Vector3 initialScreenPos;
	
	public SettingsBtn cubeScript;

	public Sprite oversprite;
	public Sprite sprite;
	
	public GameObject parent;
	
	void OnMouseEnter(){
		// textura over
		gameObject.GetComponent<SpriteRenderer> ().sprite = oversprite;
	}
	
	void OnMouseDown(){
		initialScreenPos = new Vector3 (Input.mousePosition.x, Input.mousePosition.y, Input.mousePosition.z);
	}
	
	void OnMouseExit(){
		// textura normal
		gameObject.GetComponent<SpriteRenderer> ().sprite = sprite;
		
	}
	
	
	void OnMouseUp() 
	{        
		Vector3 finalScreenPos = new Vector3 (Input.mousePosition.x, Input.mousePosition.y, Input.mousePosition.z);
		
		if (initialScreenPos == finalScreenPos){
			//Button action
			iTween.MoveTo (parent, iTween.Hash ("y", 20));
			cubeScript.setAppears(false);
		}
	}
}
