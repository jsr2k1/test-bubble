using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class MissionCounter : MonoBehaviour
{
	public static MissionCounter instance;
	PlayingObjectManager.MissionType currentMission;
	public Image imgMission;
	public Text textMission;
	
	//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
	
	void Awake()
	{
		instance = this;
		imgMission.enabled=false;
		textMission.enabled=false;
		currentMission = PlayingObjectManager.GetLevelMission();
	}

	//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

	void Start()
	{
		if(currentMission == PlayingObjectManager.MissionType.Animals /*&& !imgMission.enabled && PlayingObjectManager.missionCountTotal>0*/){
			imgMission.enabled=true;
			textMission.enabled=true;
		}
	}

	//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

	public void SetCounter()
	{
		if(currentMission == PlayingObjectManager.MissionType.Animals){
			textMission.text = PlayingObjectManager.missionCount + "/" + PlayingObjectManager.missionCountTotal;
		}
	}

	//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
	
	void Update()
	{/*
		if(currentMission == PlayingObjectManager.MissionType.Animals && !imgMission.enabled && PlayingObjectManager.missionCountTotal>0){
			imgMission.enabled=true;
			textMission.enabled=true;
		}*/
		/*
		if(currentMission == PlayingObjectManager.MissionType.Animals){
			textMission.text = PlayingObjectManager.missionCount + "/" + PlayingObjectManager.missionCountTotal;
		}*/
	}
}
