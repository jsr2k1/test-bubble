using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System;
using System.Collections.Generic;

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
		try{
			//Pedir vidas
			if(bForceConnect){
				FriendSelectorFilters = "[\"app_users\"]";
				if(!FB.IsLoggedIn){
					FB.Login("email", AuthCallback);
				}
			}
			//Invitar amigos
			else{
				FriendSelectorFilters = "[\"app_non_users\"]";
			}
			CallAppRequestAsFriendSelector();
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
	
	void CallAppRequestAsFriendSelector()
	{
		// If there's a Max Recipients specified, include it
		int? maxRecipients = null;
		if(FriendSelectorMax != "")
		{
			try{
				maxRecipients = Int32.Parse(FriendSelectorMax);
			}catch(Exception e){
				status = e.Message;
			}
		}
		
		// include the exclude ids
		string[] excludeIds =(FriendSelectorExcludeIds == "") ? null : FriendSelectorExcludeIds.Split(',');
		List<object> FriendSelectorFiltersArr = null;
		if(!String.IsNullOrEmpty(FriendSelectorFilters))
		{
			try{
				FriendSelectorFiltersArr = Facebook.MiniJSON.Json.Deserialize(FriendSelectorFilters) as List<object>;
			}
			catch{
				throw new Exception("JSON Parse error");
			}
		}
		
		FB.AppRequest(
			FriendSelectorMessage,
			null,
			FriendSelectorFiltersArr,
			excludeIds,
			maxRecipients,
			FriendSelectorData,
			FriendSelectorTitle,
			callback : appRequestCallback
		);
	}
	
	////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
	
	void appRequestCallback(FBResult result)
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




