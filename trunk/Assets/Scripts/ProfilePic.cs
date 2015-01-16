using UnityEngine;
using System.Collections;
using UnityEngine.UI;

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
		string sLevel = (PlayerPrefs.GetInt("Level")+1).ToString();
		if(PlayerPrefs.GetInt("Level")+1 > LevelParser.instance.maxLevels){
			sLevel = LevelParser.instance.maxLevels.ToString();
		}
		currentLevel = GameObject.Find(sLevel).transform;
		ratio = (float)Screen.width/ref_width;
	}
	
	////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
	
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
		StartCoroutine(GetLevel());
	}
	
	////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
	
	IEnumerator GetLevel()
	{
		while(ParseManager.instance.isBusy){
			yield return null;
		}
		ParseManager.instance.GetFacebookFriend(friendID, facebookName);
	}
	
	////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
	
	void OnEnable()
	{
		ParseManager.OnGetFacebookFriendDone += SetCurrentLevel;
	}
	
	/////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
	
	void OnDisable()
	{
		ParseManager.OnGetFacebookFriendDone -= SetCurrentLevel;
	}
	
	////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
	//Si no existe una entrada en el Parse con el amigo de Facebook -> Borramos el objeto
	void SetCurrentLevel()
	{
		if(ParseManager.instance.currentFriendID==friendID){
			if(ParseManager.instance.currentFriendLevel=="not_found"){
				Destroy(gameObject);
			}else{
				currentLevel = GameObject.Find(ParseManager.instance.currentFriendLevel).transform;
				bInit=true;
			}
		}
	}
	
	////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
	
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
		if(FacebookManager.instance.friendsDict.ContainsKey(bFriend ? friendID : FB.UserId)){
			profilePic.sprite = FacebookManager.instance.friendsDict[bFriend ? friendID : FB.UserId].profilePicture;
		}else{
			yield return new WaitForSeconds(1);
			StartCoroutine(GetProfileImage());
		}
	}
}





