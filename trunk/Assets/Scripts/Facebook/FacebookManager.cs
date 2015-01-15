//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
//https://developers.facebook.com/docs/games/requests/v2.2
//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System;
using System.Collections.Generic;
using Facebook;

public class FacebookManager : MonoBehaviour
{
	public static FacebookManager instance;
	GameObject buttonInvite;
	GameObject buttonMessages;
	PopUpMgr messagesPopUp;
	public GameObject entryPrefab;
	public GameObject friendFramePrefab;
	GameObject contentMessages;
	public float timeReadRequest;
	GameObject ImageDummy;
	
	List<string> sendLifeUserList;
	List<string> requestsList;
	List<GameObject> entriesList;

	Dictionary<string, object> requestItem;
	Dictionary<string, object> fromItem;
	Dictionary<string, object> pendingRequests;
	List<object> pendingRequestsData;

	public Dictionary<string, Sprite> friendsPictures;
	
	int livesCounter=0;
		
	string status = "Ready";
	string FriendSelectorMax = "";
	string FriendSelectorExcludeIds = "";
	string FriendSelectorFilters = "";
	//string FriendSelectorMessage = "Derp";
	string FriendSelectorData = "{}";
	string FriendSelectorTitle = "";
	
	List<object> FriendSelectorFiltersArr;
	string[] excludeIds;
	int? maxRecipients;
	
	//Texture2D lastResponseTexture;
	string lastResponse = "";
	
	public bool bShowDebug=false;
	
	string objectId = "1494978434114506"; //id de la instancia de la vida que hemos creado en la consola de Facebook
	
	public static string facebookUserName; //Tiene que ser static pq se llama desde FacebookBubble en el menu y FacebookManager no existe todavia
	
	//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
	
	void Awake()
	{
		DontDestroyOnLoad(gameObject);

		instance = this;
		
		sendLifeUserList = new List<string>();
		requestsList = new List<string>();
		entriesList = new List<GameObject>();
		requestItem = new Dictionary<string, object>();
		fromItem = new Dictionary<string, object>();
		pendingRequests = new Dictionary<string, object>();
		pendingRequestsData = new List<object>();
		friendsPictures = new Dictionary<string, Sprite>();
		
		//DeleteAllRequests(); //Only for testing
	}
	
	//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
	//Al cargar el nivel de los mundos vamos leyendo las requests cada cierto tiempo
	void OnLevelWasLoaded(int level)
	{	
		//Worlds
		if(level == 2){
			GetObjectReferences();
			InvokeRepeating("ReadAllRequests", 1.0f, timeReadRequest);
			if(!friendsPictures.ContainsKey(FB.UserId)){
				StartCoroutine(GetProfileImage(FB.UserId));
			}
			GetFriendsPictures();
		}else{
			CancelInvoke("ReadAllRequests");
		}
	}
	
	//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
	
	void GetObjectReferences()
	{
		buttonInvite = GameObject.Find("ButtonFacebookInvite");
		buttonMessages = GameObject.Find("ButtonFacebookMessages");
		messagesPopUp = GameObject.Find("FacebookMessagesPopUp").GetComponent<PopUpMgr>();
		contentMessages = GameObject.Find("Content");
		ImageDummy = GameObject.Find("ImageDummy");
		
		if(buttonMessages!=null){
			buttonMessages.SetActive(false);
		}
	}
	
	//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
	
	void Update()
	{
		if(Application.loadedLevel==2){ //Worlds
			if(FB.IsLoggedIn && !buttonInvite.activeSelf){
				buttonInvite.SetActive(true);
			}
			if(!FB.IsLoggedIn && buttonInvite.activeSelf){
				buttonInvite.SetActive(false);
			}
		}
			
		if(bShowDebug) Debug.Log("status: " + status);
		if(bShowDebug) Debug.Log("lastResponse: " + lastResponse);
	}
	
	//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
	
	public static void GetFacebookUserName()
	{
		if(FB.IsLoggedIn){
			FB.API("v2.2/me?fields=id,name", HttpMethod.GET, GetFacebookUserNameCallback);
		}
	}
	
	//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
	
	static void GetFacebookUserNameCallback(FBResult result)
	{
		if(!String.IsNullOrEmpty(result.Error)){
			Debug.Log("GetFacebookUserNameCallback: Error Response:" + result.Error);
		}
		else if(!String.IsNullOrEmpty(result.Text)){
			Dictionary<string, object> dict = Facebook.MiniJSON.Json.Deserialize(result.Text) as Dictionary<string,object>;
			facebookUserName = dict["name"].ToString();
		}
		ParseManager.instance.CheckParseEntry();
	}
	
