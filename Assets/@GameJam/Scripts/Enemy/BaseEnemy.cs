using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;
using UniRx.Async;
using System;

public class BaseEnemy : MonoBehaviour
{
	/// <summary>
	/// 最大HP
	/// </summary>
	[SerializeField]
	int maxHp;

	/// <summary>
	/// 弾丸
	/// </summary>
	[SerializeField]
	Bullet bulletPrefab;

	/// <summary>
	/// 弾丸の発射間隔
	/// </summary>
	[SerializeField]
	float shootInterval = 2.0f;

	/// <summary>
	/// 弾丸の発射位置
	/// </summary>
	[SerializeField]
	Transform[] bulletPoints;

	[SerializeField]
	GameObject explosionPrefab;

	[SerializeField]
	MainCameraVisibleHandler visibleHandler;

	IntReactiveProperty hp { get; } = new IntReactiveProperty();

	bool isRender;
	bool isShootable = false;

	void Awake()
	{
		hp.Value = maxHp;
	}

	void Start()
	{
		shootLoop().Forget();
	}

	void Update()
	{
		isRender = false;
	}

	async UniTask shootLoop()
	{
		while (true) {
			Observable.NextFrame()
				.TakeUntilDestroy(this)
				.Where(_ => !!visibleHandler.isVisible)
				.Subscribe(_ => {
					shoot();
				});
			await UniTask.Delay((int)(shootInterval * 1000.0f));
		}
	}

	void shoot()
	{
		foreach (var point in bulletPoints) {
			var rotation = Quaternion.AngleAxis(UnityEngine.Random.Range(-15.0f, 15.0f), Vector3.up);
			var bullet = Instantiate(bulletPrefab, point.position, rotation * point.rotation);
			bullet.direction = rotation * point.forward;
			bullet.target = Target.Player;
		}
	}

	/// <summary>
	/// ダメージを与える
	/// </summary>
	/// <param name="damage"></param>
	public void NotifyDamage(int damage = 1)
	{
		hp.Value -= damage;
		if (hp.Value <= 0) {
			OnDeath();
		}
	}

	/// <summary>
	/// ダメージを受けたタイミングを監視
	/// </summary>
	/// <returns></returns>
	public IObservable<Unit> ObserveDamage()
	{
		return hp.Where(_ => hp.Value < maxHp).AsUnitObservable();
	}

	public int GetHp()
	{
		return hp.Value;
	}

	void OnWillRenderObject()
	{
		if (Camera.current == Camera.main) {
			isRender = true;
		}
	}

	void OnPostRender()
	{
		isShootable = isRender;
	}

	/// <summary>
	/// 死亡
	/// </summary>
	public virtual void OnDeath()
	{
		Instantiate(explosionPrefab, transform.position, transform.rotation);
		Destroy(gameObject);
	}

}
