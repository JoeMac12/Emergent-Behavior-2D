using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Sets the screen bounds for when boids go off screen (Might make them avoid boundaries later instead)
public class ScreenBounds : MonoBehaviour
{
	public static float leftLimit;
	public static float rightLimit;
	public static float bottomLimit;
	public static float topLimit;

	void Update()
	{
		UpdateBounds();
	}

	// Calculate screen based on camera pos
	void UpdateBounds()
	{
		Camera cam = Camera.main;

		float height = 2f * cam.orthographicSize;
		float width = height * cam.aspect;

		Vector3 camPosition = cam.transform.position;

		leftLimit = camPosition.x - (width / 2);
		rightLimit = camPosition.x + (width / 2);
		bottomLimit = camPosition.y - (height / 2);
		topLimit = camPosition.y + (height / 2);
	}
}
