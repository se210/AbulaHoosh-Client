using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class MicrophoneBehavior : MonoBehaviour {

	AudioSource aud;
	int minFrequency;
	int maxFrequency;
	public string filePath;
	public ProgressBar progressBar;

	int recordTime = 3;

	// Use this for initialization
	void Start () {
		aud = gameObject.AddComponent<AudioSource>();
		filePath = Application.persistentDataPath + "/voice.wav";
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
		aud.clip = Microphone.Start(null, false, recordTime, recordingFrequency);
		StartCoroutine(showRecordingStatus());
	}

	IEnumerator showRecordingStatus()
	{
		progressBar.setProgressSliderText("Recording team name...");
		progressBar.gameObject.SetActive(true);
		System.Diagnostics.Stopwatch stopwatch = new System.Diagnostics.Stopwatch();
		stopwatch.Start();
		long elapsedTime = stopwatch.ElapsedMilliseconds;
		while (elapsedTime < recordTime * 1000)
		{
			progressBar.setProgressSliderValue((float)elapsedTime / (recordTime * 1000));
			yield return new WaitForEndOfFrame();
			elapsedTime = stopwatch.ElapsedMilliseconds;
		}
		stopwatch.Stop();
		progressBar.gameObject.SetActive(false);
		endRecording();
		AHClient.singleton.sendVoiceToServer();
	}

	public void endRecording()
	{
		Microphone.End(null);
		SavWav.Save(filePath, aud.clip);
	}

	public void playRecordedSound()
	{
//		aud.Play();
		WWW www = new WWW("file://"+filePath);
		aud.clip = www.audioClip;
		while(aud.clip.loadState != AudioDataLoadState.Loaded);
		aud.Play();
	}
}
