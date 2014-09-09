using UnityEngine;
using System.Collections;

public class LevelButton : MonoBehaviour
{

		public int levelNo;
		public bool arcadeMode;
		public int ActualWorld;

		void Start ()
		{
				transform.FindChild ("Text").GetComponent<TextMesh> ().text = name;
				levelNo = int.Parse (name);
		}

		void OnMouseDown ()
		{
				//If its arcade mode we load the normal system as the template comes else we load from the txt files
				if (arcadeMode == true) {
						PlayerPrefs.SetString ("GameType", "Arcade");

						int val = levelNo % 4;
		
						LevelManager.patternType = (PatternType)(levelNo / 4);

						LevelManager.totalNoOfRows = 10 + val * 2;

						LevelManager.minimumNumberOfRows = 3 + val;
						LevelManager.rowAddingInterval = 9 - val;

						LevelManager.levelNo = levelNo;
						Application.LoadLevel ("Game Scene");
				} else {

						PlayerPrefs.SetString ("GameType", "Normal");
						LevelManager.patternType = PatternType.TextLevel;
						LevelParser.instance.LoadTextLevel (levelNo, ActualWorld);


						LevelManager.rowAddingInterval = 30;
						LevelManager.levelNo = levelNo;
						Application.LoadLevel ("Game Scene");
				}
		}
}
