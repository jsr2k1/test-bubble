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
		if(id!="empty" && FacebookManager.instance.friendsDict.ContainsKey(id)){
			image.sprite = FacebookManager.instance.friendsDict[id].profilePicture;
		}else{
			yield return new WaitForSeconds(1);
			StartCoroutine(GetPicture());
		}
	}
}
