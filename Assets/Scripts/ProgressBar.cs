using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class ProgressBar : MonoBehaviour {

	public Slider progressSlider;
	public Text progressText;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public void setProgressSliderValue(float value)
	{
		progressSlider.value = value;
	}

	public void setProgressSliderText(string text)
	{
		progressText.text = text;
	}
}
