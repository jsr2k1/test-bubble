using UnityEngine;
using System.Collections;

public class PlayingObjectManager : MonoBehaviour
{
		internal static int burstCounter = 0;
		public static string currentObjectName = "";
		ArrayList playingObjectList;
		public PlayingObject[]topRowObjects;
	
		void Start()
		{
				burstCounter = 0;
				currentObjectName = "";
				playingObjectList = new ArrayList();        
				RefreshPlayingObjectList();
	
		}

		//Refreshes PlaingObjectList
		public void RefreshPlayingObjectList()
		{
				GameObject[] objects = GameObject.FindGameObjectsWithTag("Playing Object");

				for(int i = 0; i < objects.Length; i++) {
						playingObjectList.Add(objects[i].GetComponent<PlayingObject>());
				}
		}

		PlayingObject[] allPlayingObjectScripts;

		internal void CheckForObjectsFall()
		{
				if(PlayingObjectManager.burstCounter < 3) {
						ResetAllObjects();
						return;
				}

				BurstObjects();
				FallDisconnectedObjects();
		}

		//Burst all the playing objects whose Burst attribute is marked true.
		void BurstObjects()
		{
				UpdatePlayingObjectsList();
				for(int i = 0; i < allPlayingObjectScripts.Length; i++) {
						if(allPlayingObjectScripts[i].burst)
								allPlayingObjectScripts[i].BurstMe(false);
				}
		}

		internal void FallDisconnectedObjects()
		{
				if(PlayingObjectGeneration.isBusy) {
						Invoke("FallDisconnectedObjects", .1f);
						return;
				}

				StartCoroutine(_FallDisconnectedObjects());
		}

		IEnumerator _FallDisconnectedObjects()
		{
				for(int i = 0; i < allPlayingObjectScripts.Length; i++) {
						allPlayingObjectScripts[i].isConnected = false;
				}
				yield return new WaitForSeconds(.01f);

				for(int i = 0; i < topRowObjects.Length; i++) {
						if(topRowObjects[i])
								topRowObjects[i].TraceForConnection();            
				}

				for(int i = 0; i < allPlayingObjectScripts.Length; i++) {
						if(allPlayingObjectScripts[i]) {
								if(allPlayingObjectScripts[i].isConnected == false)
										allPlayingObjectScripts[i].BurstMe(true);
						}
				}

				Invoke("ResetAllObjects", .02f); 
		}

		internal void UpdatePlayingObjectsList()
		{
				GameObject[] objects = GameObject.FindGameObjectsWithTag("Playing Object");
				allPlayingObjectScripts = new PlayingObject[objects.Length];

				GameObject bottomMostObject = null;

				if(objects.Length > 0)
						bottomMostObject = objects[0];

                
				for(int i = 0; i < objects.Length; i++) {
						allPlayingObjectScripts[i] = objects[i].GetComponent<PlayingObject>();
						if(objects[i].transform.position.y < bottomMostObject.transform.position.y)
								bottomMostObject = objects[i];
				}

				InGameScriptRefrences.playingObjectGeneration.CheckForMinRowCount(bottomMostObject);
		}

		internal void ResetAllObjects()
		{
				burstCounter = 0;
				currentObjectName = "";

				UpdatePlayingObjectsList();

				for(int i = 0; i < allPlayingObjectScripts.Length; i++) {
						allPlayingObjectScripts[i].Reset();
				}

				if(allPlayingObjectScripts.Length == 9 && LevelManager.instance.totalNumberOfRowsLeft == 0)
						LevelManager.instance.GameIsFinished();
		}

		internal void FallAllPlayingObjects()
		{
				UpdatePlayingObjectsList();

				for(int i = 0; i < allPlayingObjectScripts.Length; i++) {
						allPlayingObjectScripts[i].GameOverFall();
				}
		}

		ArrayList currentAvailableObjects = new ArrayList();

		internal ArrayList GetRemainingObjectsNames()
		{
				if(allPlayingObjectScripts == null)
						return null;

				if(allPlayingObjectScripts.Length > 7)
						return null;

				ArrayList currentAvailableObjectsName = new ArrayList();
				currentAvailableObjects = new ArrayList();

				GameObject[] objects = GameObject.FindGameObjectsWithTag("Playing Object");

				for(int i = 0; i < objects.Length; i++) {
						string tempName = objects[i].name;
						if(!currentAvailableObjectsName.Contains(tempName)) {
								currentAvailableObjectsName.Add(tempName);
								GetObjectRefrence(tempName);
						}
				}

				if(currentAvailableObjects.Count == 0)
						return null;

				return currentAvailableObjects;

		}

		void GetObjectRefrence(string name)
		{       
				for(int i = 0; i < InGameScriptRefrences.playingObjectGeneration.playingObjectsPrefabs.Length; i++) {            
						if(InGameScriptRefrences.playingObjectGeneration.playingObjectsPrefabs[i].name == name.Substring(0, 3)) {                
								currentAvailableObjects.Add(InGameScriptRefrences.playingObjectGeneration.playingObjectsPrefabs[i]);
						}
				}
		}

   
}
