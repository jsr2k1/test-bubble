// -------------------------------------------------------
//  Created by Andrew Witte.
// -------------------------------------------------------

using UnityEngine;
using System.Collections;
using Reign.Plugin;

#if UNITY_EDITOR
using UnityEditor;
#endif

namespace Reign
{
	/// <summary>
	/// Used to manage message boxes.
	/// </summary>
	public static class MessageBoxManager
	{
		private static IMessageBoxPlugin plugin;

		static MessageBoxManager()
		{
			ReignServices.CheckStatus();
			
			#if !DISABLE_REIGN
				#if UNITY_EDITOR || UNITY_METRO || UNITY_WP8
				plugin = new MessageBoxPlugin();
				#elif UNITY_STANDALONE_WIN
				plugin = new MessageBoxPlugin_Win32();
				#elif UNITY_STANDALONE_OSX
				plugin = new MessageBoxPlugin_OSX();
				#elif UNITY_WEBPLAYER
				plugin = new MessageBoxPlugin_Web();
				#elif UNITY_IOS
				plugin = new MessageBoxPlugin_iOS();
				#elif UNITY_ANDROID
				plugin = new MessageBoxPlugin_Android();
				#elif UNITY_BB10
				plugin = new MessageBoxPlugin_BB10();
				#endif
				
				ReignServices.AddService(update, null, null);
			#endif
		}

		/// <summary>
		/// Use to show a simple OK message box.
		/// </summary>
		/// <param name="title">MessageBox Title.</param>
		/// <param name="message">MessageBox Message.</param>
		public static void Show(string title, string message)
		{
			#if UNITY_EDITOR
			EditorUtility.DisplayDialog(title, message, "OK");
			#else
			plugin.Show(title, message, MessageBoxTypes.Ok, null);
			#endif
		}

		/// <summary>
		/// Use to show an OK or OK/Cancel message box.
		/// </summary>
		/// <param name="title">MessageBox Title.</param>
		/// <param name="message">MessageBox Message.</param>
		/// <param name="type">MessageBox Type.</param>
		/// <param name="callback">The callback that fires when done.</param>
		public static void Show(string title, string message, MessageBoxTypes type, MessageBoxCallback callback)
		{
			#if UNITY_EDITOR
			if (type == MessageBoxTypes.Ok)
			{
				Show(title, message);
				if (callback != null) callback(MessageBoxResult.Ok);
			}
			else
			{
				bool value = EditorUtility.DisplayDialog(title, message, "OK", "Cancel");
				if (callback != null) callback(value ? MessageBoxResult.Ok : MessageBoxResult.Cancel);
			}
			#else
			plugin.Show(title, message, type, callback);
			#endif
		}
		
		private static void update()
		{
			plugin.Update();
		}
	}
}