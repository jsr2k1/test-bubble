using UnityEngine;
using System.Collections;

public class Splash : MonoBehaviour
{
	public float timer;
	public static Sprite spriteIMG;
	public int force_current_level=-1; //debug
	public int initial_lives;

	void Awake()
	{
		if(!PlayerPrefs.HasKey("Lives")){
			PlayerPrefs.SetInt("Lives", initial_lives);
			LivesManager.lives = 5;
		}
		if(PlayerPrefs.HasKey("Music")==false){
			PlayerPrefs.SetInt("Music", 1);
			AudioManager.instance.bMusicOn=true;
		}
		if(PlayerPrefs.HasKey("Sounds")==false){
			PlayerPrefs.SetInt("Sounds", 1);
			AudioManager.instance.bSoundsOn=true;
		}
		if(PlayerPrefs.HasKey("Multicolor Ball")==false){
			PlayerPrefs.SetInt("Multicolor Ball", 2);
		}
		if(PlayerPrefs.HasKey("Fire Ball")==false){
			PlayerPrefs.SetInt("Fire Ball", 2);
		}
		if(PlayerPrefs.HasKey("Bomb Ball")==false){
			PlayerPrefs.SetInt("Bomb Ball", 2);
		}
		if(PlayerPrefs.HasKey("Coins")==false){
			PlayerPrefs.SetInt("Coins", 200);
		}
		if(!PlayerPrefs.HasKey("Level")){
			PlayerPrefs.SetInt("Level", 0);
		}
		if(!PlayerPrefs.HasKey("Highscore")){
			PlayerPrefs.SetInt("Highscore", 0);
		}
		if(!PlayerPrefs.HasKey("FirstTimeFacebookLogin")){
			PlayerPrefs.SetInt("FirstTimeFacebookLogin", 1);
		}
		if(!PlayerPrefs.HasKey("RateMeClicked")){
			PlayerPrefs.SetInt("RateMeClicked", 0);
		}
		if(!PlayerPrefs.HasKey("NumTimesPlayed")){
			PlayerPrefs.SetInt("NumTimesPlayed", 0);
		}else{
			int n = PlayerPrefs.GetInt("NumTimesPlayed");
			PlayerPrefs.SetInt("NumTimesPlayed", n+1);
		}
		//Test Only
		if(force_current_level>0){
			PlayerPrefs.SetInt("Level", force_current_level);
		}
		
		FB.Init(OnInitComplete);
		StartCoroutine("DisplayScene");
	}
	
	////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

	IEnumerator DisplayScene()
	{
		yield return new WaitForSeconds(timer);
		Application.LoadLevel("03 Menu");
	}
	
	////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

	private void OnInitComplete()
	{
		//Debug.Log("FB.Init completed: Is user logged in? " + FB.IsLoggedIn);
		if(FB.IsLoggedIn){
			ParseManager.instance.CheckParseEntry();
		}
	}
}



