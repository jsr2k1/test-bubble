// -------------------------------------------------------
//  Created by Andrew Witte.
// -------------------------------------------------------

using UnityEngine;
using UnityEditor;
using Reign;
using System.IO;
using System.Xml;
using System.Linq;
using System.Collections.Generic;
using System;

namespace Reign.EditorTools
{
	public static class BuildMenu
	{
		/*[UnityEditor.Callbacks.PostProcessBuild]
		public static void OnPostprocessBuild(BuildTarget target, string pathToBuiltProject)
		{
			Debug.Log(target);
			Debug.Log(pathToBuiltProject);
		}*/

		[MenuItem("Edit/Reign/Tools/Download Win8-WP8 Patch")]
		static void DownloadWin8WP8Patch()
		{
			EditorUtility.DisplayDialog("Win8-WP8 Patch", "Get the Win8-WP8 patch to fix Mono.Cecil build errors.\n\nGo to \"http://www.reign-studios.net/docs/unity-plugin/\"", "Ok");
		}

		[MenuItem("Edit/Reign/Version Info")]
		static void ShowBuildNumber()
		{
			string buildNum = "???";
			try
			{
				using (var stream = new FileStream(Application.dataPath + "/Plugins/ReignVersionCheck", FileMode.Open, FileAccess.Read, FileShare.None))
				using (var reader = new StreamReader(stream))
				{
					buildNum = reader.ReadLine();
				}
			}
			catch (Exception e)
			{
				Debug.LogError(e.Message);
			}

			EditorUtility.DisplayDialog("Version Info", "Build Number: " + buildNum, "Ok");
		}
	
		private static string convertPathToPlatform(string path)
		{
			#if UNITY_EDITOR_WIN
			return path.Replace('/', '\\');
			#else
			return path.Replace('\\', '/');
			#endif
		}
		
		private static void disableReignForPlatform(BuildTargetGroup platform)
		{
			string valueBlock = PlayerSettings.GetScriptingDefineSymbolsForGroup(platform);
			if (string.IsNullOrEmpty(valueBlock))
			{
				PlayerSettings.SetScriptingDefineSymbolsForGroup(platform, "DISABLE_REIGN");
			}
			else
			{
				string newValue = "";
				var values = valueBlock.Split(';', ' ');
				foreach (var value in values)
				{
					if (value == "DISABLE_REIGN") return;
					newValue += value + ';';
				}
				
				newValue += "DISABLE_REIGN";
				PlayerSettings.SetScriptingDefineSymbolsForGroup(platform, newValue);
			}
		}
		
		private static void enableReignForPlatform(BuildTargetGroup platform)
		{
			string valueBlock = PlayerSettings.GetScriptingDefineSymbolsForGroup(platform);
			if (!string.IsNullOrEmpty(valueBlock))
			{
				string newValue = "";
				var values = valueBlock.Split(';', ' ');
				foreach (var value in values)
				{
					if (value != "DISABLE_REIGN") newValue += value + ';';
				}
				
				if (newValue.Length != 0 && newValue[newValue.Length-1] == ';') newValue = newValue.Substring(0, newValue.Length-1);
				PlayerSettings.SetScriptingDefineSymbolsForGroup(platform, newValue);
			}
		}
		
		private static BuildTargetGroup convertBuildTarget(BuildTarget target)
		{
			switch (target)
			{
				case BuildTarget.Android: return BuildTargetGroup.Android;
				#if !UNITY_4_5
				//case BuildTarget.Wii: return BuildTargetGroup.Wii;
				case BuildTarget.BB10: return BuildTargetGroup.BB10;
				#else
				case BuildTarget.BlackBerry: return BuildTargetGroup.BlackBerry;
				#endif
				case BuildTarget.FlashPlayer: return BuildTargetGroup.FlashPlayer;
				case BuildTarget.iPhone: return BuildTargetGroup.iPhone;
				case BuildTarget.MetroPlayer: return BuildTargetGroup.Metro;
				case BuildTarget.NaCl: return BuildTargetGroup.NaCl;
				case BuildTarget.PS3: return BuildTargetGroup.PS3;
				case BuildTarget.WebPlayer: return BuildTargetGroup.WebPlayer;
				case BuildTarget.WP8Player: return BuildTargetGroup.WP8;
				case BuildTarget.XBOX360: return BuildTargetGroup.XBOX360;
				
				case BuildTarget.StandaloneLinux: return BuildTargetGroup.Standalone;
				case BuildTarget.StandaloneLinux64: return BuildTargetGroup.Standalone;
				case BuildTarget.StandaloneLinuxUniversal: return BuildTargetGroup.Standalone;
				case BuildTarget.StandaloneOSXIntel: return BuildTargetGroup.Standalone;
				case BuildTarget.StandaloneOSXIntel64: return BuildTargetGroup.Standalone;
				case BuildTarget.StandaloneOSXUniversal: return BuildTargetGroup.Standalone;
				case BuildTarget.StandaloneWindows: return BuildTargetGroup.Standalone;
				case BuildTarget.StandaloneWindows64: return BuildTargetGroup.Standalone;
				
				default: throw new System.Exception("Unknown BuildTarget: " + target);
			}
		}
		
