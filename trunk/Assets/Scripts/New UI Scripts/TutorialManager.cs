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
	public PopUpMgr tutorialPopUp;
	
	//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
	
	void Awake()
	{
		instance = this;
		
		int[] items = new int[]{1,2,3,4,5,6,7,8,9,10,41};//Lista de niveles en los que van a aparecer los tutoriales
		tutorialsList = items.ToList();
	}
	
	//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
	
	public void ShowTutorial()
	{
		if(tutorialsList.Contains(LevelManager.levelNo)){
			int index = tutorialsList.IndexOf(LevelManager.levelNo);
			tutorialImage.sprite = tutorialSprites[index];
			tutorialText.text = LanguageManager.GetText("id_"+tutorialImage.sprite.name);
			tutorialPopUp.ShowPopUp();
		}
	}
	
	//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
	
	public void ButtonPlayPressed()
	{
		tutorialPopUp.HidePopUp();
	}
}
