using UnityEngine;
using System.Collections;

public class ScoreManagerGame : MonoBehaviour 
{
    public static ScoreManagerGame instance;
    public GameObject scoreItemPrefab;
    int bonusPoint;
    int numberOfItemPoppedInARow = 0;

	void Start () 
    {
        instance = this;
	
	}

    internal void DisplayScorePopup(int score,Transform go)
    { 

        GameObject scoreItem = (GameObject)Instantiate(scoreItemPrefab, go.position + new Vector3(0, 0, 1), Quaternion.identity);
        scoreItem.transform.eulerAngles = new Vector3(0, 180, 0);

        bonusPoint = score * numberOfItemPoppedInARow;
        int points = score + bonusPoint;

        scoreItem.GetComponent<ScorePopupItem>().BringItForward(points);
        LevelManager.instance.AddToScore(points);
        numberOfItemPoppedInARow++;

        CancelInvoke("ResetNumberOfItemPopped");
        Invoke("ResetNumberOfItemPopped",.5f);
    }

    private void ResetNumberOfItemPopped()
    {
        numberOfItemPoppedInARow = 0;
        ScorePopupItem.ResetDelay();
    }
}
