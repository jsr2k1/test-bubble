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
	public bool fireBall = false;
	public bool bombBall = false;
	public bool multiBall = false;
	int deep = 0;
	Transform sliderTransform;
	public bool isBusy = false;
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
		if(LevelManager.GameType == LevelManager.GameTypes.NORMAL){
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
	public void FreeStriker(GameObject collidedObject)
	{           
		rigidbody.isKinematic = true;
		currentStrikerObject.GetComponent<SphereCollider>().enabled = true;
		currentStrikerObject.transform.parent = InGameScriptRefrences.playingObjectGeneration.gameObject.transform;
		currentStrikerObject.tag = "Playing Object";
		if(collidedObject!=null){
			currentStrikerObject.GetComponent<PlayingObject>().ObjectCollidedWithOtherObject(collidedObject);
		}
		isBusy = false;
		InGameScriptRefrences.strikerManager.GenerateStriker();
	}

	////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
	//Moves striker 
	void FixedUpdate()
	{
		if(isBusy == false)
			return;

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
						other.gameObject.GetComponent<PlayingObject>().DestroyPlayingObject();
						//ScoreManagerGame.instance.DisplayScorePopup(10, transform);
						deep = deep + 1;
						if(deep==1){
							AudioManager.instance.PlayFxSound(AudioManager.instance.burstSound);
						}
					}else{
						Destroy(currentStrikerObject);
						deep = 0;
						InGameScriptRefrences.playingObjectManager.ResetAllObjects();
						InGameScriptRefrences.playingObjectManager.FallDisconnectedObjects();
						FreeStriker(other.gameObject);
						fireBall = false;
					}
				} else{
					Destroy(currentStrikerObject);
					deep = 0;
					InGameScriptRefrences.playingObjectManager.ResetAllObjects();
					InGameScriptRefrences.playingObjectManager.FallDisconnectedObjects();
					FreeStriker(other.gameObject);
					fireBall = false;
				}
			}else if((multiBall || bombBall) && other.gameObject.name == "DummyBall(Clone)"){
				Destroy(currentStrikerObject);
				FreeStriker(other.gameObject);
			}else{
				FreeStriker(other.gameObject);
			}
		}

		//Rebound the striker on collision with left/right boundary
		if(other.gameObject.name == "Left" || other.gameObject.name == "Right"){
			AudioManager.instance.PlayFxSound(AudioManager.instance.wallCollisionSound);
			currentMovingDirection = Vector3.Reflect(currentMovingDirection, other.contacts[0].normal).normalized;
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
		
		InGameScriptRefrences.strikerManager.SaveBallsID();
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
					currentStrikerObject.GetComponent<PlayingObject>().isBooster=false;
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
					currentStrikerObject.GetComponent<PlayingObject>().isBooster=true;
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
					currentStrikerObject.GetComponent<PlayingObject>().isBooster=false;
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
					currentStrikerObject.GetComponent<PlayingObject>().isBooster=true;
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
					currentStrikerObject.GetComponent<PlayingObject>().isBooster=false;
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
					currentStrikerObject.GetComponent<PlayingObject>().isBooster=true;
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
}




