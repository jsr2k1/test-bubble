using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class LevelNumber : MonoBehaviour
{
	void Awake()
	{
		GetComponent<Text>().text = "LEVEL "+LevelManager.levelNo;
	}
}
