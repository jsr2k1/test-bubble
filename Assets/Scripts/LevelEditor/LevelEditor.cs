using UnityEngine;
using System.Collections;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class LevelEditor : MonoBehaviour
{
	public EventSystem eventSystem;
	public Sprite spriteBall;
	public Sprite spriteEmpty;

	public Sprite[] ballColors;
	public int numColors;

	int[,] currentLevel;
	int numRows=13;
	int numCols=10;

	//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

	void Start()
	{
		currentLevel = new int[numRows,numCols];
		ClearButtons();
	}

	//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

	public void ClearButtons()
	{
		Text[] textComponents;
		Image[] imageComponents;

		textComponents = gameObject.GetComponentsInChildren<Text>();
		foreach(Text textComponent in textComponents){
			if(textComponent.transform.tag!="LevelEditorComp"){
				textComponent.enabled=false;
			}
		}

		imageComponents = gameObject.GetComponentsInChildren<Image>();
		foreach(Image imageComponent in imageComponents){
			if(imageComponent.transform.tag!="LevelEditorComp"){
				imageComponent.sprite = spriteEmpty;
				imageComponent.color = new Color(1,1,1,0.2f);
			}
		}
	}

	//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

	public void LevelButtonPressed()
	{
		Debug.Log("Caller: " + eventSystem.currentSelectedObject.name);

		Image imageComponent = eventSystem.currentSelectedObject.GetComponent<Image>();
		Text textComponent = eventSystem.currentSelectedObject.transform.GetChild(0).GetComponent<Text>();

		int i,j;
		ParseButton(textComponent.text, out i, out j);

		if(currentLevel[i,j] == 0){
			int index = Random.Range(0,numColors);
			imageComponent.sprite = ballColors[index];
			imageComponent.color = new Color(1,1,1,1f);
			currentLevel[i,j] = 1;
		}else{
			imageComponent.sprite = spriteEmpty;
			imageComponent.color = new Color(1,1,1,0.2f);
			currentLevel[i,j] = 0;
		}
	}

	//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

	void ParseButton(string text, out int i, out int j)
	{
		string[] numbers = text.Split('_');
		i = int.Parse(numbers[0]) - 1;
		j = int.Parse(numbers[1]) - 1;
	}
}




