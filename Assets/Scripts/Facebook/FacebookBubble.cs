using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class FacebookBubble : MonoBehaviour
{
	public static FacebookBubble instance;
	Button facebookConnectButton;
	public Button facebookDisconnectButton;
	public Button playButton;
	public Button arcadeButton;
	Image facebookImage;
	GameObject facebookText;
	public PopUpMgr FacebookConnectedPopUp;
	public PopUpMgr ConnectToFacebookPopUp;
	
	public Sprite facebookSprite;
	public Sprite facebookGana;
	public Text earnText;
	
	////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
	
	void Awake()
	{
		instance = this;
	
		facebookConnectButton = GetComponent<Button>();
		facebookImage = GetComponent<Image>();
		facebookText = transform.GetChild(0).gameObject;
		
		if(PlayerPrefs.GetInt("FirstTimeFacebookLogin")==1){
			facebookImage.sprite = facebookGana;
			earnText.enabled=true;
		}else{
			facebookImage.sprite = facebookSprite;
			earnText.enabled=false;
		}
		
		//Debug.Log(Application.internetReachability);
		
		if(Application.internetReachability!=NetworkReachability.NotReachable){
			if(FB.IsLoggedIn){
				SetFacebookButtons(false, true);
			}else{
				SetFacebookButtons(true, false);
			}
		}else{
			SetFacebookButtons(false, false);
		}
	}
	
	////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
	//Si el jugador ha entrado en el juego 2 veces y no se ha conectado a Facebook, la tercera vez que entre mostramos el popup
	void Start()
	{
		int n = PlayerPrefs.GetInt("NumTimesPlayed");
		int i = PlayerPrefs.GetInt("FirstTimeFacebookLogin");
		if(i==1 && n==2){
			ConnectToFacebookPopUp.ShowPopUp();
		}
	}
	
	////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
		
	void Update()
	{
		if(Application.internetReachability!=NetworkReachability.NotReachable){
			if(FB.IsLoggedIn && facebookConnectButton.enabled){
				SetFacebookButtons(false, true);
			}
			if(!FB.IsLoggedIn && !facebookConnectButton.enabled){
				SetFacebookButtons(true, false);
			}
		}else{
			SetFacebookButtons(false, false);
		}
	}
	
	////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
	
	void SetFacebookButtons(bool bConnect, bool bDisconnect)
	{
		facebookConnectButton.enabled=bConnect;
		facebookImage.enabled=bConnect;
		facebookText.SetActive(bConnect);
		facebookDisconnectButton.gameObject.SetActive(bDisconnect);
	}
	
	////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
	
	public void EnableButtons()
	{
		playButton.interactable = true;
		arcadeButton.interactable = true;
	}
	
	////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

	public void ButtonPressedLogInFacebook()
	{
		playButton.interactable = false;
		arcadeButton.interactable = false;
		
		FB.Login("public_profile,email,user_friends,publish_actions", AuthCallback);
	}
	
	////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
	
	public void ButtonPressedLogOutFacebook()
	{
		FB.Logout();
		
		facebookImage.sprite = facebookSprite;
		earnText.enabled=false;
	}

	////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

	void AuthCallback(FBResult result)
	{
		if(FB.IsLoggedIn){
			//Debug.Log(FB.UserId);
			FacebookManager.GetFacebookUserName(); //Obtenemos el nombre del usuario
			FacebookConnectedPopUp.ShowPopUp();
			if(PlayerPrefs.GetInt("FirstTimeFacebookLogin")==1){
				int coins =  PlayerPrefs.GetInt("Coins");
				PlayerPrefs.SetInt("Coins", coins+40);
				//PlayerPrefs.SetInt("FirstTimeFacebookLogin",0);
				earnText.enabled=false;
			}
		} else {
			Debug.Log("User cancelled login");
			EnableButtons();
		}
	}
}


