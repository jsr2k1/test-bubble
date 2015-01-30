using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

public class HighScoreManager : MonoBehaviour
{
	public static HighScoreManager instance;
	public GameObject content;
	public GameObject entryPrefabDark;
	public GameObject entryPrefabLight;
	public Button buttonRanking;
	public Image imageRanking;
	public Text textRanking;
	public PopUpMgr popUpRanking;
	int numScoresDone=0;
	List<Friend> friendsList;
	
	/////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
	
	void Awake()
	{
		instance = this;
		friendsList = new List<Friend>();
		buttonRanking.interactable = false;
		imageRanking.color = new Color(1,1,1,0.5f);
		textRanking.color = new Color(1,1,1,0.5f);
	}
	
	/////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
	
	void OnEnable()
	{
		ParseManager.OnGetFacebookFriendInfoDone += AddScoreCounter;
	}
	
	/////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
	
	void OnDisable()
	{
		ParseManager.OnGetFacebookFriendInfoDone -= AddScoreCounter;
	}
	
	/////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
	//Cada vez que el Parse nos devuelve la informacion de algun amigo de Facebook incrementamos un contador hasta tenerlos todos
	void AddScoreCounter()
	{
		numScoresDone++;
	}
	
	/////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
	//Pedimos al parse que nos de la informacion de los amigos de Facebook
	public void StartCreateHighScoreTable()
	{
		int numFriends = FacebookManager.instance.friendsDict.Count;
		
		foreach(KeyValuePair<string, Friend> entry in FacebookManager.instance.friendsDict){
			StartCoroutine(GetHighScore(entry.Value.id, entry.Value.name));
		}
		StartCoroutine(CreateHighScoreTable(numFriends));
	}
	
	/////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
	//Esperamos a que el Parse deje de estar ocupado y le pedimos la informacion del amigo de Facebook
	IEnumerator GetHighScore(string id, string name)
	{
		while(ParseManager.instance.isBusy){
			yield return null;
		}
		ParseManager.instance.GetFacebookFriendInfo(id, name);
	}
	
	/////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
	//Una vez hemos obtenido los highscores de todos los amigos de Facebook, los ordenamos y los mostramos
	IEnumerator CreateHighScoreTable(int numFriends)
	{
		while(numScoresDone < numFriends){
			yield return null;
		}
		SortHighscores();
	}
	
	/////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
	//Ordenamos los valores del diccionario antes de mostrarlos
	void SortHighscores()
	{	
		//Recorremos el diccionario y vamos rellenando la lista con los highscores ordenados
		foreach(KeyValuePair<string, Friend> entry in FacebookManager.instance.friendsDict){
			if(friendsList.Count==0){
				friendsList.Add(entry.Value);
			}else{
				for(int i=1;i<=friendsList.Count;i++){
					if(int.Parse(entry.Value.highScore) > int.Parse(friendsList[i-1].highScore)){
						friendsList.Insert(i-1, entry.Value);
						break;
					}
					if(i==friendsList.Count){
						friendsList.Add(entry.Value);
						break;
					}
				}
			}
		}
		//Recorremos la lista y vamos creando las entradas en el PopUp
		for(int i=0;i<friendsList.Count;i++){
			GameObject goEntry = Instantiate(i%2==0 ? entryPrefabDark : entryPrefabLight) as GameObject;
			goEntry.transform.SetParent(content.transform);
			goEntry.transform.localScale = new Vector3(1.0f, 1.0f, 1.0f);
			goEntry.transform.GetChild(0).GetComponent<Text>().text = (i+1)+".";
			goEntry.transform.GetChild(1).GetComponent<Text>().text = GetName(friendsList[i].name);
			goEntry.transform.GetChild(2).GetComponent<Text>().text = friendsList[i].highScore;
			goEntry.transform.GetChild(3).GetComponent<FriendPicture>().id = friendsList[i].id;
		}
		buttonRanking.interactable = true;
		imageRanking.color = new Color(1,1,1,1);
		textRanking.color = new Color(1,1,1,1);
	}
	
	/////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
	//Mostramos solamente el nombre y primer apellido. El segundo apellido no lo mostramos para que no se corte.
	string GetName(string name)
	{
		string[] words = name.Split(' ');
		if(words.Length>1){
			return words[0]+" "+words[1];
		}else{
			return name;
		}
	}
	
	/////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
	
	void OnButtonRankingPressed()
	{
		popUpRanking.ShowPopUp();
	}
}





