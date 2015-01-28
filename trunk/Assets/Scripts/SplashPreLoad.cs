using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class SplashPreLoad : MonoBehaviour
{
	public Text loadingText;
	
	//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
	
	void Start()
	{
		StartCoroutine(LoadSplash());
	}
	
	//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
	
	IEnumerator LoadSplash()
	{
		SystemLanguage currentLang = Application.systemLanguage;
		if(currentLang == SystemLanguage.Spanish){
			loadingText.text="Cargando...";
		}else{
			loadingText.text="Loading...";
		}
		yield return new WaitForEndOfFrame();
		Application.LoadLevel("02 Splash Managers");
	}
}
