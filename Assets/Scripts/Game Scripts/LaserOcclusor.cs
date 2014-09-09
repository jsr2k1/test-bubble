using UnityEngine;
using System.Collections;

public class LaserOcclusor : MonoBehaviour {

	public Transform laserOrigin;

	// Use this for initialization
	void Start () {
		InvokeRepeating("CheckLasers",0.5f,0.2f);
	}


	void CheckLasers() 
	{
		RaycastHit hit;
		Ray ray = new Ray (laserOrigin.position, Vector3.back * 0.8f);
		Debug.DrawRay (laserOrigin.position, Vector3.back * 0.8f, Color.green);
		
		if (Physics.Raycast (ray, out hit)) {
			
			
			if(hit.collider.tag == "Playing Object")
			{
				//sticky anchor point ocuped
				ToggleCollider(false);
			}
			else
			{
				//no ball over the sticky anchor point
				ToggleCollider(true);
			}
		}
	}

	public void ToggleCollider( bool b)
	{
		this.collider.enabled = b;
	}
}
