using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class MissionCounter : MonoBehaviour
{
	//PlayingObjectManager.MissionType currentMission;
	public Image imgMission;
	public Text textMission;
	
	//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
	
	void Awake()
	{/*
		currentMission = PlayingObjectManager.GetLevelMission();
		
		if(currentMission == PlayingObjectManager.MissionType.Normal){
			imgMission.enabled=false;
			textMission.enabled=false;
		}
		else if(currentMission == PlayingObjectManager.MissionType.Animals){
			imgMission.enabled=true;
			textMission.enabled=true;
		}*/
	}
	
	//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
	
	void Update()
	{/*
		if(currentMission == PlayingObjectManager.MissionType.Animals){
			textMission.text = (PlayingObjectManager.missionCountTotal-PlayingObjectManager.missionCount) + "/" + PlayingObjectManager.missionCountTotal;
		}*/
	}
}
