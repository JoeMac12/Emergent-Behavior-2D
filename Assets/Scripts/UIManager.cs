using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
	public Slider alignmentSlider;
	public Slider cohesionSlider;
	public Slider separationSlider;

	public BoidManager boidManager;

	void Start()
	{
		alignmentSlider.value = 1f;
		cohesionSlider.value = 1f;
		separationSlider.value = 1f;
	}

	// Live settings
	void Update()
	{
		boidManager.alignmentWeight = alignmentSlider.value;
		boidManager.cohesionWeight = cohesionSlider.value;
		boidManager.separationWeight = separationSlider.value;
	}
}
