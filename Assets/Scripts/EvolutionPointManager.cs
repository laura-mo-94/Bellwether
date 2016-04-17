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
	public Slider timeScale;

	public float Multiplier = 50;

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
		World.Instance.Config.SeparationWeight = seperation.value * this.Multiplier;
		World.Instance.Config.CohesionWeight = cohesion.value * this.Multiplier;
		World.Instance.Config.AllignmentWeight = alignment.value * this.Multiplier;
		World.Instance.Config.WanderWeight = wander.value * this.Multiplier;
		World.Instance.Config.AvoidWeight = avoid.value * this.Multiplier;
		World.Instance.TimeScale = (int) timeScale.value;
		Game.instance.State = 1;
	}
}
