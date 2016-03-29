using UnityEngine;
using System.Collections;

public class PanelBehavior : MonoBehaviour {

	Animator animator;

	// Use this for initialization
	void Start () {
		animator = GetComponent<Animator> ();
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public void setState(int state)
	{
		if(state == 0)
		{
			animator.SetBool ("ChangeState", false);
		}
		else
		{
			animator.SetBool ("ChangeState", true);
		}	             
	}

	public void PauseGame()
	{
		Game.instance.State = 0;
	}
}
