using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IChronoControllable
{
	float localTimeScale { get; }

	/// <summary>
	/// 時間制御追加
	/// </summary>
	/// <param name="value"></param>
	void AddControl(ChronoField controller);

	/// <summary>
	/// 時間制御削除
	/// </summary>
	/// <param name="controller"></param>
	void RemoveControl(ChronoField controller);
}
