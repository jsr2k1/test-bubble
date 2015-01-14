using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class FacebookBubble : MonoBehaviour
{
	public static FacebookBubble instance;
	Button facebookButton;
	public Button playButton;
	public Button arcadeButton;
	Image facebookImage;
	GameObject facebookText;
	
	////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
	
	void Awake()
	{
		instance = this;
	
		facebookButton = GetComponent<Button>();
		facebookImage = GetComponent<Image>();
		facebookText = transform.GetChild(0).gameObject;
		
		if(FB.IsLoggedIn){
			facebookButton.enabled=false;
			facebookImage.enabled=false;
			facebookText.SetActive(false);
		}
	}
	
	////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
		
	void Update()
	{
		if(FB.IsLoggedIn && facebookButton.enabled){
			facebookButton.enabled=false;
			facebookImage.enabled=false;
			facebookText.SetActive(false);
		}
		
		if(!FB.IsLoggedIn && !facebookButton.enabled){
			facebookButton.enabled=true;
			facebookImage.enabled=true;
			facebookText.SetActive(true);
		}
	}
	
	////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
	
	public void EnableButtons()
	{
		playButton.interactable = true;
		arcadeButton.interactable = true;
	}
	
	////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

	public void loginfacebook()
	{
		playButton.interactable = false;
		arcadeButton.interactable = false;
		
		FB.Login("public_profile,email,user_friends,publish_actions", AuthCallback);
	}

	////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

	void AuthCallback(FBResult result)
	{
		if(FB.IsLoggedIn){
			Debug.Log(FB.UserId);
			FacebookManager.GetFacebookUserName(); //Obtenemos el nombre del usuario
		} else {
			Debug.Log("User cancelled login");
			EnableButtons();
		}
	}
}


