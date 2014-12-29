using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using System.Linq;

public class TutorialManager : MonoBehaviour
{
	public static TutorialManager instance;
	List<int> tutorialsList;
	public Sprite[] tutorialSprites;
	public Image tutorialImage;
	public Text tutorialText;
	PopUpMgr tutorialPopUp;
	
	//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
	
	void Awake()
	{
		instance = this;
		
		int[] items = new int[]{1,2,3,4,5,6,7};
		tutorialsList = items.ToList();
		tutorialPopUp = tutorialImage.GetComponent<PopUpMgr>();
	}
	
	//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
	
	public void ShowTutorial()
	{
		if(tutorialsList.Contains(LevelManager.levelNo)){
			int index = tutorialsList.IndexOf(LevelManager.levelNo);
			tutorialImage.sprite = tutorialSprites[index];
			tutorialText.text = LanguageManager.GetText("id_tutorial_"+(index+1));
			tutorialPopUp.ShowPopUp();
		}
	}
	
	//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
	
	public void ButtonPlayPressed()
	{
		tutorialPopUp.HidePopUp();
	}
}
