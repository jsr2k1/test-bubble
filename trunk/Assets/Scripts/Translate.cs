using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class Translate : MonoBehaviour
{
	void Start()
	{
		string id = gameObject.GetComponent<Text>().text;
		gameObject.GetComponent<Text>().text = LanguageManager.GetText(id);
	}
}
