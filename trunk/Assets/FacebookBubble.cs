using UnityEngine;
using System.Collections;

public class FacebookBubble : MonoBehaviour {

	public void loginfacebook()
	{
		FB.Login("email", AuthCallback);
	}

	void AuthCallback(FBResult result) {
		if(FB.IsLoggedIn) {
			Debug.Log(FB.UserId);
		} else {
			Debug.Log("User cancelled login");
		}
	}

}
