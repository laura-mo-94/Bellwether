using UnityEngine;
using System.Collections;

public abstract class EventClass : MonoBehaviour
{
    // Abstract method to be implemented
    public abstract bool ConditionMet();
    public abstract void Init();
    public abstract void AddToCount();
    public abstract void ReduceFromCount();
    public abstract string UpdateGUI();

    /// <summary>
    /// Destroy this event
    /// </summary>
    public virtual void KillSelf()
    {
        Destroy(this.gameObject);
    }
}
