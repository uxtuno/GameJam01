using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 対象
/// </summary>
public enum Target
{
	None,
	Player,
	Enemy,
}

/// <summary>
/// 弾
/// </summary>
public class Bullet : MonoBehaviour, IChronoControllable
{
	/// <summary>
	/// 速度
	/// </summary>
	[SerializeField]
	float speed = 32.0f;

	/// <summary>
	/// 爆発Prefab
	/// </summary>
	[SerializeField]
	GameObject explosionPrefab;

	/// <summary>
	/// 進行方向
	/// </summary>
	public Vector3 direction { get; set; }

	/// <summary>
	/// 攻撃対象
	/// </summary>
	public Target target { get; set; }

	public float localTimeScale { get; private set; } = 1.0f;

	/// <summary>
	/// 時間制御多重度
	/// </summary>
	int timeControlDepth = 0;

	void Awake()
	{
		localTimeScale = Time.timeScale;
	}

	void Update()
    {
		transform.position += direction * speed * Time.deltaTime * localTimeScale;
    }

	void OnTriggerEnter(Collider other)
	{
		switch (target) {
		case Target.None:
			break;
		case Target.Player:
			var player = other.GetComponentInParent<Player>();
			if (!player) {
				break;
			}
			break;
		case Target.Enemy:
			var enemy = other.GetComponentInParent<BaseEnemy>();
			if (!enemy) {
				break;
			}
			enemy.NotifyDamage();
			break;
		default:
			break;
		}

		if (other.GetComponent<ChronoField>()) {
			return;
		}

		Explosion();
	}

	public void AddControl(ChronoField controller)
	{
		++timeControlDepth;
		localTimeScale *= controller.timeMultiplier;
		Debug.Log(timeControlDepth);
	}

	public void RemoveControl(ChronoField controller)
	{
		--timeControlDepth;
		localTimeScale *= (1.0f / controller.timeMultiplier);
		Debug.Log(timeControlDepth);

		if (timeControlDepth == 0) {
			localTimeScale = 1.0f;
		}
	}

	void Explosion()
	{
		Instantiate(explosionPrefab, transform.position, transform.rotation);
		Destroy(gameObject);
	}
}
