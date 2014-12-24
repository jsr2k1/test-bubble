using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class CloseMoreBubblesPop : MonoBehaviour
{
	public PopUpMgr moreBubblesPop;

	public void closeMoreBubblesPop()
	{
		moreBubblesPop.HidePopUp();
		LevelManager.gameState = GameState.Start;
		LevelManager.instance.GameIsOver();
	}
}
