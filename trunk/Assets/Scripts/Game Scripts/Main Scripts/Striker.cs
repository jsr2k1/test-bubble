using UnityEngine;
using System.Collections;

//Striker holds the current shooting object
public class Striker : MonoBehaviour
{
		public static Striker instance;
		Vector3 currentMovingDirection = Vector3.zero;
		float speed;
		Transform myTransform;
	
		//Power-ups
		bool fireBall = false;
		bool bombBall = false;
		int deep = 0;
		Transform sliderTransform;
		internal bool isBusy = false;
		public GameObject currentStrikerObject = null;
		public Texture bombTexture;
	
		void Awake ()
		{
				instance = this;
		}
	
		void Start ()
		{  
				myTransform = transform;
		}
	
		internal void Shoot (Vector3 dir)
		{   
				speed = 10f;  
				rigidbody.isKinematic = false;
				currentMovingDirection = dir;
				isBusy = true;
				//Telling the NumberOfBallsManager on the game scene that one ball has left and its being shooted
				LevelManager.instance.BallLaunched ();
		}
	
		//Called when the striker just hit the board playing object
		private void FreeStriker (GameObject collidedObject)
		{           
				rigidbody.isKinematic = true;        

				currentStrikerObject.GetComponent<SphereCollider> ().enabled = true;
				currentStrikerObject.transform.parent = InGameScriptRefrences.playingObjectGeneration.gameObject.transform;
				currentStrikerObject.tag = "Playing Object";
				currentStrikerObject.GetComponent<PlayingObject> ().ObjectCollidedWithOtherObject (collidedObject);
		
				if (bombBall) {
						DetectAndExplode ();
						//Destroy(currentStrikerObject);
						bombBall = false;
						InGameScriptRefrences.playingObjectManager.ResetAllObjects ();
						InGameScriptRefrences.playingObjectManager.FallDisconnectedObjects ();
				}
		
				isBusy = false;
				
				InGameScriptRefrences.strikerManager.GenerateStriker ();
		}
	
		//Moves striker 
		void FixedUpdate ()
		{
				if (isBusy == false)
						return;
		
				speed += 5 * Time.deltaTime;
		
				myTransform.Translate (currentMovingDirection * speed * Time.deltaTime);
		
		}
	
		void OnCollisionEnter (Collision other)
		{
		
				//When the Striker hits board playing object
				if (other.gameObject.tag == "Playing Object" && isBusy) {
			
						if (fireBall) {
				
								if (deep < 8) {
										Destroy (other.gameObject);
										deep = deep + 1;
								} else {
										Destroy (currentStrikerObject);
										fireBall = false;
										deep = 0;
										InGameScriptRefrences.playingObjectManager.ResetAllObjects ();
										InGameScriptRefrences.playingObjectManager.FallDisconnectedObjects ();
										FreeStriker (other.gameObject);
								}
				
						} else {
								FreeStriker (other.gameObject);
						}
				}

				//Rebound the striker on collision with left/right boundary
				if (other.gameObject.name == "Left" || other.gameObject.name == "Right")
				{
					SoundFxManager.instance.Play(SoundFxManager.instance.wallCollisionSound);
					currentMovingDirection = Vector3.Reflect (currentMovingDirection, other.contacts [0].normal).normalized;
				}
		
				//Destroy current shooting object hold by striker and generate new striker object.
				if ((other.gameObject.name == "Top" || other.gameObject.name == "Top Down") && isBusy) {
						rigidbody.isKinematic = true;
						Destroy (currentStrikerObject);
						isBusy = false;
						InGameScriptRefrences.strikerManager.GenerateStriker ();
						InGameScriptRefrences.playingObjectManager.ResetAllObjects ();
						InGameScriptRefrences.playingObjectManager.FallDisconnectedObjects ();
						if (fireBall) {
								fireBall = false;
								deep = 0;
						}
				}
		
				//Its top so the balls get stuck here
				if (other.gameObject.tag == "TopLimit" && isBusy) {
						ScoreManager.instance.DisplayScorePopup (-50, other.gameObject.transform);
						InGameScriptRefrences.playingObjectManager.ResetAllObjects ();
						InGameScriptRefrences.playingObjectManager.FallDisconnectedObjects ();
						FreeStriker (other.gameObject);
				}
		}
	
		//Collision with trigger to detect the top limit sticky balls
		void OnTriggerEnter (Collider other)
		{
				if (bombBall == false && fireBall == false) {
						//Its top so the balls get stuck here
						if (other.gameObject.tag == "TopLimit" && isBusy) {
								//ScoreManager.instance.DisplayScorePopup(-50 , other.gameObject.transform);
								//Destroy(currentStrikerObject);
			
								//lo que mete la bola en el toprowobjects array
								InGameScriptRefrences.playingObjectManager.topRowObjects [int.Parse (other.gameObject.name)] = currentStrikerObject.GetComponent<PlayingObject> ();
			
								//assigning the other object collider property to false
								other.gameObject.GetComponent<LaserOcclusor> ().ToggleCollider (false);
			
								rigidbody.isKinematic = true;        
								currentStrikerObject.transform.position = other.transform.position;
								currentStrikerObject.GetComponent<SphereCollider> ().enabled = true;
								currentStrikerObject.transform.parent = InGameScriptRefrences.playingObjectGeneration.gameObject.transform;
								currentStrikerObject.GetComponent<PlayingObject> ().RefreshAdjacentObjectList ();
								currentStrikerObject.tag = "Playing Object";
			
								isBusy = false;
			
								InGameScriptRefrences.strikerManager.GenerateStriker ();
						}
				}
		}
	
		//Skip current shooting object
		internal void Skip ()
		{
				if (isBusy)
						return;
		
				rigidbody.isKinematic = true;
				Destroy (currentStrikerObject);
				isBusy = false;
				InGameScriptRefrences.strikerManager.isFirstObject = true;
				InGameScriptRefrences.strikerManager.GenerateSwapStriker ();
		}

	
		//FireBall
		internal void setFireBall ()
		{
				if (currentStrikerObject != null) {
						if (currentStrikerObject.transform.childCount > 0) {
								currentStrikerObject.transform.GetChild (0).renderer.material.mainTexture = bombTexture;
						}
				}
				fireBall = true;
		}
	
		internal bool getFireBall ()
		{
				return fireBall;
		}
	
		//Bomb Ball
		internal void setBombBall ()
		{
				if (currentStrikerObject != null) {
						if (currentStrikerObject.transform.childCount > 0) {
								currentStrikerObject.transform.GetChild (0).renderer.material.mainTexture = bombTexture;
						}
				}
				
				bombBall = true;
		}
	
		internal bool getBombBall ()
		{
				return bombBall;
		}
	
		//Function to throw raycast to see wich balls are sorrounding the bomb ball
		private void DetectAndExplode ()
		{
				Collider[] hitColliders = Physics.OverlapSphere (transform.position, 1f);
				int i = 0;
				while (i < hitColliders.Length) {
						if (hitColliders [i].tag == "Playing Object") {
								Destroy (hitColliders [i].gameObject);
						}
						i++;
				}
		}
	
}
