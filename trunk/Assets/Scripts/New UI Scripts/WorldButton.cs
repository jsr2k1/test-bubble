using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class WorldButton : MonoBehaviour
{
	public int numberBalls;
	public int ActualWorld;
	public int refScore;
	public Text numTextPrefab;
	public Image star1;
	public Image star2;
	public Image star3;
	public Image ProfileImg;
	Button button;

	void Awake ()
	{

		button = GetComponent<Button> ();

		if (PlayerPrefs.HasKey ("STARS_" + (int.Parse (gameObject.name) - 1))) {
			button.interactable = true;
		} else {
			button.interactable = false;
		}

		if (gameObject.name == "1") {
			button.interactable = true;
		}

		Text currentText = Instantiate (numTextPrefab) as Text;
		currentText.text = gameObject.name;
		currentText.transform.position = transform.position;
		currentText.transform.parent = transform;
		currentText.name = "label_" + gameObject.name;
		currentText.transform.localScale = new Vector3 (0.2f, 0.2f, 1.0f);

		Image stars;

		if (PlayerPrefs.GetInt ("STARS_" + gameObject.name) == 3) {
			stars = Instantiate (star3) as Image;

		} else if (PlayerPrefs.GetInt ("STARS_" + gameObject.name) == 2) {
			stars = Instantiate (star2) as Image;

		} else if (PlayerPrefs.GetInt ("STARS_" + gameObject.name) == 1) {
			stars = Instantiate (star1) as Image;

		} else {
			stars = null;
		}

		if (stars != null) {
			stars.transform.parent = transform;
			stars.transform.localPosition = new Vector3 (0, 36, 0);
			stars.transform.localScale = new Vector3 (0.8f, 0.8f, 1.0f);
		}


		if (gameObject.name == (PlayerPrefs.GetInt("Level")+1).ToString()){
			//ProfileImg.transform.parent = transform;
			ProfileImg.transform.position = transform.position +  new Vector3 (40, 60, 0);
			ProfileImg.transform.localScale = new Vector3 (0.8f, 0.8f, 1.0f);
		}
	}

	public void ButtonPressed ()
	{
		if (PlayerPrefs.GetInt("Lifes")>0){
			PlayerPrefs.SetString ("GameType", "Normal");
			LevelManager.patternType = PatternType.TextLevel;
			LevelParser.instance.LoadTextLevel (int.Parse (name), ActualWorld);

			LevelManager.NumberOfBalls = numberBalls;
			LevelManager.ReferenceScore = refScore;
			LevelManager.rowAddingInterval = 1;
			LevelManager.levelNo = int.Parse (name);
			Application.LoadLevel (3);
		}
	}
}
