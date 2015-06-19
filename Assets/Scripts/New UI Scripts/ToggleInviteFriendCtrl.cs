using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using UnityEngine.EventSystems;

//No podemos usar los callbacks de interface habituales pq entonces al pulsar un toggle tambien hacen trigger los otros y no funciona bien
public class ToggleInviteFriendCtrl : MonoBehaviour//, IPointerClickHandler
{
	//Toggle toggle;
	
	//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
	
	void Awake()
	{
		//toggle = GetComponent<Toggle>();
	}
	
	//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
	//No se puede usar hasta que salga la v.7 del SDK de Facebook
	/*
	public void OnPointerClick(PointerEventData data)
	{
		//Se ha pulsado el toggle de "Marcar todo"
		if(name=="ToggleSelectAll"){
			foreach(Toggle toggleFriend in FacebookManager.instance.invitableFriendsToggles){
				toggleFriend.isOn = toggle.isOn;
			}
		}//Se ha pulsado un toggle de un amigo	
		else{
			if(!toggle.isOn){
				FacebookManager.instance.SelectAllToggle.isOn = false;
			}
		}
	}*/
}
