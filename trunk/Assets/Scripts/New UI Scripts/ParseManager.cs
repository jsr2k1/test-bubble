//https://www.parse.com/docs/unity_guide#objects
//https://parse.com/apps/bubble-paradise-2/collections
using UnityEngine;
using System.Collections;
using Parse;
using System.Collections.Generic;
using System.Threading.Tasks;

public class ParseManager : MonoBehaviour
{
	public static ParseManager instance;
	ParseObject currentParseObject;
	string currentObjectID;
	string currentUserID;
	Task getObjTask;
	Task getObjIDTask;
	bool emptyEntry=false;
	public bool bDebug;
	public bool isBusy=false;
	
	public string currentFriendID;
	
	//Creamos un evento para saber el momento en que se han obtenido los datos del amigo de facebook
	public delegate void GetFacebookFriendDone();
	public static event GetFacebookFriendDone OnGetFacebookFriendInfoDone;
	
	/////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

	void Awake()
	{
		instance = this;
		currentParseObject = null;
	}
	
	/////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
	
	public void CheckParseEntry()
	{
		if(!FB.IsLoggedIn){
			return;
		}
		StartCoroutine(CheckEntry());
	}
	
	/////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
	//Si el usuario no esta dado de alta en Parse -> Creamos una nueva entrada
	//Si ya esta dado de alta -> Recuperamos los datos
	IEnumerator CheckEntry()
	{
		while(FB.UserId==null) yield return null;
		GetObject(FB.UserId);
		while(!getObjIDTask.IsCompleted) yield return null;
		while(!emptyEntry && !getObjTask.IsCompleted) yield return null;
	
		//No existe una entrada para ese usuario -> la creamos
		if(emptyEntry && currentParseObject==null){
			ParseObject facebookUserObj = new ParseObject("FacebookUser");
			facebookUserObj["facebookUserID"] = FB.UserId;
			facebookUserObj["facebookUserName"] = FacebookManager.facebookUserName;
			FillObj(facebookUserObj);
			facebookUserObj.SaveAsync();
			FacebookBubble.instance.EnableButtons();
			CustomDebug("PARSE_MANAGER: New entry created - ID:"+FB.UserId+", Name:"+FacebookManager.facebookUserName);
		}
		//Ya existe una entrada para el usuario -> recuperamos los valores
		else if(!emptyEntry && currentParseObject!=null){
			GetUserData();
			CustomDebug("PARSE_MANAGER: Entry already exists - ID:"+FB.UserId);
			currentParseObject=null;
		}
		emptyEntry=false;
	}
	
	/////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
	
	public void GetFacebookFriendInfo(string friendID, string facebookName)
	{
		if(!FB.IsLoggedIn){
			return;
		}
		isBusy=true;
		StartCoroutine(GetFacebookFriendInfoParse(friendID, facebookName));
	}
	
	/////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
	
	IEnumerator GetFacebookFriendInfoParse(string friendID, string facebookName)
	{
		GetObject(friendID);
		while(!getObjIDTask.IsCompleted) yield return null;
		while(!emptyEntry && !getObjTask.IsCompleted) yield return null;
		
		currentFriendID = friendID;
		if(currentParseObject!=null){
			FacebookManager.instance.friendsDict[friendID].id = currentParseObject.Get<string>("facebookUserID");
			FacebookManager.instance.friendsDict[friendID].name = currentParseObject.Get<string>("facebookUserName");
			FacebookManager.instance.friendsDict[friendID].highScore = currentParseObject.Get<string>("HighScore");
			
			if(Application.loadedLevelName == "04 World Menu"){
				FacebookManager.instance.friendsDict[friendID].currentLevel = GetCurrentLevel(currentParseObject.Get<string>("currentLevel"));
			}
			string name = currentParseObject.Get<string>("facebookUserName");
			CustomDebug("GetFacebookFriendParse - ID:"+friendID+", name:"+name);
			currentParseObject=null;
		}else{
			CustomDebug("GetFacebookFriendParse - No se ha encontrado el amigo: "+friendID+", facebookName:"+facebookName);
		}
		emptyEntry=false;
		isBusy=false;
		
		if(OnGetFacebookFriendInfoDone!=null){
			OnGetFacebookFriendInfoDone();
		}
	}
	
