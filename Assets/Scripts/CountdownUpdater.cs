using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class CountdownUpdater : MonoBehaviour {
	
	public Text contador;
	LifeManager lifeManager;

	////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

	void Awake()
	{
		GameObject go = GameObject.Find("LifeMgr");
		if(go!=null){
			lifeManager = go.GetComponent<LifeManager>();
		}
	}

	////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

	void Update()
	{
		if(lifeManager!=null){
			contador.text = lifeManager.countdown;
		}
	}
}
