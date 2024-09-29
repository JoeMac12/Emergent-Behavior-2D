using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoidManager : MonoBehaviour
{
	public Boid boidPrefab;
	public int boidCount = 100;
	public float maxSpeed = 20f;

	public Boid[] boids;

	// For UI settings
	[HideInInspector]
	public float alignmentWeight = 1f;
	[HideInInspector]
	public float cohesionWeight = 1f;
	[HideInInspector]
	public float separationWeight = 1f;
	[HideInInspector]
	public float avoidanceWeight = 1.5f;

	// Spawn boids
	void Start()
	{
		SpawnBoids(boidCount);
	}

	// Cell settings
	public float cellSize = 1f; // Should be big enough but not too laggy?
	private Dictionary<Vector2Int, List<Boid>> grid = new Dictionary<Vector2Int, List<Boid>>();

	void Update()
	{
		// Clear the grid
		grid.Clear();

		// Populate grid with boids inside
		foreach (Boid boid in boids)
		{
			Vector2Int cell = GetCell(boid.transform.position);
			if (!grid.ContainsKey(cell))
			{
				grid[cell] = new List<Boid>();
			}
			grid[cell].Add(boid);
		}
	}

	// Find nearby boids using cell method for optimization
	public List<Boid> GetNearbyBoids(Boid boid)
	{
		List<Boid> nearbyBoids = new List<Boid>();
		Vector2Int cell = GetCell(boid.transform.position);

		// Check nearby cells
		for (int x = -1; x <=1; x++)
		{
			for (int y = -1; y <=1; y++)
			{
				Vector2Int neighborCell = cell + new Vector2Int(x, y);
				if (grid.ContainsKey(neighborCell))
				{
					nearbyBoids.AddRange(grid[neighborCell]);
				}
			}
		}
		return nearbyBoids;
	}

	// Convert world pos to grid cell
	Vector2Int GetCell(Vector2 position)
	{
		return new Vector2Int(
			Mathf.FloorToInt(position.x / cellSize),
			Mathf.FloorToInt(position.y / cellSize)
		);
	}

	// Does what it says
	public void SpawnBoids(int count)
	{
		// Remove existing boids
		if (boids != null)
		{
			foreach (Boid boid in boids)
			{
				if (boid != null)
					Destroy(boid.gameObject);
			}
		}

		boids = new Boid[count];
		for (int i = 0; i < count; i++)
		{
			Boid boid = Instantiate(boidPrefab, Random.insideUnitCircle * 5, Quaternion.identity);
			boids[i] = boid;
		}
	}
}
