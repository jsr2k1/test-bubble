using UnityEngine;
using UnityEngine.UI;
using System.Collections;
//using GameAnalyticsSDK;

public enum GameState
{
	Start,
	Playing,
	Pause,
	GameOver,
	GameFinish
}

////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

public class LevelManager : MonoBehaviour
{
	//Balls manager Script
	public NumberBallsManager ballsManager;
	GameObject striker;
	public static LevelManager instance;
	public static GameState gameState = GameState.Start;
	public static StrikerManager strikerManager;
	public static int score = 0;
	public static int levelNo = 0;
	public static int worldNo = 0;
	public static PatternType patternType = PatternType.None;
	public static int totalNoOfRows = 50;
	public static int minimumNumberOfRows = 6;
	public static float rowAddingInterval = 10f;
	public static int NumberOfBalls = 0;
	public static int currentBalls = 0;
	internal int totalNumberOfRowsLeft = 0;
	public static Text scoreTextLabel;
	public PopUpMgr winPop;
	public PopUpMgr losePop;
	public PopUpMgr moreBubblesPop;
	public static int ReferenceScore;
	public Image star11;
	public Image star22;
	public Image star33;
	int stars = 0;
	public Text levelText1;
	public Text levelText2;
	public Slider slider;
	public Text highscoretext;
	public GameObject BallCounter;
	Animator anim;
	
	public bool bStartFinished=false;

	public AudioSource fewballs;
	
	public enum GameTypes{
		ARCADE = 0,
		NORMAL = 1	
	}
	public static GameTypes GameType;

	////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

	void Awake()
	{
		totalNumberOfRowsLeft = totalNoOfRows;
		instance = this;
		score = 0;
		if (BallCounter != null) {
			anim = BallCounter.GetComponent<Animator> ();
		}
	}

	////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

	void Start()
	{
		//Camera.main.aspect = .666f;
		gameState = GameState.Start;

		rowAddingInterval = Mathf.Max(1, rowAddingInterval);
		minimumNumberOfRows = Mathf.Max(1, minimumNumberOfRows);
		//totalNumberOfRowsLeft = Mathf.Max(minimumNumberOfRows, totalNumberOfRowsLeft);

		if(LevelManager.GameType == LevelManager.GameTypes.NORMAL){
			//GameAnalytics.NewProgressionEvent (GA_Progression.GAProgressionStatus.GAProgressionStatusStart, levelNo.ToString());
			totalNumberOfRowsLeft = minimumNumberOfRows;
			currentBalls = NumberOfBalls;
			//setting the balls of the level
			ballsManager.setBallsLeft(NumberOfBalls);
		}//ARCADE
		else{
			//GameAnalytics.NewProgressionEvent (GA_Progression.GAProgressionStatus.GAProgressionStatusStart, "Arcade");
			totalNumberOfRowsLeft = 6;
		}
		scoreTextLabel = GameObject.Find("ScoreTextLabel").GetComponent<Text>();
		if(LevelManager.GameType == LevelManager.GameTypes.NORMAL){
			levelText1.text = LanguageManager.GetText("id_level") + " " + levelNo.ToString();
			levelText2.text = LanguageManager.GetText("id_level") + " " + levelNo.ToString();
		}
		bStartFinished=true;
	}
	
	////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
	//Hemos SUPERADO el nivel
	public void GameIsFinished()
	{
		//GameAnalytics.NewProgressionEvent (GA_Progression.GAProgressionStatus.GAProgressionStatusComplete, levelNo.ToString ());
		gameState = GameState.GameFinish;
		AudioManager.instance.StopAudio();
		winPop.ShowPopUp();
	}
	
