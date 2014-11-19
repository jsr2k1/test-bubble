using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class CenterImgDummy : MonoBehaviour {

	public Image imgDummy;
	int yPos;
	int lvl;

	// Use this for initialization
	void Start () {
		lvl = PlayerPrefs.GetInt ("Level");

		if (lvl > -1 && lvl < 11) {
			yPos = 788;
		} else if (lvl > 10 && lvl < 21) {
			yPos = 397;
		} else if (lvl > 20 && lvl < 31) {
			yPos = -198;
		} else if (lvl > 30 && lvl < 41) {
			yPos = -788;
		}

		imgDummy.transform.localPosition = new Vector3(imgDummy.transform.localPosition.x, yPos, imgDummy.transform.localPosition.z);
	}
	

}
