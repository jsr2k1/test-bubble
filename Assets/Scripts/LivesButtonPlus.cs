using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class LivesButtonPlus : MonoBehaviour
{
	public Button buttonlivesplus;
	public Sprite buttonON;
	public Sprite buttonOFF;
	public Image image;

	void Update ()
	{
		if(LivesManager.lives < 5){
			buttonlivesplus.interactable = true;
			image.sprite = buttonON;
		}else if (LivesManager.lives == 5){
			buttonlivesplus.interactable = false;
			image.sprite = buttonOFF;
		}
	}
}
