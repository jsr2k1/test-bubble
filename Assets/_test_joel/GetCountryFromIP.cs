using UnityEngine;
using System.Collections;

public class GetCountryFromIP : MonoBehaviour
{
	IEnumerator Start()
	{
		WWW www = new WWW("http://api.hostip.info/country.php");
		yield return www;
		Debug.Log(www.text);
	}
}
