using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System;

// inherit from eventClass?????
public class EventManager : MonoBehaviour
{
    const int maxRuns = 10;
    public UnityEngine.UI.Text GUIText;

    private EventClass currentEvent = null;
    private UnityEngine.Object[] events;

    // Singleton
    private static EventManager instance = null;
    public static EventManager Instance
    {
        get
        {
            if (instance == null)
            {
                instance = GameObject.FindObjectOfType<EventManager>();
            }
            return instance;
        }
    }



	// Use this for initialization
	void Awake () 
    {
        // Initalize array of events
        this.events = Resources.LoadAll("Prefabs/Events");

        // Create event for user
        this.CreateEvent();
	}

    void Update()
    {
        // update text of gui
        this.GUIText.text = "HI!";
        this.GUIText.text = this.currentEvent.UpdateGUI();

        // Check if goal was met
        if (this.currentEvent.ConditionMet())
        {
            this.CreateEvent();
        }
    }

    /// <summary>
    /// Create an event that will be a goal for the user to raech
    /// </summary>
    public void CreateEvent()
    {
        int currentRuns = 0;
        int index = 0;
        bool validIndex = false;

        // loop until valid index found
        while (!validIndex || currentRuns > maxRuns)
        {
            // increment current runs
            ++currentRuns;

            // get random index
            index = UnityEngine.Random.Range(0, this.events.Length);

            // check index
            if (this.currentEvent == null)
            {
                validIndex = true;
            }
            else if (!this.currentEvent.GetType().Equals(this.events[index].GetType()))
            {
                validIndex = true;

                // Destroy event
                this.currentEvent.KillSelf();

            }
        }

        // Instantiate empty game object
        GameObject obj = GameObject.Instantiate(this.events[index] as GameObject);

        // get the event class component
        this.currentEvent = obj.GetComponent<EventClass>();

        // set parent for debugging
        obj.transform.parent = this.transform;
    }    
}