	/////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
	
	string GetCurrentLevel(string sLevel)
	{
		int level = int.Parse(sLevel) + 1;
		level = Mathf.Min(level, LevelParser.instance.maxLevels);
		return level.ToString();
	}
	
	/////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
	
	public void SaveCurrentData()
	{
		if(!FB.IsLoggedIn){
			return;
		}
		StartCoroutine(SaveCurrentDataUser());
	}
	
	/////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
	//Guardamos en el registro de Parse del usuario los datos de los niveles
	IEnumerator SaveCurrentDataUser()
	{
		GetObject(FB.UserId);
		while(!getObjIDTask.IsCompleted) yield return null;
		while(!emptyEntry && !getObjTask.IsCompleted) yield return null;
		
		if(currentParseObject!=null){
			currentParseObject.SaveAsync().ContinueWith(t => {
				FillObj(currentParseObject);
				currentParseObject.SaveAsync();
			});
		}
		emptyEntry=false;
		FacebookBubble.instance.EnableButtons();
		CustomDebug("PARSE_MANAGER: SaveCurrentDataUser - facebookUserID:"+FB.UserId);
	}
	
	/////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
	//Rellenamos los campos del ParseObject con los datos del PlayerPrefs
	void FillObj(ParseObject obj)
	{
		obj["currentLevel"] = PlayerPrefs.GetInt("Level").ToString();
		obj["Coins"] = PlayerPrefs.GetInt("Coins").ToString();
		obj["MulticolorBall"] = PlayerPrefs.GetInt("Multicolor Ball").ToString();
		obj["FireBall"] = PlayerPrefs.GetInt("Fire Ball").ToString();
		obj["BombBall"] = PlayerPrefs.GetInt("Bomb Ball").ToString();
		obj["HighScore"] = PlayerPrefs.GetInt("Highscore").ToString();
		obj["starsDic"] = FillStarsDictionary();
		obj["scoreDic"] = FillScoreDictionary();
		obj["deviceID"] = SystemInfo.deviceUniqueIdentifier;
		obj["deviceModel"] = SystemInfo.deviceModel;
		obj["deviceType"] = SystemInfo.deviceType.ToString();
		obj["operatingSystem"] = SystemInfo.operatingSystem;
	}
	
	/////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
	//Guardamos las estrellas que tiene el usuario en los niveles en un diccionario para mandarlo al Parse
	IDictionary<string, string> FillStarsDictionary()
	{
		Dictionary<string,string> dicStars = new Dictionary<string,string>();
		int currentLevel = PlayerPrefs.GetInt("Level");
		for(int i=1;i<=currentLevel;i++){
			int stars = PlayerPrefs.GetInt("STARS_" + i);
			dicStars.Add("STARS_"+i, stars.ToString());
		}
		return dicStars;
	}
	
	/////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
	//Guardamos el score que tiene el usuario en los niveles en un diccionario para mandarlo al Parse
	IDictionary<string, string> FillScoreDictionary()
	{
		Dictionary<string,string> dicStars = new Dictionary<string,string>();
		int currentLevel = PlayerPrefs.GetInt("Level");
		for(int i=1;i<=currentLevel;i++){
			int stars = PlayerPrefs.GetInt("SCORE_" + i);
			dicStars.Add("SCORE_"+i, stars.ToString());
		}
		return dicStars;
	}
	
