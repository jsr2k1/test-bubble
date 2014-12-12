using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class CloseMoreBubblesPop : MonoBehaviour {

	public Image moreBubblesPop;

	public void closeMoreBubblesPop(){
		moreBubblesPop.GetComponent<PopUpMgr> ().HidePopUp ();
		LevelManager.gameState = GameState.Start;
		LevelManager.instance.GameIsOver ();
	}
}
