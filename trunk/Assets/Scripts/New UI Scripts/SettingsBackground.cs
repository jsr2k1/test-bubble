using UnityEngine;
using System.Collections;

public class SettingsBackground : MonoBehaviour
{
	Animator anim;

	void Awake()
	{
		anim = GetComponent<Animator>();
	}

	public void OnEnable()
	{
		anim.SetTrigger("ShowSettings");
	}
}
