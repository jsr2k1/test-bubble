using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class WorldButtonNumber : MonoBehaviour
{
		public Text numTextPrefab;
		public Image star1;
		public Image star2;
		public Image star3;

		void Awake ()
		{
				Text currentText = Instantiate (numTextPrefab) as Text;
				currentText.text = gameObject.name;
				currentText.transform.position = transform.position;
				currentText.transform.parent = transform;
				currentText.name = "label_" + gameObject.name;
				currentText.transform.localScale = new Vector3 (0.2f, 0.2f, 1.0f);



				if (PlayerPrefs.GetInt ("STARS_" + gameObject.name) == 1) {
						Image stars = Instantiate (star1) as Image;
						stars.transform.position = transform.position;
						stars.transform.parent = transform;
				} else if (PlayerPrefs.GetInt ("STARS_" + gameObject.name) == 2) {
						Image stars = Instantiate (star2) as Image;
						stars.transform.position = transform.position;
						stars.transform.parent = transform;
				} else if (PlayerPrefs.GetInt ("STARS_" + gameObject.name) == 3) {
						Image stars = Instantiate (star3) as Image;
						stars.transform.position = transform.position;
						stars.transform.parent = transform;
				}
				
				
		}
}
