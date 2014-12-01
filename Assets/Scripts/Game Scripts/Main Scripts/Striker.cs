using UnityEngine;
using System.Collections;
using System.Collections.Generic;

//Striker holds the current shooting object
public class Striker : MonoBehaviour
{
	public static Striker instance;
	Vector3 currentMovingDirection = Vector3.zero;
	float speed;
	Transform myTransform;

	//Power-ups
	bool fireBall = false;
	public bool bombBall = false;
	public bool multiBall = false;
	int deep = 0;
	Transform sliderTransform;
	internal bool isBusy = false;
	public GameObject currentStrikerObject = null;
	Texture oldTexture;
	Sprite oldSprite;
	string oldName;
	public string sCurrentSpecialBall="";

	public Sprite spriteMultiBall;
	public Sprite spriteBombBall;
	public Sprite spriteFireBall;
	public InputScript inputScript;
	
	//Creamos un evento para poder saber cuando se ha disparado un booster
	public delegate void SpecialBallLaunched();
	public static event SpecialBallLaunched OnSpecialBallLaunched;

	////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

	void Awake()
	{
		instance = this;
	}

	////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

	void Start()
	{  
		myTransform = transform;
	}

	////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

	internal void Shoot(Vector3 dir)
	{   
		speed = 10f;  
		rigidbody.isKinematic = false;
		currentMovingDirection = dir;
		isBusy = true;

		//Telling the NumberOfBallsManager on the game scene that one ball has left and its being shooted
		if(PlayerPrefs.GetString("GameType") == "Normal"){
			LevelManager.instance.BallLaunched();
		}

		if(sCurrentSpecialBall!=""){
			int quantity = PlayerPrefs.GetInt(sCurrentSpecialBall) - 1;
			PlayerPrefs.SetInt(sCurrentSpecialBall, quantity);
			sCurrentSpecialBall="";
			
			if(OnSpecialBallLaunched!=null){
				OnSpecialBallLaunched();
			}
		}
	}

	////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
	//Called when the striker just hit the board playing object
	private void FreeStriker(GameObject collidedObject)
	{           
		rigidbody.isKinematic = true;        
		currentStrikerObject.GetComponent<SphereCollider>().enabled = true;
		currentStrikerObject.transform.parent = InGameScriptRefrences.playingObjectGeneration.gameObject.transform;
		currentStrikerObject.tag = "Playing Object";
		currentStrikerObject.GetComponent<PlayingObject>().ObjectCollidedWithOtherObject(collidedObject);
		
		/*Lo hago en el Trace() porque asi no funciona bien
		if(bombBall){
			DetectAndExplode();
			//Destroy(currentStrikerObject);
			bombBall = false;
			//InGameScriptRefrences.playingObjectManager.ResetAllObjects();
			InGameScriptRefrences.playingObjectManager.CheckForObjectsFall();
			InGameScriptRefrences.playingObjectManager.FallDisconnectedObjects();
		}
		*/
		isBusy = false;
		InGameScriptRefrences.strikerManager.GenerateStriker();
	}

	////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
	//Moves striker 
	void FixedUpdate()
	{
		if(isBusy == false)
			return;
		//speed += 5 * Time.deltaTime;
		myTransform.Translate(currentMovingDirection * speed * Time.deltaTime);
	}

	////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

	void OnCollisionEnter(Collision other)
	{
		//When the Striker hits board playing object
		if(other.gameObject.tag == "Playing Object" && isBusy){
			if(fireBall){
				if(deep < 8){
					if(other.gameObject.name != "DummyBall(Clone)" && other.gameObject.name != "StoneBall(Clone)"){
						Destroy(other.gameObject.gameObject);
						ScoreManagerGame.instance.DisplayScorePopup(10, transform);
						deep = deep + 1;
					}else{
						Destroy(currentStrikerObject);
						fireBall = false;
						deep = 0;
						InGameScriptRefrences.playingObjectManager.ResetAllObjects();
						InGameScriptRefrences.playingObjectManager.FallDisconnectedObjects();
						FreeStriker(other.gameObject);
					}
				} else{
					Destroy(currentStrikerObject);
					fireBall = false;
					deep = 0;
					InGameScriptRefrences.playingObjectManager.ResetAllObjects();
					InGameScriptRefrences.playingObjectManager.FallDisconnectedObjects();
					FreeStriker(other.gameObject);
				}
			}
			else if(multiBall && other.gameObject.name == "DummyBall(Clone)"){
				Destroy(currentStrikerObject);
				//multiBall=false;
				FreeStriker(other.gameObject);
			}
			else{
				FreeStriker(other.gameObject);
			}
		}

		//Rebound the striker on collision with left/right boundary
		if(other.gameObject.name == "Left" || other.gameObject.name == "Right"){
			SoundFxManager.instance.Play(SoundFxManager.instance.wallCollisionSound);
			currentMovingDirection = Vector3.Reflect(currentMovingDirection, other.contacts[0].normal).normalized;
		}

		//Destroy current shooting object hold by striker and generate new striker object.
		if((/*other.gameObject.name == "Top" || */other.gameObject.name == "Top Down") && isBusy){
			rigidbody.isKinematic = true;
			Destroy(currentStrikerObject);
			isBusy = false;
			InGameScriptRefrences.strikerManager.GenerateStriker();
			InGameScriptRefrences.playingObjectManager.ResetAllObjects();
			InGameScriptRefrences.playingObjectManager.FallDisconnectedObjects();
			if(fireBall){
				fireBall = false;
				deep = 0;
			}
		}

		if(bombBall == false && fireBall == false){
			//Its top so the balls get stuck here
			if(other.gameObject.tag == "TopLimit" && isBusy)
			{
				//lo que mete la bola en el top_row_objects array
				InGameScriptRefrences.playingObjectManager.topRowObjects[int.Parse(other.gameObject.name)] = currentStrikerObject.GetComponent<PlayingObject>();

				//Assigning the other object collider property to false
				other.gameObject.GetComponent<LaserOcclusor>().ToggleCollider(false);

				rigidbody.isKinematic = true;        

				//Para comprobar si hay colision con el techo y con otra bola a la vez
				Vector3 dir = other.transform.position - currentStrikerObject.transform.position;
				dir = Vector3.Normalize(dir);
				float dist = Vector3.Distance(other.transform.position, currentStrikerObject.transform.position);

				currentStrikerObject.transform.Translate(dir * dist);
				currentStrikerObject.GetComponent<SphereCollider>().enabled = true;
				currentStrikerObject.transform.parent = InGameScriptRefrences.playingObjectGeneration.gameObject.transform;
				currentStrikerObject.tag = "Playing Object";
				currentStrikerObject.GetComponent<PlayingObject>().RefreshAdjacentObjectList();

				isBusy = false;

				InGameScriptRefrences.strikerManager.GenerateStriker();
			}
		}
	}

