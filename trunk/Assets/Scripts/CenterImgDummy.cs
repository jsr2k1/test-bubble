using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class CenterImgDummy : MonoBehaviour
{
	public Image imgDummy;
	float levelMin=0;	//nivel minimo posible
	float levelMax;		//nivel maximo posible
	
	////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

	void Start()
	{
		//1.- Obtenemos los valores min y max en funcion del ratio de pantalla
		//En 16:9 --> El ImgDummy va de 1500 a -1500
		//En 4:3 --> El ImgDummy va de 1650 a -1650
		float ratio = Screen.height/Screen.width;
		float t = Mathf.InverseLerp(1.777f, 1.333f, ratio);
		float minY = Mathf.Lerp(1500, 1650, t);
		
		//2.- Obtenemos la posicion del ImgDummy en funcion del nivel actual
		//En el nivel 1 tiene que estar en minY
		//En el nivel maximo tiene que estar en -minY
		float yPos;
		int lvl = PlayerPrefs.GetInt("Level");
		levelMax = LevelParser.instance.maxLevels;
		
		t = Mathf.InverseLerp(levelMin, levelMax, lvl);
		yPos = Mathf.Lerp(minY, -minY, t);

		imgDummy.transform.localPosition = new Vector3(imgDummy.transform.localPosition.x, yPos, imgDummy.transform.localPosition.z);
	}
}



