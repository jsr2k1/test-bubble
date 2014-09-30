using UnityEngine;
using System.Collections;

public class SwapButton : MonoBehaviour
{
	public void SwapBalls()
	{
		Striker.instance.Swap();
	}
}
