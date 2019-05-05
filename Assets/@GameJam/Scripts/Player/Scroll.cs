using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Scroll : MonoBehaviour
{
	[SerializeField]
	Vector3 direction;

	[SerializeField]
	float speed = 10.0f;

    void Update()
    {
		transform.position += direction.normalized * speed * Time.deltaTime;
    }
}
