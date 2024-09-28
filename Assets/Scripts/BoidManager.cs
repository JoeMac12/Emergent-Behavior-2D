using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoidManager : MonoBehaviour
{
	public Boid boidPrefab;
	public int boidCount = 100;
	public Boid[] boids;

	void Start()
	{
		boids = new Boid[boidCount];
		for (int i = 0; i < boidCount; i++)
		{
			Boid boid = Instantiate(boidPrefab, Random.insideUnitCircle * 5, Quaternion.identity);
			boids[i] = boid;
		}
	}
}
