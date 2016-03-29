using UnityEngine;
using System.Collections.Generic;

public class Agent : MonoBehaviour 
{
    public Vector2 velocity;

	// Move towards wehn wondering
	private Vector3 WanderTarget;
    
    // Rigid body
    private Rigidbody2D rb;
    
    private Vector2 acceleration;
	protected LayerMask predatorLayer;
	protected LayerMask agentLayer;
	protected AgentConfig config;

	protected virtual void setLayers()
	{
		// Get predator layer
		this.predatorLayer = LayerMask.GetMask(new string[] {"Predator"});
		
		// Get agent layer
		this.agentLayer = LayerMask.GetMask(new string[] {"Agent"});
	}

    /// <summary>
    /// Initialize with basic info
    /// </summary>
    void Start ()
    {
        // Start with random velocity
        this.velocity = new Vector2(Random.Range(-2, 2), Random.Range(-2, 2));

		if(velocity.x == 0)
		{
			this.velocity.x = 3;
			this.velocity.y = -3;
		}

		// set layers
		this.setLayers();

		// Get configuration
		this.config = this.GetComponent<AgentConfig>();
	}
	
    /// <summary>
    /// Update movement of agent
    /// </summary>
	void FixedUpdate ()
    {
		if (Game.instance.State != 0) 
		{
			float additionalVelocity = 0;
			float additionalAcceleration = 0;

			// Check if this is a predator or prey
			if (this.gameObject.CompareTag ("Agent")) {
				additionalVelocity = World.Instance.Config.MaxVelocity;
				additionalAcceleration = World.Instance.Config.MaxAcceleration;
			}

			// Update acceleration
			this.acceleration = Vector2.ClampMagnitude (this.Combine (), (this.config.MaxAcceleration + additionalAcceleration));

			// Euler Forward Integration
			this.velocity = Vector2.ClampMagnitude (this.velocity + this.acceleration * Time.deltaTime, (this.config.MaxVelocity + additionalVelocity));

			// Set new position
			this.transform.position = this.transform.position + (Vector3)(this.velocity * Time.deltaTime);

			// Keep agent in world bounds
			this.transform.position = World.Instance.WrapAround (this.transform.position);
	        
			// set allignment
			if (this.velocity.magnitude > 0) {
				// get angle towards direction and convert to degrees
				float angle = Mathf.Atan2 (this.velocity.y, this.velocity.x) * Mathf.Rad2Deg - 90;

				// set rotation
				transform.rotation = Quaternion.AngleAxis (angle, Vector3.forward);
			}
		}
	}

    /// <summary>
    /// Go to center of neighbors
    /// </summary>
    /// <returns>Center Point</returns>
    public Vector2 Cohesion(ref Collider2D[] neighbors)
    {
        // Cohesion behaavior
        Vector3 result = new Vector3();

        // check if neighbors is full or not
        if (neighbors.Length > 0)
        {
			// Num agents
			int agentCount = 0;

            for (int i = 0; i < neighbors.Length; ++i)
            {
                result += neighbors[i].transform.position;
                ++agentCount;
            }

            // Divide by count
            if (agentCount > 0)
            {
                result /= agentCount;
            }
            
            // Look center
            result = result - this.transform.position;

            // normalize vector
            result.Normalize();
        }

        // Return result
        return result;
    }

    /// <summary>
    /// Move away from neighbors
    /// </summary>
    /// <returns></returns>
    public Vector2 Separation(ref Collider2D[] neighbors)
    {
        // Separation result
        Vector2 result = new Vector3();

        // check if neighbors is full or not
        for (int i = 0; i < neighbors.Length; ++i)
        {
            Vector3 towardsMe = this.transform.position - neighbors[i].transform.position;

            // Contribution depends on distance
            if (towardsMe.magnitude > 0)
            {
                result = towardsMe.normalized / towardsMe.magnitude;
            }

            // Normalize
            result.Normalize();
        }

        // return result
        return result;
    }

