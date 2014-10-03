using UnityEngine;
using System.Collections;

public class PopUpMgr : MonoBehaviour
{
	Animator anim;
	bool bExitPause=false;

	void Awake()
	{
		anim = GetComponent<Animator>();
	}

	public void ShowPopUp()
	{
		anim.SetTrigger("ShowSettings");
		if(LevelManager.instance!=null)
			LevelManager.instance.pauseCtrl();
	}

	public void HidePopUp()
	{
		anim.SetTrigger("HideSettings");
		bExitPause=true;
	}

	public void FixPosition(float y)
	{
		Debug.Log("FixPosition: "+y);
		transform.position = new Vector3(transform.position.x, y, transform.position.z);
	}

	public void GoToWorlds()
	{
		Application.LoadLevel(2);
	}

	void Update()
	{
		if(bExitPause){
			if(LevelManager.instance!=null)
				LevelManager.instance.pauseCtrl();
			bExitPause=false;
		}
	}
}
