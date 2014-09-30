using UnityEngine;
using System.Collections;

public class SettingsBackground : MonoBehaviour
{
	Animator anim;

	void Awake()
	{
		anim = GetComponent<Animator>();
	}

	public void ShowSettings()
	{
		anim.SetTrigger("ShowSettings");
	}

	public void HideSettings()
	{
		anim.SetTrigger("HideSettings");
	}

	public void FixPosition(float y)
	{
		Debug.Log("FixPosition: "+y);
		transform.position = new Vector3(transform.position.x, y, transform.position.z);
	}
}
