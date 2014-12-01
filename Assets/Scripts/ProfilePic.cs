using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class ProfilePic : MonoBehaviour
{
	public Image profilePic;
	Sprite spritefb = null;
	Transform currentWorld;
	float ref_width = 500.0f;
	float ratio;
	
	////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
	
	void Awake()
	{

		string currentLevel = (PlayerPrefs.GetInt("Level")+1).ToString();
		if (PlayerPrefs.GetInt("Level")+1 > 40) {
			currentLevel = "40";
		}
		currentWorld = GameObject.Find(currentLevel).transform;
		ratio = (float)Screen.width/ref_width;
	}
	
	////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
	
	void Start()
	{
		if(FB.IsLoggedIn && spritefb == null){
			StartCoroutine("getProfileImage");
		}
	}
	
	////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
	
	void Update()
	{
		transform.position = currentWorld.position + new Vector3(0.0f, 52.0f*ratio, 0.0f);
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
