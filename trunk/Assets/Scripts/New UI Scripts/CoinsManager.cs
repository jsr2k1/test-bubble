using UnityEngine;
using System.Collections;

public class CoinsManager : MonoBehaviour
{
	public static CoinsManager instance;
	public int coins;

	//Creamos un evento para poder saber cuando se ha cambiado el valor de las coins
	public delegate void CoinsChanged();
	public static event CoinsChanged OnCoinsChanged;

	////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
	
	void Awake()
	{
		instance = this;
	}

	////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

	public void SetCoins(int newCoins)
	{
		coins = newCoins;

		if(OnCoinsChanged!=null){
			OnCoinsChanged();
		}
	}
}
