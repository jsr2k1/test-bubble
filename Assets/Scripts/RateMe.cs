using UnityEngine;
using UnityEngine.UI;
using System.Collections;

//En este foro se habla del tema
//http://stackoverflow.com/questions/433907/how-to-link-to-apps-on-the-app-store

public class RateMe : MonoBehaviour
{
	Button button;
	Image image;
	
	public GameObject buttonRateMeSettings;
	
	////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
	
	void Awake()
	{
		button = GetComponent<Button>();
		image = GetComponent<Image>();
		
		button.enabled=false;
		image.enabled=false;
		
		buttonRateMeSettings.SetActive(false);
	}
	
	////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
	
	void Start()
	{
		if(Application.internetReachability!=NetworkReachability.NotReachable)
		{
			bool bClicked = (PlayerPrefs.GetInt("RateMeClicked") == 1);
			int n = PlayerPrefs.GetInt("NumTimesPlayed");
			if(n>0){
				button.enabled=!bClicked;
				image.enabled=!bClicked;
				buttonRateMeSettings.SetActive(bClicked);
			}
		}
	}
	
	////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
	//Mostramos el boton redondo hasta que el usuario lo pulsa. 
	//Despues, mostramos solamente el boton dentro del PopUp de settings
	public void OnButtonRateMePressed()
	{
		button.enabled=false;
		image.enabled=false;
		buttonRateMeSettings.SetActive(true);
		PlayerPrefs.SetInt("RateMeClicked", 1);
		
		#if UNITY_ANDROID
		Application.OpenURL("market://details?id=com.aratinga.bubbleparadise2");
		#elif UNITY_IPHONE
		Application.OpenURL("itms-apps://itunes.apple.com/app/id926782760");
		#endif
		
	}
	
	////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
	
	public void OnButtonRateMeSettingsPressed()
	{
		#if UNITY_ANDROID
		Application.OpenURL("market://details?id=com.aratinga.bubbleparadise2");
		#elif UNITY_IPHONE
		Application.OpenURL("itms-apps://itunes.apple.com/app/id926782760");
		#endif
	}
}



