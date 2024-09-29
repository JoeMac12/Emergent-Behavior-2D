using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Predator : MonoBehaviour
{
	public float speed = 5f;
	public float time = 1f;

	private Vector2 direction;
	private float timer;

	void Start()
	{
		ChangeDirection();
	}

	void Update()
	{
		transform.position += (Vector3)(direction * speed * Time.deltaTime);

		ScreenWrap();

		timer += Time.deltaTime;
		if (timer >= time)
		{
			ChangeDirection();
			timer = 0f;
		}
	}

	void ChangeDirection()
	{
		direction = Random.insideUnitCircle.normalized;
	}

	void ScreenWrap()
	{
		Vector3 pos = transform.position;

		if (pos.x > ScreenBounds.rightLimit)
			pos.x = ScreenBounds.leftLimit;
		else if (pos.x < ScreenBounds.leftLimit)
			pos.x = ScreenBounds.rightLimit;

		if (pos.y > ScreenBounds.topLimit)
			pos.y = ScreenBounds.bottomLimit;
		else if (pos.y < ScreenBounds.bottomLimit)
			pos.y = ScreenBounds.topLimit;

		transform.position = pos;
	}
}
