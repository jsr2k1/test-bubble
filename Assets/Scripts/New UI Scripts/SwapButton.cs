using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class SwapButton : MonoBehaviour
{
	public Animator swapImageAnim;
	
	//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
	
	public void SwapBalls()
	{
		if(Striker.instance.fireBall || Striker.instance.bombBall || Striker.instance.multiBall){
			return;
		}
		if(LevelManager.GameType == LevelManager.GameTypes.ARCADE || LevelManager.currentBalls > 1)
		{
			Striker.instance.Swap();
			swapImageAnim.SetTrigger("SwapPressed");
			
			if(AudioManager.instance.bSoundsOn){
				GetComponent<AudioSource>().Play();
			}
		}
	}
}
