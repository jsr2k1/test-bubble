//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
//https://developers.facebook.com/docs/games/requests/v2.3
//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System;
using System.Collections.Generic;
using Facebook;
using com.adjust.sdk;

public class Friend
{
	public string id;
	public string name;
	public string highScore;
	public string currentLevel;
	public Sprite profilePicture;
	public bool bPictureDone;
	public bool bLevelDone;
	
	public Friend(){
		id="empty";
		name="empty";
		highScore="0";
		currentLevel="empty";
		profilePicture=null;
		bPictureDone=false;
		bLevelDone=false;
	}
}

//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

public class FacebookManager : MonoBehaviour
{
	public static FacebookManager instance;
	//GameObject buttonInvite;
	GameObject buttonMessages;
	PopUpMgr messagesPopUp;
	
	//PopUpMgr inviteFriendsPopUp;
	//public GameObject friendPrefab;		//Prefab para la lista de amigos (invitar | pedir vidas)
	//GameObject contentFriends;			//Contenedor para la lista de amigos
	
	public GameObject entryPrefab;			//Prefab para las entradas de los mensajes pendientes del usuario
	public GameObject friendFramePrefab; 	//Foto de perfil del amigo en el mapa
	GameObject contentMessages;				//Contenedor de mensajes pendientes del usuario
	public float timeReadRequest;			//Tiempo entre lecturas de las requests (30 seg.)
	GameObject ImageDummy;
	
	//public List<Toggle> invitableFriendsToggles;	//Lista con los toggles de los amigos invitables para poder seleccionar y deseleccionar
	//public Toggle SelectAllToggle;					//Toggle para seleccionar o deseleccionar a todos los amigos
	
	List<string> sendLifeUserList;
	List<string> requestsList;
	List<GameObject> entriesList;

	Dictionary<string, object> requestItem;
	Dictionary<string, object> fromItem;
	Dictionary<string, object> pendingRequests;
	List<object> pendingRequestsData;
	
	//Dictionary<string, object> pictureItem;
	//Dictionary<string, object> dataItem;

	public Dictionary<string, Friend> friendsDict;
	//public Dictionary<string, Friend> invitableFriendsDict;
	
	int livesCounter=0;
		
	//string status = "Ready";
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
	//string lastResponse = "";
	
	public bool bShowDebug=false;
	
	string objectId = "1494978434114506"; //id de la instancia de la vida que hemos creado en la consola de Facebook
	
	public static string facebookUserName; //Tiene que ser static pq se llama desde FacebookBubble en el menu y FacebookManager no existe todavia
	public static string facebookEmail;
	public static string facebookGender;
	
	public GameObject buttonFacebookFeed;
	Button buttonFeed;
	Text textFeed;
	
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
		friendsDict = new Dictionary<string, Friend>();
		//invitableFriendsDict = new Dictionary<string, Friend>();
		
		//pictureItem = new Dictionary<string, object>();
		//dataItem = new Dictionary<string, object>();
		
		//invitableFriendsToggles = new List<Toggle>();
				
