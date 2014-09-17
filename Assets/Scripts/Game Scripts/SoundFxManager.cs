using UnityEngine;
using System.Collections;

public class SoundFxManager : MonoBehaviour 
{
    public static SoundFxManager instance;

    public AudioSource buttonClickSound;
    public AudioSource shootingSound;

    public AudioSource levelClearSound;
    public AudioSource levelFailSound;
    
    public AudioSource themeMusic;

    public AudioSource collisionSound;
    public AudioSource wallCollisionSound;

    public AudioSource burstSound;

	////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

    void Awake()
    {
        instance = this;
    }

	////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

	public void Play(AudioSource audiosource)
	{
		audiosource.Play();
	}
}




