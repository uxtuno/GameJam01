using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;
using UniRx.Async;
using System;

public class Player : MonoBehaviour
{
	const string horizontalInputName = "Horizontal";
	const string verticalInputName = "Vertical";
	const string fireInputName = "Fire";

	/// <summary>
	/// 最大HP
	/// </summary>
	[SerializeField]
	int maxHp;

	/// <summary>
	/// 移動速度
	/// </summary>
	[SerializeField]
	float speed = 10.0f;

	/// <summary>
	/// 減速時の速度
	/// </summary>
	[SerializeField]
	float slowSpeed = 4.0f;

	[SerializeField]
	AudioSource shootSESource;

	/// <summary>
	/// 射撃間隔
	/// </summary>
	[SerializeField]
	float shootInterval = 0.2f;

	[SerializeField]
	float xMovableLimitMin = -12.0f;
	[SerializeField]
	float xMovableLimitMax = 12.0f;

	[SerializeField]
	float yMovableLimitMin = -12.0f;
	[SerializeField]
	float yMovableLimitMax = 12.0f;

	/// <summary>
	/// 弾丸のPrefab
	/// </summary>
	[SerializeField]
	Bullet bulletPrefab;

	/// <summary>
	/// 弾丸の発射位置リスト
	/// </summary>
	[SerializeField]
	Transform[] shootPoints;

	IntReactiveProperty hp { get; } = new IntReactiveProperty();
	SerialDisposable shootDisposer = new SerialDisposable();

	void Awake()
	{
		hp.Value = maxHp;
	}

	void Update()
	{
		var xInput = Input.GetAxisRaw(horizontalInputName);
		var yInput = Input.GetAxisRaw(verticalInputName);

		var newPosition = transform.localPosition;
		var currentSpeed = Input.GetKey(KeyCode.LeftShift) ? slowSpeed : speed;
		newPosition.x += xInput * currentSpeed * Time.deltaTime;
		newPosition.y += yInput * currentSpeed * Time.deltaTime;

		// 移動制限
		newPosition.x = Mathf.Clamp(newPosition.x, xMovableLimitMin, xMovableLimitMax);
		newPosition.y = Mathf.Clamp(newPosition.y, yMovableLimitMin, yMovableLimitMax);

		transform.localPosition = newPosition;

		if (Input.GetButtonDown(fireInputName)) {
			shootDisposer.Disposable = Observable.Timer(TimeSpan.Zero, TimeSpan.FromSeconds(shootInterval))
				.TakeUntilDestroy(this)
				.Subscribe(_ => {
					Shoot();
				});
		}

		if (Input.GetButtonUp(fireInputName)) {
			shootDisposer.Disposable = null;
		}
	}

	/// <summary>
	/// 発射
	/// </summary>
	void Shoot()
	{
		shootSESource.Play();
		foreach (var point in shootPoints) {
			var bullet = Instantiate(bulletPrefab, point.position, point.rotation);
			bullet.direction = point.forward;
			bullet.target = Target.Enemy;
		}
	}

	public void NotifyDamage(int damage)
	{
		hp.Value -= damage;
		if (hp.Value <= 0) {
			OnDeath();
		}
	}

	/// <summary>
	/// 死亡
	/// </summary>
	void OnDeath()
	{

	}
}
