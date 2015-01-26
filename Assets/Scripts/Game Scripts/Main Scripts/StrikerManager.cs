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
	public GameObject currentStrikerObject;
	public GameObject nextStrikerObject;
	public Transform thresoldLineTransform;
	private int currentStrikerBallID;
	//private int nextStrikerBallID;
	private bool isSwap = false;
	//public GameState gameState;
	internal bool isFirstObject = true;
	ArrayList remainingObjects;
	public InputScript inputScript;
	public bool bStartDone=false;

	//bool bMove;
	//float speed=4;

	//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

	void Start()
	{
		bStartDone=false;
		StartCoroutine(StartLoader());
	}

	//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
	//Movemos el next striker a la posicion del striker
	void Update()
	{
		/*if(bMove){
			if(nextStrikerObject!=null){
				if(Vector3.Distance(nextStrikerObject.gameObject.transform.position, currentStrikerPosition.position) > 0.01f){
					float step = speed * Time.deltaTime;
					nextStrikerObject.gameObject.transform.position = Vector3.MoveTowards(nextStrikerObject.gameObject.transform.position, currentStrikerPosition.position, step);
				}else{
					bMove=false;
				}
			}
		}*/
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
		InGameScriptRefrences.playingObjectManager.GetMissionCountTotal();
		GenerateNextStriker();
		UpdateThresoldPosition();
		GenerateStriker();
		bStartDone=true;
		
		if(TutorialManager.instance!=null){
			TutorialManager.instance.ShowTutorial();
		}
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
		if(LevelManager.currentBalls<1 && LevelManager.GameType == LevelManager.GameTypes.NORMAL){
			return;
		}
		striker.transform.position = currentStrikerPosition.position;
		currentStrikerObject = nextStrikerObject;
		currentStrikerObject.transform.SetParent(striker.transform);
		
		if(isFirstObject){
			currentStrikerObject.transform.localPosition = Vector3.zero;
			isFirstObject = false;
		}
		strikerScript.currentStrikerObject = currentStrikerObject;
		GenerateNextStriker();
		CheckCurrentStrikerColor();
		inputScript.CheckColor();
		
		//Save the balls instantiated at the begining of the level
		SaveBallsID();
	}

	//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

	public void SaveBallsID()
	{
		if(currentStrikerObject != null) {
			currentStrikerBallID = int.Parse(currentStrikerObject.name.Substring(0, 1)) - 1;
			//nextStrikerBallID = int.Parse(nextStrikerObject.name.Substring(0, 1)) - 1;
		}
	}

	//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

	internal void GenerateSwapStriker()
	{     
		if(striker==null || currentStrikerPosition==null){
			return;
		}
		isSwap = true;
		striker.transform.position = currentStrikerPosition.position;
		currentStrikerObject = nextStrikerObject;
		currentStrikerObject.transform.SetParent(striker.transform);

		if(isFirstObject){
			currentStrikerObject.transform.localPosition = Vector3.zero;
			isFirstObject = false;
		}
		strikerScript.currentStrikerObject = currentStrikerObject;
		GenerateNextStriker();   
		//SaveBallsID();
		inputScript.CheckColor();
	}

	//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
	//Generates Next Shooting Object
	public void GenerateNextStriker()
	{
		int index;

		//WORLD MODE
		if(LevelManager.GameType == LevelManager.GameTypes.NORMAL){
			if(isSwap){
				index = currentStrikerBallID;
				nextStrikerObject = (GameObject)Instantiate(InGameScriptRefrences.playingObjectGeneration.playingObjectsPrefabs[index], nextStrikerPosition.position, Quaternion.identity);
			}
			else{
				remainingObjects = InGameScriptRefrences.playingObjectManager.GetRemainingObjects();
				if(LevelManager.currentBalls>1){
					if(remainingObjects!=null){
						index = Random.Range(0, remainingObjects.Count);
						nextStrikerObject = (GameObject)Instantiate((GameObject)remainingObjects[index], nextStrikerPosition.position, Quaternion.identity);
					}else{
						nextStrikerObject = (GameObject)Instantiate(InGameScriptRefrences.playingObjectGeneration.playingObjectsPrefabs[0], nextStrikerPosition.position, Quaternion.identity);
					}
				}else{
					nextStrikerObject=null;
				}
			}
		}
		//ARCADE MODE
		else{
			index = isSwap ? currentStrikerBallID : Random.Range(0,6);
			nextStrikerObject = (GameObject)Instantiate(InGameScriptRefrences.playingObjectGeneration.playingObjectsPrefabs[index], nextStrikerPosition.position, Quaternion.identity);
		}
		if(LevelManager.GameType == LevelManager.GameTypes.ARCADE || LevelManager.currentBalls>1){
			nextStrikerObject.tag = "Striker";
			nextStrikerObject.GetComponent<SphereCollider>().enabled = false;
			//iTween.PunchScale(nextStrikerObject, new Vector3(.2f, .2f, .2f), 1f);
		}
		//Switching down the swap flag
		isSwap = false;

		if(!isFirstObject){
			SaveBallsID();
		}
	}

	//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
	//Comprobar que el striker actual todavia es de un color existente en la escena
	//Sino entonces cambiar el color por otro
	public void CheckCurrentStrikerColor()
	{
		if(remainingObjects==null || remainingObjects[0]==null || LevelManager.currentBalls<1){
			return;
		}
		ArrayList listNames = InGameScriptRefrences.playingObjectManager.GetRemainingObjectsNames();
		remainingObjects = InGameScriptRefrences.playingObjectManager.GetRemainingObjects();
		
		if(listNames!=null && listNames.Count>0){
			//CurrentStrikerObject
			if(currentStrikerObject==null || !listNames.Contains(currentStrikerObject.name)){
				if(currentStrikerObject!=null){
					Destroy(currentStrikerObject);
				}
				currentStrikerObject = (GameObject)Instantiate((GameObject)remainingObjects[0], currentStrikerPosition.position, Quaternion.identity);
				currentStrikerObject.tag = "Striker";
				currentStrikerObject.GetComponent<SphereCollider>().enabled = false;
				currentStrikerObject.transform.SetParent(striker.transform);
				strikerScript.currentStrikerObject = currentStrikerObject;
			}
			//NextStrikerObject
			if(LevelManager.currentBalls>1){
				if(nextStrikerObject==null || !listNames.Contains(nextStrikerObject.name)){
					if(nextStrikerObject!=null){
						Destroy(nextStrikerObject);
					}
					nextStrikerObject = (GameObject)Instantiate((GameObject)remainingObjects[0], nextStrikerPosition.position, Quaternion.identity);
					nextStrikerObject.tag = "Striker";
					nextStrikerObject.GetComponent<SphereCollider>().enabled = false;
				}
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
			AudioManager.instance.PlayFxSound(AudioManager.instance.shootingSound);
			//ScoreManagerGame.instance.ResetScore();

			Vector3 dir = (touchedPosition - currentStrikerPosition.position).normalized;
			strikerScript.Shoot(new Vector3(-dir.x, dir.y, 0.0f));

			if(nextStrikerObject!=null){
				iTween.MoveTo(nextStrikerObject.gameObject, currentStrikerPosition.position, .4f);
				//nextStrikerObject.gameObject.transform.position = currentStrikerPosition.position;
				//bMove=true;
			}
		}
	}
	
	//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

	public void FixStrikerPosition(){
		currentStrikerObject.transform.position = currentStrikerPosition.position;
	}
}


