using UnityEngine;
using System.Collections;

public class AccelController : MonoBehaviour
{
	public float multiplier;
	Vector3 smoothAccel;
	float kFilteringFactor = 0.1f;
	Vector3 originalPosition;
	float screen_ratio;
	
	////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
	
	void Awake()
	{
		originalPosition = transform.localPosition;
		screen_ratio = Screen.width / 720.0f;
	}
	
	////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
	
	void Update()
	{
		smoothAccel.x = Input.acceleration.x * kFilteringFactor + smoothAccel.x * (1.0f - kFilteringFactor);
		smoothAccel.y = Input.acceleration.y * kFilteringFactor + smoothAccel.y * (1.0f - kFilteringFactor);
		smoothAccel.z = Input.acceleration.z * kFilteringFactor + smoothAccel.z * (1.0f - kFilteringFactor);
		
		transform.localPosition = originalPosition - new Vector3(smoothAccel.x*multiplier*screen_ratio, smoothAccel.y*multiplier*screen_ratio, 0.0f);
	}
}