		[MenuItem("Edit/Reign/Set Platform/Disable (Disables Reign for current platform)")]
		static void DisableReign()
		{
			disableReignForPlatform(convertBuildTarget(EditorUserBuildSettings.activeBuildTarget));
			
			setWin8(false);
			setWP8(false);
			setIOS(false);
			setAndroid(false);
			setBB10(false);
			
			setWin32(false);
			setOSX(false);
			setLinux(false);
			setWeb(false);
	
			finish("Disable");
		}
		
		[MenuItem("Edit/Reign/Set Platform/Enable (Enables Reign for current platform)")]
		static void EnableReign()
		{
			enableReignForPlatform(convertBuildTarget(EditorUserBuildSettings.activeBuildTarget));
			
			if (EditorUserBuildSettings.activeBuildTarget == BuildTarget.MetroPlayer) SetPlatformWin8();
			if (EditorUserBuildSettings.activeBuildTarget == BuildTarget.WP8Player) SetPlatformWP8();
			if (EditorUserBuildSettings.activeBuildTarget == BuildTarget.iPhone) SetPlatformIOS();
			if (EditorUserBuildSettings.activeBuildTarget == BuildTarget.Android) SetPlatformAndroid();
			#if !UNITY_4_5
			if (EditorUserBuildSettings.activeBuildTarget == BuildTarget.BB10) SetPlatformBB10();
			#else
			if (EditorUserBuildSettings.activeBuildTarget == BuildTarget.BlackBerry) SetPlatformBB10();
			#endif
	
			if (EditorUserBuildSettings.activeBuildTarget == BuildTarget.StandaloneWindows ||
				EditorUserBuildSettings.activeBuildTarget == BuildTarget.StandaloneWindows64) SetPlatformWin32();
	
			if (EditorUserBuildSettings.activeBuildTarget == BuildTarget.StandaloneOSXIntel ||
				EditorUserBuildSettings.activeBuildTarget == BuildTarget.StandaloneOSXIntel64 ||
				EditorUserBuildSettings.activeBuildTarget == BuildTarget.StandaloneOSXUniversal) SetPlatformOSX();
	
			if (EditorUserBuildSettings.activeBuildTarget == BuildTarget.StandaloneLinux ||
				EditorUserBuildSettings.activeBuildTarget == BuildTarget.StandaloneLinux64 ||
				EditorUserBuildSettings.activeBuildTarget == BuildTarget.StandaloneLinuxUniversal) SetPlatformLinux();
	
			if (EditorUserBuildSettings.activeBuildTarget == BuildTarget.WebPlayer ||
				EditorUserBuildSettings.activeBuildTarget == BuildTarget.WebPlayerStreamed) SetPlatformWeb();
	
			finish("Enable");
		}
	
		[MenuItem("Edit/Reign/Set Platform/Revert (Do before committing to version control)")]
		static void SetPlatformRevert()
		{
			setWin8(true);
			setWP8(true);
			setIOS(true);
			setAndroid(true);
			setBB10(true);
			
			setWin32(true);
			setOSX(true);
			setLinux(true);
			setWeb(true);
	
			finish("Revert");
		}
	
		[MenuItem("Edit/Reign/Set Platform/Win8")]
		static void SetPlatformWin8()
		{
			setWin8(true);
			setWP8(false);
			setIOS(false);
			setAndroid(false);
			setBB10(false);
			
			setWin32(false);
			setOSX(false);
			setLinux(false);
			setWeb(false);
	
			finish("Win8");
		}
	
		[MenuItem("Edit/Reign/Set Platform/WP8")]
		static void SetPlatformWP8()
		{
			setWin8(false);
			setWP8(true);
			setIOS(false);
			setAndroid(false);
			setBB10(false);
			
			setWin32(false);
			setOSX(false);
			setLinux(false);
			setWeb(false);
	
			finish("WP8");
		}
	
		[MenuItem("Edit/Reign/Set Platform/iOS")]
		static void SetPlatformIOS()
		{
			setWin8(false);
			setWP8(false);
			setIOS(true);
			setAndroid(false);
			setBB10(false);
			
			setWin32(false);
			setOSX(false);
			setLinux(false);
			setWeb(false);
	
			finish("iOS");
		}
		
		[MenuItem("Edit/Reign/Set Platform/Android")]
		static void SetPlatformAndroid()
		{
			setWin8(false);
			setWP8(false);
			setIOS(false);
			setAndroid(true);
			setBB10(false);
			
			setWin32(false);
			setOSX(false);
			setLinux(false);
			setWeb(false);
	
			finish("Android");
		}
	