		//DeleteAllRequests(); //Only for testing
	}
	
	//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
	//Al cargar el nivel de los mundos vamos leyendo las requests cada cierto tiempo
	void OnLevelWasLoaded(int level)
	{
		if(Application.loadedLevelName == "04 World Menu"){
			GetObjectReferences();
		}
		if(FB.IsLoggedIn){
			//Worlds
			if(Application.loadedLevelName == "04 World Menu"){
				InvokeRepeating("ReadAllRequests", 1.0f, timeReadRequest);
			}else{
				CancelInvoke("ReadAllRequests");
			}
			if(Application.loadedLevelName == "04 World Menu" || Application.loadedLevelName == "06 Arcade Game Scene"){
				if(!friendsDict.ContainsKey(FB.UserId)){
					Friend newFriend = new Friend();
					newFriend.id = FB.UserId;
					newFriend.name = facebookUserName;
					newFriend.highScore = PlayerPrefs.GetInt("Highscore").ToString();
					friendsDict.Add(FB.UserId, newFriend);
				}
				if(!friendsDict[FB.UserId].bPictureDone){
					StartCoroutine(GetProfileImage(FB.UserId, facebookUserName));
				}
				GetFriendsPictures();
			}
		}
	}
	
	//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
	
	void GetObjectReferences()
	{
		//buttonInvite = GameObject.Find("ButtonFacebookInvite");
		buttonMessages = GameObject.Find("ButtonFacebookMessages");
		messagesPopUp = GameObject.Find("FacebookMessagesPopUp").GetComponent<PopUpMgr>();
		//inviteFriendsPopUp = GameObject.Find("FacebookInviteFriendsPopUp").GetComponent<PopUpMgr>();
		contentMessages = GameObject.Find("ContentMessages");
		//contentFriends = GameObject.Find("ContentFriends");
		ImageDummy = GameObject.Find("ImageDummy");
			
		buttonMessages.SetActive(false);
		//buttonInvite.SetActive(FB.IsLoggedIn);
		
		//SelectAllToggle = GameObject.Find("ToggleSelectAll").GetComponent<Toggle>();
		
	}
	
	//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
	
	void Update()
	{
		/*
		if(Application.loadedLevelName=="04 World Menu"){
			if(FB.IsLoggedIn && !buttonInvite.activeSelf){
				buttonInvite.SetActive(true);
			}
			if(!FB.IsLoggedIn && buttonInvite.activeSelf){
				buttonInvite.SetActive(false);
			}
		}
		*/
		//if(bShowDebug) Debug.Log("status: " + status);
		//if(bShowDebug) Debug.Log("lastResponse: " + lastResponse);
	}
	
	//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
	
	public static void GetFacebookUserName()
	{
		if(FB.IsLoggedIn){
			//FB.API("v2.3/me?fields=id,name", HttpMethod.GET, GetFacebookUserNameCallback);
			FB.API("v2.3/me", HttpMethod.GET, GetFacebookUserNameCallback);
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
			facebookEmail = dict["email"].ToString();
			facebookGender = dict["gender"].ToString();
		}
		ParseManager.instance.CheckParseEntry();
	}
	
	//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
	
	void GetFriendsPictures()
	{
		if(bShowDebug) Debug.Log("GetFriendsPictures");
		
		if(FB.IsLoggedIn){
			FB.API("v2.3/me/friends", HttpMethod.GET, GetFriendsPicturesCallback);
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
			
			if(data!=null && data.Count>0)
			{
				//Recorremos la lista de amigos
				foreach(object friend in data)
				{
					Dictionary<string, object> currentFriend = friend as Dictionary<string,object>;
					string facebookID = currentFriend["id"] as String;
					string name = currentFriend["name"] as String;
					
					//Si no existe ese amigo en el diccionario -> lo añadimos
					if(!friendsDict.ContainsKey(facebookID)){
						Friend newFriend = new Friend();
						newFriend.id = facebookID;
						newFriend.name = name;
						friendsDict.Add(facebookID, newFriend);
					}
					//Creamos una instancia de la foto de perfil del amigo en el mapa
					if(Application.loadedLevelName == "04 World Menu"){
						GameObject friendEntry = Instantiate(friendFramePrefab, new Vector3(10000F, 10000F, 0), Quaternion.identity) as GameObject;
						friendEntry.GetComponent<ProfilePic>().Initialize(facebookID, name);
						friendEntry.transform.SetParent(ImageDummy.transform);
						friendEntry.transform.localScale = new Vector3(0.8f, 0.8f, 1.0f);
						
						//Colgamos la entrada del imageDummy y dejamos el profile pic del usuario que siga siendo el ultimo para que se vea delante
						int index = friendEntry.transform.GetSiblingIndex();
						friendEntry.transform.SetSiblingIndex(index-1);
					}
					//Si la imagen aun no ha estado cargada -> la cargamos
					if(!friendsDict[facebookID].bPictureDone){
						StartCoroutine(GetProfileImage(facebookID, name));
					}
				}
				if(Application.loadedLevelName == "06 Arcade Game Scene"){
					HighScoreManager.instance.StartCreateHighScoreTable();
				}
			}
		}
		else{
			if(bShowDebug) Debug.Log("GetFriendsPicturesCallback: Empty Response");
		}
	}
	
	//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
	//Obtenemos las imagenes de los amigos de Facebook para ponerlas en las requests
	IEnumerator GetProfileImage(string facebookID, string name)
	{
		WWW www = new WWW("https" + "://graph.facebook.com/" + facebookID + "/picture?type=large"); //+ "?access_token=" + FB.AccessToken);
		Texture2D textFb2 = new Texture2D(128, 128, TextureFormat.ARGB32, false);
		yield return www;
		if(String.IsNullOrEmpty(www.error)){
			www.LoadImageIntoTexture(textFb2);
		}else{
			Debug.LogError("Error al descargar la imagen: "+www.error);
		}
		
		if(textFb2!=null){
			if(bShowDebug) Debug.Log("GetProfileImage done, name:"+name+" id:"+facebookID);
			friendsDict[facebookID].profilePicture = Sprite.Create(textFb2, new Rect(0, 0, textFb2.width, textFb2.height), new Vector2(0.5f, 0.5f));
			friendsDict[facebookID].bPictureDone = true;
		}else{
			if(bShowDebug) Debug.Log("ERROR: textFb2 es null");
			StartCoroutine(GetProfileImage(facebookID, name));
		}
		
		//DestroyImmediate(textFb2);
		//textFb2=null;
	}
	
	/*
	//No se puede usar hasta que salga la v.7 del SDK de Facebook
	//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
	//Obtenemos las imagenes de los amigos de Facebook para ponerlas en las requests
	IEnumerator GetProfileImageFromURL(string url, string app_scoped_id, string name)
	{
		WWW www = new WWW(url);
		Texture2D textFb2 = new Texture2D(128, 128, TextureFormat.ARGB32, false);
		yield return www;
		if(String.IsNullOrEmpty(www.error)){
			www.LoadImageIntoTexture(textFb2);
		}else{
			Debug.LogError("Error al descargar la imagen: "+www.error);
		}
		
		if(textFb2!=null){
			if(bShowDebug) Debug.Log("GetProfileImage done, name:"+name+" id:"+app_scoped_id);
			invitableFriendsDict[app_scoped_id].profilePicture = Sprite.Create(textFb2, new Rect(0, 0, textFb2.width, textFb2.height), new Vector2(0.5f, 0.5f));
			invitableFriendsDict[app_scoped_id].bPictureDone = true;
		}else{
			if(bShowDebug) Debug.Log("ERROR: textFb2 es null");
			StartCoroutine(GetProfileImageFromURL(url, app_scoped_id, name));
		}
		
		//DestroyImmediate(textFb2);
		//textFb2=null;
	}*/
	
	//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
	//Pedir una vida a tus amigos
	public void ButtonPressedAskLive()
	{
		try{
			FriendSelectorFilters = "[\"app_users\"]";
			if(!FB.IsLoggedIn){
				FB.Login("public_profile,email,user_friends,publish_actions", AskLifeLoginCallback);
			}else{
				AskForOneLife();
			}
			//status = "Friend Selector called";
		}
		catch(Exception e){
			if(bShowDebug) Debug.Log("ButtonPressedAskLive error: " + e.Message);
			//status = e.Message;
		}
	}
	
	//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
	//Invitar a amigos que no tienen el juego instalado
	public void ButtonPressedInviteFriends()
	{
		if(!FB.IsLoggedIn){
			FB.Login("public_profile,email,user_friends,publish_actions", InviteFriendsLoginCallback);
		}
		else{
			DoInviteFriends();
		}
	}
	
	//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
	
	void InviteFriendsLoginCallback(FBResult result)
	{
		if(FB.IsLoggedIn){
			if(bShowDebug) Debug.Log(FB.UserId);
			DoInviteFriends();
		}
		else{
			if(bShowDebug) Debug.Log("User cancelled login");
		}
	}
	
	//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
	
	void DoInviteFriends()
	{
		try{
			if(bShowDebug) Debug.Log("Facebook Invite pressed");
			Adjust.trackEvent(new AdjustEvent("3xnjnv"));
			
			FriendSelectorFilters = "[\"app_non_users\"]";
			InviteFriends();
			//status = "Friend Selector called";
		}
		catch(Exception e){
			//status = e.Message;
			if(bShowDebug) Debug.Log("ButtonPressedInviteFriends error: " + e.Message);
		}
		
		//No se puede usar hasta que salga la v.7 del SDK de Facebook
		//GetInvitableFriendsList(); 
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
	
	//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
	//Publicar en el muro
	public void ButtonPressedFeed()
	{
		buttonFeed = buttonFacebookFeed.GetComponent<Button>();
		if(!buttonFeed.interactable){	//Es necesario ya que este boton usa el IPointerClickHandler en FacebookButtons
			return;
		}		
		buttonFeed.interactable=false;
		textFeed = buttonFacebookFeed.transform.GetChild(0).GetComponent<Text>();
		textFeed.text = LanguageManager.GetText("id_connecting");
		
		/*Con un dialogo
		FB.Feed(
			"", //string toId = "",
			"", //string link = "",
			"Level " + LevelManager.levelNo, //string linkName = "",
			"BUBBLE PARADISE 2", //string linkCaption = "",
			"I have completed the level " + LevelManager.levelNo + " of Bubble Paradise 2! Play for free and see how far you can get!", //string linkDescription = "",
			"http://aratingastudios.com/images/icon_192.png", //string picture = "",
			"", //string mediaSource = "",
			"", //string actionName = "",
			"", //string actionLink = "",
			"", //string reference = "",
			null, //Dictionary<string, string[]> properties = null,
			callback : FeedCallback); //FacebookDelegate callback = null)
		*/
		//Sin dialogo
		FB.API(
			"/me/feed",
			HttpMethod.POST,
			FeedCallback,
			new Dictionary<string, string>(){
				{"access_token",FB.AccessToken},
				{"caption","BUBBLE PARADISE 2"},
				{"description","I have completed the level "+LevelManager.levelNo+" of Bubble Paradise 2! You can play for free and see how far you can get!"},
				{"picture","http://aratingastudios.com/images/icon_192.png"},
				{"link", "https://apps.facebook.com/bubbleparadisetwo"}
			});
	}
	
	//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
	
	void FeedCallback(FBResult result)
	{
		if(!String.IsNullOrEmpty(result.Error)){
			Debug.Log("FeedCallback error: " + result.Error);
		}
		else if(!String.IsNullOrEmpty(result.Text)){
			Debug.Log("FeedCallback ok: " + result.Text);
			textFeed = buttonFacebookFeed.transform.GetChild(0).GetComponent<Text>();
			textFeed.text = LanguageManager.GetText("id_done");
		}
		else{
			Debug.Log("FeedCallback empty response");
		}
	}
	
	//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
	//No se puede usar hasta que salga la v.7 del SDK de Facebook
	/*
	public void ButtonPressedSendInvitations()
	{
		foreach(var friend in invitableFriendsDict)
		{
			FB.AppRequest(
				"Play this game!",
				//new string[]{friend.Value.id},
				new string[]{"10205103361372534"},
				null,
				null,
				null,
				"",
				"Invita a tus amigos",
				callback : CallbackInviteFriends);
			}
	}
	*/
	//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
	//No se puede usar hasta que salga la v.7 del SDK de Facebook
	/*
	void GetInvitableFriendsList()
	{
		if(messagesPopUp.bShow){
			return;
		}
		if(bShowDebug) Debug.Log("GetInvitableFriendsList");
		
		if(FB.IsLoggedIn){
			//FB.API("v2.3/me/invitable_friends", HttpMethod.GET, GetInvitableFriendsListCallback);
			FB.API("v2.3/me/invitable_friends?limit=30,fields=name,id", HttpMethod.GET, GetInvitableFriendsListCallback);
		}
	}*/
	/*
	//No se puede usar hasta que salga la v.7 del SDK de Facebook
	//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
	//Creamos entradas para el popup de amigos
	void GetInvitableFriendsListCallback(FBResult result)
	{
		if(!String.IsNullOrEmpty(result.Error)){
			if(bShowDebug) Debug.Log("GetInvitableFriendsListCallback: Error Response:" + result.Error);
		}
		else if(!String.IsNullOrEmpty(result.Text)){
			if(bShowDebug) Debug.Log("GetInvitableFriendsListCallback: Success Response:" + result.Text);
			Dictionary<string, object> friendsList = Facebook.MiniJSON.Json.Deserialize(result.Text) as Dictionary<string,object>;
			List<object> friendsListsData = friendsList["data"] as List<object>;
			
			invitableFriendsToggles.Clear();
			invitableFriendsDict.Clear();
			
			if(friendsListsData!=null && friendsListsData.Count>0){
				foreach(object entry in friendsListsData)
				{
					Dictionary<string, object> requestItem = entry as Dictionary<string,object>;
					string name = requestItem["name"] as String;
					string app_scoped_id = requestItem["id"] as String;
					pictureItem = requestItem["picture"] as Dictionary<string,object>;
					dataItem = pictureItem["data"] as Dictionary<string,object>;
					string image_url = dataItem["url"] as string;
					
					GameObject goEntry = Instantiate(friendPrefab) as GameObject;
					goEntry.transform.SetParent(contentFriends.transform);
					goEntry.transform.localScale = new Vector3(1.0f, 1.0f, 1.0f);
					goEntry.transform.GetChild(2).GetComponent<Text>().text = GetNameAndSurname(name);
					goEntry.transform.GetChild(1).GetComponent<FriendPicture>().id = app_scoped_id;
					
					invitableFriendsToggles.Add(goEntry.transform.GetChild(0).GetComponent<Toggle>());
					
					//Si no existe ese amigo en el diccionario -> lo añadimos
					if(!invitableFriendsDict.ContainsKey(app_scoped_id)){
						Friend newFriend = new Friend();
						newFriend.id = app_scoped_id;
						newFriend.name = name;
						invitableFriendsDict.Add(app_scoped_id, newFriend);
					}					
					
					//Si la imagen aun no ha estado cargada -> la cargamos
					if(!invitableFriendsDict[app_scoped_id].bPictureDone){
						StartCoroutine(GetProfileImageFromURL(image_url, app_scoped_id, name));
					}
				}
			}
		}
		else{
			if(bShowDebug) Debug.Log("GetInvitableFriendsListCallback: Empty Response");
		}
		
		inviteFriendsPopUp.ShowPopUp();
	}
	
	//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
	//Mostramos solamente el nombre y primer apellido. El segundo apellido no lo mostramos para que no se corte.
	string GetNameAndSurname(string fullname)
	{
		string[] words = fullname.Split(' ');
		if(words.Length>1){
			return words[0]+" "+words[1];
		}else{
			return fullname;
		}
	}*/
	
	//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
	
	void AskLifeLoginCallback(FBResult result)
	{
		if(FB.IsLoggedIn){
			if(bShowDebug) Debug.Log(FB.UserId);
			AskForOneLife();
		}
		else{
			if(bShowDebug) Debug.Log("User cancelled login");
		}
	}
	
	//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
	//Selecciona los amigos de facebook a los que vamos a enviar la request
	void FriendsSelector()
	{
		// If there's a Max Recipients specified, include it
		maxRecipients = null;
		if(FriendSelectorMax != ""){
			try{
				maxRecipients = Int32.Parse(FriendSelectorMax);
			}catch(Exception e){
				//status = e.Message;
				if(bShowDebug) Debug.Log("FriendsSelector error: " + e.Message);
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
			callback : CallbackInviteFriends
		);
	}
	
	//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
	
	void CallbackInviteFriends(FBResult result)
	{
		if(!String.IsNullOrEmpty(result.Error)){
			if(bShowDebug) Debug.Log("CallbackInviteFriends: Error Response:" + result.Error);
		}
		else if(!String.IsNullOrEmpty(result.Text)){
			var parameters = (Dictionary<string, object>)Facebook.MiniJSON.Json.Deserialize(result.Text);
			if(!parameters.ContainsKey("cancelled")){
				Adjust.trackEvent(new AdjustEvent("n0lux5"));
				if(bShowDebug) Debug.Log("CallbackInviteFriends: OK");
			}else{
				if(bShowDebug) Debug.Log("CallbackInviteFriends: Cancelled");
			}
		}
		else{
			if(bShowDebug) Debug.Log("CallbackInviteFriends: Empty Response");
		}
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
			callback : CallbackAskForOneLife
		);
	}
	
	//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
	
	void CallbackAskForOneLife(FBResult result)
	{
		if(!String.IsNullOrEmpty(result.Error)){
			if(bShowDebug) Debug.Log("CallbackAskForOneLife: Error Response:" + result.Error);
		}
		else if(!String.IsNullOrEmpty(result.Text)){
			Adjust.trackEvent(new AdjustEvent("1rvc9w"));
		}
		else{
			if(bShowDebug) Debug.Log("CallbackAskForOneLife: Empty Response");
		}
	}
	
	//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
	
	void ReadAllRequests()
	{
		if(messagesPopUp.bShow){
			return;
		}
		if(bShowDebug) Debug.Log("ReadAllRequests");
		
		if(FB.IsLoggedIn){
			FB.API("v2.3/me/apprequests?fields=id,from,object,action_type", HttpMethod.GET, ReadAllRequestsCallback);
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
				int count=0;
				foreach(object entry in pendingRequestsData)
				{
					requestItem = entry as Dictionary<string,object>;
					string requestID = requestItem["id"] as String;
					
					fromItem = requestItem["from"] as Dictionary<string,object>;
					string user_id = fromItem["id"] as string;
					string user_name = fromItem["name"] as string;
										
					string action_type="empty";
					if(requestItem.ContainsKey("action_type")){
						action_type = requestItem["action_type"] as string;
											
						GameObject goEntry = Instantiate(entryPrefab) as GameObject;
						goEntry.transform.SetParent(contentMessages.transform);
						goEntry.transform.localScale = new Vector3(1.0f, 1.0f, 1.0f);
						entriesList.Add(goEntry);
						
						if(action_type == OGActionType.AskFor.ToString()){
							goEntry.transform.GetChild(0).GetComponent<Text>().text = user_name + LanguageManager.GetText("id_needs_life");
							sendLifeUserList.Add(user_id);
						}else{
							goEntry.transform.GetChild(0).GetComponent<Text>().text = user_name + LanguageManager.GetText("id_gave_life");
							livesCounter++;
						}
						goEntry.transform.GetChild(1).GetComponent<FriendPicture>().id = user_id;
						count++;
					}
					requestsList.Add(requestID);
					if(bShowDebug) Debug.Log("Request: "+user_id+", "+user_name+", "+action_type);
				}
				//Puede que tengamos requests de invitacion a jugar. En ese caso, no queremos que se active el boton.
				if(count>0){
					buttonMessages.SetActive(true);
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
			FB.API("v2.3/me/apprequests?fields=id", HttpMethod.GET, DeleteAllRequestsCallback);
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
			FB.API("v2.3/"+requestID, HttpMethod.DELETE, Callback);
		}
	}
	
	//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
	//Borramos todas las listas y diccionarios que vamos usando para procesar las peticiones
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
		
		//pictureItem.Clear();
		//dataItem.Clear();
		
		livesCounter=0;

		Resources.UnloadUnusedAssets();
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
		FB.API("v2.3/me/objects/bubbleparadisetwo:life", HttpMethod.POST, AskForOneLifeCallback, formDic);
	}*/
	
	//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
	
	void Callback(FBResult result)
	{
		//lastResponseTexture = null;
		// Some platforms return the empty string instead of null.
		if(!String.IsNullOrEmpty(result.Error)){
			//lastResponse = "Error Response:\n" + result.Error;
		}
		else if(!String.IsNullOrEmpty(result.Text)){
			//lastResponse = "Success Response:\n" + result.Text;
		}
		else if(result.Texture != null){
			//lastResponseTexture = result.Texture;
		}
		else{
			//lastResponse = "Empty Response\n";
		}
	}
}




