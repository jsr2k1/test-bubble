using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class WorldButton : MonoBehaviour
{
	public int numberBalls;
	public int ActualWorld;
	public int refScore;
	public Text numTextPrefab;
	public Image star1;
	public Image star2;
	public Image star3;
	public Sprite bubbleFilled;
	Button button;
	public Image livesPop;
	public Image startPop;
	public Text levelTextPop;
	
	AudioSource audioSource;
	
	//Creamos un evento para poder saber cuando se ha pulsado un boton del mapa
	public delegate void WorldButtonPressed();
	public static event WorldButtonPressed OnWorldButtonPressed;

	//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
	
	void Awake()
	{
		button = GetComponent<Button>();

		if(gameObject.name == "1" || PlayerPrefs.HasKey("STARS_" +(int.Parse(gameObject.name) - 1))){
			button.interactable = true;
			GetComponent<Image>().sprite = bubbleFilled;
		} else{
			button.interactable = false;
		}

		Text currentText = Instantiate(numTextPrefab) as Text;
		currentText.text = gameObject.name;
		currentText.transform.position = transform.position;
		currentText.transform.SetParent(transform);
		currentText.name = "label_" + gameObject.name;
		currentText.transform.localScale = new Vector3(0.2f, 0.2f, 1.0f);

		Image stars;

		if(PlayerPrefs.GetInt("STARS_" + gameObject.name) == 3){
			stars = Instantiate(star3) as Image;

		} else if(PlayerPrefs.GetInt("STARS_" + gameObject.name) == 2){
			stars = Instantiate(star2) as Image;

		} else if(PlayerPrefs.GetInt("STARS_" + gameObject.name) == 1){
			stars = Instantiate(star1) as Image;

		} else{
			stars = null;
		}

		if(stars != null){
			stars.transform.SetParent(transform);
			stars.transform.localPosition = new Vector3(0, 40, 0);
			stars.transform.localScale = new Vector3(0.8f, 0.8f, 1.0f);
		}
		
		audioSource = gameObject.AddComponent<AudioSource>();
		audioSource.loop = false;
		audioSource.clip = AudioManager.instance.UIClick;
	}
	
	//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

	public void ButtonPressed()
	{
		if(audioSource && PlayerPrefs.GetInt("Sounds")==1){
			audioSource.Play();
		}
		
		int nLives=PlayerPrefs.GetInt("Lives");
		if(nLives > 0) {
			PlayerPrefs.SetString("GameType", "Normal");
			LevelManager.patternType = PatternType.TextLevel;
			LevelParser.instance.LoadTextLevel(int.Parse(name), ActualWorld);

			LevelManager.NumberOfBalls = numberBalls;
			LevelManager.ReferenceScore = refScore;
			LevelManager.rowAddingInterval = 1;
			LevelManager.levelNo = int.Parse(name);
			LevelManager.worldNo = ActualWorld;

			PlayerPrefs.SetInt("bPlaying", 1);
			//Application.LoadLevel(3);

			levelTextPop.text = LanguageManager.GetText("id_level")+" "+LevelManager.levelNo.ToString();
			//if(PlayingObjectManager.GetLevelMission() == PlayingObjectManager.MissionType.Animals){
			//	levelTextPop.text = LevelManager.levelNo.ToString();
			//}
			startPop.GetComponent<PopUpMgr>().ShowPopUp();
		}
		else{
			livesPop.GetComponent<PopUpMgr>().ShowPopUp();
		}
		
		if(OnWorldButtonPressed!=null){
			OnWorldButtonPressed();
		}
	}
}



