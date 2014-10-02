using UnityEngine;
using System.Collections;

public class SpecialBallButton : MonoBehaviour
{
		public GameObject ShopPopup;
		public string BallString;
	
		public void OnButtonClick ()
		{
				//Comprobamos si quedan boosters disponibles
				if (PlayerPrefs.GetInt (BallString) > 0) {
						if (BallString == "Fire Ball") {
								Striker.instance.SetFireBall ();
						} else if (BallString == "Bomb Ball") {
								Striker.instance.SetBombBall ();	
						} else if (BallString == "Multicolor Ball") {
								Striker.instance.SetMultiBall ();	
						}
				} else {
						//LevelManager.instance.pauseCtrl();
						//iTween.MoveTo(parent, iTween.Hash("x", 0));
						shopAppears ();
				}
		}

		void shopAppears ()
		{
				ShopPopup.SetActive (true);
		}
			
}
