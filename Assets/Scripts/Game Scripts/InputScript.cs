using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class InputScript : MonoBehaviour
{
	//Launcher Sprite
	public Transform launcher;
	public Transform thresoldLineTransform;

	float x, y, zRotation;
	public float ballRadius;

	public GameObject particlePrefab1;
	public GameObject particlePrefab2;
	public float spacing;
	public float initialOffset;
	public int nParticles;
	public int nBounceParticles;
	public LayerMask layermask = -1;
	public LayerMask layermask2 = -1;
	public Sprite[] targetSprites;
	public Animator characterAnimator;

	GameObject[] particles;
	GameObject[] bounceParticles;
	float maxDist=0;
	float maxDistBounce=0;
	float offsetDist=0;
	bool bBounceOn=false;
	Vector3 hitPoint;
	Vector3 hitNormal;
	Vector3 rayDirection;
	
	public PopUpMgr LosePopUpArcade;
	public PopUpMgr SettingsPopUp;
	
	////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
	
	void Awake()
	{
		CreateParticles();
		LevelManager.gameState = GameState.Start;
	}

	////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
	//Inicializamos el array con las particulas del primer rayo y el array del segundo rayo
	//Para conseguir el efecto de que las bolas aumenten y disminuyan el tamaño de manera alternada,
	//tenemos 2 prefabs con la animacion con desfase
	void CreateParticles()
	{
		GameObject particleParent = new GameObject("ParticleParent");
		
		//primer rayo
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
			particle.transform.SetParent(particleParent.transform);
		}
		//segundo rayo
		bounceParticles = new GameObject[nBounceParticles];
		for(int i=0;i<nBounceParticles;i++)
		{
			GameObject particle;
			if(i%2==0){
				particle = Instantiate(particlePrefab1) as GameObject;
			}else{
				particle = Instantiate(particlePrefab2) as GameObject;
			}
			particle.renderer.enabled=false;
			bounceParticles[i] = particle;
			particle.transform.SetParent(particleParent.transform);
		}
	}
	
	////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
	//En el modo Arcade, a veces cuando entras el juego esta en modo Pause y no deberia.
	//Comprobamos si esta en modo Pause y no hay ningun popup entonces lo ponemos en modo play.
	void CheckPauseError()
	{
		if(PlayerPrefs.GetString("GameType").Equals("Arcade")){
			if(LevelManager.gameState != GameState.Start){
				if(!LosePopUpArcade.bShow && !SettingsPopUp.bShow){
					LevelManager.gameState = GameState.Start;
				}
			}
		}
	}

	////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

	void Update()
	{
		CheckPauseError();
		
		if(LevelManager.gameState == GameState.Start)
		{
			Vector2 pos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
			//Se mantiene pulsada la pantalla
			if(Input.GetButton("Fire1") && Striker.instance!=null && Striker.instance.currentStrikerObject!=null)
			{
				CheckCollision();
				CheckCollisionBounce();

				//Launcher aim to the mouse/touch point
				x = Input.mousePosition.x - camera.WorldToScreenPoint(launcher.position).x;
				y = Input.mousePosition.y - camera.WorldToScreenPoint(launcher.position).y;

				//Dont draw the line or do actions when the click its under the launcher point
				if(pos.y > thresoldLineTransform.position.y)
				{
					SetParticles(true);
					zRotation = Mathf.Rad2Deg * Mathf.Atan2(-x, y);
					launcher.eulerAngles = new Vector3(0, 0, zRotation);
					UpdateParticlesPosition(x,y);
					UpdateBounceParticlesPosition();
				}
			}else{
				//Nos aseguramos de ocultar las particulas ya que a veces se quedan en pantalla
				SetParticles(false);
			}
			//Se acaba de levantar la pulsacion de la pantalla
			if(Input.GetButtonUp("Fire1") && Striker.instance!=null && Striker.instance.currentStrikerObject!=null)
			{
				SetParticles(false);

				//Dont draw the line or do actions when the click its under the launcher point
				if(pos.y > thresoldLineTransform.position.y){
					Vector2 FinalPos = new Vector2(pos.x, pos.y);
					InGameScriptRefrences.strikerManager.Shoot(FinalPos);
					characterAnimator.SetTrigger("Shoot");
				}
			}
		}
	}

	////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
	//Actualiza la posicion de las particulas a medida que se rota el launcher
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

	void UpdateBounceParticlesPosition()
	{
		if(!bBounceOn){
			for(int i=0;i<nBounceParticles;i++){
				bounceParticles[i].renderer.enabled = false;
			}
		}else{
			Vector3 reflection = Vector3.Reflect(rayDirection, hitNormal).normalized;
			
			for(int i=0;i<nBounceParticles;i++)
			{
				Vector3 targetPos = hitPoint + reflection*offsetDist + reflection*i*spacing;
				float dist = Vector3.Distance(targetPos, hitPoint);

				if(maxDistBounce>0){
					if(dist<maxDistBounce){
						bounceParticles[i].renderer.enabled = true;
						bounceParticles[i].transform.position = targetPos;
					}
					else{
						bounceParticles[i].renderer.enabled = false;
					}
				}
			}
		}
	}

	////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
	//Activar o desactivar las particulas del rayo principal y del rebotado
	void SetParticles(bool bEnable)
	{
		for(int i=0;i<nParticles;i++){
			particles[i].renderer.enabled = bEnable;
		}
		for(int i=0;i<nBounceParticles;i++){
			bounceParticles[i].renderer.enabled = bEnable;
		}
	}
	
	////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
	
	public void CheckColorBooster()
	{
		if(Striker.instance==null || Striker.instance.currentStrikerObject==null || Striker.instance.currentStrikerObject.transform.childCount == 0){
			return;
		}
		SpriteRenderer sp = Striker.instance.currentStrikerObject.transform.GetChild(0).GetComponent<SpriteRenderer>();
		
		for(int i=0;i<nParticles;i++){
			particles[i].GetComponent<SpriteRenderer>().sprite = sp.sprite;
		}
		for(int i=0;i<nBounceParticles;i++){
			bounceParticles[i].GetComponent<SpriteRenderer>().sprite = sp.sprite;
		}
		
	}

	////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
	//Comprobamos que el color de las particulas apuntadoras es el mismo que el color de la bola actual del striker
	public void CheckColor()
	{
		if(Striker.instance==null || Striker.instance.currentStrikerObject==null || Striker.instance.currentStrikerObject.transform.childCount == 0){
			return;
		}
		string sCurrentColor = particles[0].GetComponent<SpriteRenderer>().sprite.texture.name;
		SpriteRenderer sp = Striker.instance.currentStrikerObject.transform.GetChild(0).GetComponent<SpriteRenderer>();
		string sBallColor = sp.sprite.texture.name;

		if(sCurrentColor!=sBallColor){
			for(int i=0;i<nParticles;i++){
				particles[i].GetComponent<SpriteRenderer>().sprite = targetSprites[int.Parse(sBallColor)-1];
			}
			for(int i=0;i<nBounceParticles;i++){
				bounceParticles[i].GetComponent<SpriteRenderer>().sprite = targetSprites[int.Parse(sBallColor)-1];
			}
		}
	}

	////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
	//Comprobar si la linea de objetivo colisiona con alguna bola o con los laterales de la pantalla.
	//Lanzamos 3 rayos para asegurar que la linea de target indique realmente donde ira la bola.
	void CheckCollision()
	{
		Vector3 v1 = launcher.transform.right.normalized;
		Vector3 v2 = -launcher.transform.right.normalized;

		Vector3 p1 = launcher.transform.position + v1*ballRadius;
		Vector3 p2 = launcher.transform.position + v2*ballRadius;

		RaycastHit hit;
		Ray ray = new Ray(launcher.transform.position, launcher.transform.up);
		Ray ray1 = new Ray(p1, launcher.transform.up);
		Ray ray2 = new Ray(p2, launcher.transform.up);

		float maxDist1=100000;
		float maxDist2=100000;
		float maxDist3=100000;
		float maxDist4=100000;
		maxDist = 0;
		bool bHit=false;

		//Comprobar raycast con un rayo desde la derecha del launcher contra los PlayingObjects
		if(Physics.Raycast(ray1, out hit, 100, layermask2)){
			maxDist1 = Vector3.Distance(launcher.transform.position, hit.point) - 0.1f;
			bBounceOn=false;
			bHit=true;
		}
		//Comprobar raycast con un rayo desde la izquierda del launcher contra los PlayingObjects
		if(Physics.Raycast(ray2, out hit, 100, layermask2)){
			maxDist2 = Vector3.Distance(launcher.transform.position, hit.point) - 0.1f;
			bBounceOn = false;
			bHit=true;
		}
		//Comprobar raycast desde el centro del launcher contra los PlayingObjects
		if(Physics.Raycast(ray, out hit, 100, layermask2)){
			maxDist3 = Vector3.Distance(launcher.transform.position, hit.point) - 0.1f;
			bBounceOn = false;
			bHit=true;
		}
		if(!bHit){
			//Comprobar raycast desde el centro del launcher contra los Boundaries
			if(Physics.Raycast(ray, out hit, 100, layermask)){
				maxDist4 = Vector3.Distance(launcher.transform.position, hit.point) - ballRadius;;
				offsetDist = (1-((maxDist4/spacing)-(int)(maxDist4/spacing)))*spacing;
				Vector3 v = (launcher.transform.position - hit.point).normalized;
				hitPoint = hit.point + v*ballRadius;;
				hitNormal = hit.normal;
				rayDirection = ray.direction;
				bBounceOn=true;
			}
		}
		maxDist = Mathf.Min(maxDist1, Mathf.Min(maxDist2, Mathf.Min(maxDist3, maxDist4)));
		if(maxDist>1000)
			maxDist=0;
	}

	////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

	void CheckCollisionBounce()
	{
		RaycastHit hit;
		Vector3 reflection = Vector3.Reflect(rayDirection, hitNormal).normalized;
		Ray ray = new Ray(hitPoint, reflection);

		if(Physics.Raycast(ray, out hit, 100, layermask)){
			maxDistBounce = Vector3.Distance(hitPoint, hit.point);
		}else{
			maxDistBounce = 0;
		}
	}
}








