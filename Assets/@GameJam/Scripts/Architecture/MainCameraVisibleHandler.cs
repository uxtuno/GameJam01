using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// メインカメラに写っているか
/// </summary>
public class MainCameraVisibleHandler : MonoBehaviour
{
	public bool isVisible { get; private set; }

	private void OnBecameVisible()
	{
		if (Camera.current == Camera.main) {
			isVisible = true;
		}
	}

	void OnBecameInvisible()
	{
		if (Camera.current == Camera.main) {
			isVisible = false;
		}
	}
}
