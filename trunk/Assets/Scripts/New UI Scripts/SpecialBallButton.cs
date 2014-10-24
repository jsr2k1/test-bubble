using UnityEngine;
using System.Collections;

public class SpecialBallButton : MonoBehaviour
{
	public string BallString;
	
	public void OnButtonClick ()
	{
		if(PlayerPrefs.GetInt("Sounds")==1){
			audio.Play();
		}
		//Comprobamos si quedan boosters disponibles
		if (PlayerPrefs.GetInt (BallString) > 0){
			if(BallString == "Fire Ball"){
				Striker.instance.SetFireBall();
			}else if (BallString == "Bomb Ball"){
				Striker.instance.SetBombBall();	
			}else if (BallString == "Multicolor Ball"){
				Striker.instance.SetMultiBall();	
			}
		}
	}
}
