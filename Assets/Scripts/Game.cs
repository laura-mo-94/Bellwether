using UnityEngine;
using System.Collections;

public class Game : MonoBehaviour 
{
	
	public static Game instance;
	int state = 0;

	// Use this for initialization
	void Start () 
    {
		instance = this;
	}

	public int State
	{
		get 
        {
			return state;
		}
		set
        {
			state = value;
		}
	}
}
