using UnityEngine;
using System.Collections;

public class PlayingObject : MonoBehaviour
{
	static bool multiBall = false;
	public GameObject burstParticle;
	static int numberOfAdjacentObjects = 6;

	//Angles of neighbour objects (this will be used while detecting neighbour objects through raycast
	public static float[]adjacentObjectAngles = { 0, 60, 120, 180, 240, 300 };
	public PlayingObject[] adjacentPlayingObjects; // List of Adjacent playing objects

	internal bool isTraced = false;
	internal bool burst = false;
	internal bool isConnected = true;
	internal bool isTracedForConnection = false;
	bool isDestroyed = false;
	static Transform thresoldLineTransform; //The lower bottom point
	public LayerMask layerMask;

	void Start ()
	{
			if (thresoldLineTransform == null)
					thresoldLineTransform = GameObject.Find ("Thresold Line").transform;
	}

	internal void Reset ()
	{
			isTracedForConnection = false;
			isTraced = false;
			burst = false;
	}

	//Reassign Adjacent Playing Objects of the current Object
	internal void RefreshAdjacentObjectList ()
	{
			adjacentPlayingObjects = new PlayingObject[numberOfAdjacentObjects];

			for (int i = 0; i < numberOfAdjacentObjects; i++) {
					PlayingObject temp = GetObjectInTheDirection (adjacentObjectAngles [i]);
					adjacentPlayingObjects [i] = temp;
   
					if (temp != null) {
							if (i < 3) {
									temp.adjacentPlayingObjects [i + 3] = this;
							} else {
									temp.adjacentPlayingObjects [i - 3] = this;
							}
					}
			}
	}    

	//Get reference of playing object in the given direction
	PlayingObject GetObjectInTheDirection (float angle)
	{
			RaycastHit hit;
			float maxDistance = .6f;

			float radAngle = angle * Mathf.Deg2Rad;

			Vector3 dir = new Vector3 (Mathf.Cos (radAngle), Mathf.Sin (radAngle), 0);

			if (Physics.Raycast (transform.position, dir, out hit, maxDistance, layerMask)) {
					if (hit.collider.gameObject.tag == "Playing Object") {
							return hit.collider.gameObject.GetComponent<PlayingObject> ();
					}
					//print (hit.collider.gameObject.name); // coz of striker
			}

			return null;
	}

	//Reassign Neighbours of Adjacent Playing Objects
	void RefreshNeighbourAdjacentList ()
	{
			for (int i = 0; i < numberOfAdjacentObjects; i++) {
					if (adjacentPlayingObjects [i]) {
							if (i < 3) {
									adjacentPlayingObjects [i].adjacentPlayingObjects [i + 3] = null;
							} else {
									adjacentPlayingObjects [i].adjacentPlayingObjects [i - 3] = null;
							}
					}
			}     
	}    

	//Destroy the Playing Object after right combination is formed
	internal void BurstMe (bool fall)
	{
			if (isDestroyed)
					return;

			isDestroyed = true;

			Destroy (GetComponent<SphereCollider> ());

			RefreshNeighbourAdjacentList ();
			gameObject.tag = "Untagged";

			if (fall) {
					if (burst == false) {
							ScoreManager.instance.DisplayScorePopup (10, transform);
							rigidbody.useGravity = true;
							rigidbody.isKinematic = false;
							rigidbody.AddForce (new Vector3 (0, Random.Range (1.5f, 2.5f), 0), ForceMode.VelocityChange);
							GetComponent<RotationScript> ().enabled = true;
							Destroy (gameObject, 3f);
					} else {                
							Destroy (gameObject);
					}
			} else {
					ScoreManager.instance.DisplayScorePopup (10, transform);
   
					Instantiate (burstParticle, transform.position, Quaternion.identity);
					Destroy (gameObject);
			}

	}

	////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

	//Trace All the Playing objects connected to this Playing Object and match for the fall/burst.
	void Trace()
	{
		if (!isTraced)
		{
			isTraced = true;
			AssignBurst ();

			PlayingObjectManager.burstCounter++;
			iTween.PunchScale (gameObject, new Vector3 (.2f, .2f, .2f), 1f);

			for(int i = 0; i < numberOfAdjacentObjects; i++)
			{
				if(adjacentPlayingObjects[i])
				{
					if(adjacentPlayingObjects[i].name == PlayingObjectManager.currentObjectName){
						adjacentPlayingObjects[i].Trace();
					}else{
						adjacentPlayingObjects[i].isTraced = true;
						iTween.PunchScale(adjacentPlayingObjects[i].gameObject, new Vector3(.2f, .2f, .2f), 1f);
					}
				}
			}
		}
	}

