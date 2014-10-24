using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class FacebookBubble : MonoBehaviour
{
	public Image profilePic;
	Button facebookButton;
	Image facebookImage;
	
	////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
	
	void Awake()
	{
		facebookButton = GetComponent<Button>();
		facebookImage = GetComponent<Image>();
		
		if(FB.IsLoggedIn){
			facebookButton.enabled=false;
			facebookImage.enabled=false;
		}
	}
	
	////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
		
	void Update()
	{
		if(FB.IsLoggedIn && facebookButton.enabled){
			facebookButton.enabled=false;
			facebookImage.enabled=false;
		}
		
		if(!FB.IsLoggedIn && !facebookButton.enabled){
			facebookButton.enabled=true;
			facebookImage.enabled=true;
		}
	}
	
	////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

	public void loginfacebook()
	{
		FB.Login("email", AuthCallback);
		
		if(PlayerPrefs.GetInt("Sounds")==1){
			audio.Play();
		}
	}

	////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

	void AuthCallback(FBResult result)
	{
		if(FB.IsLoggedIn){
			Debug.Log(FB.UserId);
			//StartCoroutine("OnLoggedIn"); 
		} else {
			Debug.Log("User cancelled login");
		}
	}                                                                                     

	////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
	
	/*
	IEnumerator OnLoggedIn()

	{
		Debug.Log ("Entra 2");

		WWW url = new WWW("https" + "://graph.facebook.com/" + FB.UserId + "/picture?type=large"); //+ "?access_token=" + FB.AccessToken);
		
		Texture2D textFb2 = new Texture2D(128, 128, TextureFormat.DXT1, false); //TextureFormat must be DXT5

		yield return url;

		url.LoadImageIntoTexture(textFb2);

		Sprite spritefb = Sprite.Create(textFb2, new Rect(0, 0, textFb2.width, textFb2.height), new Vector2(0.5f, 0.5f));

		profilePic.sprite = spritefb;

	}
	*/

}
