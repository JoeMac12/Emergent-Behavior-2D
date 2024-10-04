using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
	public Slider alignmentSlider;
	public Slider cohesionSlider;
	public Slider separationSlider;
	public Slider avoidanceSlider;
	public Slider boidCountSlider;
	public Slider boidMaxSpeedSlider;

	public BoidManager boidManager;

	void Start()
	{
		alignmentSlider.value = 1f;
		cohesionSlider.value = 1f;
		separationSlider.value = 1f;
		avoidanceSlider.value = boidManager.avoidanceWeight;
		boidCountSlider.value = boidManager.boidCount;
		boidMaxSpeedSlider.value = boidManager.maxSpeed;
	}

	// Live settings
	void Update()
	{
		boidManager.alignmentWeight = alignmentSlider.value;
		boidManager.cohesionWeight = cohesionSlider.value;
		boidManager.separationWeight = separationSlider.value;
		boidManager.avoidanceWeight = avoidanceSlider.value;

		int newBoidCount = (int)boidCountSlider.value;
		if (newBoidCount != boidManager.boidCount)
		{
			boidManager.boidCount = newBoidCount;
			boidManager.SpawnBoids(newBoidCount);
		}

		boidManager.maxSpeed = boidMaxSpeedSlider.value;
	}
}
