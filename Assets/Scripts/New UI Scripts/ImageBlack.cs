using UnityEngine;
using System.Collections;

public class ImageBlack : MonoBehaviour
{
	public enum PopUpAction{
		OnShow,
		OnHide,
		Idle
	}
	public static PopUpAction popUpActionImageBlack = PopUpAction.Idle;
	Animator animator;
	bool bShow=false;
	
	////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
	
	void Awake()
	{
		animator = GetComponent<Animator>();
		popUpActionImageBlack = PopUpAction.Idle;
	}
	
	////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
	
	void Update()
	{
		if(popUpActionImageBlack==PopUpAction.OnShow && !bShow){
			animator.SetTrigger("ShowPopUp");
			popUpActionImageBlack=PopUpAction.Idle;
			bShow=true;
		}
		else if(popUpActionImageBlack==PopUpAction.OnHide && bShow){
			animator.SetTrigger("HidePopUp");
			popUpActionImageBlack=PopUpAction.Idle;
			bShow=false;
		}
	}
}
