// -----------------------------------------------
// Documentation: http://www.reign-studios.net/docs/unity-plugin/
// -----------------------------------------------

using UnityEngine;
using System;
using System.IO;
using System.Collections;
using Reign;

public class StreamsDemo : MonoBehaviour
{
	public Texture2D ReignLogo, ReignLogo2;
	private Texture2D currentImage;

	private bool saveDataFileStatus, loadDataFileStatus;
	private string saveDataFileStatusText, loadDataFileStatusText;

	private bool waiting;

	void Start()
	{
		currentImage = ReignLogo;

		// NOTE: Other usfull methods...
		//StreamManager.SaveScreenShotToPictures(...);
		//StreamManager.MakeFourCC(...);
	}

	void OnGUI()
	{
		// ui scale
		float scale = new Vector2(Screen.width, Screen.height).magnitude / new Vector2(1280, 720).magnitude;

		// draw logo
		GUI.DrawTexture(new Rect((Screen.width/2)-(64*scale), 64*scale, 128*scale, 128*scale), currentImage);
		if (GUI.Button(new Rect((Screen.width/2)-(64), 0, 128, 64*scale), "Clear PlayerPrefs")) PlayerPrefs.DeleteAll();

		// Local data files
		if (!saveDataFileStatus && !loadDataFileStatus && GUI.Button(new Rect(0, 64*scale, 128, 64*scale), "Save Data file"))
		{
			// NOTE: Only supported on Win8
			saveDataFileStatusText = "Saving Data...";
			saveDataFileStatus = true;
			var data = new byte[1] {(byte)UnityEngine.Random.Range(0, 255)};
			StreamManager.SaveFile("MyFile.data", data, FolderLocations.Storage, dataFileSavedCallback);
		}
		else
		{
			GUI.Label(new Rect(132, 64*scale, 256, 64*scale), saveDataFileStatusText);
		}

		if (!saveDataFileStatus && !loadDataFileStatus && GUI.Button(new Rect(0, 128*scale, 128, 64*scale), "Load Data file"))
		{
			loadDataFileStatusText = "Loading Data...";
			loadDataFileStatus = true;
			StreamManager.LoadFile("MyFile.data", FolderLocations.Storage, dataFileLoadedCallback);
		}
		else
		{
			GUI.Label(new Rect(132, 128*scale, 256, 64*scale), loadDataFileStatusText);
		}

		// save and load images
		if (!waiting && GUI.Button(new Rect(0, 200*scale, 128, 64*scale), "Save Reign Logo"))
		{
			waiting = true;
			var data = ReignLogo.EncodeToPNG();
			StreamManager.SaveFile("TEST.png", data, FolderLocations.Pictures, imageSavedCallback);
		}

		if (!waiting && GUI.Button(new Rect(0, (200+64)*scale, 128, 64*scale), "Load Reign Logo"))
		{
			waiting = true;
			// NOTE: LoadFile doesn't support the Pictures folder on iOS.
			StreamManager.LoadFile("TEST.png", FolderLocations.Pictures, imageLoadedCallback);
		}

		if (!waiting && GUI.Button(new Rect(0, 332*scale, 128, 64*scale), "Save Image Picker"))
		{
			waiting = true;
			// NOTE: SaveFileDialog for pictures not supported on iOS.
			var data = ReignLogo.EncodeToPNG();
			StreamManager.SaveFileDialog(data, FolderLocations.Pictures, new string[]{".png"}, imageSavedCallback);
		}

		if (!waiting && GUI.Button(new Rect(0, (332+64)*scale, 128, 64*scale), "Image Picker"))
		{
			waiting = true;
			// NOTE: Unity only supports loading png and jpg data
			StreamManager.LoadFileDialog(FolderLocations.Pictures, new string[]{".png", ".jpg"}, imageLoadedCallback);
		}
	}

	private void dataFileSavedCallback(bool succeeded)
	{
		saveDataFileStatus = false;
		saveDataFileStatusText = "Data Saved Status: " + succeeded;
	}

	private void dataFileLoadedCallback(Stream stream, bool succeeded)
	{
		try
		{
			loadDataFileStatus = false;
			loadDataFileStatusText = "Data Loaded Status: " + succeeded;
			if (succeeded) loadDataFileStatusText += " Data: " + stream.ReadByte();
		}
		catch (Exception e)
		{
			MessageBoxManager.Show("Error", e.Message);
		}
		finally
		{
			// NOTE: Make sure you dispose of this stream !!!
			if (stream != null) stream.Dispose();
		}
	}

	private void imageSavedCallback(bool succeeded)
	{
		waiting = false;
		MessageBoxManager.Show("Image Status", "Image Saved: " + succeeded);
		if (succeeded) currentImage = ReignLogo2;
	}

	private void imageLoadedCallback(Stream stream, bool succeeded)
	{
		waiting = false;
		MessageBoxManager.Show("Image Status", "Image Loaded: " + succeeded);
		if (!succeeded)
		{
			if (stream != null) stream.Dispose();
			return;
		}
		
		try
		{
			var data = new byte[stream.Length];
			stream.Read(data, 0, data.Length);
			currentImage = new Texture2D(4, 4);
			currentImage.LoadImage(data);
		}
		catch (Exception e)
		{
			MessageBoxManager.Show("Error", e.Message);
		}
		finally
		{
			// NOTE: Make sure you dispose of this stream !!!
			if (stream != null) stream.Dispose();
		}
	}

	void Update()
	{
		// NOTE: If you are getting unity activity pause timeout issues on Android, call "ApplicationEx.Quit();"
		// There seems to be what may be a memeory leak in Unity4.3+
		// Until this is fixed I recomend trying to calling this quit method on Android.
		// (It will save your player prefs and use "System.exit(0)" instead of "finish()" on Android)
		// If you have a better work-around, email support, Thanks.
		if (Input.GetKeyUp(KeyCode.Escape)) ApplicationEx.Quit();// NOTE: Unity 4.5 does not need this
	}
}
