using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public enum PatternType
{
    None,
    ZigZag1,
    ZigZag2,
    ZigZag3,
    Net1,
    Net2,
    Net3,
    Net4,
	TextLevel,
}

//This script creates different formation of pattern of the playing object.

public class ObjectFormationPattern : MonoBehaviour 
{
    public static ObjectFormationPattern instance;

    PatternType type;
    int x;
    int y;

	void Awake () 
    {
        type = LevelManager.patternType;
        instance = this;
	
	}

    //New Playing object is added to the game only if it returns TRUE 
    internal bool ShouldAddObject(int _columnNo,int _rowNo)
    {
        x = _columnNo;
        y = _rowNo;

      

        if (type == PatternType.ZigZag1)
            return ZigZag(2);
        if (type == PatternType.ZigZag2)
            return ZigZag(3);
        if (type == PatternType.ZigZag3)
            return ZigZag(4);

        if (type == PatternType.Net1)
            return Net1();
        if (type == PatternType.Net2)
            return Net2();
        if (type == PatternType.Net3)
            return Net3();
        if (type == PatternType.Net4)
            return Net4();

		if (type == PatternType.TextLevel)
			return TextLevelLoad();

        return true;
        
    }

    bool Net4()
    {
        if (x % 3 == 0 || y % 3 == 0)
            return true;
        else
            return false;
    }

    bool Net3()
    {
        if (x % 2 == 0 || y % 3 == 0)
            return true;
        else
            return false;
    }

    bool Net2()
    {
        if (x % 3 == 0 || y % 2 == 0)
            return true;
        else
            return false;
    }

    bool Net1()
    {
        if (x % 2 == 0 || y % 2 == 0)
            return true;
        else
            return false;
    }

    private bool ZigZag(int factor)
    {
        if (x % factor == 0)
            return true;
        else
            return false;
    }

	bool TextLevelLoad()
	{
		if(y%2==0 && x>7)
		{
			//On the unpairs files there are only 7 balls instead of 8 balls
			return false;
		}

		//Temp levelArray
		List<List<string>> levelArray = LevelParser.instance.GetLevelArray();

		if (!levelArray[y][x].Equals("-"))
			return true;
		else
			return false;

	}
}
