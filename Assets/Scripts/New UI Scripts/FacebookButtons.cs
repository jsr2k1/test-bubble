﻿using UnityEngine;
using System.Collections;
using UnityEngine.EventSystems;

public class FacebookButtons : MonoBehaviour, IPointerClickHandler
{
	//Estamos en la escena de mundos y tenemos que acceder al objeto FacebookManager que viene de la escena de splash
	//No podemos asignar el objeto mediante la interfaz de Unity pq en esta escena no existe
	public void OnPointerClick(PointerEventData data)
	{
		if(name=="ButtonFacebookInvite"){
			FacebookManager.instance.ButtonPressedInviteFriends();
		}
		else if(name=="ButtonFacebookMessages"){
			FacebookManager.instance.ButtonPressedMessages();
		}
	}
}
