﻿using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class PopUpMgr : MonoBehaviour
{
	Animator anim;
	bool bExitPause=false;
	bool bShow=false;

	////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

	void Awake()
	{
		anim = GetComponent<Animator>();
	}

	////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

	public void ShowPopUp()
	{
		if(!bShow){
			anim.SetTrigger("ShowSettings");
			bShow=true;
			if(LevelManager.instance!=null){
				LevelManager.instance.pauseCtrl();
			}
		}
	}

	////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

	public void HidePopUp()
	{
		if(bShow){
			anim.SetTrigger("HideSettings");
			bShow=false;
			bExitPause=true;
		}
	}

	////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
	/*
	public void FixPosition(float y)
	{
		transform.position = new Vector3(transform.position.x, y, transform.position.z);
	}
	*/
	////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

	public void GoToWorlds()
	{
		Application.LoadLevel(2);
	}

	////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

	void Update()
	{
		if(bExitPause){
			if(LevelManager.instance!=null){
				LevelManager.instance.pauseCtrl();
			}
			bExitPause=false;
		}
	}
}
