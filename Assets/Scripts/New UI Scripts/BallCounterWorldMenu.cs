using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class BallCounterWorldMenu : MonoBehaviour
{
	public Text counter;
	public string BallString;
	
	void Update()
	{
		int count = PlayerPrefs.GetInt(BallString);
		
		if(count>0){
			counter.text = count.ToString();
		}else{
			counter.text = "+";
		}
	}
}
