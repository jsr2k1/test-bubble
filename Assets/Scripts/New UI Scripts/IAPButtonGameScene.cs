using UnityEngine;
using System.Collections;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class IAPButtonGameScene : MonoBehaviour, IPointerClickHandler
{
	IABManager iabManager;
	public string item;
	Text textPrice;

	//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

	void Start()
	{
		GameObject go = GameObject.Find("IABManager");
		textPrice = transform.GetChild(0).GetComponent<Text>();

		if(go!=null){
			iabManager = go.GetComponent<IABManager>();
		}else{
			Debug.Log("ERROR: No se encuentra el objeto IABManager");
		}
		if(iabManager.dictPrices.ContainsKey(item)){
			textPrice.text = iabManager.dictPrices[item];
		}
	}

	//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
	//Estamos en la escena de juego y tenemos que acceder al objeto IABManager que viene de la escena de mundos
	//No podemos asignar el objeto mediante la interfaz de Unity pq en esta escena no existe
	public void OnPointerClick(PointerEventData data)
	{
		if(AudioManager.instance.bSoundsOn){
			audio.Play();
		}
		if(iabManager!=null){
			iabManager.PurchaseSomething(item);
		}else{
			Debug.Log("ERROR: No se encuentra el complemento IABManager");
		}
	}
}
