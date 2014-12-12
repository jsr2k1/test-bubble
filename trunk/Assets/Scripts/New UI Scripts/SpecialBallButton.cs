using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class SpecialBallButton : MonoBehaviour
{
	public string BallString;
	public Text counter;
	public Button buttonCount;
	Button buttonBall;
	
	///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
	
	void Awake()
	{
		buttonBall = GetComponent<Button>();
		SetButtons();
		SetText();
	}
	
	///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
	
	void OnEnable()
	{
		Striker.OnSpecialBallLaunched += SetButtons;
		Striker.OnSpecialBallLaunched += SetText;
		
		PurchaseSpecialBall.OnSpecialBallBuyed += SetButtons;
		PurchaseSpecialBall.OnSpecialBallBuyed += SetText;
	}
	
	///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
	
	void OnDisable()
	{
		Striker.OnSpecialBallLaunched -= SetButtons;
		Striker.OnSpecialBallLaunched -= SetText;
		
		PurchaseSpecialBall.OnSpecialBallBuyed -= SetButtons;
		PurchaseSpecialBall.OnSpecialBallBuyed -= SetText;
	}
	
	///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
	
	public void SetButtons()
	{
		if(PlayerPrefs.GetInt(BallString) > 0){
			buttonCount.interactable = false;
			buttonBall.interactable = true;
		}else{
			buttonCount.interactable = true;
			buttonBall.interactable = false;
		}
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
		if(PlayerPrefs.GetInt(BallString) > 0){
			if(BallString == "Fire Ball"){
				Striker.instance.SetFireBall();
			}else if(BallString == "Bomb Ball"){
				Striker.instance.SetBombBall();	
			}else if(BallString == "Multicolor Ball"){
				Striker.instance.SetMultiBall();	
			}
		}
		SetButtons();
		SetText();
	}
}