	//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
	
	void GetFriendsPictures()
	{
		if(bShowDebug) Debug.Log("GetFriendsPictures");
		
		if(FB.IsLoggedIn){
			FB.API("v2.2/me/friends", HttpMethod.GET, GetFriendsPicturesCallback);
		}
	}
	
	//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
	//Obtenemos las imagenes de los amigos de Facebook y las guardamos en un diccionario
	//Creamos una instancia del prefab de imagen de amigos para cada uno
	void GetFriendsPicturesCallback(FBResult result)
	{
		if(!String.IsNullOrEmpty(result.Error)){
			if(bShowDebug) Debug.Log("GetFriendsPicturesCallback: Error Response:" + result.Error);
		}
		else if(!String.IsNullOrEmpty(result.Text)){
			if(bShowDebug) Debug.Log("GetFriendsPicturesCallback: Success Response:" + result.Text);
			Dictionary<string, object> friends = Facebook.MiniJSON.Json.Deserialize(result.Text) as Dictionary<string,object>;
			List<object> data = friends["data"] as List<object>;
			
			if(data!=null && data.Count>0){
				foreach(object friend in data){
					Dictionary<string, object> currentFriend = friend as Dictionary<string,object>;
					string facebookID = currentFriend["id"] as String;
					string name = currentFriend["name"] as String;
					GameObject friendEntry = Instantiate(friendFramePrefab, new Vector3(10000F, 10000F, 0), Quaternion.identity) as GameObject;
					friendEntry.GetComponent<ProfilePic>().Initialize(facebookID, name);
					friendEntry.transform.SetParent(ImageDummy.transform);
					friendEntry.transform.localScale = new Vector3(0.8f, 0.8f, 1.0f);
					
					if(!friendsPictures.ContainsKey(facebookID)){
						StartCoroutine(GetProfileImage(facebookID));
					}
				}
			}
		}
		else{
			if(bShowDebug) Debug.Log("GetFriendsPicturesCallback: Empty Response");
		}
	}
	
	//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
	//Obtenemos las imagenes de los amigos de Facebook para ponerlas en las requests
	IEnumerator GetProfileImage(string facebookID)
	{
		WWW url = new WWW("https" + "://graph.facebook.com/" + facebookID + "/picture?type=large"); //+ "?access_token=" + FB.AccessToken);
		Texture2D textFb2 = new Texture2D(128, 128, TextureFormat.ARGB32, false);
		yield return url;
		url.LoadImageIntoTexture(textFb2);
		
		if(textFb2!=null){
			if(bShowDebug) Debug.Log("GetProfileImage:"+facebookID);
			friendsPictures.Add(facebookID, Sprite.Create(textFb2, new Rect(0, 0, textFb2.width, textFb2.height), new Vector2(0.5f, 0.5f)));
		}else{
			if(bShowDebug) Debug.Log("ERROR: textFb2 es null");
			StartCoroutine(GetProfileImage(facebookID));
		}
		
		//DestroyImmediate(textFb2);
		//textFb2=null;
	}
	
	//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
	//Pedir una vida a tus amigos
	public void ButtonPressedAskLive()
	{
		try{
			FriendSelectorFilters = "[\"app_users\"]";
			if(!FB.IsLoggedIn){
				FB.Login("public_profile,email,user_friends,publish_actions", AuthCallback);
			}
			AskForOneLife();
			status = "Friend Selector called";
		}
		catch(Exception e){
			status = e.Message;
		}
	}
	
	//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
	//Invitar a amigos que no tienen el juego instalado
	public void ButtonPressedInviteFriends()
	{
		try{
			if(bShowDebug) Debug.Log("Facebook Invite pressed");
			//Adjust.trackEvent("3xnjnv");
			FriendSelectorFilters = "[\"app_non_users\"]";
			InviteFriends();
			status = "Friend Selector called";
		}
		catch(Exception e){
			status = e.Message;
		}
	}
	
	//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
	
	void AuthCallback(FBResult result)
	{
		if(FB.IsLoggedIn){
			if(bShowDebug) Debug.Log(FB.UserId);
			//StartCoroutine("OnLoggedIn"); 
		} else {
			if(bShowDebug) Debug.Log("User cancelled login");
		}
	}
	
	//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
	
