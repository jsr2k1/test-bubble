using UnityEngine;
using System.Collections;

public class BuySpecialBall : MonoBehaviour {
	
	public Sprite sprite;
	public Sprite oversprite;
	private Vector3 initialScreenPos;
	public TextMesh NumberBallText;
	public TextMesh CoinsText;
	public string BallString;
	private int quantity;
	private int coins;

	void Start(){
		NumberBallText.text = PlayerPrefs.GetInt (BallString).ToString ();
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
		if (d < 10f)
			//Button action
				if (PlayerPrefs.GetInt ("Coins") >= 10) {

						quantity = PlayerPrefs.GetInt (BallString) + 1;
						PlayerPrefs.SetInt (BallString, quantity);
						
						coins = PlayerPrefs.GetInt ("Coins") - 10;
						PlayerPrefs.SetInt ("Coins", coins);
						
						NumberBallText.text = PlayerPrefs.GetInt (BallString).ToString ();

						CoinsText.text = "Coins: " + PlayerPrefs.GetInt ("Coins").ToString ();

						gameObject.GetComponent<SpriteRenderer> ().sprite = sprite;
				}
	}
}
