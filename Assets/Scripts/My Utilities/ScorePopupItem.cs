using UnityEngine;
using System.Collections;

public class ScorePopupItem : MonoBehaviour 
{
    public TextMesh myTextMesh;
    static float delay = 0;

	void Start () 
    {        
        delay = .1f;
        renderer.material.color = Color.red;
        renderer.material.color = new Color(renderer.material.color.r, renderer.material.color.g, renderer.material.color.b, .9f);
	}

    internal static void ResetDelay()
    {
        delay = 0;
    }

    internal void BringItForward(int score)
    {        
        myTextMesh.text = score.ToString();
        Invoke("ZoomIn", delay);
        delay += .1f;
    }

    

    void ZoomIn()
    {
		SoundFxManager.instance.Play(SoundFxManager.instance.burstSound);
        iTween.MoveBy(gameObject, new Vector3(0, .5f, 0), .5f);
        Invoke("ZoomOut", .5f);
    }

    void ZoomOut()
    {
        iTween.ScaleTo(gameObject, new Vector3(0, 0, 0), .4f);
        Destroy(gameObject, .2f);
    }
	
	
}
