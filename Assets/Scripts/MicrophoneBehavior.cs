﻿using UnityEngine;
using System.Collections;

public class MicrophoneBehavior : MonoBehaviour {

	AudioSource aud;
	int minFrequency;
	int maxFrequency;

	// Use this for initialization
	void Start () {
		aud = gameObject.AddComponent<AudioSource>();
		foreach (string device in Microphone.devices) {
			Debug.Log("Name: " + device);
		}
		Microphone.GetDeviceCaps(null, out minFrequency, out maxFrequency);
		Debug.Log("Default microphone minFrequency = " + minFrequency.ToString() + " maxFrequency = " + maxFrequency.ToString() );
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public void startRecording()
	{
		int recordingFrequency = 44100;
		if (maxFrequency != 0)
			recordingFrequency = maxFrequency;
		aud.clip = Microphone.Start(null, false, 10, recordingFrequency);
	}

	public void endRecording()
	{
		Microphone.End(null);
	}

	public void playRecordedSound()
	{
		aud.Play();
	}
}