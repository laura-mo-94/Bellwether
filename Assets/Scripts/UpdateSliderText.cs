using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class UpdateSliderText : MonoBehaviour {

	public Slider slider;
	public bool hasLimits;

	Text label;
	string type;
	float lastValue;

	// Use this for initialization
	void Start () {
		label = GetComponent<Text> ();
		type = label.text;
		label.text = type + ": 0";
		lastValue = slider.value;
		slider.onValueChanged.AddListener (delegate {updateInformation (slider.value);});
	}

	void updateInformation(float val)
	{
		if (hasLimits) {
			float dif = EvolutionPointManager.instance.allocatePoints (lastValue - val);

			if (dif != 0) {
				label.text = type + ": " + (val - dif);
				lastValue = (val - dif);
			} else {
				label.text = type + ": " + val;
				lastValue = val;
			}

			slider.value = lastValue;
		}
		else 
		{
			label.text = type + ": " + val;

		}

	}
}
