using UnityEngine;
using System.Collections;

public class AgentConfig : MonoBehaviour 
{
	public const float MaxRadius = 20;
	public const float MaxWeight = 300;
	public const float MaxSpeed = 10;
	public const float MaxLifeTime = 5;

    // TODO: add line breaks in inspector and create better names
	[Header("Find Nearby Radius")]
	public float SearchRadius;

	[Header("Weights for Given Behavior")]
	public float CohesionWeight;
	public float SeparationWeight;
	public float AllignmentWeight;
	public float WanderWeight;
	public float AvoidWeight;

	[Header ("Physics")]
	public float MaxAcceleration;
	public float MaxVelocity;

    [Header ("Smoothing Movement")]
	public float Jitter;
	public float WanderDistanceRadius;

	[Header ("Life Time")]
	public float LifeTime;

	[Header("Vision")]
	[Tooltip("Unused")]
	public float MaxFieldOfViewAngle = 180;

	/// <summary>
	/// Randomizes the paramaters
	/// </summary>
	public void RandomizeSelf()
	{
		// Radius
		this.SearchRadius = Random.Range(0, AgentConfig.MaxRadius);

		// Weights
		this.CohesionWeight = Random.Range(0, AgentConfig.MaxWeight);
		this.SeparationWeight = Random.Range(0, AgentConfig.MaxWeight);
		this.AllignmentWeight = Random.Range(0, AgentConfig.MaxWeight);
		this.WanderWeight = Random.Range(0, AgentConfig.MaxWeight);
		this.AvoidWeight = Random.Range(0, AgentConfig.MaxWeight);

		// Smooth Movement
		this.Jitter = Random.Range(0, AgentConfig.MaxWeight);
		this.WanderDistanceRadius = Random.Range(0, AgentConfig.MaxRadius);

		// Life time
		this.LifeTime = Random.Range(1, AgentConfig.MaxLifeTime);

		// Speed
		this.MaxAcceleration = Random.Range(1, AgentConfig.MaxSpeed);
		this.MaxVelocity = Random.Range(1, AgentConfig.MaxSpeed);
	}

	public void SetConfig(AgentConfig newConfig)
	{
		// Get fields
		System.Reflection.FieldInfo[] fields = newConfig.GetType().GetFields();
		
		// loop through fields
		for(int i = 0; i < fields.Length; ++i)
		{
			// Set value for field
			fields[i].SetValue(this, fields[i].GetValue(newConfig));
		}
	}
}
