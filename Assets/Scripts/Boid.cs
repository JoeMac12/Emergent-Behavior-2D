using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boid : MonoBehaviour
{
	public Vector2 velocity;
	public float radius = 2f;
	public float predatorRadius = 5f;

	BoidManager boidManager;
	private Predator[] predators;

	// Set random speed at start
	void Start()
	{
		boidManager = FindObjectOfType<BoidManager>();
		predators = FindObjectsOfType<Predator>();
		velocity = Random.insideUnitCircle.normalized * boidManager.maxSpeed;
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

		// Avoid the predator grrr
		Vector2 avoidance = Vector2.zero;

		foreach (Predator predator in predators)
		{
			float predatorDistance = Vector2.Distance(transform.position, predator.transform.position);
			if (predatorDistance < predatorRadius)
			{
				Vector2 fleeDirection = (Vector2)(transform.position - predator.transform.position);
				avoidance += fleeDirection.normalized / predatorDistance;
			}
		}

		avoidance = avoidance.normalized * boidManager.maxSpeed - velocity;

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
			alignment = (alignment / total).normalized * boidManager.maxSpeed - velocity;
			cohesion = ((cohesion / total) - (Vector2)transform.position).normalized * boidManager.maxSpeed - velocity;
			separation = (separation / total).normalized * boidManager.maxSpeed - velocity;

			acceleration += (alignment * boidManager.alignmentWeight) +
							(cohesion * boidManager.cohesionWeight) +
							(separation * boidManager.separationWeight) +
							(avoidance * boidManager.avoidanceWeight);
		}

		// Update vel and pos
		velocity += acceleration * Time.deltaTime;
		velocity = Vector2.ClampMagnitude(velocity, boidManager.maxSpeed);
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
