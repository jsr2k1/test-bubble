using UnityEngine;
using System.Collections;

public class AccelController : MonoBehaviour
{
	public float multiplier;
	Vector3 smoothAccel;
	float kFilteringFactor = 0.1f;
	Vector3 originalPosition;
	
	////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
	
	void Awake()
	{
		originalPosition = transform.localPosition;
	}
	
	////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
	
	void Update()
	{
		smoothAccel.x = Input.acceleration.x * kFilteringFactor + smoothAccel.x * (1.0f - kFilteringFactor);
		smoothAccel.y = Input.acceleration.y * kFilteringFactor + smoothAccel.y * (1.0f - kFilteringFactor);
		smoothAccel.z = Input.acceleration.z * kFilteringFactor + smoothAccel.z * (1.0f - kFilteringFactor);

		float x = Mathf.Clamp(smoothAccel.x*multiplier, -70.0f, 70.0f);
		transform.localPosition = originalPosition - new Vector3(x, smoothAccel.y*multiplier, 0.0f);
	}
}
