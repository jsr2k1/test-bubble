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
	ArrayList currentAvailableObjects = new ArrayList();
	ArrayList currentAvailableObjectsNames = new ArrayList();

	GameObject bottomMostObject, topMostObject;
	GameObject BottomBoundaryObj, TopBoundaryObj;

	static List<int> listAnimalLevels = new List<int>(new int[]{5, 6, 8, 12, 15, 20, 25, 27, 32, 37, 40, 47, 51, 56, 59, 61, 64, 66, 68, 69});
	
	public enum MissionType{
		Normal=0,
		Animals=1,
		Fruits=2
	}
	
	public static int missionCount;
	public static int missionCountTotal=-1;
	
	bool bRayBall=false;
	
	//public static List<PlayingObject> objectsToBurst;

	////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

	void Awake()
	{
		BottomBoundaryObj = GameObject.Find("Thresold Line");
		TopBoundaryObj = GameObject.Find("Top");
		missionCountTotal=-1;
		//objectsToBurst = new List<PlayingObject>();
	}

	////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

	void Start()
	{
		burstCounter = 0;
		currentObjectName = "";
	}

	////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

	internal void CheckForObjectsFall()
	{
		if(PlayingObjectManager.burstCounter<3){
			AudioManager.instance.PlayFxSound(AudioManager.instance.hookSound);
		}else{
			AudioManager.instance.PlayFxSound(AudioManager.instance.burstSound);
		}

		if(PlayingObjectManager.burstCounter > 2 || Striker.instance.multiBall || Striker.instance.bombBall || Striker.instance.fireBall){
			ScoreManagerGame.instance.SetBonus(true);
		}else{
			ScoreManagerGame.instance.SetBonus(false);
		}

		if(PlayingObjectManager.burstCounter < 3 && !bRayBall && !Striker.instance.multiBall && !Striker.instance.bombBall && !Striker.instance.fireBall){
			ResetAllObjects();
		}else{
			BurstObjects();
			FallDisconnectedObjects();
			bRayBall=false;
		}
		
		if(CheckGameIsFinished()){
			LevelManager.instance.GameIsFinished();
		}else{
			CheckGameIsOver();
		}
	}

	////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
	//Burst all the playing objects whose Burst attribute is marked true.
	void BurstObjects()
	{
		UpdatePlayingObjectsList();

		for(int i = 0; i < allPlayingObjectScripts.Length; i++) {
			if(allPlayingObjectScripts[i].burst || allPlayingObjectScripts[i].burstRayBall){
				allPlayingObjectScripts[i].BurstMe(false);
			}
		}
	}
	
	////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
	//Cambiar el bonus de todos los PlayingObjects
	public void SetBonus()
	{
		if(allPlayingObjectScripts==null){
			return;
		}
		for(int i = 0; i < allPlayingObjectScripts.Length; i++) {
			allPlayingObjectScripts[i].SetNextBonus();
		}
		if(Striker.instance.currentStrikerObject!=null){
			Striker.instance.currentStrikerObject.GetComponent<PlayingObject>().SetNextBonus();
		}
		if(InGameScriptRefrences.strikerManager.nextStrikerObject!=null){
			InGameScriptRefrences.strikerManager.nextStrikerObject.GetComponent<PlayingObject>().SetNextBonus();
		}
	}
