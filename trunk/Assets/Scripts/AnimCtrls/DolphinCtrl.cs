﻿using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class DolphinCtrl : MonoBehaviour
{
	int waitTime=4;
	Animator animator;
	
	//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
	
	void Start()
	{
		animator = GetComponent<Animator>();
		StartCoroutine(DoBlink());
	}
	
	//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
	
	IEnumerator DoBlink()
	{
		yield return new WaitForSeconds(waitTime);
		animator.SetTrigger("Blink");
		StartCoroutine(DoBlink());
	}
}
