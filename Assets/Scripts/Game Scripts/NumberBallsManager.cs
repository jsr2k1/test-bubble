﻿using UnityEngine;
using System.Collections;

public class NumberBallsManager : MonoBehaviour
{

		public TextMesh ballsText;
		private int ballsLeft;

		// Use this for initialization
		void Start ()
		{
				if (PlayerPrefs.GetString ("GameType").Equals ("Arcade")) {
						ballsText.text = "";
				}
		}
	
		// Update is called once per frame
		void Update ()
		{
		
		}

		public void setBallsLeft (int b)
		{
				ballsLeft = b;
				if (ballsLeft == -1) {
						ballsText.text = "0";
				} else {
						ballsText.text = ballsLeft.ToString ();
				}
		}
}