	////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
	//Skip current shooting object
	internal void Swap()
	{
		if(isBusy){
			return;
		}
		if(fireBall || bombBall || multiBall){
			return;
		}
		rigidbody.isKinematic = true;
		Destroy(currentStrikerObject);
		isBusy = false;
		InGameScriptRefrences.strikerManager.isFirstObject = true;
		InGameScriptRefrences.strikerManager.GenerateSwapStriker();
	}

	////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
	//FIREBALL
	internal void SetFireBall()
	{
		if(currentStrikerObject != null){
			if(currentStrikerObject.transform.childCount > 0){
				//Desactivar booster
				if(fireBall){
					currentStrikerObject.transform.GetChild(0).GetComponent<SpriteRenderer>().sprite = oldSprite;
					currentStrikerObject.name = oldName;
					inputScript.CheckColor();
					fireBall = false;
					sCurrentSpecialBall = "";
				}
				//Activar booster
				else{
					if(!bombBall && !multiBall){
						oldSprite = currentStrikerObject.transform.GetChild(0).GetComponent<SpriteRenderer>().sprite;
						oldName = currentStrikerObject.name;
					}
					currentStrikerObject.transform.GetChild(0).GetComponent<SpriteRenderer>().sprite = spriteFireBall;
					currentStrikerObject.name = "FireBall";
					inputScript.CheckColorBooster();
					fireBall = true;
					bombBall = false;
					multiBall = false;
					sCurrentSpecialBall = "Fire Ball";
				}
			}
		}
	}

	////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

	internal bool GetFireBall()
	{
		return fireBall;
	}

	////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
	//BOMBBALL
	internal void SetBombBall()
	{
		if(currentStrikerObject != null){
			if(currentStrikerObject.transform.childCount > 0){
				//Desactivar booster
				if(bombBall){
					currentStrikerObject.transform.GetChild(0).GetComponent<SpriteRenderer>().sprite = oldSprite;
					currentStrikerObject.name = oldName;
					inputScript.CheckColor();
					bombBall = false;
					sCurrentSpecialBall = "";
				}
				//Activar booster
				else{
					if(!fireBall && !multiBall){
						oldSprite = currentStrikerObject.transform.GetChild(0).GetComponent<SpriteRenderer>().sprite;
						oldName = currentStrikerObject.name;
					}
					currentStrikerObject.transform.GetChild(0).GetComponent<SpriteRenderer>().sprite = spriteBombBall;
					currentStrikerObject.name = "BombBall";
					inputScript.CheckColorBooster();
					bombBall = true;
					fireBall = false;
					multiBall = false;
					sCurrentSpecialBall = "Bomb Ball";
				}
			}
		}
	}

	////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

	internal bool GetBombBall()
	{
		return bombBall;
	}
	
	////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
	//MULTIBALL
	public void SetMultiBall()
	{
		if(currentStrikerObject != null){
			if(currentStrikerObject.transform.childCount > 0){
				//Desactivar booster
				if(multiBall){
					currentStrikerObject.transform.GetChild(0).GetComponent<SpriteRenderer>().sprite = oldSprite;
					currentStrikerObject.name = oldName;
					inputScript.CheckColor();
					multiBall = false;
					sCurrentSpecialBall = "";
				}
				//Activar booster
				else{
					if(!bombBall && !fireBall){
						oldSprite = currentStrikerObject.transform.GetChild(0).GetComponent<SpriteRenderer>().sprite;
						oldName = currentStrikerObject.name;
					}
					currentStrikerObject.transform.GetChild(0).GetComponent<SpriteRenderer>().sprite = spriteMultiBall;
					currentStrikerObject.name = "MultiBall";
					inputScript.CheckColorBooster();
					multiBall = true;
					fireBall = false;
					bombBall = false;
					sCurrentSpecialBall = "Multicolor Ball";
				}
			}
		}
	}
	
	////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
	
	internal bool GetMultiBall()
	{
		return multiBall;
	}

	////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
	//Function to throw raycast to see wich balls are sorrounding the bomb ball
	private void DetectAndExplode()
	{
		Collider[] hitColliders = Physics.OverlapSphere(transform.position, 1f);
		int i = 0;
		while(i < hitColliders.Length){
			if(hitColliders[i].tag == "Playing Object"){
				if(hitColliders[i].name != "DummyBall(Clone)" && hitColliders[i].name != "StoneBall(Clone)"){
					Destroy(hitColliders[i].gameObject);
					//hitColliders[i].GetComponent<PlayingObject>().burst=true;
					ScoreManagerGame.instance.DisplayScorePopup(10, transform);
				}
			}
			i++;
		}
	}
}




