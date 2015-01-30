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
		dict.Add("id_quit", new string[] {"¿SALIR DEL JUEGO?", "EXIT GAME?"});
		dict.Add("id_yes", new string[] {"SI", "YES"});
		dict.Add("id_no", new string[] {"NO", "NO"});
		dict.Add("id_cash", new string[] {"ORO:", "CASH:"});
		dict.Add("id_buylives", new string[] {"Comprar Vidas", "Buy Lives"});
		dict.Add("id_askfriends", new string[] {"Pedir a Amigos", "Ask Friends"});
		dict.Add("id_level", new string[] {"NIVEL", "LEVEL"});
		dict.Add("id_levelfail", new string[] {"¡Nivel fallido!", "Level failed!"});
		dict.Add("id_levelsuccess", new string[] {"¡Nivel completado!", "Level completed!"});
		dict.Add("id_score", new string[] {"Puntuaci\u00F3n: ", "Score: "});
		dict.Add("id_quitlevel", new string[] {"¿SALIR DEL NIVEL?", "QUIT LEVEL?"});
		dict.Add("id_lifeinfo", new string[] {"¡Perder\u00E1s una vida!", "You will lose a life!"});
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
		dict.Add("id_PLAY", new string[] {"JUGAR", "PLAY"});
		dict.Add("id_ARCADE", new string[] {"ARCADE", "ARCADE"});
		dict.Add("id_CONNECT", new string[] {"CONECTA", "CONNECT"});
		dict.Add("id_outofbubbles", new string[] {"¡No te quedan m\u00E1s bolas!", "You are out of bubbles!"});
		dict.Add("id_infooutofbubbles", new string[] {"¡Consigue 5 bolas extra y una bola multicolor!", "Get one multicolor ball and 5 extra bubbles!"});
		dict.Add("id_quitarcade", new string[] {"¡Perder\u00E1s el progreso actual!", "You will lose the current progress!"});
		dict.Add("id_loading", new string[] {"Cargando...", "Loading..."});
		dict.Add("id_rank", new string[] {"Pos.", "Rank"});
		dict.Add("id_rank_name", new string[] {"Nombre", "Name"});
		dict.Add("id_rank_score", new string[] {"Puntos", "Score"});
		
		//Tutoriales
		dict.Add("id_tutorial_pulsa_pantalla", new string[] {"Pulsa la pantalla para apuntar y disparar", "Touch the screen to aim and shoot"});
		dict.Add("id_tutorial_junta_3_bolitas", new string[] {"Junta 3 bolas para que exploten", "Match 3 bubbles to explode them"});
		dict.Add("id_tutorial_despeja_techo", new string[] {"Despeja el techo para superar el nivel", "Clear all ceiling bubbles to win the level"});
		dict.Add("id_tutorial_libera_lorito", new string[] {"Peta las bolas que rodean al loro para liberarlo", "Free the parrot by bursting the bubbles around him"});
		dict.Add("id_tutorial_num_tiros", new string[] {"Tienes un n\u00FAmero limitado de disparos para completar el nivel", "You have a limited number of shots to complete the level"});
		dict.Add("id_tutorial_paredes", new string[] {"Puedes hacer rebotar las bolas en las paredes", "Use the walls to rebound the bubbles"});
		dict.Add("id_tutorial_cambia_bolas", new string[] {"Pulsa las flechas para cambiar las bolas", "Switch bubbles by touching the arrows"});
		dict.Add("id_tutorial_booster_bomba", new string[] {"Usa la bomba para petar varias bolas, incluso obst\u00E1culos", "Use the bomb to destroy several bubbles, including obstacles"});
		dict.Add("id_tutorial_booster_fuego", new string[] {"Usa la bola fuego para destruir muchas bolas", "Use the fireball to destroy everything in its way"});
		dict.Add("id_tutorial_booster_multicolor", new string[] {"Usa la bola multicolor para combinar con cualquier color", "Use the Multicolor bubble to combine the match with any color"});
	}
}



