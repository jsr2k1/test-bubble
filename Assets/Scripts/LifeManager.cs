using UnityEngine;
using System.Collections;
using System;
using UnityEngine.UI;

public class LifeManager : MonoBehaviour
{
	public String countdown;
	TimeSpan t;
	int secondsToLife = 300;
	bool counting;
	DateTime savedTime;
	DateTime actualTime;
	int seconds = 0;

	void Awake ()
	{
		DontDestroyOnLoad (gameObject);
	}

	void Start ()
	{		
		actualTime = DateTime.Now;
		savedTime = DateTime.Parse (PlayerPrefs.GetString ("savedTime"));

		TimeSpan span = actualTime.Subtract (savedTime);
		
		int segundostotales = PlayerPrefs.GetInt ("seconds") + span.Seconds;
		
		PlayerPrefs.SetInt ("Lifes", PlayerPrefs.GetInt ("Lifes") + (segundostotales / secondsToLife));
		PlayerPrefs.SetInt ("seconds", segundostotales % secondsToLife);

		seconds = PlayerPrefs.GetInt ("seconds");

	}

	void Update ()
	{
		if (PlayerPrefs.GetInt ("Lifes") == 5) {
				counting = false;
				PlayerPrefs.SetInt ("seconds", 0);
		} else if (PlayerPrefs.GetInt ("Lifes") < 5 && !counting) {
				StartCoroutine ("counter");
				counting = true;
		}

		savedTime = DateTime.Now;

		PlayerPrefs.SetString ("savedTime", savedTime.ToString ());
		PlayerPrefs.SetInt ("seconds", seconds);

		if (PlayerPrefs.GetInt ("Lifes") >= 5) {
			PlayerPrefs.SetInt ("Lifes", 5);
			PlayerPrefs.SetInt ("seconds", 0);
			countdown = "00:00";
		} 
	}

	IEnumerator counter ()
	{
		if (PlayerPrefs.GetInt ("Lifes") < 5) {

			yield return new WaitForSeconds (1f);

			t = TimeSpan.FromSeconds (secondsToLife - seconds);

			countdown = t.Minutes.ToString ("00") + ":" + t.Seconds.ToString ("00");

			if (seconds < 300) {
				seconds = seconds + 1;

			} else {
				seconds = 0;
				PlayerPrefs.SetInt ("Lifes", PlayerPrefs.GetInt ("Lifes") + 1);
			}

			StartCoroutine ("counter");
		}
	}

	void OnApplicationPause (bool pause)
	{
		if(pause)
		{
			// we are in background
		}
		else
		{			
			actualTime = DateTime.Now;
			savedTime = DateTime.Parse (PlayerPrefs.GetString ("savedTime"));

			TimeSpan span = actualTime.Subtract (savedTime);
			
			int segundostotales = PlayerPrefs.GetInt ("seconds") + span.Seconds;
			
			PlayerPrefs.SetInt ("Lifes", PlayerPrefs.GetInt ("Lifes") + (segundostotales / secondsToLife));
			PlayerPrefs.SetInt ("seconds", segundostotales % secondsToLife);

			seconds = PlayerPrefs.GetInt ("seconds");

		}
	}

}
