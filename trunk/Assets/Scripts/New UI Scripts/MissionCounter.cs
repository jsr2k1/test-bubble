using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class MissionCounter : MonoBehaviour
{
	PlayingObjectManager.MissionType currentMission;
	public Image imgMission;
	public Text textMission;
	
	//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
	
	void Awake()
	{
		imgMission.enabled=false;
		textMission.enabled=false;
		currentMission = PlayingObjectManager.GetLevelMission();
	}
	
	//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
	
	void Update()
	{
		if(currentMission == PlayingObjectManager.MissionType.Animals && !imgMission.enabled && PlayingObjectManager.missionCountTotal>0){
			imgMission.enabled=true;
			textMission.enabled=true;
		}
		
		if(currentMission == PlayingObjectManager.MissionType.Animals){
			textMission.text = PlayingObjectManager.missionCount + "/" + PlayingObjectManager.missionCountTotal;
		}
	}
}
