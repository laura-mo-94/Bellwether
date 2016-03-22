using UnityEngine;
using System.Collections;

public class Ripple : MonoBehaviour {

	public Color lineColor = Color.white;
	public int degreesPerSegment = 8;
	public float growthRate = 0.051f;
	public float fadeRate = 0.01f;
	public float z = 0f;

	LineRenderer line;
	CircleCollider2D col;

	private float radialScale;
	private float opacity;
	private int vertexCount;

	public Ripple()
	{

	}

	// Use this for initialization
	public void Start() {
		line = GetComponent<LineRenderer> ();
		line.SetVertexCount (0);
		vertexCount = 360 / degreesPerSegment + 1;
		col = GetComponent<CircleCollider2D> ();
	}
	
	// Update is called once per frame
	void Update () {

	}

	public void StartRipple()
	{
		radialScale = 1f;
		opacity = 1f;
		line.SetVertexCount (vertexCount);
		StartCoroutine ("Pulse");
	}

	public IEnumerator Pulse()
	{
		while (opacity > 0f)
		{
			Debug.Log ("Hi");
			DrawVertexes();
			col.radius = radialScale;
			SetColors();
			radialScale += growthRate;
			opacity -= fadeRate;
			yield return null;
		}

		line.SetVertexCount(0);
		Touch.instance.rippleFinished (this);
	}

	void DrawVertexes()
	{
		float angle = 0;
		for (int i = 0; i < vertexCount; ++i)
		{
			float rads = Mathf.Deg2Rad * angle;
			
			Vector3 pos = new Vector3(Mathf.Sin(rads), Mathf.Cos(rads),z);
			line.SetPosition(i, pos*radialScale + transform.position);
			angle += degreesPerSegment;
		}
	}
	
	void SetColors()
	{
		lineColor.a = opacity;
		line.SetColors (lineColor, lineColor);
	}
}
