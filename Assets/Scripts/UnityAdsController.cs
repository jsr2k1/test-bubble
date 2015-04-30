using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Advertisements;
using System.Collections;
using System;

public class UnityAdsController : MonoBehaviour
{
	bool bTestMode=false;
	public Button button;
	public Image image;
	float secondsToShow = 180;	//Tiempo en segundos hasta que vuelva a estar activo el boton
	bool bShow=false;
	public Animator textCoinsAnimator;
	
	//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
	
	void Awake()
	{
		if(Advertisement.isSupported) {
			Advertisement.allowPrecache = true;
			#if UNITY_ANDROID
				Advertisement.Initialize("35427", bTestMode); //Android
			#else
				Advertisement.Initialize("35428", bTestMode); //iOS
			#endif
		}else{
			Debug.Log("Platform not supported");
		}
		button = GetComponent<Button>();
		image = GetComponent<Image>();
		bShow=false;
		button.enabled = false;
		image.enabled = false;
	}
	
	//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
	//Si al cargar el mapa de los mundos han pasado mas de 3 minutos desde la ultima vez se muestra el cartelito
	void Start()
	{
		DateTime actualTime = DateTime.Now;
		int secondsElapsed=0;
		
		if(PlayerPrefs.HasKey("savedTimeAds")){
			DateTime savedTime = DateTime.Parse(PlayerPrefs.GetString("savedTimeAds"));
			secondsElapsed = (int)actualTime.Subtract(savedTime).TotalSeconds;
			if(secondsElapsed > secondsToShow){
				bShow=true;
			}
		}else{
			bShow=true;
		}
	}
	
	//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
	
	void Update()
	{
		if(Advertisement.isReady() && bShow){
			button.enabled = true;
			image.enabled = true;
		}
	}
	
	//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
	
	public void OnShowAdButtonPressed()
	{
		//Debug.Log("OnShowAdButtonPressed: " + Advertisement.isReady());
		
		if(Advertisement.isReady()){
			Advertisement.Show(null, new ShowOptions { pause = false, resultCallback = ResultCallback } );
		}
	}
	
	//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
	
	void ResultCallback(ShowResult result)
	{
		//Debug.Log("-_-_ ResultCallback: " + result.ToString());
		
		if(result == ShowResult.Finished){
			int coins = PlayerPrefs.GetInt("Coins") + 5;
			CoinsManager.instance.SetCoins(coins);
			bShow = false;
			button.enabled = false;
			image.enabled = false;
			PlayerPrefs.SetString("savedTimeAds", DateTime.Now.ToString());
			textCoinsAnimator.SetTrigger("StartAnim");
			audio.Play();
		}
	}	
}






