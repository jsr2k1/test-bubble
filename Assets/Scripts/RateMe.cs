using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class RateMe : MonoBehaviour
{
	Button button;
	Image image;
	
	////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
	
	void Awake()
	{
		button = GetComponent<Button>();
		image = GetComponent<Image>();
		
		button.enabled=false;
		image.enabled=false;
	}
	
	////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
	
	void Start()
	{
		int n = PlayerPrefs.GetInt("NumTimesPlayed");
		if(n>3){
			button.enabled=true;
			image.enabled=true;
		}
	}
	
	////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
	
	public void OnButtonRateMePressed()
	{
		#if UNITY_ANDROID
		Application.OpenURL("market://details?id=com.aratinga.bubbleparadise2");
		#elif UNITY_IPHONE
		Application.OpenURL("itms-apps://itunes.apple.com/app/com.aratinga.bubbleparadise2");
		#endif
	}
}
