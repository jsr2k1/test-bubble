using UnityEngine;
using System.Collections;

public class SwapButton : MonoBehaviour
{
	public void SwapBalls()
	{
		if(PlayerPrefs.GetInt("Sounds")==1){
			audio.Play();
		}

		if (LevelManager.currentBalls > 1) {
			Striker.instance.Swap ();
		}
	}
}
