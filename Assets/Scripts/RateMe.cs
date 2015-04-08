using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class RateMe : MonoBehaviour
{
	Button button;
	Image image;
	public Text text;
	public bool bHideAfterClick;
	static bool bClickedOnce;
	
	////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
	
	void Awake()
	{
		button = GetComponent<Button>();
		image = GetComponent<Image>();
		
		button.enabled=false;
		image.enabled=false;
		if(text!=null){
			text.enabled=false;
		}
		
		bClickedOnce = (PlayerPrefs.GetInt("RateMeClicked") == 1);
	}
	
	////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
	
	void Start()
	{
		int n = PlayerPrefs.GetInt("NumTimesPlayed");
		if(n>3 && bHideAfterClick && !bClickedOnce){
			button.enabled=true;
			image.enabled=true;
		}
	}
	
	////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
	//Mostramos el boton redondo hasta que el usuario lo pulsa. 
	//Despues, mostramos solamente el boton dentro del PopUp de settings
	public void OnButtonRateMePressed()
	{
		if(bHideAfterClick){
			button.enabled=false;
			image.enabled=false;
			bClickedOnce=true;
			PlayerPrefs.SetInt("RateMeClicked", 1);
		}
		
		#if UNITY_ANDROID
		Application.OpenURL("market://details?id=com.aratinga.bubbleparadise2");
		#elif UNITY_IPHONE
		Application.OpenURL("itms-apps://itunes.apple.com/app/926782760");
		#endif
		
	}
	
	////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
	
	void Update()
	{
		if(!button.enabled && bClickedOnce && !bHideAfterClick){
			button.enabled=true;
			image.enabled=true;
			if(text!=null){
				text.enabled=true;
			}
		}
	}	
}



