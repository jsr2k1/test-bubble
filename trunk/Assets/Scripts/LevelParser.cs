using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System; 

public class LevelParser : MonoBehaviour
{
	public static LevelParser instance;
	
	public int maxLevels=40; 

	//FilesContaining the levels
	public string level1;
	public string level2;
	public string level3;
	public string level4;
	public string level5;
	public string level6;

	//ArrayList containing all the txt lines
	public List<List<string>> filedata;

	public List<List<string>> level = new List<List<string>>();
	//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
			
	void Awake()
	{
		instance = this;
	}

	//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

	public void LoadTextLevel(int levelNumber , int worldToLoad)
	{
		//Number of rows with balls on this level
		int numberRowsWithBalls = 0;
		/*
		 * calc to get the line of the level we want to load
		 * The levels have 10 lines so with this calc que can get the first line of the level
		 *(n-1)*numero_linea+n , this returns the first line and this final line + 10 its the last line of the level.
		 * */
		int lineLevel = 0;

		if(levelNumber > 1){
			lineLevel =((levelNumber-1)*14+levelNumber)-1;
		}
		//if(worldToLoad == 1){
			ReadFile(level1);
		//}
		//else if(worldToLoad == 2){
		//	ReadFile(level2);
		//}
		//Temporary y to fill the level ListArray
		int tempy = 0;
		//Every row
		for(int y=lineLevel; y <(lineLevel+14); y++)
		{
			bool isRowWithBall = false;
			//Temporary string array to split the level line and clean it of empty spaces
			string[] line;
			//Spliting the filedata row by spaces
			line  = filedata[y][0].Split(' ');

			//Temprow to introduce on the final level multidimensional listArray
			List<string> tempRow = new List<string>();
			//Reading first line(Every col) and parsing the spaces and empty places into filedata[n][x] again
			for(int x=0; x < line.Length; x++)
			{
				//if we had a string on it saving it again on filedata
				if(!line[x].Equals(" ") && !line[x].Equals("")){
					tempRow.Add(line[x].ToString());
				}
				//Setting the number of rows with balls
				if(!line[x].Equals(" ") && !line[x].Equals("") && !line[x].Equals("-"))
				{
					isRowWithBall=true;
				}
			}
			if(isRowWithBall==true){
				numberRowsWithBalls++;
			}
			//filling the level second dimension with the temp list array
			level.Add(tempRow);
			//Increasing tempY
			tempy++;
		}

		//Reverse to array cos the PlayingObjectGeneration scripts fills the level with balls from the bottom to the top
		level.Reverse();

		//Sending to levelmanager how many rows with balls there are on this level
		//LevelManager.minimumNumberOfRows = numberRowsWithBalls;
		LevelManager.minimumNumberOfRows = 14;
		LevelManager.totalNoOfRows = numberRowsWithBalls;
		//Debug print of the final level parsed uncoment to see on the console **Tip remember to unlock the collapse option on the console unity
		/*for(int row=0; row < level.Count; row++)
		{
			for(int col=0; col < level[row].Count; col++)
			{
				//print(level[row][col]);
				print("Row:"+row+"|Col:"+col);
			}
		}*/
	}

	//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

	public void ReadFile(string filename)
	{
		string LevelPath = System.IO.Path.Combine(Application.streamingAssetsPath, filename+".txt");

		if(Application.platform == RuntimePlatform.Android)
		{
			// Android
			string oriPath = System.IO.Path.Combine(Application.streamingAssetsPath, filename+".txt");
			
			// Android only use WWW to read file
			WWW reader = new WWW(oriPath);
			while( ! reader.isDone){}
			
			string realPath = Application.persistentDataPath + "/" + filename+".txt";
			System.IO.File.WriteAllBytes(realPath, reader.bytes);
			
			LevelPath = realPath;
		}

		StreamReader sr = File.OpenText(LevelPath);
		filedata = new List<List<string>>();
		string line;
		while((line = sr.ReadLine()) != null)
		{
			List<string> list = new List<string>();
			list = line.Split('\t').ToList();
			filedata.Add(list);
		}
		sr.Close();
	}

	//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

	public List<List<string>> GetLevelArray()
	{
		return level;
	}

	//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

	public int GetBallColor(int row , int col)
	{
		int color = 0;
		int res;

		if(int.TryParse(level[row][col], out res)){
			color = int.Parse(level[row][col]);
		}
		return color;
	}
}



