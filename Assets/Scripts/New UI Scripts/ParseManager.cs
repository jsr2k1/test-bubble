//https://www.parse.com/docs/unity_guide#objects
//https://parse.com/apps/bubble-paradise-2/collections
using UnityEngine;
using System.Collections;
using Parse;

public class ParseManager : MonoBehaviour
{
	void Start()
	{
		//Create new
		/*ParseObject testObject = new ParseObject("TestObject");
		testObject["foo"] = "bar";
		testObject.SaveAsync();
		*/
		//Update
		ParseObject testObject = new ParseObject("TestObject");
		testObject.SaveAsync().ContinueWith(t =>
		{
			testObject["foo"] = "bar333";
			testObject.SaveAsync();
		});
	}
}
