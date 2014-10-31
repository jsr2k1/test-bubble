using UnityEngine;
using System.Collections;

public class FBProfileImage : MonoBehaviour
{
	public GameObject dummyParent;
	
	void Awake()
	{
		dummyParent.SetActive(FB.IsLoggedIn);
	}
}
