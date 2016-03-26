using UnityEngine;
using System.Collections;

public class Predator : Agent
{
    void Start()
    {
        // Start with random velocity
        this.velocity = new Vector2(Random.Range(-5, 5), Random.Range(-5, 5));

        // Get configuration
        this.config = this.GetComponent<AgentConfig>();

        // set layers
        base.setLayers();
    }

    /// <summary>
    /// Use Allignment, Cohesion, and Separation to define behavior with diferent proportions based on importance
    /// </summary>
    /// <returns>Vector with correct behavior</returns>
    public override Vector2 Combine()
    {
		// Get all prey
		Collider2D[] prey = Physics2D.OverlapCircleAll(this.transform.position, this.config.SearchRadius, base.agentLayer);

		// Get all neighbors
		Collider2D[] neighbors = Physics2D.OverlapCircleAll(this.transform.position, this.config.SearchRadius, base.predatorLayer);

        return base.config.CohesionWeight * base.Cohesion(ref prey)
             + base.config.WanderWeight * base.Wander()
             + base.config.SeparationWeight * base.Separation(ref neighbors);
    }
}
