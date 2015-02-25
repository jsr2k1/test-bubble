using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class CenterImgDummy : MonoBehaviour
{
	public Image imgDummy;
	float minY=1500; 	//posicion mas baja posible de la camara
	float maxY=-1500;	//posicion mas alta posible de la camara
	float levelMin=0;	//nivel minimo posible
	float levelMax;		//nivel maximo posible
	
	////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

	void Start()
	{
		float yPos;
		int lvl = PlayerPrefs.GetInt("Level");
		levelMax = LevelParser.instance.maxLevels;
		
		/*
		if (lvl > -1 && lvl < 11) {
			yPos = 788;
		} else if (lvl > 10 && lvl < 21) {
			yPos = 397;
		} else if (lvl > 20 && lvl < 31) {
			yPos = -198;
		} else if (lvl > 30 && lvl < 41) {
			yPos = -788;
		}*/
		
		float t = Mathf.InverseLerp(levelMin, levelMax, lvl);
		yPos = Mathf.Lerp(minY, maxY, t);

		imgDummy.transform.localPosition = new Vector3(imgDummy.transform.localPosition.x, yPos, imgDummy.transform.localPosition.z);
	}
}



