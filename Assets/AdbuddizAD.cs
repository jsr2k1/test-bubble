using UnityEngine;
using System.Collections;

public class AdbuddizAD : MonoBehaviour {

	void Start() { 
		AdBuddizBinding.SetAndroidPublisherKey("9fed98cf-4c88-4109-a9a2-9e7b6888e7ed");
		AdBuddizBinding.SetIOSPublisherKey("TEST_PUBLISHER_KEY_IOS");

		//AdBuddizBinding.SetTestModeActive();


		if (Random.Range (0, 100) > 50) {
			AdBuddizBinding.CacheAds();
			AdBuddizBinding.ShowAd ();
		}
	}

}