	void FriendsSelector()
	{
		// If there's a Max Recipients specified, include it
		maxRecipients = null;
		if(FriendSelectorMax != ""){
			try{
				maxRecipients = Int32.Parse(FriendSelectorMax);
			}catch(Exception e){
				status = e.Message;
			}
		}
		// include the exclude ids
		excludeIds =(FriendSelectorExcludeIds == "") ? null : FriendSelectorExcludeIds.Split(',');
		FriendSelectorFiltersArr = null;
		if(!String.IsNullOrEmpty(FriendSelectorFilters)){
			try{
				FriendSelectorFiltersArr = Facebook.MiniJSON.Json.Deserialize(FriendSelectorFilters) as List<object>;
			}
			catch{
				throw new Exception("JSON Parse error");
			}
		}
	}
	
	//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
	//Invite facebook friends to play this game
	void InviteFriends()
	{
		FriendsSelector();
		
		FB.AppRequest(
			"Invite friends to play",
			null,
			FriendSelectorFiltersArr,
			excludeIds,
			maxRecipients,
			FriendSelectorData,
			FriendSelectorTitle,
			callback : Callback
		);
	}
	
	//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
	//Pedir una vida de un usuario a otro
	void AskForOneLife()
	{
		FriendsSelector();
		
		FB.AppRequest(
			"Ask for one life",
			OGActionType.AskFor,
			objectId,
			FriendSelectorFiltersArr,
			excludeIds,
			maxRecipients,
			FriendSelectorData,
			FriendSelectorTitle,
			callback : Callback
		);
	}
	
	//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
	
	void ReadAllRequests()
	{
		if(messagesPopUp.bShow){
			return;
		}
		if(bShowDebug) Debug.Log("ReadAllRequests");
		
		if(FB.IsLoggedIn){
			FB.API("v2.2/me/apprequests?fields=id,from,object,action_type", HttpMethod.GET, ReadAllRequestsCallback);
		}
	}
	
	//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
	//Leemos todas las peticiones pendientes del servidor y creamos una instancia del prefab de peticion para cada una de ellas
	void ReadAllRequestsCallback(FBResult result)
	{	
		if(buttonMessages==null){
			return;
		}
		ClearData();
		
		if(!String.IsNullOrEmpty(result.Error)){
			if(bShowDebug) Debug.Log("ReadAllRequestsCallback: Error Response:" + result.Error);
		}
		else if(!String.IsNullOrEmpty(result.Text)){
			if(bShowDebug) Debug.Log("ReadAllRequestsCallback: Success Response:" + result.Text);
			pendingRequests = Facebook.MiniJSON.Json.Deserialize(result.Text) as Dictionary<string,object>;
			pendingRequestsData = pendingRequests["data"] as List<object>;
			
			if(pendingRequestsData!=null && pendingRequestsData.Count>0){
				buttonMessages.SetActive(true);
				int count=0;
				foreach(object entry in pendingRequestsData)
				{
					requestItem = entry as Dictionary<string,object>;
					string requestID = requestItem["id"] as String;
					string action_type = requestItem["action_type"] as string;
					
					fromItem = requestItem["from"] as Dictionary<string,object>;
					string user_id = fromItem["id"] as string;
					string user_name = fromItem["name"] as string;
					
					GameObject goEntry = Instantiate(entryPrefab) as GameObject;
					goEntry.transform.SetParent(contentMessages.transform);
					goEntry.transform.localPosition = new Vector3(0.0f, 145.0f-count*100.0f, 0.0f);
					goEntry.transform.localScale = new Vector3(1.0f, 1.0f, 1.0f);
					entriesList.Add(goEntry);
					count++;
					
					if(action_type == OGActionType.AskFor.ToString()){
						goEntry.transform.GetChild(0).GetComponent<Text>().text = user_name+" needs a life!";
						sendLifeUserList.Add(user_id);
					}else{
						goEntry.transform.GetChild(0).GetComponent<Text>().text = user_name+" gave you a life!";
						livesCounter++;
					}
					goEntry.transform.GetChild(1).GetComponent<RequestPicture>().id = user_id;
					requestsList.Add(requestID);
					if(bShowDebug) Debug.Log("Request: "+user_id+", "+user_name+", "+action_type);
				}
			}
		}
		else{
			if(bShowDebug) Debug.Log("ReadAllRequestsCallback: Empty Response");
		}
	}
	
	//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
	//Esta funcion solo de usa a modo de test
	//La funcion que se usa despues de procesar cada peticion es -> DeleteRequest
	void DeleteAllRequests()
	{
		if(bShowDebug) Debug.Log("DeleteAllRequests");
		
		if(FB.IsLoggedIn){
			FB.API("v2.2/me/apprequests?fields=id", HttpMethod.GET, DeleteAllRequestsCallback);
		}
	}
	