	/////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
	//Obtenemos los datos del usuario y actualizamos el PlayerPrefs
	//Cuidado con que el usuario no pierda los avances que ha hecho mientras estaba offline
	void GetUserData()
	{
		//Siempre prevalece el nivel mas alto al que ha llegado el usuario tanto si estaba online como si estaba offline.
		//Idem para stars, score y highscore
		PlayerPrefs.SetInt("Level", Mathf.Max(PlayerPrefs.GetInt("Level"), int.Parse(currentParseObject.Get<string>("currentLevel"))));
		PlayerPrefs.SetInt("Highscore", Mathf.Max(PlayerPrefs.GetInt("Highscore"), int.Parse(currentParseObject.Get<string>("HighScore"))));

		//Miramos cual es el nivel minimo guardado en PlayerPrefs o en Parse para no salirnos de rango en el otro diccionario
		int currentLevel = Mathf.Min(PlayerPrefs.GetInt("Level"), int.Parse(currentParseObject.Get<string>("currentLevel")));
		if(currentLevel>0){
			//STARS
			IDictionary<string, string> dicStars = currentParseObject.Get<IDictionary<string, string>>("starsDic");
			for(int i=1;i<=currentLevel;i++){
				PlayerPrefs.SetInt("STARS_"+i, Mathf.Max(PlayerPrefs.GetInt("STARS_"+i), int.Parse(dicStars["STARS_"+i])));
			}
			//SCORE
			IDictionary<string, string> dicScore = currentParseObject.Get<IDictionary<string, string>>("scoreDic");
			for(int i=1;i<=currentLevel;i++){
				PlayerPrefs.SetInt("SCORE_"+i, Mathf.Max(PlayerPrefs.GetInt("SCORE_"+i), int.Parse(dicScore["SCORE_"+i])));
			}
		}
		//Si es la primera vez que el usuario se conecta a facebook en ese dispositivo -> Cogemos los valores maximos entre Parse y PlayerPrefs
		//Si ya se ha conectado antes -> 
		//------> Si se esta conectando con el mismo dispositivo -> Se mantienen los valores que habia en el PlayerPrefs
		//------> Si es otro dispositivo -> Cogemos los valores que nos dice el Parse
		string deviceID = currentParseObject.Get<string>("deviceID");
		if(PlayerPrefs.GetInt("FirstTimeFacebookLogin")==1 || (PlayerPrefs.GetInt("FirstTimeFacebookLogin")==0 && deviceID!=SystemInfo.deviceUniqueIdentifier)){
			//PlayerPrefs.SetInt("Coins", Mathf.Max(PlayerPrefs.GetInt("Coins"), int.Parse(currentParseObject.Get<string>("Coins"))));
			CoinsManager.instance.SetCoins(int.Parse(currentParseObject.Get<string>("Coins")));
			PlayerPrefs.SetInt("Multicolor Ball", int.Parse(currentParseObject.Get<string>("MulticolorBall")));
			PlayerPrefs.SetInt("Fire Ball", int.Parse(currentParseObject.Get<string>("FireBall")));
			PlayerPrefs.SetInt("Bomb Ball", int.Parse(currentParseObject.Get<string>("BombBall")));
		}
		if(PlayerPrefs.GetInt("FirstTimeFacebookLogin")==1){
			PlayerPrefs.SetInt("FirstTimeFacebookLogin",0);
		}
		//Dado que en algunos casos cogemos el valor maximo, hay que actualizar los datos en Parse tambien
		SaveCurrentData();
	}
	
	/////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
	//Para poder obtener el Object, primero tenemos que obtener su ObjectID
	void GetObject(string facebookUserID)
	{
		currentUserID=facebookUserID;
		ParseQuery<ParseObject> query = ParseObject.GetQuery("FacebookUser").WhereEqualTo("facebookUserID", facebookUserID);
		getObjIDTask = query.FindAsync().ContinueWith(CallbackGetObject);
	}
	
	/////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
	//Una vez ya tenemos el ObjectID ya podemos hacer una query para obtener el Object
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
				CustomDebug("CallbackGetObject: No entries - facebookUserID:"+currentUserID);
			}
			emptyEntry=true;
			currentParseObject=null;
			return;
		}
		else if(count==1){
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










