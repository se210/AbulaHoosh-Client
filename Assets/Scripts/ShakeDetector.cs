using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class ShakeDetector : MonoBehaviour {

	public float accelerometerUpdateInterval = 1.0f / 60.0f;
	// The greater the value of LowPassKernelWidthInSeconds, the slower the filtered value will converge towards current input sample (and vice versa).
	public float lowPassKernelWidthInSeconds = 1.0f;
	// This next parameter is initialized to 2.0 per Apple's recommendation, or at least according to Brady! ;)
	public float shakeDetectionThreshold = 2.0f;

	public Transform accelBox;
	public Text shakeCountText;

	float lowPassFilterFactor;
	Vector3 lowPassValue = Vector3.zero;
	Vector3 acceleration;
	Vector3 deltaAcceleration;
	int shakeCount = 0;
	int shakeCountPerFrame = 0;

	// Use this for initialization
	void Start () {
		shakeDetectionThreshold *= shakeDetectionThreshold;
		lowPassValue = Input.acceleration;
		lowPassFilterFactor = accelerometerUpdateInterval / lowPassKernelWidthInSeconds;
	}
	
	// Update is called once per frame
	void Update () {
		shakeCountText.text = string.Format("Shake count: {0:D}", shakeCount);
		float shakeRate = shakeCountPerFrame/Time.deltaTime;
		Debug.Log(string.Format("Shake rate = {0:F}", shakeRate));
		AHClient.singleton.sendShakeRateToServer(shakeRate, shakeCountPerFrame);
		shakeCountPerFrame = 0;
	}

	void FixedUpdate () {
		acceleration = Input.acceleration;
		lowPassValue = Vector3.Lerp(lowPassValue, acceleration, lowPassFilterFactor);
		deltaAcceleration = acceleration - lowPassValue;
		//		Debug.Log(string.Format("deltaAcceleration.sqrMagnitude = {0}", deltaAcceleration.sqrMagnitude));
		if (deltaAcceleration.sqrMagnitude >= shakeDetectionThreshold)
		{
			// Perform your "shaking actions" here, with suitable guards in the if check above, if necessary to not, to not fire again if they're already being performed.
			Debug.Log("Shake event detected at time "+Time.time);
			shakeCount++;
			shakeCountPerFrame++;
		}

		accelBox.localPosition = new Vector3(acceleration.x * Screen.width / 2.0f, acceleration.y * Screen.height / 2.0f);
	}
}
