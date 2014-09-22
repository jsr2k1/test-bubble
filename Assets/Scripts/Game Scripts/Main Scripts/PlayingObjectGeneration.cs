using UnityEngine;
using System.Collections;

public class PlayingObjectGeneration : MonoBehaviour
{
	public static PlayingObjectGeneration instance;
	internal int numberOfObjectsInARow = 9;
	public float objectGap; // gap between two playing objects
	public GameObject[] playingObjectsPrefabs; //6 different Playing objects prefabs

	internal float rowGap;
	internal float startingXPos = 2.5f;
	float currentYPos = 0;
	Transform thresoldLineTransform;
	public static bool isBusy = false;
	float rowStartingPos;
	float objectGenerationheight;
	bool isStarting = true;

	//Row ad for the top limit vars
	private int rowAddCount = 0;
	//public Transform limit; --las stickyballs de momento no se usan


	////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

	void Awake()
	{
		instance = this;
	}

	////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

	void Start()
	{   
		numberOfObjectsInARow = 9;
		//limit = GameObject.Find("StickyBalls").transform;
		//Checking for the GameType to put the correct number of rows per level
		/*if(PlayerPrefs.GetString("GameType").Equals("Normal"))

		numberOfObjectsInARow = 8;*/

		objectGenerationheight = transform.position.y;
		rowStartingPos = startingXPos;
		isBusy = false;
		thresoldLineTransform = GameObject.Find("Thresold Line").transform;
		rowGap = objectGap;

		Invoke("InitiateRowAdd", .1f);
		Invoke("FalsenIsStarting", 2f);
	}

	////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

	int rowCounter = 0;
	int numberOfRowsGenerated = 0;
	float fallDownTime;
	float currentRowAddingInterval;

	void FalsenIsStarting()
	{
		isStarting = false;
	}
	
	////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

	//Ensures that there is atleast a minimum no of rows in the screen
	internal void CheckForMinRowCount(GameObject bottomMostObject)
	{
		int numberOfRowsPresent = 0;

		if(bottomMostObject == null) {
				numberOfRowsPresent = 0;
		} else
				numberOfRowsPresent = Mathf.RoundToInt((objectGenerationheight - bottomMostObject.transform.position.y) / rowGap);

		rowCounter = numberOfRowsPresent;

		if(rowCounter < Mathf.Min(6, LevelManager.minimumNumberOfRows) && LevelManager.instance.totalNumberOfRowsLeft > 0) {
				CancelInvoke("InitiateRowAdd");
				InitiateRowAdd();
		}
	}
	
	////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

	void InitiateRowAdd()
	{
		rowAddCount++;

		//Moving down the limiter 
		if(PlayerPrefs.GetString("GameType").Equals("Normal") && rowAddCount > 14) {
				Invoke("MoveLimiter", .12f);
		}

		if(LevelManager.gameState == GameState.GameFinish || LevelManager.gameState == GameState.GameOver)
				return;

		if(LevelManager.instance.totalNumberOfRowsLeft == 0) {
				currentRowAddingInterval = LevelManager.rowAddingInterval;
				//iTween.MoveBy(gameObject, new Vector3(0, -rowGap, 0), fallDownTime);
				//Invoke("InitiateRowAdd", currentRowAddingInterval);
				Invoke("CheckForGameOver", fallDownTime);
				return;
		}

		if(isBusy) {
				Invoke("InitiateRowAdd", .1f);
		} else {
				StartCoroutine(AddRow());
		}
	}
	
	////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

	//Adds new row at top of the screen and move the objects down by rowgap
	IEnumerator AddRow()
	{        
		isBusy = true;
		InGameScriptRefrences.playingObjectManager.topRowObjects = new PlayingObject[numberOfObjectsInARow];

		float x;
		if(rowStartingPos == startingXPos)
				rowStartingPos = startingXPos - objectGap * .5f;
		else
				rowStartingPos = startingXPos;

		x = rowStartingPos;

		//int objectCount = GetObjectCount();

		numberOfRowsGenerated++;

		GameObject tempObject;

		for(int i = 0; i < numberOfObjectsInARow; i++) {
				//int index = Random.Range(0, objectCount);
				int index = Random.Range(0, 6); //Queremos un random de 0-5 pq tenemos 6 bolitas

				if(PlayerPrefs.GetString("GameType").Equals("Normal")) {

						//Checking the unpair rows that only contains 8 objects
						if(rowCounter % 2 == 1 || rowCounter % 2 == 0 && i <= 7) {
								//print(rowCounter + "|" + i);
								index = LevelParser.instance.GetBallColor(rowCounter, i);
						}
				}

				Vector3 pos = new Vector3(x, currentYPos, 0);

				if(ObjectFormationPattern.instance.ShouldAddObject(i, rowCounter)) {
						tempObject = (GameObject)Instantiate(playingObjectsPrefabs[index], Vector3.zero, Quaternion.identity);
						tempObject.transform.parent = transform;
						tempObject.transform.localPosition = pos;
						tempObject.GetComponent<PlayingObject>().RefreshAdjacentObjectList();
						InGameScriptRefrences.playingObjectManager.topRowObjects [i] = tempObject.GetComponent<PlayingObject>();
				}

				x -= objectGap;


				if(i % 4 == 0)
						yield return new WaitForSeconds(.02f);
		}

		isBusy = false;
		iTween.Defaults.easeType = iTween.EaseType.linear;

		currentYPos = numberOfRowsGenerated * rowGap;
		iTween.MoveTo(gameObject, new Vector3(0, objectGenerationheight - currentYPos, 0), fallDownTime);

		rowCounter++;
		if(rowCounter >= LevelManager.minimumNumberOfRows) {
				currentRowAddingInterval = LevelManager.rowAddingInterval;
				fallDownTime = .5f;
		} else {
				currentRowAddingInterval = 0f;

				if(isStarting)
						fallDownTime = .2f;
				else
						fallDownTime = .5f;
		}
		LevelManager.instance.totalNumberOfRowsLeft--;
		Invoke("InitiateRowAdd", currentRowAddingInterval);
		Invoke("CheckForGameOver", .5f);
	}
	
	////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

	internal int GetObjectCount()
	{
		int objectCount = playingObjectsPrefabs.Length;
		return objectCount;
	}    
	
	////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
	//If any of the objects touches threshold line. The game is over.
	private void CheckForGameOver()
	{
		GameObject[] objects = GameObject.FindGameObjectsWithTag("Playing Object");

		for(int i = 0; i < objects.Length; i++)
		{
			if(objects[i].transform.position.y < thresoldLineTransform.position.y)
			{
				//Cuando una bola atraviesa el umbral inferior no se termina el juego sino que las bolas suben
				//LevelManager.instance.GameIsOver();
				iTween.MoveBy(gameObject, new Vector3(0, rowGap, 0), fallDownTime);
				break;
			}
		}
	}

	////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

	private void MoveLimiter()
	{
		//limit.Translate(Vector3.down * 0.6f);
	}
}




