using UnityEngine;
using System.Collections;

public class DontDestroyOnStart : MonoBehaviour
{
	void Awake()
    {
		DontDestroyOnLoad(gameObject);
	}
}
