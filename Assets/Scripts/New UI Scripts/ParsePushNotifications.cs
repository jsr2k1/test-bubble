//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
//https://parse.com/apps/quickstart#parse_push/unity/android/existing
//https://parse.com/tutorials/android-push-notifications
//https://parse.com/docs/unity/guide#push-notifications
//http://stackoverflow.com/questions/25237736/parse-com-ios-push-notifications-and-unity-integration
//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
using UnityEngine;
using System.Collections.Generic;
using System.Collections;
using Parse;
using System;

public class ParsePushNotifications : MonoBehaviour
{
	void Awake()
	{
		Debug.Log("_-_-_ ParsePushNotifications: Awake");
		
		#if UNITY_IOS
		NotificationServices.RegisterForRemoteNotificationTypes(RemoteNotificationType.Alert | RemoteNotificationType.Badge | RemoteNotificationType.Sound);
		#endif
		ParsePush.ParsePushNotificationReceived +=(sender, args) =>
		{
			#if UNITY_ANDROID
			AndroidJavaClass parseUnityHelper = new AndroidJavaClass("com.parse.ParsePushUnityHelper");
			AndroidJavaClass unityPlayer = new AndroidJavaClass("com.unity3d.player.UnityPlayer");
			AndroidJavaObject currentActivity = unityPlayer.GetStatic<AndroidJavaObject>("currentActivity");
			
			Debug.Log("_-_-_ ParsePushNotifications: Payload: " + args.Payload);
			
			parseUnityHelper.CallStatic("handleParsePushNotificationReceived", currentActivity, args.Payload);
			
			#elif UNITY_IOS
			IDictionary<string, object> payload = args.Payload;
			foreach(var key in payload.Keys) {
				Debug.Log("Payload: " + key + ": " + payload[key]);
			}
			#endif
		};
	}
	
	//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
	
	void Start()
	{
		Debug.Log("_-_-_ ParsePushNotifications: Start");
		
		if(ParseInstallation.CurrentInstallation != null && !string.IsNullOrEmpty(ParseInstallation.CurrentInstallation.DeviceToken)){
			Debug.Log("_-_-_ ParsePushNotifications: Device Token: " + ParseInstallation.CurrentInstallation.DeviceToken);
		}else{
			ParseInstallation installation = ParseInstallation.CurrentInstallation;
			installation.Channels = new List<string> { "" };
			installation.SaveAsync().ContinueWith(t => {
				if (t.IsFaulted || t.IsCanceled){
					Debug.Log("_-_-_ ParsePushNotifications: Push subscription failed.");
				}else{
					Debug.Log("_-_-_ ParsePushNotifications: Push subscription success.");
				}
			});
		}
	}
}




