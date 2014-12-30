using UnityEngine;
using UnityEngine.UI;
using System.Collections;

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
	Text scoreTextLabel;
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
	
	//Creamos un evento para saber el momento exacto en el que se ha superado un nivel para guardarlo en el Parse
	public delegate void LevelIsCompleted();
	public static event LevelIsCompleted OnLevelIsCompleted;

	////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

	void Awake()
	{
		totalNumberOfRowsLeft = totalNoOfRows;
		instance = this;
		score = 0;
	}

	////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

	void Start()
	{
		//Camera.main.aspect = .666f;
		gameState = GameState.Start;
		rowAddingInterval = Mathf.Max(1, rowAddingInterval);
		minimumNumberOfRows = Mathf.Max(1, minimumNumberOfRows);
		totalNumberOfRowsLeft = Mathf.Max(minimumNumberOfRows, totalNumberOfRowsLeft);

		if(PlayerPrefs.GetString("GameType").Equals("Normal")){
			totalNumberOfRowsLeft = minimumNumberOfRows;
			currentBalls = NumberOfBalls;
			//setting the balls of the level
			ballsManager.setBallsLeft(NumberOfBalls);
		}
		scoreTextLabel = GameObject.Find("ScoreTextLabel").GetComponent<Text>();
		if(PlayerPrefs.GetString("GameType") == "Normal"){
			levelText1.text = LanguageManager.GetText("id_level") + " " + levelNo.ToString();
			levelText2.text = LanguageManager.GetText("id_level") + " " + levelNo.ToString();
		}
	}
	
	////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
	//Hemos SUPERADO el nivel
	internal void GameIsFinished()
	{
		gameState = GameState.GameFinish;
		AudioManager.instance.StopAudio();
		winPop.ShowPopUp();
		
		if(PlayerPrefs.GetInt("SCORE_" + levelNo) < score || !PlayerPrefs.HasKey("SCORE_" + levelNo)){
			PlayerPrefs.SetInt("STARS_" + levelNo, stars);
			PlayerPrefs.SetInt("SCORE_" + levelNo, score);
		}
		if(PlayerPrefs.GetInt("Level") < levelNo){
			PlayerPrefs.SetInt("Level", levelNo);
		}
		PlayerPrefs.SetInt("bPlaying", 0);
		
		if(OnLevelIsCompleted!=null){
			OnLevelIsCompleted(); //Lanzamos este evento para que guarde el progreso en el parse
		}
	}

	////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
	//Hemos PERDIDO el nivel
	public void GameIsOver()
	{	
		if(gameState != GameState.Start){
			return;
		}
		gameState = GameState.GameOver;
		InGameScriptRefrences.playingObjectManager.FallAllPlayingObjects();
		AudioManager.instance.StopAudio();
		losePop.ShowPopUp();

		if(PlayerPrefs.GetString("GameType") == "Arcade"){
			if(score > PlayerPrefs.GetInt("Highscore")){
				PlayerPrefs.SetInt("Highscore", score);
				highscoretext.text = score.ToString();
				if(OnLevelIsCompleted!=null){
					OnLevelIsCompleted(); //Lanzamos este evento para que guarde el highscore en el Parse
				}
			}
		}else{
			LivesManager.lives--;
		}

		//PlayerPrefs.SetInt("Lives", PlayerPrefs.GetInt("Lives") - 1 );
		//Invoke("LoadLevelAgain", 3f);
	}
	
	////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

	void LoadLevelAgain()
	{
		if(PlayerPrefs.GetString("GameType") == "Arcade"){
			Application.LoadLevel(1);
		} else{
			Application.LoadLevel(2);
		}
	}

	////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

	internal void AddToScore(int points)
	{
		score += points;
		scoreTextLabel.text = score.ToString("000000");

		if(PlayerPrefs.GetString("GameType") == "Normal")
		{
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
			if(PlayerPrefs.GetString("GameType") == "Normal"){
				ballsManager.setBallsLeft(currentBalls);
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



