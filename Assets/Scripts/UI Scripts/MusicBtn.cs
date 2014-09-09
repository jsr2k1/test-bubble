using UnityEngine;
using System.Collections;

public class MusicBtn : MonoBehaviour
{

		public Sprite oversprite;
		public Sprite sprite;
		private Vector3 initialScreenPos;
		public AudioSource MusicSource;
		//= GameObject.Find("Music").GetComponent("AudioSource").audio;
		float VolumeMusic ;

		void Start ()
		{
				VolumeMusic = PlayerPrefs.GetFloat ("Music");
				MusicSource.volume = VolumeMusic; 
		}
	
		void OnMouseEnter ()
		{
				gameObject.GetComponent<SpriteRenderer> ().sprite = oversprite;
				//this.renderer.material.mainTexture = texture;
		}
	
		void OnMouseDown ()
		{
				initialScreenPos = new Vector3 (Input.mousePosition.x, Input.mousePosition.y, Input.mousePosition.z);
		}
	
		void OnMouseExit ()
		{
				gameObject.GetComponent<SpriteRenderer> ().sprite = sprite;
		}
	
		void OnMouseUp ()
		{        
				Vector3 finalScreenPos = new Vector3 (Input.mousePosition.x, Input.mousePosition.y, Input.mousePosition.z);
		
				float d = Mathf.Sqrt (Mathf.Pow ((initialScreenPos.x - finalScreenPos.x), 2f) + Mathf.Pow ((initialScreenPos.y - finalScreenPos.y), 2f));
				if (d < 10f)
				//Button action
				if (PlayerPrefs.GetFloat ("VolumeMusic") == 0f) {
						PlayerPrefs.SetFloat ("VolumeMusic", 1f);
						VolumeMusic = PlayerPrefs.GetFloat ("VolumeMusic");
						MusicSource.volume = VolumeMusic; 
				} else {
						PlayerPrefs.SetFloat ("VolumeMusic", 0f);
						VolumeMusic = PlayerPrefs.GetFloat ("VolumeMusic");
						MusicSource.volume = VolumeMusic; 
				}
		gameObject.GetComponent<SpriteRenderer> ().sprite = sprite;
		}
}