    /// <summary>
    /// Rotate in direction of movement
    /// </summary>
    /// <returns></returns>
    public Vector2 Allignment(ref Collider2D[] neighbors)
    {
        Vector2 result = new Vector2();

        // check if neighbors is full or not
        if (neighbors.Length > 0)
        {
            for (int i = 0; i < neighbors.Length; ++i)
            {
                result += neighbors[i].gameObject.GetComponent<Agent>().velocity;
            }

            // Nomalize vector
            result.Normalize();
        }

        // return result
        return result;
    }

	/// <summary>
	/// Flee from all enemies
	/// </summary>
	public Vector2 AvoidEnemies()
	{
		Vector2 result = new Vector3();
		
		Collider2D[] enemies = Physics2D.OverlapCircleAll(this.transform.position, this.config.SearchRadius + World.Instance.Config.SearchRadius, this.predatorLayer);

		for (int i = 0; i < enemies.Length; ++i)
		{
		//	Debug.Log (enemies.Length);
			result += this.Flee(enemies[i].transform.position);
		}
		
		return result;
	}

	/// <summary>
	/// Smooth out movement
	/// </summary>
	/// <returns></returns>
	public Vector2 Wander()
	{
		float jitter = (this.config.Jitter + World.Instance.Config.Jitter) * Time.deltaTime;
		
		// 
		this.WanderTarget += new Vector3(this.randomBinomial() * jitter, this.randomBinomial() * jitter, 0);
		
		this.WanderTarget.Normalize();
		this.WanderTarget *= this.config.SearchRadius;
		Vector3 targetInLocalSpace = this.WanderTarget + new Vector3(0, 0, this.config.WanderDistanceRadius + World.Instance.Config.WanderDistanceRadius);
		Vector3 targetInWorldSpace = this.transform.TransformPoint(targetInLocalSpace);
		return (targetInWorldSpace - this.transform.position).normalized;
	}

    /// <summary>
    /// Use Allignment, Cohesion, and Separation to define behavior with diferent proportions based on importance
    /// </summary>
    /// <returns>Vector with correct behavior</returns>
    public virtual Vector2 Combine()
    {
		// Get all neighbors
		Collider2D[] neighbors = Physics2D.OverlapCircleAll(this.transform.position, this.config.SearchRadius + World.Instance.Config.SearchRadius, this.gameObject.layer);

		return (this.config.CohesionWeight +  World.Instance.Config.CohesionWeight) * this.Cohesion(ref neighbors) 
             + (this.config.SeparationWeight +  World.Instance.Config.CohesionWeight) * this.Separation(ref neighbors)
             + (this.config.AllignmentWeight +  World.Instance.Config.AllignmentWeight) * this.Allignment(ref neighbors)
//             + (this.config.WanderWeight + World.Instance.Config.WanderWeight) * this.Wander()
			 + (this.config.AvoidWeight + World.Instance.Config.AvoidWeight) * this.AvoidEnemies();
    }

    /// <summary>
    /// Check if agent is in field of fiew for this agent
    /// </summary>
    /// <param name="agent"></param>
    /// <returns></returns>
    public bool InFieldOfView(Vector3 agent)
    {
        return Vector2.Angle(this.velocity, agent - this.transform.position) <= this.config.MaxFieldOfViewAngle;
    } 

	/// <summary>
	/// Randoms binomial with higher likelihood of 0
	/// </summary>
	/// <returns>The binomial.</returns>
    private float randomBinomial()
    {
        return Random.Range(0f, 1f) - Random.Range(0f, 1f);
    }

    /// <summary>
    /// Run opposite direction from the target
    /// </summary>
    /// <param name="targ"></param>
    /// <returns></returns>
    public Vector2 Flee(Vector3 targ)
    {
		Vector2 desiredVel = (this.transform.position - targ).normalized * (this.config.MaxVelocity +  World.Instance.Config.MaxVelocity);
        return desiredVel - this.velocity;
    }


}