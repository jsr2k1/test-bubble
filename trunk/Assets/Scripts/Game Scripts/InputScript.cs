using UnityEngine;
using System.Collections;

public class InputScript : MonoBehaviour
{
	//Launcher Sprite
	public Transform launcher;
	private float x, y, zRotation;
	private Vector3 reflection;
	public Transform thresoldLineTransform;

	//Line Renderer
	public LineRenderer line;

	////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

	void Update()
	{
		if(LevelManager.gameState == GameState.Start){
			Vector2 pos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
			if(Input.GetButton("Fire1")){
				//Launcher aim to the mouse/touch point
				x = Input.mousePosition.x - camera.WorldToScreenPoint(launcher.position).x;
				y = Input.mousePosition.y - camera.WorldToScreenPoint(launcher.position).y;
				//Dont draw the line or do actions when the click its under the launcher point
				if(pos.y > thresoldLineTransform.position.y){
					//Te rotation for aim
					zRotation = Mathf.Rad2Deg * Mathf.Atan2(x, y);
					//Rotating the spaceship
					launcher.eulerAngles = new Vector3(0, 0, zRotation);
					//LineRenderer to draw the trace
					DrawLine();
				}
			}
			if(Input.GetButtonUp("Fire1")){
				//Dont draw the line or do actions when the click its under the launcher point
				if(pos.y > thresoldLineTransform.position.y){
					line.enabled = false;
					Vector2 FinalPos = new Vector2(-pos.x, pos.y);
					InGameScriptRefrences.strikerManager.Shoot(FinalPos);
				}
			}
		}
	}

	////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

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
	}

		//Shoot the playing object in the touched direction
		/*Deprecated used instead the input get button fire1 to use it in more platforms
    void OnMouseDown()
   {
        Vector2 pos = Camera.mainCamera.ScreenToWorldPoint(Input.mousePosition);
        InGameScriptRefrences.strikerManager.Shoot(pos);
    }*/

}
