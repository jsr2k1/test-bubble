using UnityEngine;
using System.Collections;

public class IGbtnSpecialBall : MonoBehaviour
{
		public GameObject parent;
		private int quantity;
		public string BallString;

		void OnMouseDown ()
		{
				if (PlayerPrefs.GetInt (BallString) > 0) {

						quantity = PlayerPrefs.GetInt (BallString) - 1;
						PlayerPrefs.SetInt (BallString, quantity);

						if (BallString == "Fire Ball") {
		
								Striker.instance.setFireBall ();

						} else if (BallString == "Bomb Ball") {

								Striker.instance.setBombBall ();			

						} else {

							PlayingObject.setmultiBall();

						}
				} else {
						LevelManager.instance.pauseCtrl ();
						iTween.MoveTo (parent, iTween.Hash ("x", 0));
				}
		}
}


