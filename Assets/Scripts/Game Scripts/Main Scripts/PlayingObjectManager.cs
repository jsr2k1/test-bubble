using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PlayingObjectManager : MonoBehaviour
{
	internal static int burstCounter = 0;
	public static string currentObjectName = "";
	public PlayingObject[]topRowObjects;

	float thresholdOffsetWorldMode = 2.0f;

	PlayingObject[] allPlayingObjectScripts;
	ArrayList playingObjectList;
	ArrayList currentAvailableObjects = new ArrayList();
	ArrayList currentAvailableObjectsNames = new ArrayList();

	GameObject bottomMostObject, topMostObject;
	GameObject BottomBoundaryObj, TopBoundaryObj;
	
	public enum MissionType{
		Normal=0,
		Animals=1,
		Fruits=2
	}
	
	public static int missionCount;
	public static int missionCountTotal=-1;

	////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

	void Awake()
	{
		BottomBoundaryObj = GameObject.Find("Thresold Line");
		TopBoundaryObj = GameObject.Find("Top");
		missionCountTotal=-1;
	}

	////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

	void Start()
	{
		burstCounter = 0;
		currentObjectName = "";
		playingObjectList = new ArrayList();        
		RefreshPlayingObjectList();
		
		UpdatePlayingObjectsList();
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
		if(PlayingObjectManager.burstCounter < 3 && !Striker.instance.multiBall){
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
		if(PlayerPrefs.GetString("GameType") == "Arcade"){
			if(PlayingObjectGeneration.isBusy) {
				Invoke("FallDisconnectedObjects", .1f);
				return;
			}
		}
		//StartCoroutine(_FallDisconnectedObjects());
		_FallDisconnectedObjects();
	}

	////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
	//No funciona bien con corutinas pq entonces cuando hay bolas cayendo el striker sigue generando
	//bolas de colores que en realidad ya no van a estar en la escena
	//IEnumerator _FallDisconnectedObjects()
	void _FallDisconnectedObjects()
	{
		for(int i = 0; i < allPlayingObjectScripts.Length; i++){
			allPlayingObjectScripts[i].isConnected = false;
		}
		//yield return new WaitForSeconds(.01f);

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
		//ResetAllObjects();
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

		if(PlayerPrefs.GetString("GameType") == "Normal"){
			CheckMovePlayingObjects();
		}
	}

	////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

	internal void ResetAllObjects()
	{
		burstCounter = 0;
		currentObjectName = "";

		UpdatePlayingObjectsList();
		GetMissionCountTotal();

		for(int i = 0; i < allPlayingObjectScripts.Length; i++) {
			if(allPlayingObjectScripts[i]!=null){
				allPlayingObjectScripts[i].Reset();
				//En el modo Arcade, los vecinos de cada bola no se asignan correctamente
				//Hacemos otra reasignacion para solucionarlo
				allPlayingObjectScripts[i].RefreshAdjacentObjectList();
			}
		}
		if(CheckGameIsFinished()){
			LevelManager.instance.GameIsFinished();
		}
	}
	
	////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
	//Dependiendo de la mision del nivel actual, comprobamos si se ha superado el nivel o no
	bool CheckGameIsFinished()
	{
		if(PlayerPrefs.GetString("GameType").Equals("Arcade")){
			return false;
		}
		bool res=false;
		if(GetLevelMission()==MissionType.Normal){
			if(allPlayingObjectScripts.Length == 10 && LevelManager.instance.totalNumberOfRowsLeft == 0){
				res=true;
			}
		}else if(GetLevelMission()==MissionType.Animals){
			if(AllAnimalsAreFree()){
				res=true;
			}
		}
		return res;
	}
	
	////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
	
	public static MissionType GetLevelMission()
	{
		List<int> listAnimalLevels = new List<int>(new int[]{4, 6, 8, 12});
		if(listAnimalLevels.IndexOf(LevelManager.levelNo) > -1){
			return MissionType.Animals;
		}else{
			return MissionType.Normal;
		}
	}
	
	////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
	
	void GetMissionCountTotal()
	{
		if(missionCountTotal<0){
			missionCountTotal=0;
			for(int i = 0; i < allPlayingObjectScripts.Length; i++){
				if(allPlayingObjectScripts[i].name=="parrot_ball(Clone)"){
					missionCountTotal++;
				}
			}
		}
	}
	
	////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

	bool AllAnimalsAreFree()
	{
		int count=0;
		for(int i = 0; i < allPlayingObjectScripts.Length; i++){
			if(allPlayingObjectScripts[i].name=="parrot_ball(Clone)"){
				count++;
			}
		}
		missionCount = missionCountTotal-count;
		return (missionCount==missionCountTotal);
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

	internal ArrayList GetRemainingObjects()
	{
		if(allPlayingObjectScripts == null){
			return null;
		}
		currentAvailableObjectsNames = new ArrayList();
		currentAvailableObjects = new ArrayList();

		GameObject[] objects = GameObject.FindGameObjectsWithTag("Playing Object");

		for(int i = 0; i < objects.Length; i++)
		{
			string tempName = objects[i].name;
			PlayingObject playObj = objects[i].GetComponent<PlayingObject>();
			if(!currentAvailableObjectsNames.Contains(tempName) && objects[i].name!="777(Clone)" && objects[i].name!="888(Clone)" && playObj.isConnected){
				currentAvailableObjectsNames.Add(tempName);
				GetObjectReference(tempName);
			}
		}
		if(currentAvailableObjects.Count == 0){
			return null;
		}
		return currentAvailableObjects;
	}

	////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

	public ArrayList GetRemainingObjectsNames()
	{
		return currentAvailableObjectsNames;
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
		if(bottomMostObject==null || topMostObject==null)
			return;

		float distBottom, distTop;

		distBottom = Mathf.Abs(bottomMostObject.transform.position.y - (BottomBoundaryObj.transform.position.y + thresholdOffsetWorldMode));
		distTop = Mathf.Abs(topMostObject.transform.position.y - TopBoundaryObj.transform.position.y);

		//Las bolas estan demasiado abajo -> las subimos
		if(bottomMostObject.transform.position.y < BottomBoundaryObj.transform.position.y + InGameScriptRefrences.playingObjectManager.thresholdOffsetWorldMode)
		{
			float currentY = InGameScriptRefrences.playingObjectGeneration.gameObject.transform.position.y;
			iTween.MoveTo(InGameScriptRefrences.playingObjectGeneration.gameObject, new Vector3(0, currentY+distBottom, 0), InGameScriptRefrences.playingObjectGeneration.fallDownTime);
		}
		else if(topMostObject.transform.position.y > TopBoundaryObj.transform.position.y)
		{
			float currentY = InGameScriptRefrences.playingObjectGeneration.gameObject.transform.position.y;

			//Las bolas estan demasiado altas -> las bajamos
			//El limite SUPERIOR nos marca la distancia que podemos bajar todas las bolas
			if(distTop < distBottom){
				iTween.MoveTo(InGameScriptRefrences.playingObjectGeneration.gameObject, new Vector3(0, currentY-distTop, 0), InGameScriptRefrences.playingObjectGeneration.fallDownTime);
			}
			//Las bolas estan demasiado altas -> las bajamos
			//El limite INFERIOR nos marca la distancia que podemos bajar todas las bolas
			else if(distBottom > 0.05f){
				iTween.MoveTo(InGameScriptRefrences.playingObjectGeneration.gameObject, new Vector3(0, currentY-distBottom, 0), InGameScriptRefrences.playingObjectGeneration.fallDownTime);
			}
		}
	}
}








