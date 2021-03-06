using UnityEngine;
using System.Collections;

public class ScoreManagerGame : MonoBehaviour 
{
    public static ScoreManagerGame instance;
    //int numberOfItemPoppedInARow = 0;
	static int bonusCounter=0;

	////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

	void Start () 
    {
        instance = this;
		bonusCounter=0;
	}

	////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
	 
	public string GetCurrentScore(int score)
	{
		//int points = score + (score * numberOfItemPoppedInARow);
		int points = score + bonusCounter;
		//numberOfItemPoppedInARow++;
		//LevelManager.instance.AddToScore(points);

		return points.ToString();
	}

	////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
	/*
	public void ResetScore()
	{
		numberOfItemPoppedInARow=0;
	}
	*/
	////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
	
	public void SetBonus(bool bAdd)
	{
		if(bAdd){
			bonusCounter+=10;
		}else{
			bonusCounter=0;
		}
	}

	////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
	//Nos saltamos un frame para que no haya lag al cambiar el valor del texto de la puntuacion
	public void AddScore()
	{
		StartCoroutine(AddToScore());
	}

	////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

	IEnumerator AddToScore()
	{
		yield return new WaitForEndOfFrame();
		LevelManager.instance.AddToScore(10 + bonusCounter);
	}

	////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
	/*
    public void DisplayScorePopup(int score, Transform go)
    { 
		if(numberOfItemPoppedInARow > maxItems){
			return;
		}
		int points = score + (score * numberOfItemPoppedInARow);
		//scoreArray[numberOfItemPoppedInARow].transform.localScale = new Vector3(1, 1, 1);
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
    }*/
}



