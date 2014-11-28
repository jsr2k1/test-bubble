using UnityEngine;
using System.Collections;
using System;
using UnityEngine.UI;

public class LivesManager : MonoBehaviour
{
	public static String sCountdown;
	public int secondsToLife;
	float seconds = 0;
	
	static int m_lives;
	public static int lives{
		get{ 
			return m_lives;
		}set{
			m_lives = Mathf.Clamp(value, 0, 5);
			PlayerPrefs.SetInt("Lives", m_lives);
		}
	}
	
	//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

	void Start()
	{
		DontDestroyOnLoad(gameObject);
		//UpdateCurrentTime(); No es necesario llamar desde aqui pq siempre se llama desde OnApplicationPause()
		CheckLives();
	}
	
	//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
	//Si el usuario ha cerrado la aplicacion durante una partida debe perder una vida
	void CheckLives()
	{
		if(!PlayerPrefs.HasKey("bPlaying")){
			PlayerPrefs.SetInt("bPlaying", 0);
		}
		else if(PlayerPrefs.GetInt("bPlaying") == 1){
			lives--;
			PlayerPrefs.SetInt("bPlaying", 0);
		}
	}
	
	//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

	void Update()
	{
		if(lives>=5){
			sCountdown = "FULL";
			seconds=0;
			return;
		}
		else{	
			if(seconds < secondsToLife){
				seconds += Time.deltaTime;
			}else{
				seconds = 0;
				lives++;
			}
			TimeSpan t = TimeSpan.FromSeconds(secondsToLife - seconds);
			sCountdown = t.Minutes.ToString("00") + ":" + t.Seconds.ToString("00");
		}
	}
	
	//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
	
	void UpdateCurrentTime()
	{
		//Debug.LogWarning("UpdateCurrentTime(), DateTime.Now:"+DateTime.Now+" seconds:"+PlayerPrefs.GetFloat("seconds"));
		
		lives = PlayerPrefs.GetInt("Lives");
		if(lives==5){
			return;
		}
		seconds = PlayerPrefs.GetFloat("seconds");

		DateTime actualTime = DateTime.Now;
		int span=0;
		if(PlayerPrefs.HasKey("savedTime")){
			DateTime savedTime = DateTime.Parse(PlayerPrefs.GetString("savedTime"));
			span = (int)actualTime.Subtract(savedTime).TotalSeconds;
		}
		int elapsed = (int)seconds + span;
		seconds = elapsed % secondsToLife;
		lives = Mathf.Min(lives + (elapsed / secondsToLife), 5);
		
		//Debug.LogWarning("elapsed:"+elapsed+" seconds:"+seconds+" lives:"+lives+" span:"+span);
	}
	
	//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
	
	void SaveCurrentTime()
	{
		//Debug.LogWarning("SaveCurrentTime(), DateTime.Now:"+DateTime.Now+" seconds:"+seconds+", lives:"+lives);
		
		PlayerPrefs.SetInt("Lives", lives);
		PlayerPrefs.SetString("savedTime", DateTime.Now.ToString());
		PlayerPrefs.SetFloat("seconds", seconds);
	}
	
	//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

	void OnApplicationPause(bool pause)
	{
		//La applicacion ha pasado a segundo plano
		if(pause){
			SaveCurrentTime();
		}
		//La aplicacion vuelve a estar en primer plano
		else{			
			UpdateCurrentTime();
		}
	}
	
	//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
	//Parece que no funciona en ANDROID
	void OnApplicationQuit()
	{
		SaveCurrentTime();
	}
}