	////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
	//Guardamos los datos en el PlayerPrefs
	public void SaveGameData()
	{
		if(PlayerPrefs.GetInt("SCORE_" + levelNo) < score || !PlayerPrefs.HasKey("SCORE_" + levelNo)){
			PlayerPrefs.SetInt("STARS_" + levelNo, stars);
			PlayerPrefs.SetInt("SCORE_" + levelNo, score);
		}
		int level = PlayerPrefs.GetInt("Level"); 
		if(level < levelNo){
			PlayerPrefs.SetInt("Level", levelNo);
		}
		PlayerPrefs.SetInt("bPlaying", 0);
		
		//ParseManager.instance.SaveCurrentData();
	}

	////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
	//Hemos PERDIDO el nivel
	public void GameIsOver()
	{	
		if(gameState != GameState.Start){
			return;
		}

		if (LevelManager.GameType == LevelManager.GameTypes.NORMAL) {
			//GameAnalytics.NewProgressionEvent (GA_Progression.GAProgressionStatus.GAProgressionStatusFail, levelNo.ToString ());
		} else {
			//GameAnalytics.NewProgressionEvent (GA_Progression.GAProgressionStatus.GAProgressionStatusComplete, "Arcade", score);
		}

		gameState = GameState.GameOver;
		AudioManager.instance.StopAudio();
		losePop.ShowPopUp();

		if(LevelManager.GameType == LevelManager.GameTypes.ARCADE){
			if(score > PlayerPrefs.GetInt("Highscore")){
				PlayerPrefs.SetInt("Highscore", score);
				highscoretext.text = score.ToString();
			}
		}else{
			//LivesManager.lives--;
		}
		
		//ParseManager.instance.SaveCurrentData();

		//PlayerPrefs.SetInt("Lives", PlayerPrefs.GetInt("Lives") - 1 );
		//Invoke("LoadLevelAgain", 3f);
	}
	
	////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

	void LoadLevelAgain()
	{
		if(LevelManager.GameType == LevelManager.GameTypes.ARCADE){
			Application.LoadLevel(2);
		} else{
			//GameAnalytics.NewProgressionEvent (GA_Progression.GAProgressionStatus.GAProgressionStatusStart, levelNo.ToString());
			Application.LoadLevel(3);
		}
	}

	////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

	internal void AddToScore(int points)
	{
		score += points;
		scoreTextLabel.text = score.ToString("000000");

		if(LevelManager.GameType == LevelManager.GameTypes.NORMAL){
			if(score > 0.3 * ReferenceScore){
				star11.enabled = true;
				stars = 1;
			}
			if(score > 0.6 * ReferenceScore){
				star22.enabled = true;
				stars = 2;
			}
			if(score > ReferenceScore){
				star33.enabled = true;
				stars = 3;
			}
			float currentScore =((float)score /(float)ReferenceScore);
			slider.value = Mathf.Clamp(currentScore, 0.2f, 1.0f);
		}
	}

	////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

	internal bool running()
	{
		if(gameState == GameState.GameFinish || gameState == GameState.GameOver){
			return false;
		} else{
			return true;
		}
	}

	////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

	internal void pauseCtrl()
	{
		Debug.Log(gameState);

		if(gameState == GameState.Pause){
			gameState = GameState.Start;
		} else{
			gameState = GameState.Pause;
		}
	}

	////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

	internal void pauseCtrlForced(GameState state)
	{
		gameState = state;
	}

	////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

	public void BallLaunched()
	{	
		if(currentBalls>0){
			currentBalls--;
			if(LevelManager.GameType == LevelManager.GameTypes.NORMAL){
				ballsManager.setBallsLeft(currentBalls);
				if(currentBalls==5){
					if(AudioManager.instance.bSoundsOn){
						fewballs.Play();
					}
					anim.SetTrigger("FewBalls");
				}
			}
		}
		
		//No lo podemos hacer aqui pq primero tenemos que comprobar si ha completado el nivel justo con el ultimo disparo
		//Esto lo haremos en PlayingObjectManager.CheckGameIsOver()
		//if(currentBalls == 0){
		//	StartCoroutine("Finishing");
		//}
	}

	////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
	
	public IEnumerator Finishing()
	{
		yield return new WaitForSeconds(0.6f);
		moreBubblesPop.ShowPopUp();
	}
}