	//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
	
	void DeleteAllRequestsCallback(FBResult result)
	{
		if(!String.IsNullOrEmpty(result.Error)){
			if(bShowDebug) Debug.Log("DeleteAllRequestsCallback: Error Response:" + result.Error);
		}
		else if(!String.IsNullOrEmpty(result.Text)){
			if(bShowDebug) Debug.Log("DeleteAllRequestsCallback: Success Response:" + result.Text);
			Dictionary<string, object> requests = Facebook.MiniJSON.Json.Deserialize(result.Text) as Dictionary<string,object>;
			List<object> data = requests["data"] as List<object>;
			
			if(data!=null && data.Count>0){
				foreach(object request in data){
					Dictionary<string, object> currentRequest = request as Dictionary<string,object>;
					string requestID = currentRequest["id"] as String;
					DeleteRequest(requestID);
				}
			}
		}
		else{
			if(bShowDebug) Debug.Log("DeleteAllRequestsCallback: Empty Response");
		}
	}
	
	//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
	
	void DeleteRequest(string requestID)
	{
		if(bShowDebug) Debug.Log("DeleteRequest");
		
		if(FB.IsLoggedIn){
			FB.API("v2.2/"+requestID, HttpMethod.DELETE, Callback);
		}
	}
	
	//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
	//Borramoa todas las listas y diccionarios que vamos usando para procesar las peticiones
	void ClearData()
	{
		if(buttonMessages!=null){
			buttonMessages.SetActive(false);
		}
		foreach(GameObject go in entriesList){
			DestroyImmediate(go);
		}
		requestsList.Clear();
		entriesList.Clear();
		requestItem.Clear();
		fromItem.Clear();
		pendingRequests.Clear();
		pendingRequestsData.Clear();
		
		livesCounter=0;

		Resources.UnloadUnusedAssets();
	}
	
	//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
	//Mostrar el popup con los mensajes
	public void ButtonPressedMessages()
	{
		messagesPopUp.ShowPopUp();
	}
	
	//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
	//Responder a todos los mensajes de pedir vida
	public void ButtonPressedAccept()
	{
		//Enviar vidas a quien las haya pedido
		foreach(string user in sendLifeUserList){
			FB.AppRequest(
				"Send one life",
				OGActionType.Send,
				objectId,
				new string[]{user},
				FriendSelectorData,
				FriendSelectorTitle,
				callback : Callback
			);
		}
		sendLifeUserList.Clear();
		
		//Sumar las vidas recibidas
		LivesManager.lives+=livesCounter;
		
		//Borrar las requests ya procesadas
		foreach(string request in requestsList){
			DeleteRequest(request);
		}
		requestsList.Clear();
		
		//Borrar las entradas del popup
		foreach(GameObject go in entriesList){
			Destroy(go);
		}
		entriesList.Clear();
		
		messagesPopUp.HidePopUp();
		
		buttonMessages.SetActive(false);
	}
	
	/*
	//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
	//Crear una instancia de la vida que vamos a enviar
	void CreateOneLife()
	{
		Dictionary<string, string> formData = new Dictionary<string, string>();
	
		formData["og:title"] = "Sample Life";
		formData["og:type"] = "bubbleparadisetwo:life";
		formData["fb:app_id"] = "730922200278965";
		//formData["og:url"] = "http://samples.ogp.me/746045498766635";
		//formData["og:image"] = "https://fbstatic-a.akamaihd.net/images/devsite/attachment_blank.png";
		//formData["og:description"] = "";
		
		Dictionary<string, string> formDic = new Dictionary<string, string>();
		formDic["object"] = Facebook.MiniJSON.Json.Serialize(formData);
		FB.API("v2.2/me/objects/bubbleparadisetwo:life", HttpMethod.POST, AskForOneLifeCallback, formDic);
	}*/
	
	//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
	
	void Callback(FBResult result)
	{
		//lastResponseTexture = null;
		// Some platforms return the empty string instead of null.
		if(!String.IsNullOrEmpty(result.Error)){
			lastResponse = "Error Response:\n" + result.Error;
		}
		else if(!String.IsNullOrEmpty(result.Text)){
			lastResponse = "Success Response:\n" + result.Text;
		}
		else if(result.Texture != null){
			//lastResponseTexture = result.Texture;
			lastResponse = "Success Response: texture\n";
		}
		else{
			lastResponse = "Empty Response\n";
		}
	}
}




