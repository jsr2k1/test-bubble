////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
//http://blog.bigfootgaming.net/custom-facebook-open-graph-objects-unity3d/
//https://developers.facebook.com/docs/games/requests/v2.2
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
	
	public void ButtonPressed()
	{
		if(PlayerPrefs.GetInt("Sounds")==1){
			audio.Play();
		}
		try{
			//Pedir vidas
			if(bForceConnect){
				FriendSelectorFilters = "[\"app_users\"]";
				if(!FB.IsLoggedIn){
					FB.Login("public_profile,email,user_friends,publish_actions", AuthCallback);
				}
				CallAppRequestSendLives();
			}
			//Invitar amigos
			else{
				Debug.Log("Facebook Invite pressed");
				//Adjust.trackEvent("3xnjnv");
				FriendSelectorFilters = "[\"app_non_users\"]";
				CallAppRequestInvite();
			}
			
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
	void CallAppRequestInvite()
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
	//Crear una instancia de la vida que vamos a enviar
	void CallAppRequestSendLives()
	{
		Dictionary<string, string> formData = new Dictionary<string, string>();
		
		//formData["og:url"] = "http://samples.ogp.me/746045498766635";
		formData["og:title"] = "Sample Life";
		formData["og:type"] = "bubbleparadisetwo:life";
		//formData["og:image"] = "https://fbstatic-a.akamaihd.net/images/devsite/attachment_blank.png";
		//formData["og:description"] = "";
		formData["fb:app_id"] = "730922200278965";
		
		Dictionary<string, string> formDic = new Dictionary<string, string>();
		formDic["object"] = Facebook.MiniJSON.Json.Serialize(formData);
		FB.API("me/objects/bubbleparadisetwo:life", HttpMethod.POST, SendLiveCallback, formDic);
	}
	
	////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
	//Si se ha creado correctamente la instancia del objeto tipo Lifes -> hacemos el AppRequest
	void SendLiveCallback(FBResult result)
	{
		if(!String.IsNullOrEmpty(result.Error)){
			Debug.Log("-_-_ Send Live: Error Response:" + result.Error);
		}
		else if(!String.IsNullOrEmpty(result.Text)){
			Dictionary<string, object> dictJson = Facebook.MiniJSON.Json.Deserialize(result.Text) as Dictionary<string,object>;
			
			string objectId = dictJson["id"] as string;
			
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
		else{
			lastResponse = "Empty Response\n";
		}
	}
	
	////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
	
	void Callback(FBResult result)
	{
		//lastResponseTexture = null;
		// Some platforms return the empty string instead of null.
		if(!String.IsNullOrEmpty(result.Error))
		{
			lastResponse = "Error Response:\n" + result.Error;
		}
		else if(!String.IsNullOrEmpty(result.Text))
		{
			lastResponse = "Success Response:\n" + result.Text;
		}
		else if(result.Texture != null)
		{
			//lastResponseTexture = result.Texture;
			lastResponse = "Success Response: texture\n";
		}
		else
		{
			lastResponse = "Empty Response\n";
		}
	}
}




