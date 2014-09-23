using UnityEngine;
using System.Collections;

public class PlayingObjectManager : MonoBehaviour
{
	internal static int burstCounter = 0;
	public static string currentObjectName = "";
	public PlayingObject[]topRowObjects;
	public float thresholdOffsetWorldMode = 1.5f;

	PlayingObject[] allPlayingObjectScripts;
	ArrayList playingObjectList;
	ArrayList currentAvailableObjects = new ArrayList();

	GameObject bottomMostObject, topMostObject;
	GameObject BottomBoundaryObj, TopBoundaryObj;

	////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

	void Awake()
	{
		BottomBoundaryObj = GameObject.Find("Thresold Line");
		TopBoundaryObj = GameObject.Find("Top");
	}

	////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

	void Start()
	{
		burstCounter = 0;
		currentObjectName = "";
		playingObjectList = new ArrayList();        
		RefreshPlayingObjectList();
	}

	////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

	//Refreshes PlaingObjectList
	public void RefreshPlayingObjectList()
	{
		GameObject[] objects = GameObject.FindGameObjectsWithTag("Playing Object");

		for(int i = 0; i < objects.Length; i++) {
			playingObjectList.Add(objects[i].GetComponent<PlayingObject>());
		}
	}

	////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

	internal void CheckForObjectsFall()
	{
		if(PlayingObjectManager.burstCounter < 3){
			ResetAllObjects();
			return;
		}
		BurstObjects();
		FallDisconnectedObjects();
	}

	////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
	//Burst all the playing objects whose Burst attribute is marked true.
	void BurstObjects()
	{
		UpdatePlayingObjectsList();

		for(int i = 0; i < allPlayingObjectScripts.Length; i++) {
			if(allPlayingObjectScripts[i].burst){
				allPlayingObjectScripts[i].BurstMe(false);
			}
		}
	}

	////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

	internal void FallDisconnectedObjects()
	{
		if(PlayingObjectGeneration.isBusy) {
			Invoke("FallDisconnectedObjects", .1f);
			return;
		}
		StartCoroutine(_FallDisconnectedObjects());
	}

	////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

	IEnumerator _FallDisconnectedObjects()
	{
		for(int i = 0; i < allPlayingObjectScripts.Length; i++){
			allPlayingObjectScripts[i].isConnected = false;
		}
		yield return new WaitForSeconds(.01f);

		for(int i = 0; i < topRowObjects.Length; i++){
			if(topRowObjects[i]){
				topRowObjects[i].TraceForConnection();            
			}
		}
		for(int i = 0; i < allPlayingObjectScripts.Length; i++){
			if(allPlayingObjectScripts[i]){
				if(allPlayingObjectScripts[i].isConnected == false){
					allPlayingObjectScripts[i].BurstMe(true);
				}
			}
		}
		Invoke("ResetAllObjects", .02f); 
	}

	////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

	internal void UpdatePlayingObjectsList()
	{
		GameObject[] objects = GameObject.FindGameObjectsWithTag("Playing Object");
		allPlayingObjectScripts = new PlayingObject[objects.Length];

		bottomMostObject = null;
		topMostObject = null;

		if(objects.Length > 0){
			bottomMostObject = objects[0];
			topMostObject = objects[0];
		}
		for(int i = 0; i < objects.Length; i++)
		{
			allPlayingObjectScripts[i] = objects[i].GetComponent<PlayingObject>();

			if(objects[i].transform.position.y < bottomMostObject.transform.position.y){
				bottomMostObject = objects[i];
			}
			if(objects[i].transform.position.y > topMostObject.transform.position.y){
				topMostObject = objects[i];
			}
		}
		InGameScriptRefrences.playingObjectGeneration.CheckForMinRowCount(bottomMostObject);

		CheckMovePlayingObjects();
	}

	////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

	internal void ResetAllObjects()
	{
		burstCounter = 0;
		currentObjectName = "";

		UpdatePlayingObjectsList();

		for(int i = 0; i < allPlayingObjectScripts.Length; i++) {
			allPlayingObjectScripts[i].Reset();
		}
		if(allPlayingObjectScripts.Length == 9 && LevelManager.instance.totalNumberOfRowsLeft == 0){
			LevelManager.instance.GameIsFinished();
		}
	}

	////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

	internal void FallAllPlayingObjects()
	{
		UpdatePlayingObjectsList();

		for(int i = 0; i < allPlayingObjectScripts.Length; i++){
			allPlayingObjectScripts[i].GameOverFall();
		}
	}

	////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

	internal ArrayList GetRemainingObjectsNames()
	{
		if(allPlayingObjectScripts == null){
			return null;
		}
		if(allPlayingObjectScripts.Length > 7){
			return null;
		}
		ArrayList currentAvailableObjectsName = new ArrayList();
		currentAvailableObjects = new ArrayList();

		GameObject[] objects = GameObject.FindGameObjectsWithTag("Playing Object");

		for(int i = 0; i < objects.Length; i++)
		{
			string tempName = objects[i].name;
			if(!currentAvailableObjectsName.Contains(tempName)){
				currentAvailableObjectsName.Add(tempName);
				GetObjectReference(tempName);
			}
		}
		if(currentAvailableObjects.Count == 0){
			return null;
		}
		return currentAvailableObjects;
	}

	////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

	void GetObjectReference(string name)
	{       
		for(int i = 0; i < InGameScriptRefrences.playingObjectGeneration.playingObjectsPrefabs.Length; i++)
		{
			if(InGameScriptRefrences.playingObjectGeneration.playingObjectsPrefabs[i].name == name.Substring(0, 3))
			{
				currentAvailableObjects.Add(InGameScriptRefrences.playingObjectGeneration.playingObjectsPrefabs[i]);
			}
		}
	}

	////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
	//Comprobar si hay que subir o bajar todas las bolas de golpe en funcion de la distancias con los limites
	public void CheckMovePlayingObjects()
	{
		float distBottom, distTop;

		distBottom = Mathf.Abs(bottomMostObject.transform.position.y - (BottomBoundaryObj.transform.position.y + thresholdOffsetWorldMode));
		distTop = Mathf.Abs(topMostObject.transform.position.y - TopBoundaryObj.transform.position.y);

		if(topMostObject.transform.position.y > TopBoundaryObj.transform.position.y)
		{
			float currentY = InGameScriptRefrences.playingObjectGeneration.gameObject.transform.position.y;

			//El limite superior nos marca la distancia que podemos bajar todas las bolas
			if(distTop < distBottom){
				iTween.MoveTo(InGameScriptRefrences.playingObjectGeneration.gameObject, new Vector3(0, currentY-distTop, 0), InGameScriptRefrences.playingObjectGeneration.fallDownTime);
			}
			//Las bolas estan demasiado altas -> las bajamos
			else if(distBottom > 0.5f){
				iTween.MoveTo(InGameScriptRefrences.playingObjectGeneration.gameObject, new Vector3(0, currentY-distBottom, 0), InGameScriptRefrences.playingObjectGeneration.fallDownTime);
			}
		}
	}
}








