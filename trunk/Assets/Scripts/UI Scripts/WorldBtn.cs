using UnityEngine;
using System.Collections;

public class WorldBtn : MonoBehaviour {

	private Vector3 offset; 
	private float _lockedYPosition;
	private Vector3 screenPoint;

	public GameObject parent;

	private float initialParentY;

	bool drag = false;

	public int numberBalls;
	public int levelNo;
	public bool arcadeMode;
	public int ActualWorld;
	
	void Start ()
	{
		transform.FindChild ("Text").GetComponent<TextMesh> ().text = name;
		levelNo = int.Parse (name);
		drag = false;
	}
	
	void OnMouseDown() {
		//screenPoint = Camera.main.WorldToScreenPoint(gameObject.transform.position); // I removed this line to prevent centring 
		_lockedYPosition = screenPoint.y;
		offset = parent.transform.position - Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, screenPoint.z));
		initialParentY = parent.transform.position.y;
	}
	
	void OnMouseDrag() 
	{ 	
			Vector3 curScreenPoint = new Vector3 (Input.mousePosition.x, Input.mousePosition.y, screenPoint.z);
			Vector3 curPosition = Camera.main.ScreenToWorldPoint (curScreenPoint) + offset;
			curPosition.x = parent.transform.position.x;
			if (curPosition.y > 63){
				curPosition.y = 63;
			}
			drag=true;

			parent.transform.position = curPosition;
			//Debug.Log (transform.position);
	}

	void OnMouseUp() {

		//print(Mathf.Abs(initialParentY - parent.transform.position.y));
		if (Mathf.Abs(initialParentY - parent.transform.position.y) < 0.5f) {

			//If its arcade mode we load the normal system as the template comes else we load from the txt files
			if (arcadeMode == true) {
				PlayerPrefs.SetString ("GameType", "Arcade");
				
				int val = levelNo % 4;
				
				LevelManager.patternType = (PatternType)(levelNo / 4);
				
				LevelManager.totalNoOfRows = 10 + val * 2;
				
				LevelManager.minimumNumberOfRows = 3 + val;
				LevelManager.rowAddingInterval = 9 - val;
				
				LevelManager.levelNo = levelNo;
				Application.LoadLevel (3);

			} else {
				
				PlayerPrefs.SetString ("GameType", "Normal");
				LevelManager.patternType = PatternType.TextLevel;
				LevelParser.instance.LoadTextLevel (levelNo, ActualWorld);

				LevelManager.NumberOfBalls = numberBalls;
				LevelManager.rowAddingInterval = 30;
				LevelManager.levelNo = levelNo;
				Application.LoadLevel (3);
			}
		}
	}

}
