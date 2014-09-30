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
	/// Used to manage marketing features.
	/// </summary>
	public static class MarketingManager
	{
		private static IIMarketingPlugin plugin;

		static MarketingManager()
		{
			ReignServices.CheckStatus();
			
			#if !DISABLE_REIGN
				#if UNITY_EDITOR || UNITY_STANDALONE_WIN || UNITY_STANDALONE_LINUX || UNITY_STANDALONE_OSX || UNITY_METRO || UNITY_WP8
				plugin = new MarketingPlugin();
				#elif UNITY_BB10
				plugin = new MarketingPlugin_BB10();
				#elif UNITY_ANDROID
				plugin = new MarketingPlugin_Android();
				#elif UNITY_IOS
				plugin = new MarketingPlugin_iOS();
				#endif
			#endif
		}

		/// <summary>
		/// Show your app in the store.
		/// </summary>
		/// <param name="desc">Marketing Desc.</param>
		public static void OpenStore(MarketingDesc desc)
		{
			plugin.OpenStore(desc);
		}

		/// <summary>
		/// Show your app in the store for the user to review.
		/// </summary>
		/// <param name="desc">Marketing Desc.</param>
		public static void OpenStoreForReview(MarketingDesc desc)
		{
			plugin.OpenStoreForReview(desc);
		}
	}
}