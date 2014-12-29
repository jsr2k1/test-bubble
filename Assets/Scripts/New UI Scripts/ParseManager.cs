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
	string currentObjectID;
	Task getObjTask;
	Task getObjIDTask;
	bool emptyEntry=false;
	public bool bDebug;
	
	//Creamos un evento para saber el momento en que se ha creado la entrada en el Parse
	public delegate void NewEntryCreated();
	public static event NewEntryCreated OnNewEntryCreated;
	
	/////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

	void Awake()
	{
		currentParseObject = null;
	}
	
	/////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
	//Cuando el usuario hace login en Facebook se guarda un registro en Parse (si no existe ya)
	//Cuando el usuario consigue terminar un nivel se guarda en Parse
	void OnEnable()
	{
		FacebookRequest.OnUserIsLoggedInFacebook += CreateParseEntry;
		LevelManager.OnLevelIsCompleted += SaveCurrentLevel;
	}
	
	/////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
	
	void OnDisable()
	{
		FacebookRequest.OnUserIsLoggedInFacebook += CreateParseEntry;
		LevelManager.OnLevelIsCompleted -= SaveCurrentLevel;
	}
	
	/////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
	
	void CreateParseEntry()
	{
		if(FB.UserId==null){
			return;
		}
		StartCoroutine(CreateNewEntry());
	}
	
	/////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
	
	IEnumerator CreateNewEntry()
	{
		while(FB.UserId==null) yield return null;
		GetObject(FB.UserId);
		while(!getObjIDTask.IsCompleted) yield return null;
		while(!emptyEntry && !getObjTask.IsCompleted) yield return null;
		
		//No existe una entrada para ese usuario -> la creamos
		if(currentParseObject==null){
			ParseObject facebookUserObj = new ParseObject("FacebookUser");
			facebookUserObj["facebookUserID"] = FB.UserId;
			facebookUserObj["facebookUserName"] = FacebookRequest.facebookUserName;
			facebookUserObj["currentLevel"] = PlayerPrefs.GetInt("Level").ToString();
			facebookUserObj.SaveAsync();
			CustomDebug("PARSE_MANAGER: New entry created - ID:"+FB.UserId+", Name:"+FacebookRequest.facebookUserName);
		}
		//Ya existe una entrada para el usuario -> recuperamos los valores
		else{
			string currentLevel = currentParseObject.Get<string>("currentLevel");
			PlayerPrefs.SetInt("Level", int.Parse(currentLevel));
			CustomDebug("PARSE_MANAGER: Entry already exists - ID:"+FB.UserId+", currentLevel:"+currentLevel);
		}
		
		if(OnNewEntryCreated!=null){
			OnNewEntryCreated();
		}
	}
	
	/////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
	
	void SaveCurrentLevel()
	{
		if(FB.UserId==null){
			return;
		}
		StartCoroutine(SetValue("currentLevel", PlayerPrefs.GetInt("Level").ToString()));
	}
	
	/////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
	
	IEnumerator GetCurrentLevel(string facebookUserID)
	{
		GetObject(facebookUserID);
		while(!getObjIDTask.IsCompleted) yield return null;
		while(!getObjTask.IsCompleted) yield return null;
		int currentLevel = int.Parse(currentParseObject.Get<string>("currentLevel"));
		CustomDebug("PARSE_MANAGER: GetCurrentLevel - facebookUserID:"+FB.UserId+", currentLevel:"+currentLevel);
	}
	
	/////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
	//Guardamos en el registro de Parse del usuario el valor correspondiente a la key
	IEnumerator SetValue(string key, string value)
	{
		GetObject(FB.UserId);
		while(!getObjIDTask.IsCompleted) yield return null;
		while(!getObjTask.IsCompleted) yield return null;
		
		currentParseObject.SaveAsync().ContinueWith(t => {
			currentParseObject[key] = value;
			currentParseObject.SaveAsync();
		});
		CustomDebug("PARSE_MANAGER: SetValue - facebookUserID:"+FB.UserId+", key:"+key+", value:"+value);
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
				Debug.Log("PARSE_MANAGER: No entries - facebookUserID:"+FB.UserId);
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
	
	/////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
	
	void CustomDebug(string s)
	{
		if(bDebug){
			Debug.Log(s);
		}
	}

}










