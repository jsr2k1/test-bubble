using UnityEngine;
using System.Collections;

public class Purchase: MonoBehaviour {

	public Sprite sprite;
	public Sprite oversprite;
	private Vector3 initialScreenPos;
	public TextMesh CoinsText;
	public int coins;
	
	void Start(){
		CoinsText.text = "Coins: " + PlayerPrefs.GetInt ("Coins").ToString ();
	}
	
	void OnMouseEnter ()
	{
		gameObject.GetComponent<SpriteRenderer> ().sprite = oversprite;
		//this.renderer.material.mainTexture = texture;
	}
	
	void OnMouseDown ()
	{
		initialScreenPos = new Vector3 (Input.mousePosition.x, Input.mousePosition.y, Input.mousePosition.z);
	}
	
	void OnMouseExit ()
	{
		gameObject.GetComponent<SpriteRenderer> ().sprite = sprite;
	}
	
	void OnMouseUp ()
	{        
		Vector3 finalScreenPos = new Vector3 (Input.mousePosition.x, Input.mousePosition.y, Input.mousePosition.z);
		
		float d = Mathf.Sqrt (Mathf.Pow ((initialScreenPos.x - finalScreenPos.x), 2f) + Mathf.Pow ((initialScreenPos.y - finalScreenPos.y), 2f));
		if (d < 10f){
			//Button action

			PlayerPrefs.SetInt ("Coins", PlayerPrefs.GetInt ("Coins") + coins);
			
			CoinsText.text = "Coins: " + PlayerPrefs.GetInt ("Coins").ToString ();
			
			gameObject.GetComponent<SpriteRenderer> ().sprite = sprite;
		}
	}
}


