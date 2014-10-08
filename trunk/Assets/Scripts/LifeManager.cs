using UnityEngine;
using System.Collections;
using System;
using UnityEngine.UI;

public class LifeManager : MonoBehaviour
{
		public Text countdown;
		TimeSpan t;
		int secondsToLife = 30;
		bool counting;
		DateTime savedTime;
		DateTime actualTime;
		int seconds = 30;

		void Start ()
		{

				PlayerPrefs.SetInt ("Lifes", 2);
				
				if (PlayerPrefs.GetInt ("Lifes") == 0 || PlayerPrefs.GetInt ("Lifes") == 5) {
						counting = false;
				} else if (PlayerPrefs.GetInt ("Lifes") < 5 && !counting) {
						StartCoroutine ("counter");
						counting = true;
				}
				
				actualTime = DateTime.Now;
				
				savedTime = DateTime.Parse (PlayerPrefs.GetString ("savedTime"));
				TimeSpan span = actualTime.Subtract (savedTime);

				Debug.Log (actualTime);
				Debug.Log (savedTime);
				Debug.Log (span.Seconds);
				
				if (span.Seconds > secondsToLife && span.Seconds < secondsToLife * 2) {
						PlayerPrefs.SetInt ("Lifes", PlayerPrefs.GetInt ("Lifes") + 1);
				} else if (span.Seconds > secondsToLife * 2 && span.Seconds < secondsToLife * 3) {
						PlayerPrefs.SetInt ("Lifes", PlayerPrefs.GetInt ("Lifes") + 2);
				} else if (span.Seconds > secondsToLife * 3 && span.Seconds < secondsToLife * 4) {
						PlayerPrefs.SetInt ("Lifes", PlayerPrefs.GetInt ("Lifes") + 3);
				} else if (span.Seconds > secondsToLife * 4 && span.Seconds < secondsToLife * 5) {
						PlayerPrefs.SetInt ("Lifes", PlayerPrefs.GetInt ("Lifes") + 4);
				} else if (span.Seconds > secondsToLife * 5) {
						PlayerPrefs.SetInt ("Lifes", 5);
				}
				

				if (PlayerPrefs.GetInt ("Lifes") > 5) {
						PlayerPrefs.SetInt ("Lifes", 5);
				} 

		}

		void Update ()
		{
				if (PlayerPrefs.GetInt ("Lifes") == 0 || PlayerPrefs.GetInt ("Lifes") == 5) {
						counting = false;
				} else if (PlayerPrefs.GetInt ("Lifes") < 5 && !counting) {
						StartCoroutine ("counter");
						counting = true;
				}
				savedTime = DateTime.Now;
				PlayerPrefs.SetString ("savedTime", savedTime.ToString ());
				//PlayerPrefs.SetInt ("seconds", seconds);
		}

		IEnumerator counter ()
		{
				if (PlayerPrefs.GetInt ("Lifes") > 0 && PlayerPrefs.GetInt ("Lifes") < 5) {

						yield return new WaitForSeconds (1f);

						t = TimeSpan.FromSeconds (seconds);

						countdown.text = t.Minutes.ToString ("00") + ":" + t.Seconds.ToString ("00");

						if (seconds > 0) {
								seconds = seconds - 1;
						} else {
								seconds = secondsToLife;
								PlayerPrefs.SetInt ("Lifes", PlayerPrefs.GetInt ("Lifes") + 1);
						}

						StartCoroutine ("counter");
				}
		}


}
