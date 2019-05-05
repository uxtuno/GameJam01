using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 時間制御エリア
/// </summary>
[RequireComponent(typeof(Collider))]
public class ChronoField : MonoBehaviour
{
	[SerializeField]
	float _timeMultiplier = 0.5f;

	/// <summary>
	/// 時間に対する乗算値
	/// </summary>
	public float timeMultiplier { get; private set; }

	void Awake()
	{
		timeMultiplier = _timeMultiplier;	
	}

	void OnTriggerEnter(Collider other)
	{
		var controllable = other.GetComponent<IChronoControllable>();
		if (controllable != null) {
			controllable.AddControl(this);
		}
	}

	void OnTriggerExit(Collider other)
	{
		var controllable = other.GetComponent<IChronoControllable>();
		if (controllable != null) {
			controllable.RemoveControl(this);
		}
	}
}
