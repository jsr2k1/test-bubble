using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class BackgroundImage : MonoBehaviour
{
	public Sprite[] imgWorlds;
	
	void Start()
	{
		GetComponent<Image>().sprite = imgWorlds[LevelManager.worldNo-1];
	}
}
