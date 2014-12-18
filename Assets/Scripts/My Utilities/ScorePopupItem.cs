using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class ScorePopupItem : MonoBehaviour 
{
    //public TextMesh myTextMesh;
    public Text text;
    static float delay = 0;

	////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

	void Awake()
	{
		//renderer.sortingLayerName = "FrontLayer";
		//transform.localScale = new Vector3(1,1,1);
		//iTween.ScaleTo(gameObject, new Vector3(0.5f, 0.5f, 1), 0.3f);
		//text.transform.localScale = new Vector3(1.0f, 1.0f, 1.0f);
	}

	////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

	void Start() 
    {        
        delay = .1f;
        //renderer.material.color = Color.red;
        //renderer.material.color = new Color(renderer.material.color.r, renderer.material.color.g, renderer.material.color.b, .9f);
	}

	////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

    internal static void ResetDelay()
    {
        delay = 0;
    }

	////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

    internal void BringItForward(int score)
    {        
        //myTextMesh.text = score.ToString();
		text.text = score.ToString();
        Invoke("ZoomIn", delay);
		//Invoke("ZoomIn", 0.1f);
        delay += .1f;
    }

	////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

    void ZoomIn()
    {
		AudioManager.instance.PlayFxSound(AudioManager.instance.burstSound);
        iTween.MoveBy(gameObject, new Vector3(0, .2f, 0), 2.0f);
        Invoke("ZoomOut", .5f);
    }

	////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

    void ZoomOut()
    {
        iTween.ScaleTo(gameObject, new Vector3(0, 0, 0), .1f);
        Destroy(gameObject, .2f);
    }
}


