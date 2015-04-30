using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class SpecialBallButton : MonoBehaviour
{
	public string BallString;
	public Text counter;
	public Image shopPop;
	
	///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
	
	void Awake()
	{
		SetText();
	}
	
	///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
	
	void OnEnable()
	{
		Striker.OnSpecialBallLaunched += SetText;
		PurchaseSpecialBall.OnSpecialBallBuyed += SetText;
	}
	
	///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
	
	void OnDisable()
	{
		Striker.OnSpecialBallLaunched -= SetText;
		PurchaseSpecialBall.OnSpecialBallBuyed -= SetText;
	}
	
	///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
	
	void SetText()
	{
		int count = PlayerPrefs.GetInt(BallString);
		
		if(count>0){
			counter.text = count.ToString();
		}else{
			counter.text = "+";
		}
	}
	
	///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
	
	public void OnButtonClick()
	{
		//Comprobamos si quedan boosters disponibles
		if (PlayerPrefs.GetInt (BallString) > 0) {
			if (BallString == "Fire Ball") {
				Striker.instance.SetFireBall ();
			} else if (BallString == "Bomb Ball") {
				Striker.instance.SetBombBall ();	
			} else if (BallString == "Multicolor Ball") {
				Striker.instance.SetMultiBall ();	
			}
		} else {
			shopPop.GetComponent<PopUpMgr>().ShowPopUp();
		}

		SetText();
	}
}
