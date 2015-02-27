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
	public PopUpMgr FacebookConnectedPopUp;
	public PopUpMgr ConnectToFacebookPopUp;
	
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
	//Si el jugador ha entrado en el juego 2 veces y no se ha conectado a Facebook, la tercera vez que entre mostramos el popup
	void Start()
	{
		int n = PlayerPrefs.GetInt("NumTimesPlayed");
		if(n==2){
			ConnectToFacebookPopUp.ShowPopUp();
		}
		PlayerPrefs.SetInt("NumTimesPlayed", n+1);
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
			FacebookConnectedPopUp.ShowPopUp();
		} else {
			Debug.Log("User cancelled login");
			EnableButtons();
		}
	}
}


