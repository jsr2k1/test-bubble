using UnityEngine;
using System.Collections;

//Generates current and next shooting object
public class StrikerManager : MonoBehaviour
{
	GameObject striker;
	Striker strikerScript;
	LevelManager levelManager;
	public GameObject[] specialStrikerPrefabs;
	Transform currentStrikerPosition;
	Transform nextStrikerPosition;
	GameObject currentStrikerObject;
	GameObject nextStrikerObject;
	public Transform thresoldLineTransform;
	private int currentStrikerBallID;
	//private int nextStrikerBallID;
	private bool isSwap = false;
	//public GameState gameState;
	internal bool isFirstObject = true;

	//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

	void Start()
	{
		StartCoroutine(StartLoader());
	}

	//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

	void Update()
	{
		//GameState gameState;
	}

	//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

	IEnumerator StartLoader()
	{    
		yield return new WaitForSeconds(2);
		isSwap = false;
		thresoldLineTransform = GameObject.Find("Thresold Line").transform;
		striker = GameObject.Find("Striker");
		strikerScript = striker.GetComponent<Striker>();

		currentStrikerPosition = GameObject.Find("Current Striker Position").transform;
		nextStrikerPosition = GameObject.Find("Next Striker Position").transform;

		Invoke("UpdateThresoldPosition", .2f);
		Invoke("GenerateStriker", .2f);
		GenerateNextStriker();
	}

	//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

	void UpdateThresoldPosition()
	{
		thresoldLineTransform.position = new Vector3(thresoldLineTransform.position.x, currentStrikerPosition.position.y + .6f, thresoldLineTransform.position.z);
	}

	//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
	//Generates shooting object
	internal void GenerateStriker()
	{        
		striker.transform.position = currentStrikerPosition.position;

		currentStrikerObject = nextStrikerObject;

		currentStrikerObject.transform.parent = striker.transform;
		if(isFirstObject){
			currentStrikerObject.transform.localPosition = Vector3.zero;
			isFirstObject = false;
		}
		strikerScript.currentStrikerObject = currentStrikerObject;
		GenerateNextStriker();  
		
		//Save the balls instantiated at the begining of the level
		SaveBallsID();
	}

	//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

	internal void SaveBallsID()
	{
		currentStrikerBallID = int.Parse(currentStrikerObject.name.Substring(0, 1)) - 1;
		//nextStrikerBallID = int.Parse(nextStrikerObject.name.Substring(0, 1)) - 1;
	}

	//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

	internal void GenerateSwapStriker()
	{     
		isSwap = true;

		striker.transform.position = currentStrikerPosition.position;
		currentStrikerObject = nextStrikerObject;
		currentStrikerObject.transform.parent = striker.transform;

		if(isFirstObject){
			currentStrikerObject.transform.localPosition = Vector3.zero;
			isFirstObject = false;
		}
		strikerScript.currentStrikerObject = currentStrikerObject;
		GenerateNextStriker();   
		SaveBallsID();
	}

	//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

	//Generates Next Shooting Object
	void GenerateNextStriker()
	{ 
		ArrayList remainingObjects = InGameScriptRefrences.playingObjectManager.GetRemainingObjectsNames();
		
		if(remainingObjects != null){
			int index = Random.Range(0, remainingObjects.Count);

			/*if(PlayerPrefs.GetString("GameType").Equals("Normal")){
				//To infinite strike object uncment next 
				//index = LevelParser.instance.GetRandomBall();
				if(isSwap == false){
					index = LevelParser.instance.GetRandomBall();
				} else{
					index = currentStrikerBallID;
				}
			}*/
			//if(PlayerPrefs.GetString("GameType").Equals("Normal")){
				//nextStrikerObject = (GameObject)Instantiate(InGameScriptRefrences.playingObjectGeneration.playingObjectsPrefabs[index], nextStrikerPosition.position, Quaternion.identity);
			//}else{
				nextStrikerObject = (GameObject)Instantiate((GameObject)remainingObjects[index], nextStrikerPosition.position, Quaternion.identity);
			//}
		}
		else{
			//int objectCount = InGameScriptRefrences.playingObjectGeneration.GetObjectCount();
			//int index = Random.Range(0, objectCount);
			int index = Random.Range(0, 5);
			if(PlayerPrefs.GetString("GameType").Equals("Normal")){
				//index = LevelParser.instance.GetRandomBall();
				if(isSwap == false){
						index = LevelParser.instance.GetRandomBall();
				} else{
						index = currentStrikerBallID;
				}
			}
			nextStrikerObject = (GameObject)Instantiate(InGameScriptRefrences.playingObjectGeneration.playingObjectsPrefabs[index], nextStrikerPosition.position, Quaternion.identity);
		}

		nextStrikerObject.tag = "Striker";
		nextStrikerObject.GetComponent<SphereCollider>().enabled = false;
		//nextStrikerObject.GetComponent<SphereCollider>().radius *= .8f;
		iTween.PunchScale(nextStrikerObject, new Vector3(.2f, .2f, .2f), 1f);

		//Switching down the swap flag
		isSwap = false;

		if(!isFirstObject){
			SaveBallsID();
		}
	}

	//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

	//Shoots current playing object in the direction of touch
	internal void Shoot(Vector3 touchedPosition)
	{
		if(strikerScript==null ||strikerScript.isBusy)
			return;

		if(touchedPosition.y < thresoldLineTransform.position.y)
			return;
			
		if(LevelManager.instance.running())
		{
			SoundFxManager.instance.Play(SoundFxManager.instance.shootingSound);

			Vector3 dir = (touchedPosition - currentStrikerPosition.position).normalized;
			strikerScript.Shoot(dir);        

			iTween.MoveTo(nextStrikerObject.gameObject, currentStrikerPosition.position, .4f);
		}
	}
}


