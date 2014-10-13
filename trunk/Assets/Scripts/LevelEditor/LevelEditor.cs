using UnityEngine;
using System.Collections;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System;

public class LevelEditor : MonoBehaviour
{
	public EventSystem eventSystem;
	public Sprite spriteBall;
	public Sprite spriteEmpty;

	public Sprite[] ballColors;
	public Text textNumColor;
	public Text textNumLevel;

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
				int i,j;
				ParseButton(textComponent.text, out i, out j);
				currentLevel[i,j] = -1;
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
		int numColor = ParseColor(textNumColor.text);
		if(numColor<1){
			return;
		}
		Image imageComponent = eventSystem.currentSelectedObject.GetComponent<Image>();
		Text textComponent = eventSystem.currentSelectedObject.transform.GetChild(0).GetComponent<Text>();

		int i,j;
		ParseButton(textComponent.text, out i, out j);

		if(currentLevel[i,j] < 0){
			int index = UnityEngine.Random.Range(0,numColor);
			imageComponent.sprite = ballColors[index];
			imageComponent.color = new Color(1,1,1,1f);
			currentLevel[i,j] = index;
		}else{
			imageComponent.sprite = spriteEmpty;
			imageComponent.color = new Color(1,1,1,0.2f);
			currentLevel[i,j] = -1;
		}
	}

	//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
	//Obtener el numero de fila y columna del boton a partir de su nombre
	void ParseButton(string text, out int i, out int j)
	{
		string[] numbers = text.Split('_');
		i = int.Parse(numbers[0]) - 1;
		j = int.Parse(numbers[1]) - 1;
	}

	//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

	int ParseLevel(string sLevel)
	{
		//Do some checks
		int res;
		if(!int.TryParse(sLevel, out res)){
			Debug.Log("El numero del nivel es incorrecto");
			return -1;
		}
		int numLevel = int.Parse(sLevel);
		if(numLevel<1 || numLevel>20){
			Debug.Log("El numero tiene que ser entre 1 y 20");
			return -1;
		}
		return numLevel;
	}
	
	//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
	
	int ParseColor(string sColor)
	{
		//Do some checks
		int res;
		if(!int.TryParse(sColor, out res)){
			Debug.Log("El numero de colores es incorrecto");
			return -1;
		}
		int numColor = int.Parse(textNumColor.text);
		if(numColor<1 || numColor>6){
			Debug.Log("El numero tiene que ser entre 1 y 6");
			return -1;
		}
		return numColor;
	}
	
	//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
	
	public void LoadLevel()
	{
		ClearButtons();
		ClearArray();
		FillArray();
		DrawBalls();
	}

	//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
	//Poner todos los valores del array a -1
	void ClearArray()
	{
		for(int i=0;i<numRows;i++){
			for(int j=0;j<numCols;j++){
				currentLevel[i,j] = -1;
			}
		}
	}

	//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
	//Leer el archivo levels.txt y rellenar el array con los valores
	void FillArray()
	{
		string LevelPath = System.IO.Path.Combine(Application.streamingAssetsPath, "levels.txt");
		StreamReader sr = File.OpenText(LevelPath);
		string line;
		int iLine=0;

		int numLevel = ParseLevel(textNumLevel.text);
		if(numLevel<1){
			return;
		}
		//Skip lines
		for(int i=0;i<(15*(numLevel-1))+1;i++){
			line = sr.ReadLine();
		}
		//Read level
		while((line = sr.ReadLine()) != null && iLine<numRows+1)
		{
			List<string> list = line.Split(new char[0], StringSplitOptions.RemoveEmptyEntries).ToList();
			int iNum=0;
			foreach(string s in list)
			{
				if(s!="-"){
					currentLevel[iLine,iNum] = int.Parse(s);
				}else{
					currentLevel[iLine,iNum] = -1;
				}
				iNum++;
			}
			iLine++;
		}
		sr.Close();
	}

	//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

	void DrawBalls()
	{
		for(int i=0;i<numRows;i++){
			for(int j=0;j<numCols;j++){
				if(currentLevel[i,j] != -1){
					GameObject goButton = GameObject.Find("Button_"+(i+1).ToString("00")+"_"+(j+1).ToString("00"));
					if(goButton!=null){
						goButton.GetComponent<Image>().sprite = ballColors[currentLevel[i,j]];
						goButton.GetComponent<Image>().color = new Color(1,1,1,1f);
					}
				}
			}
		}
	}

	//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
	
	public void SaveLevel()
	{
		
	}
}




