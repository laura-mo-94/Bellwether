using UnityEngine;
using System.Collections.Generic;

public class World : MonoBehaviour
{
    public GameObject AgentPrefab;
    public int StartNumberOfAgents;

	public int MaxNumberOfAgents = 150;
	public int TimeScale;

    public Vector2 ScreenBounds;
    public float SpawnRadius;

    [Header ("Number Of Agents Running")]
    public int AgentCount = 0;

	private int totalAmount = 0;

	private int killedCount = 0;
	private int generationCount = 0;

	[Header ("Generation Checker")]
	public float Generation = 0;

	[HideInInspector]
	public AgentConfig Config;
    
    private static World instance = null;
    public static World Instance
    {
        get
        {
            if (instance == null)
            {
                instance = GameObject.FindObjectOfType<World>();
            }
            return instance;
        }
    }

    void Start()
    {
		this.Generation = 0;
        this.AgentCount = 0;
		this.Config = this.gameObject.GetComponent<AgentConfig>();
        this.Spawn(AgentPrefab, this.StartNumberOfAgents);
    }

    /// <summary>
    /// Spawn Agents given a size and prefab
    /// </summary>
    /// <param name="position"></param>
    /// <param name="n"></param>
    public void Spawn(GameObject agentPrefab, int n)
    {
        // loop through n agents to spawn
        for (int i = 0; i < n; ++i)
        {
            // Increment agent count
            ++this.AgentCount;

            // Instantiate agent
            GameObject agent = Instantiate(agentPrefab, new Vector3(Random.Range(-this.SpawnRadius, this.SpawnRadius), Random.Range(-this.SpawnRadius, this.SpawnRadius), 0), Quaternion.identity) as GameObject;

            // Randomize configuration
            agent.GetComponent<AgentConfig>().RandomizeSelf();
        }
    }

	public void AddCheckToGeneration(int generation)
	{
		++this.killedCount;
		this.generationCount += generation;

		this.Generation = (float) this.generationCount / ((float) this.killedCount + (float) this.AgentCount);
	}

    /// <summary>
    /// Wrap around to keep agent in screen
    /// </summary>
    /// <param name="velocity"></param>
    /// <param name="min"></param>
    /// <param name="max"></param>
    public Vector3 WrapAround(Vector3 pos)
    {
        pos.x = this.wrapAroundFloat(pos.x, -ScreenBounds.x, ScreenBounds.x);
        pos.y = this.wrapAroundFloat(pos.y, -ScreenBounds.y, ScreenBounds.y);

        return pos;
    }

    /// <summary>
    /// Increments the agent count.
    /// </summary>
    public void IncrementAgentCount()
    {
		++this.totalAmount;
        ++this.AgentCount;
    }

    /// <summary>
    /// Decrements the agent count and checks total count
    /// </summary>
    public void DecrementAgentCount()
    {
        // Decrement
        --this.AgentCount;

        // Check count
        if(this.AgentCount <= 0)
        {
            Debug.Log("GAME OVER!!!!!!!");
        }
    }

    private float wrapAroundFloat(float val, float min, float max)
    {
        if (val > max)
        {
            val = min;
        }
        else if (val < min)
        {
            val = max;
        }

        return val;
    }

	/// <summary>
	/// Agents whehter or not the agent can make
	/// </summary>
	/// <returns><c>true</c>, if max number of agents, <c>false</c> otherwise.</returns>
	public bool AgentCanMate()
	{
		return (this.AgentCount < this.MaxNumberOfAgents);
	}	
}
