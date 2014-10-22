using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class LivesText : MonoBehaviour {
	
	public Text lives;

	void Update()
	{
		lives.text = LivesManager.lives.ToString();
	}
}
