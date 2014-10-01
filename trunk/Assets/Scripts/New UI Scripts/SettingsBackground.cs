using UnityEngine;
using System.Collections;

public class SettingsBackground : MonoBehaviour
{
	Animator anim;
	bool bExitPause=false;

	void Awake()
	{
		anim = GetComponent<Animator>();
	}

	public void ShowSettings()
	{
		anim.SetTrigger("ShowSettings");
		LevelManager.instance.pauseCtrl();
	}

	public void HideSettings()
	{
		anim.SetTrigger("HideSettings");
		bExitPause=true;
	}

	public void FixPosition(float y)
	{
		Debug.Log("FixPosition: "+y);
		transform.position = new Vector3(transform.position.x, y, transform.position.z);
	}

	void Update()
	{
		if(bExitPause){
			LevelManager.instance.pauseCtrl();
			bExitPause=false;
		}
	}
}
