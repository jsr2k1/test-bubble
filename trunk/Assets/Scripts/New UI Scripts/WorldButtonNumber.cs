using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class WorldButtonNumber : MonoBehaviour
{
	public Text numTextPrefab;

	void Awake()
	{
		Text currentText = Instantiate(numTextPrefab) as Text;
		currentText.text = gameObject.name;
		currentText.transform.position = transform.position;
		currentText.transform.parent = transform;
		currentText.name = "label_"+gameObject.name;
		currentText.transform.localScale = new Vector3(0.2f, 0.2f, 1.0f);
	}
}
