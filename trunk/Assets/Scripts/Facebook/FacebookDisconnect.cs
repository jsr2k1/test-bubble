using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class FacebookDisconnect : MonoBehaviour
{
	Button facebookButton;
	Image facebookImage;
	
	////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
	
	void Awake()
	{
		facebookButton = GetComponent<Button>();
		facebookImage = GetComponent<Image>();
		
		if(FB.IsLoggedIn){
			facebookButton.enabled=true;
			facebookImage.enabled=true;
		}
	}
	
	////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
	
	void Update()
	{
		if(FB.IsLoggedIn && !facebookButton.enabled){
			facebookButton.enabled=true;
			facebookImage.enabled=true;
		}
		
		if(!FB.IsLoggedIn && facebookButton.enabled){
			facebookButton.enabled=false;
			facebookImage.enabled=false;
		}
	}
	
	////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
	
	public void ButtonPressed()
	{
		FB.Logout();
	}
}
