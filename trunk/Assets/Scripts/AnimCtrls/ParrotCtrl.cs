using UnityEngine;
using System.Collections;

public class ParrotCtrl : MonoBehaviour
{
	int waitTime;
	Animator animator;
	
	//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
	
	void Start()
	{
		animator = GetComponent<Animator>();
		waitTime = Random.Range(1,5);
		StartCoroutine(DoBlink());
	}
	
	//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
	
	IEnumerator DoBlink()
	{
		yield return new WaitForSeconds(waitTime);
		waitTime = Random.Range(3,7);
		animator.SetTrigger("DoBlink");
		StartCoroutine(DoBlink());
	}
}
