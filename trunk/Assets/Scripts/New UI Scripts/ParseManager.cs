//https://www.parse.com/docs/unity_guide#objects
//https://parse.com/apps/bubble-paradise-2/collections
using UnityEngine;
using System.Collections;
using Parse;
using System.Collections.Generic;
using System.Threading.Tasks;

public class ParseManager : MonoBehaviour
{
	ParseObject currentParseObject;
	string currentLevel;
	string currentObjectID;
	Task getObjTask;
	Task getObjIDTask;
	bool emptyEntry=false;
	public bool bDebug;
	
	/////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

	void Awake()
	{
		currentParseObject = null;
	}

	/////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
	
	void Start()
	{
		if(FB.IsLoggedIn){
			StartCoroutine(CreateEntry("currentLevel", "1"));
			//StartCoroutine(GetCurrentLevel(facebookUserID));
			//StartCoroutine(SetCurrentLevel(facebookUserID, "14"));
		}
	}
	
	/////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
	
	IEnumerator CreateEntry(string entry, string currentLevel)
	{
		while(FacebookRequest.facebookUserID==null) yield return null;
		GetObject(FacebookRequest.facebookUserID);
		while(!getObjIDTask.IsCompleted) yield return null;
		while(!emptyEntry && !getObjTask.IsCompleted) yield return null;
		
		if(currentParseObject==null){
			ParseObject facebookUserObj = new ParseObject("FacebookUser");
			facebookUserObj["facebookUserID"] = FacebookRequest.facebookUserID;
			facebookUserObj["facebookUserName"] = FacebookRequest.facebookUserName;
			facebookUserObj[entry] = currentLevel;
			facebookUserObj.SaveAsync();
			if(bDebug){
				Debug.Log("PARSE_MANAGER: New entry created - facebookUserID:"+FacebookRequest.facebookUserID);
			}
		}
	}
	
	/////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
	
	IEnumerator GetCurrentLevel(string facebookUserID)
	{
		GetObject(facebookUserID);
		while(!getObjIDTask.IsCompleted) yield return null;
		while(!getObjTask.IsCompleted) yield return null;
		
		currentLevel = currentParseObject.Get<string>("currentLevel");
		
		if(bDebug){
			Debug.Log("PARSE_MANAGER: GetCurrentLevel - facebookUserID:"+FacebookRequest.facebookUserID+", currentLevel:"+currentLevel);
		}
	}
	
	/////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
	
	IEnumerator SetCurrentLevel(string facebookUserID, string currentLevel)
	{
		GetObject(facebookUserID);
		while(!getObjIDTask.IsCompleted) yield return null;
		while(!getObjTask.IsCompleted) yield return null;
		
		currentParseObject.SaveAsync().ContinueWith(t => {
			currentParseObject["currentLevel"] = currentLevel;
			currentParseObject.SaveAsync();
		});
		if(bDebug){
			Debug.Log("PARSE_MANAGER: SetCurrentLevel - facebookUserID:"+FacebookRequest.facebookUserID+", currentLevel:"+currentLevel);
		}
	}
	
	/////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
	
	void GetObject(string facebookUserID)
	{
		ParseQuery<ParseObject> query = ParseObject.GetQuery("FacebookUser").WhereEqualTo("facebookUserID", facebookUserID);
		getObjIDTask = query.FindAsync().ContinueWith(CallbackGetObject);
	}
	
	/////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
	
	void CallbackGetObject(Task<IEnumerable<ParseObject>> task)
	{
		//Obtenemos el objectID del resultado
		IEnumerable<ParseObject> results = task.Result;
		int count=0;
		foreach(ParseObject res in results){
			if(count>0){
				Debug.LogError("ERROR: Hay mas de un resultado");
				return;
			}else{
				currentObjectID = res.ObjectId;
				count++;
			}
		}
		if(count==0){
			if(bDebug){
				Debug.Log("PARSE_MANAGER: No entries - facebookUserID:"+FacebookRequest.facebookUserID);
			}
			emptyEntry=true;
			return;
		}else{
			//Obtenemos el ParseObject a partir de su objectID
			ParseQuery<ParseObject> query = ParseObject.GetQuery("FacebookUser");
			getObjTask = query.GetAsync(currentObjectID).ContinueWith(t => {
				currentParseObject = t.Result;
				emptyEntry=false;
			});
		}
	}
}










