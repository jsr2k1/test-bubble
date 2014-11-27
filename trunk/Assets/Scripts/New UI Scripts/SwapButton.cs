using UnityEngine;
using System.Collections;

public class SwapButton : MonoBehaviour
{
	public void SwapBalls()
	{
		if(PlayerPrefs.GetInt("Sounds")==1){
			audio.Play();
		}

		if(PlayerPrefs.GetString("GameType") == "Arcade" || LevelManager.currentBalls > 1){
			Striker.instance.Swap ();
		}
	}
}
