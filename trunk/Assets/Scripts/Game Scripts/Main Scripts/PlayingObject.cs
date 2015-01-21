using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PlayingObject : MonoBehaviour
{
	public GameObject burstParticle;
	GameObject burstParticleInstance;
	ParticleAnimator particleAnim;
	ParticleEmitter particleEmit;
	static int numberOfAdjacentObjects = 6;

	//Angles of neighbour objects(this will be used while detecting neighbour objects through raycast
	public static float[]adjacentObjectAngles ={ 0, 60, 120, 180, 240, 300 };
	public PlayingObject[] adjacentPlayingObjects; // List of Adjacent playing objects

	internal bool isTraced = false;
	internal bool burst = false;
	internal bool isConnected = true;
	internal bool isTracedForConnection = false;
	bool isDestroyed = false;
	static Transform thresoldLineTransform; //The lower bottom point
	public LayerMask layerMask;

	Transform leftCollider;
	Transform rightCollider;
	
	float destroy_threshold; //umbral para destruir las bolas que caen
	SpriteRenderer spriteRenderer;
	SphereCollider sphereCollider;
	RotationScript rotationScript;

	////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

	void Awake()
	{
		leftCollider = GameObject.Find("Left").transform;
		rightCollider = GameObject.Find("Right").transform;
		
		destroy_threshold = GameObject.Find("Down").transform.position.y;
		spriteRenderer = transform.GetChild(0).GetComponent<SpriteRenderer>();
		sphereCollider = GetComponent<SphereCollider>();
		rotationScript = GetComponent<RotationScript>();
		
		burstParticleInstance = Instantiate(burstParticle, new Vector3(10000,10000,1), Quaternion.identity) as GameObject;
		particleAnim = burstParticleInstance.GetComponent<ParticleAnimator>();
		particleAnim.autodestruct = false;
		particleEmit = burstParticleInstance.GetComponent<ParticleEmitter>();
		particleEmit.emit = false;
	}

	////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

	void Start()
	{
		if(thresoldLineTransform == null){
			thresoldLineTransform = GameObject.Find("Thresold Line").transform;
		}
	}
	
	////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
	//void Update()
	void FixedUpdate()
	{
		//Cuando una bola que esta cayendo supera el umbral, la destruimos
		if(rigidbody.useGravity){
			float dist = Random.Range(0.0f,1.5f);
			if(transform.position.y < destroy_threshold-dist){
				if(LevelManager.GameType == LevelManager.GameTypes.ARCADE){
					DestroyPlayingObject(false);
				}else{
					DestroyPlayingObject(true);
				}
			}
		}
	}

	////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

	internal void Reset()
	{
		isTracedForConnection = false;
		isTraced = false;
		burst = false;
	}

	////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
	//Reassign Adjacent Playing Objects of the current Object
	internal void RefreshAdjacentObjectList()
	{
		adjacentPlayingObjects = new PlayingObject[numberOfAdjacentObjects];

		for(int i = 0; i < numberOfAdjacentObjects; i++)
		{
			PlayingObject temp = GetObjectInTheDirection(adjacentObjectAngles[i]);

			if(temp != null){
				adjacentPlayingObjects[i] = temp;
				if(i < 3){
					temp.adjacentPlayingObjects[i + 3] = this;
				}else{
					temp.adjacentPlayingObjects[i - 3] = this;
				}
			}
		}
	}    

	////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
	//Get reference of playing object in the given direction
	PlayingObject GetObjectInTheDirection(float angle)
	{
		RaycastHit hit;
		float maxDistance = .4f;

		float radAngle = angle * Mathf.Deg2Rad;

		Vector3 dir = new Vector3(Mathf.Cos(radAngle), Mathf.Sin(radAngle), 0);

		if(Physics.Raycast(transform.position, dir, out hit, maxDistance, layerMask)){
			if(hit.collider.gameObject.tag == "Playing Object"){
				return hit.collider.gameObject.GetComponent<PlayingObject>();
			}
			//print(hit.collider.gameObject.name); // coz of striker
		}

		return null;
	}

	////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
	//Reassign Neighbours of Adjacent Playing Objects
	void RefreshNeighbourAdjacentList()
	{
		for(int i = 0; i < numberOfAdjacentObjects; i++){
			if(adjacentPlayingObjects[i]){
				if(i < 3){
					adjacentPlayingObjects[i].adjacentPlayingObjects[i + 3] = null;
				} else{
					adjacentPlayingObjects[i].adjacentPlayingObjects[i - 3] = null;
				}
			}
		}     
	}    

	////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
	//Destroy the Playing Object after right combination is formed
	internal void BurstMe(bool fall)
	{
		if(this==null){
			return;
		}
		if(isDestroyed){
			return;
		}
		if(gameObject.name=="DummyBall(Clone)"){
			return;
		}
		isDestroyed = true;
		Destroy(sphereCollider);
		RefreshNeighbourAdjacentList();
		gameObject.tag = "Untagged";

		if(fall){
			if(burst == false){
				rigidbody.useGravity = true;
				rigidbody.isKinematic = false;
				rigidbody.AddForce(new Vector3(0, Random.Range(1.5f, 2.5f), 0), ForceMode.VelocityChange);
				rotationScript.enabled = true;
				spriteRenderer.sortingLayerName = "FallingObjLayer";
			} else{                
				Destroy(gameObject);
			}
		} else{
			DestroyPlayingObject(true);
		}
	}
	
	////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

	public void DestroyPlayingObject(bool bAddScore)
	{
		if(bAddScore){
			ScoreManagerGame.instance.DisplayScorePopup(10, transform);
		}
		//Instantiate(burstParticle, transform.position, Quaternion.identity);
		burstParticleInstance.transform.position = transform.position;
		//burstParticle.renderer.sortingLayerName = "MiddleLayer";
		burstParticleInstance.renderer.sortingLayerName = "MiddleLayer";
		particleEmit.emit = true;
		particleAnim.autodestruct = true;
		Destroy(gameObject);
	}

	////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
	//Trace All the Playing objects connected to this Playing Object and match for the fall/burst.
	void Trace(int iDeep)
	{
		if(!isTraced)
		{
			isTraced = true;
			burst = true;
			PlayingObjectManager.objectsToBurst.Add(this);
			PlayingObjectManager.burstCounter++;
			//iTween.PunchScale(gameObject, new Vector3(.2f, .2f, .2f), 1f);
	
			for(int i=0; i<numberOfAdjacentObjects; i++)
			{
				if(adjacentPlayingObjects[i]!=null && adjacentPlayingObjects[i].name!="DummyBall(Clone)" /*&& adjacentPlayingObjects[i].name!="StoneBall(Clone)"*/)
				{
					//MULTIBALL
					if(Striker.instance.multiBall && adjacentPlayingObjects[i].name!="StoneBall(Clone)"){
						if(gameObject.name=="MultiBall" || adjacentPlayingObjects[i].name==gameObject.name){
							adjacentPlayingObjects[i].Trace(iDeep+1);
						}else{
							//iTween.PunchScale(adjacentPlayingObjects[i].gameObject, new Vector3(.2f, .2f, .2f), 1f);
						}
					}//BOMBBALL -> Tambien rompe las bolas piedra
					else if(Striker.instance.bombBall){
						if(iDeep<2){
							adjacentPlayingObjects[i].Trace(iDeep+1);
						}else{
							//iTween.PunchScale(adjacentPlayingObjects[i].gameObject, new Vector3(.2f, .2f, .2f), 1f);
						}
					}//NORMAL
					else if(adjacentPlayingObjects[i].name!="StoneBall(Clone)" && !Striker.instance.fireBall){
						if(adjacentPlayingObjects[i].name==PlayingObjectManager.currentObjectName){
							adjacentPlayingObjects[i].Trace(iDeep+1);
						}else{
							adjacentPlayingObjects[i].isTraced = true;
							//iTween.PunchScale(adjacentPlayingObjects[i].gameObject, new Vector3(.2f, .2f, .2f), 1f);
						}
					}
				}
			}
		}
	}

	////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
	//Checks if the Playing object is connected to the top , If its free it will fall/destroyed.
	internal void TraceForConnection()
	{
		if(isTracedForConnection || burst){
			return;
		}
		isTracedForConnection = true;
		isConnected = true;

		for(int i = 0; i < numberOfAdjacentObjects; i++){
			if(adjacentPlayingObjects[i]){
				adjacentPlayingObjects[i].TraceForConnection();
			}
		}
	}
	
	////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

	internal void ObjectCollidedWithOtherObject(GameObject collidedObject)
	{
		if(!Striker.instance.multiBall){
			PlayingObjectManager.currentObjectName = gameObject.name;
		}
		AdjustPosition(collidedObject.transform.position);

		if(LevelManager.GameType == LevelManager.GameTypes.ARCADE){
			if(transform.position.y < thresoldLineTransform.position.y + PlayingObjectGeneration.thresholdOffsetGameOver){
				LevelManager.instance.GameIsOver();
				return;
			}
		}
		RefreshAdjacentObjectList();
		Trace(0);
		InGameScriptRefrences.playingObjectManager.CheckForObjectsFall();
		
		if(Striker.instance.multiBall){
			Striker.instance.multiBall=false;
		}
		if(Striker.instance.bombBall){
			Striker.instance.bombBall=false;
		}
	}

	////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
	//Align Current Playing Object properly.
	void AdjustPosition(Vector3 collidedObjectPos)
	{   
		float x = 0;
		float y = 0;

		//LEFT
		if(transform.position.x < collidedObjectPos.x)
		{ 
			//LEFT-UPPER
			if(transform.position.y > collidedObjectPos.y){ 
				x = collidedObjectPos.x - InGameScriptRefrences.playingObjectGeneration.objectGap;
				y = collidedObjectPos.y;
				//Comprobar que la bola no se quede enganchada fuera de la pantalla por la izquierda
				float leftDist = Mathf.Abs(x-leftCollider.position.x);
				if(leftDist<0.45f){
					x = collidedObjectPos.x - InGameScriptRefrences.playingObjectGeneration.objectGap * .5f;
					y = collidedObjectPos.y - InGameScriptRefrences.playingObjectGeneration.rowGap;
				}
			}
			//LEFT-LOWER
			else{
				x = collidedObjectPos.x - InGameScriptRefrences.playingObjectGeneration.objectGap * .5f;
				y = collidedObjectPos.y - InGameScriptRefrences.playingObjectGeneration.rowGap;
				//Comprobar que la bola no se quede enganchada fuera de la pantalla por la izquierda
				float leftDist = Mathf.Abs(x-leftCollider.position.x);
				if(leftDist<0.45f){
					x = collidedObjectPos.x + InGameScriptRefrences.playingObjectGeneration.objectGap * .5f;
				}
			}
		}
		//RIGHT
		else{
			//RIGHT-UPPER
			if(transform.position.y > collidedObjectPos.y){
				x = collidedObjectPos.x + InGameScriptRefrences.playingObjectGeneration.objectGap;
				y = collidedObjectPos.y;
				//Comprobar que la bola no se quede enganchada fuera de la pantalla por la derecha
				float rightDist = Mathf.Abs(x-rightCollider.position.x);
				if(rightDist<0.45f){
					x = collidedObjectPos.x + InGameScriptRefrences.playingObjectGeneration.objectGap * .5f;
					y = collidedObjectPos.y - InGameScriptRefrences.playingObjectGeneration.rowGap;
				}
			}
			//RIGHT-LOWER
			else{ 
				x = collidedObjectPos.x + InGameScriptRefrences.playingObjectGeneration.objectGap * .5f;
				y = collidedObjectPos.y - InGameScriptRefrences.playingObjectGeneration.rowGap;
				//Comprobar que la bola no se quede enganchada fuera de la pantalla por la derecha
				float rightDist = Mathf.Abs(x-rightCollider.position.x);
				if(rightDist<0.45f){
					x = collidedObjectPos.x - InGameScriptRefrences.playingObjectGeneration.objectGap * .5f;
				}
			}
		}
		transform.position = new Vector3(x, y, 0);
	}
}




