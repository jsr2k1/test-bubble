using UnityEngine;
using System.Collections;

public class InputScript : MonoBehaviour
{
	//Launcher Sprite
	public Transform launcher;
	public Transform lineTarget;
	public Transform lineTargetCollision;
	private float x, y, zRotation;
	private Vector3 reflection;
	public Transform thresoldLineTransform;

	//Line Renderer
	public LineRenderer line;
	
	ParticleSystem particles;
	ParticleSystem particlesCollision;
	
	////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
	
	void Awake()
	{
		particles = lineTarget.GetComponent<ParticleSystem>();
		particlesCollision = lineTargetCollision.GetComponent<ParticleSystem>();

		particles.renderer.sortingLayerName = "MiddleLayer";
		particlesCollision.renderer.sortingLayerName = "MiddleLayer";
	}

	////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

	void Update()
	{
		CheckColor();

		if(LevelManager.gameState == GameState.Start)
		{
			Vector2 pos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
			//Se mantiene pulsada la pantalla
			if(Input.GetButton("Fire1"))
			{
				CheckCollision();

				//Launcher aim to the mouse/touch point
				x = Input.mousePosition.x - camera.WorldToScreenPoint(launcher.position).x;
				y = Input.mousePosition.y - camera.WorldToScreenPoint(launcher.position).y;
				//Dont draw the line or do actions when the click its under the launcher point
				if(pos.y > thresoldLineTransform.position.y)
				{
					//lineTarget.gameObject.SetActive(true);
					particles.renderer.enabled = true;

					//Te rotation for aim
					zRotation = Mathf.Rad2Deg * Mathf.Atan2(x, y);
					//Rotating the spaceship
					launcher.eulerAngles = new Vector3(0, 0, zRotation);
					Vector3 target = new Vector3(-x, y, 0);
					Vector3 dir = target - lineTarget.position;
					lineTarget.forward = dir.normalized;
				}
			}else{
				//lineTarget.gameObject.SetActive(false);
				particles.renderer.enabled = false;
				//lineTargetCollision.gameObject.SetActive(false);
				particlesCollision.renderer.enabled = false;
			}

			//Se acaba de levantar la pulsacion de la pantalla
			if(Input.GetButtonUp("Fire1"))
			{
				//Dont draw the line or do actions when the click its under the launcher point
				if(pos.y > thresoldLineTransform.position.y){
					line.enabled = false;
					Vector2 FinalPos = new Vector2(pos.x, pos.y);
					InGameScriptRefrences.strikerManager.Shoot(FinalPos);
				}
			}
		}
	}

	////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

	void CheckColor()
	{
		if(Striker.instance==null || Striker.instance.currentStrikerObject==null || Striker.instance.currentStrikerObject.transform.childCount == 0)
			return;

		string sLineColor = lineTarget.renderer.material.mainTexture.name;
		SpriteRenderer sp = Striker.instance.currentStrikerObject.transform.GetChild(0).GetComponent<SpriteRenderer>();
		string sBallColor = sp.sprite.texture.name;
		//string sBallColor = Striker.instance.currentStrikerObject.transform.GetChild(0).renderer.material.mainTexture.name;

		if(sLineColor!=sBallColor){
			//lineTarget.renderer.material.mainTexture = 	      Striker.instance.currentStrikerObject.transform.GetChild(0).renderer.material.mainTexture;
			//lineTargetCollision.renderer.material.mainTexture = Striker.instance.currentStrikerObject.transform.GetChild(0).renderer.material.mainTexture;
			lineTarget.renderer.material.mainTexture = sp.sprite.texture;
			lineTargetCollision.renderer.material.mainTexture = sp.sprite.texture;
		}
	}

	////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
	//Comprobar si la linea de objetivo colisiona con las parederes y en ese caso activar el segundo 
	//sistema de particulas para mostar el rebote
	void CheckCollision()
	{
		RaycastHit hit;
		Ray ray = new Ray(lineTarget.position, lineTarget.forward);
		
		if(Physics.Raycast(ray, out hit) && hit.collider.tag == "boundary")
		{
			//lineTargetCollision.gameObject.SetActive(true);
			particlesCollision.renderer.enabled = true;
			lineTargetCollision.position = hit.point;
			reflection = Vector3.Reflect(ray.direction, hit.normal);
			lineTargetCollision.forward = reflection.normalized;
		}
		else{
			//lineTargetCollision.gameObject.SetActive(false);
			particlesCollision.renderer.enabled = false;
		}
	}

	////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
	/*
	void DrawLine()
	{
		line.enabled = true;
		RaycastHit hit;
		Ray ray = new Ray(launcher.position, launcher.up);
	
		line.SetPosition(0, ray.origin);
		if(Physics.Raycast(ray, out hit))
		{
			if(hit.collider.tag == "boundary")
			{
				line.SetPosition(1, hit.point);
				//Reflecting the ray cast on the 
				reflection = Vector3.Reflect(ray.direction, hit.normal);
				Ray ray2 = new Ray(hit.point, reflection);
				line.SetPosition(2, ray2.GetPoint(1));
			}
			else{
				line.SetPosition(1, hit.point);
				line.SetPosition(2, hit.point);
			}
		}
		else{
			line.SetPosition(1, ray.GetPoint(10));
			line.SetPosition(2, ray.GetPoint(0));
		}
	}*/
}
