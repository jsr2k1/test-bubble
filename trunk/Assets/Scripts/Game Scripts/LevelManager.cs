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
	internal int score = 0;
	public static int levelNo = 0;
	public static PatternType patternType = PatternType.None;
	public static int totalNoOfRows = 50;
	public static int minimumNumberOfRows = 6;
	public static float rowAddingInterval = 10f;
	public static int NumberOfBalls = 0;
	private int currentBalls = 0;
	internal int totalNumberOfRowsLeft = 0;
	Text scoreTextLabel;
	public Image winPop;
	public Image loosePop;
	public static int ReferenceScore;
	public Image star1;
	public Image star2;
	public Image star3;
	int stars = 0;

		public Text levelText1;
		public Text levelText2;

	////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

	void Awake()
	{
		totalNumberOfRowsLeft = totalNoOfRows;
		instance = this;
	}

	////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

		void Start ()
		{
				//Camera.main.aspect = .666f;
				gameState = GameState.Start;
				rowAddingInterval = Mathf.Max (1, rowAddingInterval);
				minimumNumberOfRows = Mathf.Max (1, minimumNumberOfRows);
				totalNumberOfRowsLeft = Mathf.Max (minimumNumberOfRows, totalNumberOfRowsLeft);

				if (PlayerPrefs.GetString ("GameType").Equals ("Normal")) {
						totalNumberOfRowsLeft = minimumNumberOfRows;
						currentBalls = NumberOfBalls;
						//setting the balls of the level
						ballsManager.setBallsLeft (NumberOfBalls);
				}
				scoreTextLabel = GameObject.Find ("ScoreTextLabel").GetComponent<Text> ();
				levelText1.text = "Level " + levelNo;
				levelText2.text = "Level " + levelNo;
		}
	
		////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

		internal void GameIsFinished ()
		{
				gameState = GameState.GameFinish;
				SoundFxManager.instance.themeMusic.volume *= .4f;
				SoundFxManager.instance.Play (SoundFxManager.instance.levelClearSound);
				winPop.GetComponent<PopUpMgr> ().ShowPopUp ();

				
				if (PlayerPrefs.GetInt ("SCORE_" + levelNo) < score || !PlayerPrefs.HasKey("SCORE_" + levelNo)) {
					PlayerPrefs.SetInt ("STARS_" + levelNo, stars);
					PlayerPrefs.SetInt ("SCORE_" + levelNo, score);
				}

				if (PlayerPrefs.GetInt ("Level") < levelNo) {
					PlayerPrefs.SetInt ("Level", levelNo);
				}
				
=======
		if(PlayerPrefs.GetString("GameType").Equals("Normal")){
			totalNumberOfRowsLeft = minimumNumberOfRows;
			currentBalls = NumberOfBalls;
			//setting the balls of the level
			ballsManager.setBallsLeft(NumberOfBalls);
>>>>>>> .r169
		}
		scoreTextLabel = GameObject.Find("ScoreTextLabel").GetComponent<Text>();
	}

	////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

	internal void GameIsFinished()
	{
		gameState = GameState.GameFinish;
		SoundFxManager.instance.themeMusic.volume *= .4f;
		SoundFxManager.instance.Play(SoundFxManager.instance.levelClearSound);
		winPop.GetComponent<PopUpMgr>().ShowPopUp();
		PlayerPrefs.SetInt("STARS_" + levelNo, stars);
		PlayerPrefs.SetInt("SCORE_" + levelNo, score);
		//Invoke("LoadLevelAgain", 2f); 
	}

	////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

	internal void GameIsOver()
	{	
		gameState = GameState.GameOver;
		InGameScriptRefrences.playingObjectManager.FallAllPlayingObjects();
		SoundFxManager.instance.themeMusic.volume *= .4f;
		loosePop.GetComponent<PopUpMgr>().ShowPopUp();
		Invoke("PlayLevelFailSound", .2f);
		//Invoke("LoadLevelAgain", 3f);
	}

	////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

	void PlayLevelFailSound()
	{
		SoundFxManager.instance.Play(SoundFxManager.instance.levelFailSound);
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

		if(score > 0.5 * ReferenceScore){
			star1.GetComponent<Image>().enabled = true;
			stars = 1;
		}
		if(score > 0.75 * ReferenceScore){
			star2.GetComponent<Image>().enabled = true;
			stars = 2;
		}
		if(score > ReferenceScore){
			star3.GetComponent<Image>().enabled = true;
			stars = 3;
		}

		//slider.value =(float) score /(float) ReferenceScore;
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
		currentBalls--;
		ballsManager.setBallsLeft(currentBalls);

		if(currentBalls == 0){
			StartCoroutine("Finishing");
		}
	}

	////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

	IEnumerator Finishing()
	{
		yield return new WaitForSeconds(1);
		GameIsOver();
	}
}



