using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class InputScript : MonoBehaviour
{
	//Launcher Sprite
	public Transform launcher;
	public Transform thresoldLineTransform;

	float x, y, zRotation;

	public GameObject particlePrefab1;
	public GameObject particlePrefab2;
	public float spacing;
	public float initialOffset;
	public int nParticles=5;
	public LayerMask layermask = -1;

	GameObject[] particles;
	float maxDist=0;
	
	////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
	
	void Awake()
	{
		CreateParticles();
	}

	////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

	void CreateParticles()
	{
		particles = new GameObject[nParticles];
		for(int i=0;i<nParticles;i++)
		{
			GameObject particle;
			if(i%2==0){
				particle = Instantiate(particlePrefab1) as GameObject;
			}else{
				particle = Instantiate(particlePrefab2) as GameObject;
			}
			particle.renderer.enabled=false;
			particles[i] = particle;
		}
	}

	////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

	void Update()
	{
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
					zRotation = Mathf.Rad2Deg * Mathf.Atan2(-x, y);
					launcher.eulerAngles = new Vector3(0, 0, zRotation);
					UpdateParticlesPosition(x,y);
				}
			}
			//Se acaba de levantar la pulsacion de la pantalla
			if(Input.GetButtonUp("Fire1"))
			{
				SetParticles(false);

				//Dont draw the line or do actions when the click its under the launcher point
				if(pos.y > thresoldLineTransform.position.y){
					Vector2 FinalPos = new Vector2(pos.x, pos.y);
					InGameScriptRefrences.strikerManager.Shoot(FinalPos);
				}
			}
			//Se ha pulsado en la pantalla
			if(Input.GetButtonDown("Fire1"))
			{
				SetParticles(true);
			}
		}
	}

	////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

	void UpdateParticlesPosition(float x, float y)
	{
		Vector3 target = new Vector3(x, y, 0);
		Vector3 dir = (target - launcher.transform.position).normalized;

		for(int i=0;i<nParticles;i++)
		{
			Vector3 targetPos = launcher.transform.position + dir*initialOffset + dir*(i+1)*spacing;
			float dist = Vector3.Distance(targetPos, launcher.transform.position);

			if(maxDist>0){
				if(dist<maxDist){
					particles[i].renderer.enabled = true;
					particles[i].transform.position = targetPos;
				}else{
					particles[i].renderer.enabled = false;
				}
			}
		}
	}

	////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

	void SetParticles(bool bEnable)
	{
		for(int i=0;i<nParticles;i++){
			particles[i].renderer.enabled = bEnable;
		}
	}

	////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

	public void CheckColor()
	{
		if(Striker.instance==null || Striker.instance.currentStrikerObject==null || Striker.instance.currentStrikerObject.transform.childCount == 0)
			return;

		string sCurrentColor = particles[0].GetComponent<SpriteRenderer>().sprite.texture.name;
		SpriteRenderer sp = Striker.instance.currentStrikerObject.transform.GetChild(0).GetComponent<SpriteRenderer>();
		string sBallColor = sp.sprite.texture.name;

		if(sCurrentColor!=sBallColor){
			for(int i=0;i<nParticles;i++){
				particles[i].GetComponent<SpriteRenderer>().sprite = sp.sprite;
			}
		}
	}

	////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
	//Comprobar si la linea de objetivo colisiona con alguna bola
	void CheckCollision()
	{
		RaycastHit hit;
		Ray ray = new Ray(launcher.transform.position, launcher.transform.up);
		
		if(Physics.Raycast(ray, out hit, layermask)){
			maxDist = Vector3.Distance(launcher.transform.position, hit.point);
		}else{
			maxDist=0;
		}
	}
}




