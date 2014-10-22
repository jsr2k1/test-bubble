using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class CountdownUpdater : MonoBehaviour {
	
	public Text contador;

	////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

	void Update()
	{
		contador.text = LivesManager.sCountdown;
	}
}
