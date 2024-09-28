using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boid : MonoBehaviour
{
	public Vector2 velocity;
	public float maxSpeed = 20f;
	public float radius = 2f;

	BoidManager boidManager;

	// Set random speed at start
	void Start()
	{
		velocity = Random.insideUnitCircle.normalized * maxSpeed;
		boidManager = FindObjectOfType<BoidManager>();
	}

	void Update()
	{
		Vector2 acceleration = Vector2.zero;

		// Find nearby boids using grid for optimization
		List<Boid> nearbyBoids = boidManager.GetNearbyBoids(this);
		Vector2 alignment = Vector2.zero;
		Vector2 cohesion = Vector2.zero;
		Vector2 separation = Vector2.zero;
		int total = 0;

		// Calculate the bird flocking behavior
		foreach (Boid other in nearbyBoids)
		{
			if (other == this) continue;

			float distance = Vector2.Distance(transform.position, other.transform.position);
			if (distance < radius)
			{
				alignment += other.velocity;
				cohesion += (Vector2)other.transform.position;
				separation += (Vector2)(transform.position - other.transform.position) / distance;
				total++;
			}
		}

		// Apply flocking behavior if there are other boids nearby
		if (total > 0)
		{
			alignment = (alignment / total).normalized * maxSpeed - velocity;
			cohesion = ((cohesion / total) - (Vector2)transform.position).normalized * maxSpeed - velocity;
			separation = (separation / total).normalized * maxSpeed - velocity;

			acceleration += (alignment * boidManager.alignmentWeight) +
				(cohesion * boidManager.cohesionWeight) +
				(separation * boidManager.separationWeight);
		}

		// Update vel and pos
		velocity += acceleration * Time.deltaTime;
		velocity = Vector2.ClampMagnitude(velocity, maxSpeed);
		transform.position += (Vector3)(velocity * Time.deltaTime);

		// Make boids face movement direction
		float angle = Mathf.Atan2(velocity.y, velocity.x) * Mathf.Rad2Deg;
		transform.rotation = Quaternion.Euler(new Vector3(0, 0, angle - 90));

		// Screen wrap boids
		ScreenWrap();
	}

	// Make boids appear on other side of screen
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
