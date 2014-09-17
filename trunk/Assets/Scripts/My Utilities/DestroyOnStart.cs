using UnityEngine;
using System.Collections;

public class DestroyOnStart : MonoBehaviour
{
	void Awake()
    {
		DontDestroyOnLoad(gameObject);
	}
}
