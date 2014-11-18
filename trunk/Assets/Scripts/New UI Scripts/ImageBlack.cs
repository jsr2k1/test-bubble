using UnityEngine;
using System.Collections;

public class ImageBlack : MonoBehaviour
{
	public static PopUpMgr.PopUpAction popUpActionImageBlack = PopUpMgr.PopUpAction.Skip;
	Animator animator;
	bool bShow=false;
	
	////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
	
	void Awake()
	{
		animator = GetComponent<Animator>();
		popUpActionImageBlack = PopUpMgr.PopUpAction.Skip;
	}
	
	////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
	
	void Update()
	{
		if(popUpActionImageBlack==PopUpMgr.PopUpAction.On && !bShow){
			animator.SetTrigger("ShowPopUp");
			popUpActionImageBlack=PopUpMgr.PopUpAction.Skip;
			bShow=true;
		}
		else if(popUpActionImageBlack==PopUpMgr.PopUpAction.Off && bShow){
			animator.SetTrigger("HidePopUp");
			popUpActionImageBlack=PopUpMgr.PopUpAction.Skip;
			bShow=false;
		}
	}
}
