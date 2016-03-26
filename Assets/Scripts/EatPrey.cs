﻿using UnityEngine;
using System.Collections;

public class EatPrey : MonoBehaviour 
{
	/// <summary>
	/// Raises the collision enter2d event and eat the bird
	/// </summary>
	/// <param name="col">Col.</param>
	void OnCollisionEnter2D(Collision2D col) 
	{
		// Check tag
		if(col.collider.transform.CompareTag("Agent"))
		{
			// generation count
			World.Instance.AddCheckToGeneration(col.gameObject.GetComponent<MateWithAgent>().GetGeneration());

			// Eat prey
			Destroy(col.gameObject);
		}
	}
}
