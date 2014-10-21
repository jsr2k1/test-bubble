using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using UnityEngine.EventSystems;

public class ToggleController : MonoBehaviour, IPointerClickHandler
{
	public void OnPointerClick(PointerEventData data)
	{
		audio.Play();
	}
}
