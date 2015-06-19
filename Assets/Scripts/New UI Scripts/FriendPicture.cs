using UnityEngine;
using System.Collections;
using UnityEngine.UI;

//Este componente lo tendremos en las entradas de los mensajes de facebook (en la escena con los mundos y niveles)
//Tambien lo tendremos en el modo Arcade para la lista de higscores de los amigos
//No reusamos el componente "ProfilePic" ya que tiene el marco de la foto y el posicionamiento en el mapa
public class FriendPicture : MonoBehaviour
{
	Image image;
	public string id="empty";
	//bool is_invitable_friend=false;
	
	////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
	
	void Awake()
	{
		image = GetComponent<Image>();
	}
	
	////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
	
	void Start()
	{
		StartCoroutine(GetPicture());
	}
	
	////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
	//Obtenemos la imagen de facebook y la ponemos en la request
	IEnumerator GetPicture()
	{
		//No se puede usar hasta que salga la v.7 del SDK de Facebook
		/*
		if(is_invitable_friend){
			if(id!="empty" && FacebookManager.instance.invitableFriendsDict.ContainsKey(id) && FacebookManager.instance.invitableFriendsDict[id].profilePicture!=null){
				image.sprite = FacebookManager.instance.invitableFriendsDict[id].profilePicture;
			}else{
				yield return new WaitForSeconds(1);
				StartCoroutine(GetPicture());
			}
		}
		else{*/
			if(id!="empty" && FacebookManager.instance.friendsDict.ContainsKey(id) && FacebookManager.instance.friendsDict[id].profilePicture!=null){
				image.sprite = FacebookManager.instance.friendsDict[id].profilePicture;
			}else{
				yield return new WaitForSeconds(1);
				StartCoroutine(GetPicture());
			}
		//}
	}
}
