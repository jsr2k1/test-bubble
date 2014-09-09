using UnityEngine;
using System.Collections;

public class InGameScriptRefrences : MonoBehaviour 
{  
    internal static PlayingObjectManager playingObjectManager;
    internal static PlayingObjectGeneration playingObjectGeneration;
    internal static StrikerManager strikerManager;

    void Awake()
    { 
        playingObjectManager = GameObject.Find("Playing Object Manager").GetComponent<PlayingObjectManager>();
        playingObjectGeneration = GameObject.Find("Playing Object Generation").GetComponent<PlayingObjectGeneration>();
        strikerManager = GameObject.Find("Striker Manager").GetComponent<StrikerManager>();

    }
}
