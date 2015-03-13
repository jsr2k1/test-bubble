using UnityEngine;
using System.Collections;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class IAPButtonGameScene : MonoBehaviour, IPointerClickHandler
{
	IAPManager iapManager;
	public string item;
	Text textPrice;

	//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

	void Start()
	{
		GameObject go = GameObject.Find("IAPManager");
		textPrice = transform.GetChild(0).GetComponent<Text>();

		if(go!=null){
			iapManager = go.GetComponent<IAPManager>();
		}else{
			Debug.Log("ERROR: No se encuentra el objeto IAPManager");
		}
		textPrice.text = iapManager.dictPrices[item];
	}

	//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
	//Estamos en la escena de juego y tenemos que acceder al objeto IAPManager que viene de la escena de mundos
	//No podemos asignar el objeto mediante la interfaz de Unity pq en esta escena no existe
	public void OnPointerClick(PointerEventData data)
	{
		if(AudioManager.instance.bSoundsOn){
			audio.Play();
		}
		if(iapManager!=null){
			iapManager.PurchaseSomething(item);
		}else{
			Debug.Log("ERROR: No se encuentra el complemento IAPManager");
		}
	}
}
