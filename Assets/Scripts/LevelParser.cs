using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System; 

public class LevelParser : MonoBehaviour {

	public static LevelParser instance;

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

	//Colors array and checkers
	public List<int> colorArray = new List<int>();

	bool isColor0 = false;
	bool isColor1 = false;
	bool isColor2 = false;
	bool isColor3 = false;
	bool isColor4 = false;
	bool isColor5 = false;
	bool isColor6 = false;
	bool isColor7 = false;
			
	void Awake()
	{
		instance = this;
	}

	// Use this for initialization
	void Start () {

	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public void LoadTextLevel(int levelNumber , int worldToLoad)
	{
		//Number of rows with balls on this level
		int numberRowsWithBalls = 0;

		/*
		 * calc to get the line of the level we want to load
		 * The levels have 10 lines so with this calc que can get the first line of the level
		 * (n-1)*numero_linea+n , this returns the first line and this final line + 10 its the last line of the level.
		 * */
		int lineLevel = 0;

		if(levelNumber > 1)
		{
			lineLevel = ((levelNumber-1)*14+levelNumber)-1;
		}
		 
		if(worldToLoad == 1)
		{
			ReadFile(level1);
		}
		else if(worldToLoad == 2)
		{
			ReadFile(level2);
		}

		//Temporary y to fill the level ListArray
		int tempy = 0;
		//Every row
		for(int y=lineLevel; y < (lineLevel+14); y++)
		{
			bool isRowWithBall = false;

			//Temporary string array to split the level line and clean it of empty spaces
			string[] line;
			//Spliting the filedata row by spaces
			line  = filedata[y][0].Split(' ');

			//print (filedata[y][0]);
			//print ("line lenght :"+line.Length);

			//Temprow to introduce on the final level multidimensional listArray
			List<string> tempRow = new List<string>();
			//Reading first line (Every col) and parsing the spaces and empty places into filedata[n][x] again
			for(int x=0; x < line.Length; x++)
			{
				//if we had a string on it saving it again on filedata
				if(!line[x].Equals(" ") && !line[x].Equals(""))
				{
					//Saving the filedata parsed line with only numbers and -
					//level[tempy].Add(line[x].ToString());
					tempRow.Add(line[x].ToString());

					//Checking and adding the color of the balls on the level to the color array
					if(line[x].Equals("0"))
					{
						isColor0 = true;
					}
					else if(line[x].Equals("1"))
					{
						isColor1 = true;
					}
					else if(line[x].Equals("2"))
					{
						isColor2 = true;
					}
					else if(line[x].Equals("3"))
					{
						isColor3 = true;
					}
					else if(line[x].Equals("4"))
					{
						isColor4 = true;
					}
					else if(line[x].Equals("5"))
					{
						isColor5 = true;
					}
					else if(line[x].Equals("6"))
					{
						isColor6 = true;
					}
					else if(line[x].Equals("7"))
					{
						isColor7 = true;
					}
				}

				//Setting the number of rows with balls
				if(!line[x].Equals(" ") && !line[x].Equals("") && !line[x].Equals("-"))
				{
					isRowWithBall=true;
				}
			}

			if(isRowWithBall==true)
			{
				numberRowsWithBalls++;
			}

			//filling the level second dimension with the temp list array
			level.Add(tempRow);
			
			//Increasing tempY
			tempy++;
		}

		//Reverse to array cos the PlayingObjectGeneration scripts fills the level with balls from the bottom to the top
		level.Reverse();

		//Filling the colorArray
		if(isColor0 == true)
		{
			colorArray.Add(0);
		}

		if(isColor1 == true)
		{
			colorArray.Add(1);
		}

		if(isColor2 == true)
		{
			colorArray.Add(2);
		}

		if(isColor3 == true)
		{
			colorArray.Add(3);
		}

		if(isColor4 == true)
		{
			colorArray.Add(4);
		}

		if(isColor5 == true)
		{
			colorArray.Add(5);
		}

		if(isColor6 == true)
		{
			colorArray.Add(6);
		}

		if(isColor7 == true)
		{
			colorArray.Add(7);
		}

		//Sending to levelmanager how many rows with balls there are on this level
		//LevelManager.minimumNumberOfRows = numberRowsWithBalls;
		LevelManager.minimumNumberOfRows = 14;
		LevelManager.totalNoOfRows = numberRowsWithBalls;
		//Debug print of the final level parsed uncoment to see on the console **Tip remember to unlock the collapse option on the console unity
		/*for(int row=0; row < level.Count; row++)
		{
			for(int col=0; col < level[row].Count; col++)
			{
				//print (level[row][col]);
				print ("Row:"+row+"|Col:"+col);
			}
		}*/
	}


	public void ReadFile(string filename)
	{
		string LevelPath = System.IO.Path.Combine(Application.streamingAssetsPath, filename+".txt");

		if (Application.platform == RuntimePlatform.Android)
		{
			// Android
			string oriPath = System.IO.Path.Combine(Application.streamingAssetsPath, filename+".txt");
			
			// Android only use WWW to read file
			WWW reader = new WWW(oriPath);
			while ( ! reader.isDone) {}
			
			string realPath = Application.persistentDataPath + "/" + filename+".txt";
			System.IO.File.WriteAllBytes(realPath, reader.bytes);
			
			LevelPath = realPath;
		}

		StreamReader sr = File.OpenText(LevelPath); 

		//filedata = sr.ReadToEnd().Split('\n').Select(s=>s.Split('\t').ToList()).ToList();

		filedata = new List<List<string>>();
		string line;
		while ((line = sr.ReadLine()) != null)
		{
			List<string> list = new List<string>();
			list = line.Split('\t').ToList();
			filedata.Add(list);
		}
		
		sr.Close();
	}

	public List<List<string>> GetLevelArray()
	{
		return level;
	}

	public int GetBallColor(int row , int col)
	{
		int color = 0;
		//print (row+"|"+col);
		//print (level[row][col]);
		//Checking the unpair rows that only have 7 objects

		/*if(!level[row][col].Equals("-")){
			color = int.Parse(level[row][col]);
		}
		*/
		int res;
		if(int.TryParse(level[row][col], out res)){
			color = int.Parse(level[row][col]);
		}
		return color;
	}

	public int GetRandomBall()
	{
		bool found = false;
		int ball = 0;

		while(found!= true)
		{
			ball = UnityEngine.Random.Range(0,6);
			for(int x = 0; x<colorArray.Count; x++)
			{
				if(colorArray[x] == ball)
				{
					found = true;
					return ball;
				}
			}
		}

		return ball;
	}

}
