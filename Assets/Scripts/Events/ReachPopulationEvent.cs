using UnityEngine;
using System.Collections;

public class ReachPopulationEvent : EventClass 
{
    public int PopulationGoal = 300;

    public override bool ConditionMet()
    {
        return World.Instance.AgentCount > this.PopulationGoal;
    }

    /// <summary>
    /// Return string to update gui text with
    /// </summary>
    /// <returns></returns>
    public override string UpdateGUI()
    {
        return World.Instance.AgentCount + " / " + this.PopulationGoal;
    }

    /// <summary>
    /// Passed
    /// </summary>
    public override void Init()
    {

    }

    /// <summary>
    /// Passed
    /// </summary>
    public override void AddToCount()
    {

    }

    /// <summary>
    /// Passed
    /// </summary>
    public override void ReduceFromCount()
    {

    }
}
