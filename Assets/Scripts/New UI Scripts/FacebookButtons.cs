using UnityEngine;
using System.Collections;
using UnityEngine.EventSystems;
using GameAnalyticsSDK;

public class FacebookButtons : MonoBehaviour, IPointerClickHandler
{
	//Estamos en la escena de mundos y tenemos que acceder al objeto FacebookManager que viene de la escena de splash
	//No podemos asignar el objeto mediante la interfaz de Unity pq en esta escena no existe
	public void OnPointerClick(PointerEventData data)
	{
		if(name=="ButtonFacebookInvite"){
			FacebookManager.instance.ButtonPressedInviteFriends();
			GameAnalytics.NewDesignEvent ("InviteFriendsButton");
		}
		else if(name=="ButtonFacebookMessages"){
			FacebookManager.instance.ButtonPressedMessages();
		}
		else if(name=="ButtonAskFriends"){
			FacebookManager.instance.ButtonPressedAskLive();
		}
		else if(name=="ButtonAccept"){
			FacebookManager.instance.ButtonPressedAccept();
		}
		//No se puede usar hasta que salga la v.7 del SDK de Facebook
		//else if(name=="ButtonSendInvitations"){
		//	FacebookManager.instance.ButtonPressedSendInvitations();
		//}
	}
}
