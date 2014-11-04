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

	public ToggleGroup toggleColors;

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
	// Al iniciar el Editor y antes de cargar un nivel, ponemos todos los botones de las bolas vacios.
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
	// Si se pulsa un boton vacio, entonces ponemos una bola random. Si el boton esta lleno, se vacia.
	public void LevelButtonPressed()
	{
		Image imageComponent = eventSystem.currentSelectedGameObject.GetComponent<Image>();
		Text textComponent = eventSystem.currentSelectedGameObject.transform.GetChild(0).GetComponent<Text>();

		int i,j;
		ParseButton(textComponent.text, out i, out j);

		if(currentLevel[i,j] < 0){
			int index = GetColor();
			if(index<0) return;
			imageComponent.sprite = ballColors[index];
			imageComponent.color = new Color(1,1,1,1f);
			currentLevel[i,j] = GetIndex(index);
		}else{
			imageComponent.sprite = spriteEmpty;
			imageComponent.color = new Color(1,1,1,0.2f);
			currentLevel[i,j] = -1;
		}
	}
	
	//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

	int GetIndex(int index)
	{
		if(index==6){ //bola loro
			return 7;
		}else{
			return index;
		}
	}
	
	//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
	
	int GetIndexInv(int index)
	{
		if(index==7){ //bola loro
			return 6;
		}else{
			return index;
		}
	}

	//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
	//Si esta pulsado algun boton de color, devolvemos el indice del color
	//Si esta pulsado el boton de random, devolvemos un color aleatorio
	int GetColor()
	{
		int numColors = ParseNumColors(textNumColor.text);
		if(numColors<1){
			return -1;
		}
		int nColor = int.Parse(toggleColors.ActiveToggles().First().name);
		if(nColor==0){
			return UnityEngine.Random.Range(0,numColors);
		//}else if(nColor==7){
		//	return nColor;
		}else{
			return nColor-1;
		}
	}

	//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
	// Obtener el numero de fila y columna del boton a partir de su nombre.
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
	
	int ParseNumColors(string sColor)
	{
		//Do some checks
		int res;
		if(!int.TryParse(sColor, out res)){
			Debug.Log("El numero de colores es incorrecto");
			return -1;
		}
		int numColors = int.Parse(textNumColor.text);
		if(numColors<1 || numColors>6){
			Debug.Log("El numero tiene que ser entre 1 y 6");
			return -1;
		}
		return numColors;
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
	//Devuelve un string con la linea de bolas
	string GetLine(int index)
	{
		string sRes="";
		int numCols = index%2==0 ? 10 : 9;

		if(numCols==9){
			sRes+="   ";
		}
		for(int i=0;i<numCols;i++)
		{
			if(currentLevel[index-1,i]>-1){
				sRes+=currentLevel[index-1,i];
			}else{
				sRes+="-";
			}
			if(i<numCols-1){
				sRes+="   ";
			}
		}
		return sRes;
	}

	//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
	//Leemos todo el archivo de texto en un array, sustituimos el nivel y volvemos a escribir todo el archivo.
	public void SaveLevel()
	{
		string LevelPath = System.IO.Path.Combine(Application.streamingAssetsPath, "levels.txt");
		string[] lines = File.ReadAllLines(LevelPath);

		int numLevel = ParseLevel(textNumLevel.text);
		if(numLevel<1){
			return;
		}
		int iFirstLine = 15*(numLevel-1);
		lines[iFirstLine] = "6   6   6   6   6   6   6   6   6   6";

		for(int i=1;i<=numRows;i++){
			lines[iFirstLine+i] = GetLine(i);
		}
		File.WriteAllLines(LevelPath, lines);
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
					currentLevel[iLine,iNum] = GetIndexInv(int.Parse(s));
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
}




