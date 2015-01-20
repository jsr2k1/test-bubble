using UnityEngine;
using System.Collections;

public class ScoreManagerGame : MonoBehaviour 
{
    public static ScoreManagerGame instance;
    public GameObject scoreItemPrefab;
    int numberOfItemPoppedInARow = 0;
	GameObject DummyScore;
	
	GameObject[] scoreArray;
	ScorePopupItem[] ScorePopupItemArray;
	int maxItems=20;

	////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

	void Start () 
    {
        instance = this;
		DummyScore = GameObject.Find("DummyScore");
		CreateArray();
	}
	
	////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
	
	void CreateArray()
	{
		scoreArray = new GameObject[maxItems];
		ScorePopupItemArray = new ScorePopupItem[maxItems];
		
		for(int i=0;i<maxItems;i++){
			GameObject scoreItem = (GameObject)Instantiate(scoreItemPrefab, new Vector3(10000, 10000, 1), Quaternion.identity);
			scoreItem.transform.SetParent(DummyScore.transform);
			scoreItem.transform.localScale=new Vector3(1,1,1);
			scoreArray[i] = scoreItem;
			ScorePopupItemArray[i] = scoreItem.GetComponent<ScorePopupItem>();
		}
	}

	////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

    internal void DisplayScorePopup(int score, Transform go)
    { 
		if(numberOfItemPoppedInARow>maxItems){
			return;
		}
		int points = score + (score * numberOfItemPoppedInARow);
		scoreArray[numberOfItemPoppedInARow].transform.localScale = new Vector3(1, 1, 1);
		scoreArray[numberOfItemPoppedInARow].transform.position = go.position + new Vector3(0, 0, 1);
		ScorePopupItemArray[numberOfItemPoppedInARow].BringItForward(points);
        	
        LevelManager.instance.AddToScore(points);
        numberOfItemPoppedInARow++;

        CancelInvoke("ResetNumberOfItemPopped");
        Invoke("ResetNumberOfItemPopped",.5f);
    }

	////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

    private void ResetNumberOfItemPopped()
    {
        numberOfItemPoppedInARow = 0;
        ScorePopupItem.ResetDelay();
    }
}



