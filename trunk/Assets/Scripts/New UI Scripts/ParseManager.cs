//https://www.parse.com/docs/unity_guide#objects
using UnityEngine;
using System.Collections;
using Parse;

public class ParseManager : MonoBehaviour
{
	void Start()
	{
		ParseObject testObject = new ParseObject("TestObject");
		testObject["foo"] = "bar";
		testObject.SaveAsync();
	}
}