	////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

	//Checks if the Playing object is connected to the top , If its free it will fall/destroyed.
	internal void TraceForConnection ()
	{
			if (isTracedForConnection || burst)
					return;

			isTracedForConnection = true;

			isConnected = true;

			for (int i = 0; i < numberOfAdjacentObjects; i++) {
					if (adjacentPlayingObjects [i]) {
							adjacentPlayingObjects [i].TraceForConnection ();
					}
			}
	}

	internal void ObjectCollidedWithOtherObject (GameObject collidedObject)
	{

			if (multiBall) {
					PlayingObjectManager.currentObjectName = collidedObject.name;
					gameObject.name = collidedObject.name; 
			} else {
					PlayingObjectManager.currentObjectName = gameObject.name;
			}

			AdjustPosition (collidedObject.transform.localPosition);

			GetComponent<SphereCollider> ().radius /= .8f;

			if (transform.position.y < thresoldLineTransform.position.y) {
					LevelManager.instance.GameIsOver ();
					return;
			}
	
			RefreshAdjacentObjectList ();

			SoundFxManager.instance.Play(SoundFxManager.instance.collisionSound);
			Invoke ("Trace", .02f);

			Invoke ("CheckForObjectFall", .2f);
	}

	//Align Current Playing Object properly.
	void AdjustPosition (Vector3 collidedObjectPos)
	{   
			float x = 0;
			float y = 0;

			if (transform.localPosition.x < collidedObjectPos.x) { //right touched
					if (transform.localPosition.y > collidedObjectPos.y) { //upper part
							x = collidedObjectPos.x - InGameScriptRefrences.playingObjectGeneration.objectGap;
							y = collidedObjectPos.y;
					} else { //lower part
							x = collidedObjectPos.x - InGameScriptRefrences.playingObjectGeneration.objectGap * .5f;
							y = collidedObjectPos.y - InGameScriptRefrences.playingObjectGeneration.rowGap;
							/*Commented because it generates a bug when a ball collide on the right wall and then collide 
							 * with another ball next to the wall
							 * if (x < -(InGameScriptRefrences.playingObjectGeneration.startingXPos + .2f)) {
									x = collidedObjectPos.x + InGameScriptRefrences.playingObjectGeneration.objectGap * .5f;
							}*/
					}
			} else { //left touched
					if (transform.localPosition.y > collidedObjectPos.y) { //upper part
							x = collidedObjectPos.x + InGameScriptRefrences.playingObjectGeneration.objectGap;
							y = collidedObjectPos.y;
					} else { //lower part
							x = collidedObjectPos.x + InGameScriptRefrences.playingObjectGeneration.objectGap * .5f;
							y = collidedObjectPos.y - InGameScriptRefrences.playingObjectGeneration.rowGap;
							/*Commented because it generates a bug when a ball collide on the left wall and then collide with 
							 * another wall next to the wall
							 * if (x > InGameScriptRefrences.playingObjectGeneration.startingXPos) {
									x = collidedObjectPos.x - InGameScriptRefrences.playingObjectGeneration.objectGap * .5f;
							}*/
					}
			}

			Vector3 newPos = new Vector3 (x, y, 0);

			transform.localPosition = newPos;


	}

	void CheckForObjectFall ()
	{
			InGameScriptRefrences.playingObjectManager.CheckForObjectsFall ();        
	}

	internal void AssignBurst ()
	{
			burst = true;
	}

	internal void GameOverFall ()
	{
			rigidbody.useGravity = true;
			rigidbody.isKinematic = false;
			rigidbody.AddForce (new Vector3 (0, Random.Range (2f, 3f), 0), ForceMode.VelocityChange);
	}

	public static void setmultiBall ()
	{
			if (Striker.instance.currentStrikerObject != null) {
					if (Striker.instance.currentStrikerObject.transform.childCount > 0) {
							Striker.instance.currentStrikerObject.transform.GetChild (0).renderer.material.mainTexture = Striker.instance.bombTexture;
					}
			}
			multiBall = true;
	}

	static bool getmultiBall ()
	{
			return multiBall;
	}
}
