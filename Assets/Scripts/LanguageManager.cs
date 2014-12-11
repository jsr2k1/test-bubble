using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System.Collections.Generic;

// www.utf8-chartable.de

public class LanguageManager : MonoBehaviour
{
	static int lang;
	static Dictionary<string, string[]> dict;
	
	//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
	
	void Start()
	{
		DontDestroyOnLoad(gameObject);
		SystemLanguage currentLang = Application.systemLanguage;

		if(currentLang == SystemLanguage.Spanish){
			lang=0;
		}else{
			lang=1;
		}
		dict = new Dictionary<string, string[]>();
		fillDict();
	}
	
	//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
	
	public static string GetText(string id)
	{
		if(dict.ContainsKey(id)){
			string[] res = LanguageManager.dict[id];
			string value = res[lang];
			return value;
		}else{
			Debug.Log("No se encuentra la key en el diccionario: " + id);
			return "ID_ERROR";
		}
	}
	
	//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
	
	void fillDict()
	{
		dict.Add("id_play", new string[] {"JUGAR", "PLAY"});
		dict.Add("id_connect", new string[] {"CONECTAR", "CONNECT"});
		dict.Add("id_settings", new string[] {"AJUSTES", "SETTINGS"});
		dict.Add("id_quit", new string[] {"QUITAR JUEGO?", "QUIT GAME?"});
		dict.Add("id_yes", new string[] {"SI", "YES"});
		dict.Add("id_no", new string[] {"NO", "NO"});
		dict.Add("id_cash", new string[] {"ORO:", "CASH:"});
		dict.Add("id_buylives", new string[] {"Comprar Vidas", "CASH: "});
		dict.Add("id_askfriends", new string[] {"Preguntar Amigos", "Ask Friends"});
		dict.Add("id_level", new string[] {"NIVEL", "LEVEL"});
		dict.Add("id_levelfail", new string[] {"¡Nivel fallido!", "Level failed!"});
		dict.Add("id_levelsuccess", new string[] {"¡Nivel completado!", "Level completed!"});
		dict.Add("id_score", new string[] {"Puntuaci\u00F3n: ", "Score: "});
		dict.Add("id_quitlevel", new string[] {"SALIR NIVEL?", "QUIT LEVEL?"});
		dict.Add("id_lifeinfo", new string[] {"Perder\u00E1s una vida!", "You will lose a life!"});
		dict.Add("id_noballs", new string[] {"No tienes m\u00E1s disparos...", "You are out of bubbles..."});
		dict.Add("id_infoclear", new string[] {"Despeja el nivel", "Clear the level"});
		dict.Add("id_info_animals", new string[] {"Libera los loros", "Free the parrots"});
		dict.Add("id_highscore", new string[] {"Puntuaci\u00F3n m\u00E1xima:", "High Score:"});
		dict.Add("id_playon", new string[] {"Continuar", "Play on"});
		dict.Add("id_endgame", new string[] {"Salir", "End game"});
		dict.Add("id_win", new string[] {"¡Has ganado!", "You won!"});
		dict.Add("id_messages", new string[] {"Mensajes", "Messages"});
		dict.Add("id_full", new string[] {"LLENO", "FULL"});
		dict.Add("id_soon", new string[] {"Pr\u00F3ximamente...", "Coming soon..."});
	}
}



