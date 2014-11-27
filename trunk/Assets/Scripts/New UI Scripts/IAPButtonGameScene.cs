using UnityEngine;
using System.Collections;
using UnityEngine.EventSystems;

public class IAPButtonGameScene : MonoBehaviour, IPointerClickHandler
{
	IAPManager iapManager;
	public string item;

	//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

	void Start()
	{
		GameObject go = GameObject.Find("IAPManager");

		if(go!=null){
			iapManager = go.GetComponent<IAPManager>();
		}else{
			Debug.Log("ERROR: No se encuentra el objeto IAPManager");
		}
	}

	//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
	//Estamos en la escena de juego y tenemos que acceder al objeto IAPManager que viene de la escena de mundos
	//No podemos asignar el objeto mediante la interfaz de Unity pq en esta escena no existe
	public void OnPointerClick(PointerEventData data)
	{
		int i = PlayerPrefs.GetInt("Sounds");

		if(i>0){
			audio.Play();
		}

		if(iapManager!=null){
			iapManager.PurchaseSomething(item);
		}else{
			Debug.Log("ERROR: No se encuentra el complemento IAPManager");
		}
	}
}
