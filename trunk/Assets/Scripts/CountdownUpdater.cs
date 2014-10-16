using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class CountdownUpdater : MonoBehaviour {
	
	public Text contador;

	LifeManager lifeManager;

	void Awake (){
		lifeManager = GameObject.Find ("LifeMgr").GetComponent<LifeManager> ();
	}

	void Update () {
		contador.text = lifeManager.countdown;
	}
}
