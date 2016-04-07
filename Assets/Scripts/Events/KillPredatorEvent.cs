using UnityEngine;
using System.Collections;

public class KillPredatorEvent : EventClass
{


    public override bool ConditionMet() 
    {
        return false;
    }

    /// <summary>
    /// Return string to update gui text with
    /// </summary>
    /// <returns></returns>
    public override string UpdateGUI()
    {
        return "";
    }

    public override void Init()
    { 
    
    }

    public override void AddToCount()
    { 
    
    }

    public override void ReduceFromCount()
    { 
    
    }
}
