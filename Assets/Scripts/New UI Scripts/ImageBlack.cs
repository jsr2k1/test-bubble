using UnityEngine;
using System.Collections;

public class ImageBlack : MonoBehaviour
{
	public static PopUpMgr.PopUpAction popUpActionImageBlack = PopUpMgr.PopUpAction.Idle;
	Animator animator;
	bool bShow=false;
	
	////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
	
	void Awake()
	{
		animator = GetComponent<Animator>();
		popUpActionImageBlack = PopUpMgr.PopUpAction.Idle;
	}
	
	////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
	
	void Update()
	{
		if(popUpActionImageBlack==PopUpMgr.PopUpAction.OnShow && !bShow){
			animator.SetTrigger("ShowPopUp");
			popUpActionImageBlack=PopUpMgr.PopUpAction.Idle;
			bShow=true;
		}
		else if(popUpActionImageBlack==PopUpMgr.PopUpAction.OnHide && bShow){
			animator.SetTrigger("HidePopUp");
			popUpActionImageBlack=PopUpMgr.PopUpAction.Idle;
			bShow=false;
		}
	}
}
