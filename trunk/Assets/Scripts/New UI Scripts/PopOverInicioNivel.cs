using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class PopOverInicioNivel : MonoBehaviour
{
	public Sprite image_despeja;
	public Sprite image_animales;
	public Text text;
	Image image;
	
	////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
	
	void Awake()
	{
		image = GetComponent<Image>();
	}
	
	////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
	
	void OnEnable()
	{
		WorldButton.OnWorldButtonPressed += SetContents;
	}
	
	////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
	
	void OnDisable()
	{
		WorldButton.OnWorldButtonPressed -= SetContents;
	}
	
	////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
	
	void SetContents()
	{
		if(PlayingObjectManager.GetLevelMission() == PlayingObjectManager.MissionType.Normal){
			image.sprite = image_despeja;
			text.text = LanguageManager.GetText("id_infoclear");
		}
		else if(PlayingObjectManager.GetLevelMission() == PlayingObjectManager.MissionType.Animals){
			image.sprite = image_animales;
			text.text = LanguageManager.GetText("id_info_animals");
		}
		else{
			Debug.Log("ERROR: El tipo de mision no existe");
		}
	}
}
