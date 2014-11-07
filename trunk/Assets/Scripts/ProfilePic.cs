using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class ProfilePic : MonoBehaviour
{
	Image profilePic;
	Sprite spritefb = null;
	
	////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

	void Awake()
	{
		profilePic = GameObject.Find("ProfileImage").GetComponent<Image>();
		DontDestroyOnLoad(gameObject);
	}
	
	////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
	
	void Start()
	{
		if(spritefb == null){
			StartCoroutine("getProfileImage");
		}
	}
	
	////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

	IEnumerator getProfileImage()
	{
		WWW url = new WWW("https" + "://graph.facebook.com/" + FB.UserId + "/picture?type=large"); //+ "?access_token=" + FB.AccessToken);
		Texture2D textFb2 = new Texture2D(128, 128, TextureFormat.DXT1, false); //TextureFormat must be DXT5
		yield return url;
		url.LoadImageIntoTexture(textFb2);
		spritefb = Sprite.Create(textFb2, new Rect(0, 0, textFb2.width, textFb2.height), new Vector2(0.5f, 0.5f));
		profilePic.sprite = spritefb;
	}
}
