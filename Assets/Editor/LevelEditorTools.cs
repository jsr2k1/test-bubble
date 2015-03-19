using UnityEngine;
using UnityEngine.UI;
using UnityEditor;
using System.Collections;

class MyWindow : EditorWindow
{
	[MenuItem ("Custom/RenameChild")]
	public static void RenameChild()
	{
		int i=1;
		foreach(Transform child in Selection.activeGameObject.transform)
		{
			child.name = "Button_" + Selection.activeGameObject.transform.name + "_" + i.ToString("00");
			child.GetChild(0).GetComponent<Text>().text = Selection.activeGameObject.transform.name + "_" + i.ToString("00");
			i++;
		}
	}
	
	////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
	
	[MenuItem ("Custom/DeleteAllKeysPlayerPrefs")]
	public static void DeleteAllKeysPlayerPrefs()
	{
		PlayerPrefs.DeleteAll();
	}
}