		[MenuItem("Edit/Reign/Set Platform/BB10")]
		static void SetPlatformBB10()
		{
			setWin8(false);
			setWP8(false);
			setIOS(false);
			setAndroid(false);
			setBB10(true);
	
			setWin32(false);
			setOSX(false);
			setLinux(false);
			setWeb(false);
	
			finish("BB10");
		}
	
		[MenuItem("Edit/Reign/Set Platform/Win32")]
		static void SetPlatformWin32()
		{
			setWin8(false);
			setWP8(false);
			setIOS(false);
			setAndroid(false);
			setBB10(false);
	
			setWin32(true);
			setOSX(false);
			setLinux(false);
			setWeb(false);
	
			finish("Win32");
		}
	
		[MenuItem("Edit/Reign/Set Platform/OSX")]
		static void SetPlatformOSX()
		{
			setWin8(false);
			setWP8(false);
			setIOS(false);
			setAndroid(false);
			setBB10(false);
	
			setWin32(false);
			setOSX(true);
			setLinux(false);
			setWeb(false);
	
			finish("OSX");
		}
	
		[MenuItem("Edit/Reign/Set Platform/Linux")]
		static void SetPlatformLinux()
		{
			setWin8(false);
			setWP8(false);
			setIOS(false);
			setAndroid(false);
			setBB10(false);
	
			setWin32(false);
			setOSX(false);
			setLinux(true);
			setWeb(false);
	
			finish("Linux");
		}
	
		[MenuItem("Edit/Reign/Set Platform/Web")]
		static void SetPlatformWeb()
		{
			setWin8(false);
			setWP8(false);
			setIOS(false);
			setAndroid(false);
			setBB10(false);
	
			setWin32(false);
			setOSX(false);
			setLinux(false);
			setWeb(true);
	
			finish("Web");
		}
	
		private static void finish(string platform)
		{
			AssetDatabase.Refresh();
			EditorUtility.DisplayDialog("Set to " + platform, "NOTE: Changed some Reign libs from .dll to dllx or vise-versa.\nThis is done to remove uneeded or conflicting libs in the build process.", "OK");
		}
	
		private static void setWin8(bool enable)
		{
			// do nothing...
		}
	
		private static void setWP8(bool enable)
		{
			// do nothing...
		}
	
		private static void setIOS(bool enable)
		{
			setDll(enable, "/IOS/Reign.iOS");
			setDll(enable, "/IOS/Reign.iOS.AdMob");
			setDll(enable, "/IOS/Reign.iOS.iAd");
			setDll(enable, "/IOS/Reign.iOS.IAP");
			setDll(enable, "/IOS/Reign.iOS.GameCenter");
		}
		
		private static void setAndroid(bool enable)
		{
			setDll(enable, "/Android/Reign.Android");
		}
		
		private static void setBB10(bool enable)
		{
			setDll(enable, "/BlackBerry/Reign.BB10");
		}
	
		private static void setWin32(bool enable)
		{
			setDll(enable, "/Win32/Reign.Win32");
		}
	
		private static void setOSX(bool enable)
		{
			setDll(enable, "/OSX/Reign.OSX");
		}
	
		private static void setLinux(bool enable)
		{
			setDll(enable, "/Linux/Reign.Linux");
		}
	
		private static void setWeb(bool enable)
		{
			setDll(enable, "/Web/Reign.Web");
		}
	
		private static void setDll(bool enable, string dllName)
		{
			string dllFile = convertPathToPlatform(Application.dataPath + "/Plugins" + dllName + ".dll");
			string dllxFile = convertPathToPlatform(Application.dataPath + "/Plugins" + dllName + ".dllx");
			if (enable)
			{
				// delete junk lib
				if (File.Exists(dllFile) && File.Exists(dllxFile))
				{
					File.SetAttributes(dllxFile, FileAttributes.Normal);
					File.Delete(dllxFile);
					return;
				}

				// change lib ext
				if (File.Exists(dllxFile))
				{
					File.SetAttributes(dllxFile, FileAttributes.Normal);
					File.Move(dllxFile, Path.ChangeExtension(dllxFile, ".dll"));
					if (File.Exists(dllxFile + ".meta"))
					{
						File.SetAttributes(dllxFile + ".meta", FileAttributes.Normal);
						File.Delete(dllxFile + ".meta");
					}
				}
			}
			else
			{
				// delete junk lib
				if (File.Exists(dllxFile) && File.Exists(dllFile))
				{
					File.SetAttributes(dllxFile, FileAttributes.Normal);
					File.Delete(dllxFile);
				}

				// change lib ext
				if (File.Exists(dllFile))
				{
					File.SetAttributes(dllFile, FileAttributes.Normal);
					File.Move(dllFile, Path.ChangeExtension(dllFile, ".dllx"));
					if (File.Exists(dllFile + ".meta"))
					{
						File.SetAttributes(dllFile + ".meta", FileAttributes.Normal);
						File.Delete(dllFile + ".meta");
					}
				}
			}
		}
	}
}