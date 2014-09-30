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
	ArrayList remainingObjects;

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

		//Invoke("UpdateThresoldPosition", .2f);
		//Invoke("GenerateStriker", .2f);
		InGameScriptRefrences.playingObjectManager.ResetAllObjects();
		GenerateNextStriker();
		UpdateThresoldPosition();
		GenerateStriker();
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
		CheckCurrentStrikerColor();
		
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
		int index;

		//WORLD MODE
		if(PlayerPrefs.GetString("GameType").Equals("Normal"))
		{
			if(isSwap){
				index = currentStrikerBallID;
				nextStrikerObject = (GameObject)Instantiate(InGameScriptRefrences.playingObjectGeneration.playingObjectsPrefabs[index], nextStrikerPosition.position, Quaternion.identity);
			}
			else{
				remainingObjects = InGameScriptRefrences.playingObjectManager.GetRemainingObjects();
				if(remainingObjects!=null){
					index = Random.Range(0, remainingObjects.Count);
					nextStrikerObject = (GameObject)Instantiate((GameObject)remainingObjects[index], nextStrikerPosition.position, Quaternion.identity);
				}else{
					nextStrikerObject = (GameObject)Instantiate(InGameScriptRefrences.playingObjectGeneration.playingObjectsPrefabs[0], nextStrikerPosition.position, Quaternion.identity);
				}
			}
		}
		//ARCADE MODE
		else{
			index = Random.Range(0, 5);
			nextStrikerObject = (GameObject)Instantiate(InGameScriptRefrences.playingObjectGeneration.playingObjectsPrefabs[index], nextStrikerPosition.position, Quaternion.identity);
		}

		nextStrikerObject.tag = "Striker";
		nextStrikerObject.GetComponent<SphereCollider>().enabled = false;
		iTween.PunchScale(nextStrikerObject, new Vector3(.2f, .2f, .2f), 1f);

		//Switching down the swap flag
		isSwap = false;

		if(!isFirstObject){
			SaveBallsID();
		}
	}

	//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
	//Verificar que el striker actual todavia es de un color existente en la escena
	//Sino entonces cambiar el color por otro
	void CheckCurrentStrikerColor()
	{
		ArrayList listNames =  InGameScriptRefrences.playingObjectManager.GetRemainingObjectsNames();

		if(listNames!=null && listNames.Count>0){
			if(!listNames.Contains(currentStrikerObject.name)){
				Destroy(currentStrikerObject);
				currentStrikerObject = (GameObject)Instantiate((GameObject)remainingObjects[0], currentStrikerObject.transform.position, Quaternion.identity);
				currentStrikerObject.tag = "Striker";
				currentStrikerObject.GetComponent<SphereCollider>().enabled = false;
				currentStrikerObject.transform.parent = striker.transform;
				strikerScript.currentStrikerObject = currentStrikerObject;
			}
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


