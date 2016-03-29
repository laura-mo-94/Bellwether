using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class EvolutionPointManager : MonoBehaviour {

	public static EvolutionPointManager instance;
	public float startingPoints;

	public Text currentPointLabel;
	public Slider cohesion;
	public Slider seperation;
	public Slider alignment;
	public Slider wander;
	public Slider avoid;

	float currentPoints;

	// Use this for initialization
	void Start () {
		instance = this;
		currentPoints = startingPoints;
		currentPointLabel.text = currentPoints + "";
	}

	public float getPointsLeft()
	{
		return currentPoints;
	}

	public float allocatePoints(float points)
	{
		if(currentPoints + points >= 0 && (currentPoints + points) < startingPoints)
		{
			currentPoints = currentPoints + points;
			currentPointLabel.text = currentPoints + "";
			return 0;
		}
		else if((currentPoints + points) >= startingPoints)
		{
			currentPoints = startingPoints;
			currentPointLabel.text = currentPoints + "";
			return 0;
		}
		else
		{
			float res = Mathf.Abs(points) - currentPoints;
			currentPoints = 0;
			currentPointLabel.text = currentPoints + "";
			return res;
		}
	}

	public void configureFlock()
	{
		Debug.Log ("hey");
		AgentConfig config = World.Instance.Config;
		config.SeparationWeight = seperation.value;
		config.CohesionWeight = cohesion.value;
		config.AllignmentWeight = alignment.value;
		config.WanderWeight = wander.value;
		config.AvoidWeight = avoid.value;
		Debug.Log ("ho");
		Game.instance.State = 1;
		Debug.Log (Game.instance.State);
	}
}
