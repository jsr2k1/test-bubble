using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System;

public class ProfilePic : MonoBehaviour
{
	public Image profilePic;
	bool bFriend;
	string friendID;
	string facebookName;
	Transform currentLevel;
	bool bInit = false;
	
	Sprite spritefb;
	float ref_width = 500.0f;
	float ratio;
	
	////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
	
	void Awake()
	{
		spritefb=null;
		ratio = (float)Screen.width/ref_width;
		
		//Solo se usa para la imagen del user no de los amigos
		if(!bFriend){
			string sLevel = (PlayerPrefs.GetInt("Level")+1).ToString();
			if(PlayerPrefs.GetInt("Level")+1 > LevelParser.instance.maxLevels){
				sLevel = LevelParser.instance.maxLevels.ToString();
			}
			currentLevel = GameObject.Find(sLevel).transform;
		}
	}
	
	////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
	//Solo se usa para la imagen del user no de los amigos
	void Start()
	{
		if(FB.IsLoggedIn && spritefb == null && !bFriend){
			StartCoroutine(GetProfileImage());
		}
	}
	
	////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
	//No puedo hacer esto en el Start pq todavia no tiene el valor correcto de friendID
	public void Initialize(string sID, string sName)
	{
		bInit=false;
		bFriend = true;
		friendID = sID;
		facebookName = sName;
		StartCoroutine(GetProfileImage());
		
		if(FacebookManager.instance.friendsDict.ContainsKey(friendID) && FacebookManager.instance.friendsDict[friendID].bLevelDone){
			SetLevel();
		}else{
			StartCoroutine(GetLevel());
		}
	}
	
	////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
	//Pedimos la info del amigo al ParseManager, al obtener la respuesta nos avisa con el evento: OnGetFacebookFriendInfoDone
	IEnumerator GetLevel()
	{
		while(ParseManager.instance.isBusy){
			yield return null;
		}
		ParseManager.instance.GetFacebookFriendInfo(friendID, facebookName);
	}
	
	////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
	
	void OnEnable()
	{
		ParseManager.OnGetFacebookFriendInfoDone += SetCurrentLevel;
	}
	
	/////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
	
	void OnDisable()
	{
		ParseManager.OnGetFacebookFriendInfoDone -= SetCurrentLevel;
	}
	
	////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
	
	void SetCurrentLevel()
	{
		if(friendID!=null && ParseManager.instance.currentFriendID==friendID){
			SetLevel();
		}
	}
	
	////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
	//Si no existe una entrada en el Parse con el amigo de Facebook -> Borramos el objeto
	void SetLevel()
	{
		if(FacebookManager.instance.friendsDict.ContainsKey(friendID)){
			string sLevel = FacebookManager.instance.friendsDict[friendID].currentLevel;
			if(sLevel!=null && sLevel!=""){
				//El amigo de facebook todavia no tiene una entrada en Parse
				if(sLevel=="empty"){
					Destroy(gameObject);
				}else{
					currentLevel = GameObject.Find(sLevel).transform;
					FacebookManager.instance.friendsDict[friendID].bLevelDone=true;
					bInit=true;
				}
			}
		}
	}
	
	////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
	//Colocamos la foto del amigo en el nivel correspondiente
	void LateUpdate()
	{
		if((bFriend && bInit) || !bFriend){
			transform.position = currentLevel.position + new Vector3(0.0f, 52.0f*ratio, 0.0f);
		}
	}
	
	////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
	//Obtenemos la imagen de facebook y la ponemos en la request
	IEnumerator GetProfileImage()
	{
		if(FacebookManager.instance.friendsDict.ContainsKey(bFriend ? friendID : FB.UserId) && FacebookManager.instance.friendsDict[bFriend ? friendID : FB.UserId].bPictureDone){
			profilePic.sprite = FacebookManager.instance.friendsDict[bFriend ? friendID : FB.UserId].profilePicture;
		}else{
			yield return new WaitForSeconds(1);
			StartCoroutine(GetProfileImage());
		}
	}
}





