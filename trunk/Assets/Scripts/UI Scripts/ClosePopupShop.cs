using UnityEngine;
using System.Collections;

public class ClosePopupShop : MonoBehaviour
{

		public Texture texture;
		public Texture overtexture;
		private Vector3 initialScreenPos;
		public ShopButton Script;
		public GameObject parent;
	
		void OnMouseEnter ()
		{
				// textura over
				renderer.material.mainTexture = overtexture;
		}
	
		void OnMouseDown ()
		{
				initialScreenPos = new Vector3 (Input.mousePosition.x, Input.mousePosition.y, Input.mousePosition.z);
		}
	
		void OnMouseExit ()
		{
				// textura normal
				renderer.material.mainTexture = texture;
		
		}
	
		void OnMouseUp ()
		{        
				Vector3 finalScreenPos = new Vector3 (Input.mousePosition.x, Input.mousePosition.y, Input.mousePosition.z);
		
				//print (Mathf.Sqrt((Mathf.Pow(initialScreenPos.x-finalScreenPos.x), 2f) + (Mathf.Pow(initialScreenPos.y-finalScreenPos.y), 2f)));
		
				float d = Mathf.Sqrt (Mathf.Pow ((initialScreenPos.x - finalScreenPos.x), 2f) + Mathf.Pow ((initialScreenPos.y - finalScreenPos.y), 2f));
		
		
				if (d < 10f) {
						//Button action
						StartCoroutine ("ResumeDelay");

						iTween.MoveTo (parent, iTween.Hash ("x", -13));

						//Script.setAppears(false);
				}
		}

		IEnumerator ResumeDelay ()
		{
				yield return new WaitForSeconds (0.5f);

				if (Application.loadedLevel == 3) {
						LevelManager.instance.pauseCtrl ();
				}
		}


}