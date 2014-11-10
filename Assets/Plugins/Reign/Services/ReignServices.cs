// -------------------------------------------------------
//  Created by Andrew Witte.
// -------------------------------------------------------

using UnityEngine;
using System.Collections.Generic;
using System.Collections;

/// <summary>
/// Helper hacks.
/// </summary>
public static class ApplicationEx
{
	#if UNITY_ANDROID && !UNITY_EDITOR
	private static AndroidJavaClass native;
	#endif
	
	static ApplicationEx()
	{
		#if UNITY_ANDROID && !UNITY_EDITOR
		native = new AndroidJavaClass("com.reignstudios.reignnative.ReignUnityActivity");
		#endif
	}

	/// <summary>
	/// Used to force quit and Android app.
	/// </summary>
	public static void Quit()
	{
		//#if UNITY_ANDROID && !UNITY_4_5
		#if UNITY_ANDROID && !UNITY_EDITOR
		if (native != null)
		{
			PlayerPrefs.Save();
			System.Threading.Thread.Sleep(1000);
			native.CallStatic("QuitApp");
			native.Dispose();
			native = null;
		}
		#else
		Application.Quit();
		#endif
	}
}

/// <summary>
/// Main Reign delegate class.
/// </summary>
public class ReignServices : MonoBehaviour
{
	/// <summary>
	/// Current instance of the ReignServices object.
	/// </summary>
	public static ReignServices Singleton {get; private set;}

	internal bool requestFrameDone, canDestroy;
	internal delegate void frameDoneCallbackMethod();
	internal frameDoneCallbackMethod frameDoneCallback;

	/// <summary>
	/// Sevice delegate method.
	/// </summary>
	public delegate void ServiceMethod();
	private static event ServiceMethod updateService, onguiService, destroyService;

	/// <summary>
	/// Used to add service callbacks.
	/// </summary>
	/// <param name="update">Update callback.</param>
	/// <param name="onGUI">OnGUI callback.</param>
	/// <param name="destroy">OnDestroy callback.</param>
	public static void AddService(ServiceMethod update, ServiceMethod onGUI, ServiceMethod destroy)
	{
		if (update != null)
		{
			updateService -= update;
			updateService += update;
		}

		if (onGUI != null)
		{
			onguiService -= onGUI;
			onguiService += onGUI;
		}

		if (destroy != null)
		{
			destroyService -= destroy;
			destroyService += destroy;
		}
	}

	/// <summary>
	/// Used to remove service callbacks.
	/// </summary>
	/// <param name="update">Update callback.</param>
	/// <param name="onGUI">OnGUI callback.</param>
	/// <param name="destroy">OnDestroy callback.</param>
	public static void RemoveService(ServiceMethod update, ServiceMethod onGUI, ServiceMethod destroy)
	{
		if (update != null) updateService -= update;
		if (onGUI != null) onguiService -= onGUI;
		if (destroy != null) destroyService -= destroy;
	}

	static ReignServices()
	{
		#if DISABLE_REIGN
		Debug.LogWarning("NOTE: Reign is disabled for this platform. Check Player Settings for 'DISABLE_REIGN'");
		#endif
	}
	
	/// <summary>
	/// Used to check if the ReignServices obj exists in the scene.
	/// </summary>
	public static void CheckStatus()
	{
		if (Singleton == null) Debug.LogError("ReignServices Prefab or Script does NOT exist in your scene!");
	}

	void Awake()
	{
		if (Singleton != null)
		{
			canDestroy = false;
			Destroy(gameObject);
			return;
		}

		canDestroy = true;
		Singleton = this;
		DontDestroyOnLoad(gameObject);
	}

	void OnDestroy()
	{
		if (!canDestroy) return;

		PlayerPrefs.Save ();
		if (destroyService != null) destroyService();
		
		Singleton = null;
		updateService = null;
		onguiService = null;
		destroyService = null;
	}

	void Update()
	{
		if (updateService != null) updateService();

		if (requestFrameDone)
		{
			requestFrameDone = false;
			StartCoroutine(frameSync());
		}
	}

	private IEnumerator frameSync()
	{
		yield return new WaitForEndOfFrame();
		if (frameDoneCallback != null) frameDoneCallback();
	}

	void OnGUI()
	{
		if (onguiService != null) onguiService(); 
	}
}
