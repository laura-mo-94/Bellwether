using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Touch : MonoBehaviour {

	public int maxRipples;
	public GameObject rippleObject;

	public static Touch instance;
	List<Ripple> ripples;
	List<Ripple> inUse;
	Camera cam;

	// Use this for initialization
	void Start () {
		instance = this;
		ripples = new List<Ripple> ();
		inUse = new List<Ripple> ();

		cam = Camera.main;

		for(int i = 0; i < maxRipples; ++i)
		{
			GameObject ripOb = GameObject.Instantiate(rippleObject);

			ripples.Add(ripOb.GetComponent<Ripple>());
		}
	}
	
	// Update is called once per frame
	void Update () {
		if (Input.GetMouseButtonUp(0))
		{
			if(ripples.Count > 0)
			{
				Ripple newRip = ripples[0];
				ripples.RemoveAt(0);
				newRip.transform.position = (Vector2)cam.ScreenToWorldPoint(Input.mousePosition);
				newRip.StartRipple();
			}
		}
	}

	public void rippleFinished(Ripple r)
	{
		ripples.Add (r);
		inUse.Remove (r);
	}
}
