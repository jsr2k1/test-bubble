////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
//http://blog.bigfootgaming.net/custom-facebook-open-graph-objects-unity3d/
//https://developers.facebook.com/docs/games/requests/v2.1#gifting
//http://www.neatplug.com/integration-guide-unity3d-facebook-sns-plugin
//https://developers.facebook.com/docs/games/requests/v2.2
//https://www.parse.com/tutorials/integrating-facebook-in-unity
////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System;
using System.Collections.Generic;
using Facebook;

public class FacebookRequest : MonoBehaviour
{
	public bool bForceConnect=false;
	Button facebookButton;
	Image facebookImage;
	
	string status = "Ready";
	string FriendSelectorMax = "";
	string FriendSelectorExcludeIds = "";
	string FriendSelectorFilters = "";
	string FriendSelectorMessage = "Derp";
	string FriendSelectorData = "{}";
	string FriendSelectorTitle = "";
	
	List<object> FriendSelectorFiltersArr;
	string[] excludeIds;
	int? maxRecipients;
	
	//Texture2D lastResponseTexture;
	string lastResponse = "";
	
	bool showDebug=false;
	
	////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
	
	void Awake()
	{
		facebookButton = GetComponent<Button>();
		facebookImage = GetComponent<Image>();
		
		if(FB.IsLoggedIn){
			facebookButton.enabled=true;
			facebookImage.enabled=true;
		}
	}
	
	////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
	
	void Update()
	{
		if(FB.IsLoggedIn && !facebookButton.enabled){
			facebookButton.enabled=true;
			facebookImage.enabled=true;
		}
		
		if(!FB.IsLoggedIn && facebookButton.enabled && !bForceConnect){
			facebookButton.enabled=false;
			facebookImage.enabled=false;
		}
		
		if(showDebug){
			Debug.Log("status: " + status);
			Debug.Log("lastResponse: " + lastResponse);
		}
	}
	
	////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
	//Pedir una vida a tus amigos
	public void ButtonPressedAskLive()
	{
		if(PlayerPrefs.GetInt("Sounds")==1){
			audio.Play();
		}
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
	
	////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
	//Invitar a amigos que no tienen el juego instalado
	public void ButtonPressedInviteFriends()
	{
		if(PlayerPrefs.GetInt("Sounds")==1){
			audio.Play();
		}
		try{
			Debug.Log("Facebook Invite pressed");
			//Adjust.trackEvent("3xnjnv");
			FriendSelectorFilters = "[\"app_non_users\"]";
			//InviteFriends();
			ReadAllRequests();
			status = "Friend Selector called";
		}
		catch(Exception e){
			status = e.Message;
		}
	}
	
	////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
	
	void AuthCallback(FBResult result)
	{
		if(FB.IsLoggedIn){
			Debug.Log(FB.UserId);
			//StartCoroutine("OnLoggedIn"); 
		} else {
			Debug.Log("User cancelled login");
		}
	}
	
	////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
	
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
	
	////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
	//Invite facebook friends to play this game
	void InviteFriends()
	{
		FriendsSelector();
		
		FB.AppRequest(
			FriendSelectorMessage,
			null,
			FriendSelectorFiltersArr,
			excludeIds,
			maxRecipients,
			FriendSelectorData,
			FriendSelectorTitle,
			callback : Callback
		);
	}
	
	////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
	void AskForOneLife()
	{
		string objectId = "1494978434114506";
		
		FriendsSelector();
		
		FB.AppRequest(FriendSelectorMessage,
			OGActionType.Send,
			objectId,
			FriendSelectorFiltersArr,
			excludeIds,
			maxRecipients,
			FriendSelectorData,
			FriendSelectorTitle,
			callback : Callback
		);
	}
	
	////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
	
	void ReadAllRequests()
	{
		FB.API("v2.2/me/apprequests?fields=id,from,object,action_type", HttpMethod.GET, ReadAllRequestsCallback);
	}
	
	////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
	
	void ReadAllRequestsCallback(FBResult result)
	{
		if(!String.IsNullOrEmpty(result.Error)){
			Debug.Log("ReadAllRequestsCallback: Error Response:" + result.Error);
		}
		else if(!String.IsNullOrEmpty(result.Text)){
			Debug.Log("ReadAllRequestsCallback: Success Response:" + result.Text);
			Dictionary<string, object> requests = Facebook.MiniJSON.Json.Deserialize(result.Text) as Dictionary<string,object>;
			List<object> data = requests["data"] as List<object>;
			Dictionary<string, object> request0 = data[0] as Dictionary<string,object>;
			Dictionary<string, object> from = request0["from"] as Dictionary<string,object>;
		
			Debug.Log(from["id"]);
			Debug.Log(from["name"]);
			Debug.Log(request0["action_type"] as string);
		}
		else{
			Debug.Log("ReadAllRequestsCallback: Empty Response");
		}
	}	
	
	/*
	////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
	//Crear una instancia de la vida que vamos a enviar
	void AskForOneLife()
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
	}
	
	////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
	//Si se ha creado correctamente la instancia del objeto tipo Lifes -> hacemos el AppRequest
	void AskForOneLifeCallback(FBResult result)
	{
		if(!String.IsNullOrEmpty(result.Error)){
			lastResponse="-_-_ Send Live: Error Response:" + result.Error;
		}
		else if(!String.IsNullOrEmpty(result.Text)){
			lastResponse="-_-_ Send Live: Object created ok:" + result.Text;
			Dictionary<string, object> dictJson = Facebook.MiniJSON.Json.Deserialize(result.Text) as Dictionary<string,object>;
			string objectId = dictJson["id"] as string;
			
			FriendsSelector();
			
			FB.AppRequest(FriendSelectorMessage,
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
		else{
			lastResponse = "Empty Response\n";
		}
	}*/
	
	////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
	
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