/*
	////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
	//Burst all the playing objects whose Burst attribute is marked true.
	void BurstObjects()
	{
		//Striker.instance.currentStrikerObject.GetComponent<PlayingObject>().BurstMe(false);
		Striker.instance.currentStrikerObject.transform.GetChild(0).renderer.enabled = false; //Para que no se vea que la ultima bola que peta es la lanzada
		
		UpdatePlayingObjectsList();
		
		StartCoroutine(BurstOneObject(0));
		
		//for(int i = 0; i < allPlayingObjectScripts.Length; i++) {
		//	if(allPlayingObjectScripts[i].burst){
		//		allPlayingObjectScripts[i].BurstMe(false);
		//	}
		//}
	}

	////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

	IEnumerator BurstOneObject(int i)
	{
//		if(i < allPlayingObjectScripts.Length){
//			if(allPlayingObjectScripts[i].burst){
//				allPlayingObjectScripts[i].BurstMe(false);
//				yield return new WaitForSeconds(0.025f);
//			}
//			StartCoroutine(BurstOneObject(i+1));
//		}
		if(i<objectsToBurst.Count){
			if(objectsToBurst[i].burst){
				objectsToBurst[i].BurstMe(false);
			}
			yield return new WaitForSeconds(0.025f);
			StartCoroutine(BurstOneObject(i+1));
		}
		else{
			FallDisconnectedObjects();
			InGameScriptRefrences.playingObjectManager.GetRemainingObjects();
			InGameScriptRefrences.strikerManager.CheckCurrentStrikerColor();
			Striker.instance.inputScript.CheckColor();
			
			if(CheckGameIsFinished()){
				LevelManager.instance.GameIsFinished();
			}else{
				CheckGameIsOver();
			}
			
			objectsToBurst.Clear();
			
			//InGameScriptRefrences.strikerManager.GenerateNextStriker();
			//InGameScriptRefrences.strikerManager.GenerateStriker();
		}
	}
*/
	////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

	internal void FallDisconnectedObjects()
	{
		if(LevelManager.GameType == LevelManager.GameTypes.ARCADE){
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
			if(allPlayingObjectScripts[i]!=null){
				if(allPlayingObjectScripts[i].isConnected == false){
					allPlayingObjectScripts[i].BurstMe(true);
				}
			}
		}
		Invoke("ResetAllObjects", .02f);
		//ScoreManagerGame.instance.SetBonus(burstCounter);
		ResetAllObjects();
	}

	////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

	internal void UpdatePlayingObjectsList()
	{
		if(LevelManager.gameState == GameState.Pause){
			return;
		}
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

		if(LevelManager.GameType == LevelManager.GameTypes.NORMAL){
			CheckMovePlayingObjects();
		}
	}

	////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

	internal void ResetAllObjects()
	{
		burstCounter = 0;
		currentObjectName = "";

		UpdatePlayingObjectsList();
		
		if(allPlayingObjectScripts==null){
			return;
		}
		for(int i = 0; i < allPlayingObjectScripts.Length; i++) {
			if(allPlayingObjectScripts[i]!=null){
				allPlayingObjectScripts[i].Reset();
				//En el modo Arcade, los vecinos de cada bola no se asignan correctamente
				//Hacemos otra reasignacion para solucionarlo
				if(LevelManager.GameType == LevelManager.GameTypes.ARCADE){
					allPlayingObjectScripts[i].RefreshAdjacentObjectList();
				}
			}
		}
	}
	
	////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
	//Dependiendo de la mision del nivel actual, comprobamos si se ha superado el nivel o no
	bool CheckGameIsFinished()
	{
		if(LevelManager.GameType == LevelManager.GameTypes.ARCADE){
			return false;
		}
		bool res=false;
		if(GetLevelMission()==MissionType.Normal){
			//if(allPlayingObjectScripts.Length==10 && LevelManager.instance.totalNumberOfRowsLeft == 0){
			if(CheckRemainingObjects() && LevelManager.instance.totalNumberOfRowsLeft == 0){
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
	//Comprobar si ya no tenemos mas disparos
	void CheckGameIsOver()
	{
		if(LevelManager.GameType == LevelManager.GameTypes.NORMAL && LevelManager.currentBalls == 0){
			StartCoroutine(LevelManager.instance.Finishing());
		}
	}
	
	////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
	//Creamos una funcion para contar objectos ya que los objetos piedra tambien tienen que tener el tag "Playing Object"
	bool CheckRemainingObjects()
	{
		int counter=0;
		foreach(PlayingObject obj in allPlayingObjectScripts){
			if(obj!=null && obj.name!="StoneBall(Clone)" && obj.name!="FireBall" && obj.isConnected){
				counter++;
			}	
		}
		return(counter == 10);
	}
	
	////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
	
	public static MissionType GetLevelMission()
	{
		if(listAnimalLevels.IndexOf(LevelManager.levelNo) > -1){
			return MissionType.Animals;
		}else{
			return MissionType.Normal;
		}
	}
	
	////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
	
	public void GetMissionCountTotal()
	{
		if(missionCountTotal<0){
			missionCountTotal=0;
			for(int i = 0; i < allPlayingObjectScripts.Length; i++){
				if(allPlayingObjectScripts[i].name=="ParrotBall(Clone)"){
					missionCountTotal++;
				}
			}
		}
	}
	
	////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

	bool AllAnimalsAreFree()
	{
		int count=0; //Loros que quedan en la pantalla
		for(int i = 0; i < allPlayingObjectScripts.Length; i++){
			if(allPlayingObjectScripts[i] && allPlayingObjectScripts[i].name=="ParrotBall(Clone)" && allPlayingObjectScripts[i].isConnected){
				count++;
			}
		}
		missionCount = missionCountTotal-count;
		MissionCounter.instance.SetCounter();

		return (missionCount==missionCountTotal);
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

			if(!currentAvailableObjectsNames.Contains(tempName) && objects[i].name!="DummyBall(Clone)" && objects[i].name!="StoneBall(Clone)" && playObj.isConnected){
				currentAvailableObjectsNames.Add(tempName);
				GetObjectReference(tempName);
			}
		}

		if(currentAvailableObjects.Count == 0){
			return null;
		}else{
			return currentAvailableObjects;
		}
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
	void CheckMovePlayingObjects()
	{
		if(bottomMostObject==null || topMostObject==null)
			return;

		float distBottom, distTop;

		distBottom = Mathf.Abs(bottomMostObject.transform.position.y - (BottomBoundaryObj.transform.position.y + thresholdOffsetWorldMode));
		distTop = Mathf.Abs(topMostObject.transform.position.y - TopBoundaryObj.transform.position.y);

		//Las bolas estan demasiado abajo -> las subimos
		//Este codigo tambien se usa al cargar los niveles grandes. En ese caso, ajustamos el tiempo para que no vaya tan rapido.
		if(bottomMostObject.transform.position.y < BottomBoundaryObj.transform.position.y + InGameScriptRefrences.playingObjectManager.thresholdOffsetWorldMode)
		{
			float tiempo;
			if(distBottom>1.0f){
				tiempo = InGameScriptRefrences.playingObjectGeneration.fallDownTime*(distBottom/4.0f);
			}else{
				tiempo=1.0f;
			}
			float currentY = InGameScriptRefrences.playingObjectGeneration.gameObject.transform.position.y;
			iTween.MoveTo(InGameScriptRefrences.playingObjectGeneration.gameObject, new Vector3(0, currentY+distBottom, 0), tiempo);
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
	
	////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
	//Destruye todas las bolas que estan a la misma altura que la bola rayo
	public void DestroyPlayingObjsRayBall(float y_pos)
	{
		bRayBall=true;
		
		for(int i=0;i<allPlayingObjectScripts.Length;i++){
			if(allPlayingObjectScripts[i]!=null){
				if(Mathf.Abs(allPlayingObjectScripts[i].transform.position.y - y_pos) < 0.1f){
					allPlayingObjectScripts[i].burstRayBall = true;
					allPlayingObjectScripts[i].isTraced = true;
					PlayingObjectManager.burstCounter++;
				} 
			}
		}
	}
}








