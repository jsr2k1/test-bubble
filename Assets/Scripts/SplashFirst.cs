using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class SplashFirst : MonoBehaviour
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
		yield return new WaitForSeconds(2.0f);
		loadingText.enabled=true;
		SystemLanguage currentLang = Application.systemLanguage;
		if(currentLang == SystemLanguage.Spanish){
			loadingText.text="Cargando...";
		}else{
			loadingText.text="Loading...";
		}
		yield return new WaitForEndOfFrame();
		Application.LoadLevel("01 Splash Preload");
	}
}
