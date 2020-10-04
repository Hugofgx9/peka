using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BeatCounter : MonoBehaviour
{
	public int Base;
	public int Step;
	public float BPM;
	public int CurrentStep = 1;
	public int CurrentMeasure;

	private float interval;
	private float nextTime;

	public AudioSource LeadsAudio;
	private AudioManager AudioManager;

	void Start () {
		AudioManager = gameObject.GetComponent<AudioManager>();
		StartMetronome();
	}

	public void StartMetronome()
	{
		StopCoroutine("DoTick");
		CurrentStep = 0; 
		var multiplier = Base / 4f;
		var tmpInterval = 60f / BPM;
		interval = tmpInterval / multiplier; 
		nextTime = Time.time; // set the relative time to now
		StartCoroutine("DoTick"); 
	}

	IEnumerator DoTick() // yield methods return IEnumerator
	{
		for (; ; )
		{
			if (CurrentStep == 1 && CurrentMeasure == 0) {
				AudioManager.StartTrack2();
			}
			else if (CurrentMeasure == 8 && CurrentStep == 1) {
				LeadsAudio.Play();
			}
			else if (CurrentMeasure == 16 && CurrentStep == 1) {
				AudioManager.StartLead2();
			}

			// do something with this beat
			nextTime += interval; // add interval to our relative time
			yield return new WaitForSeconds(nextTime - Time.time); // wait for the difference delta between now and expected next time of hit
			CurrentStep++;
			if (CurrentStep > Step)
			{
				CurrentStep = 1;
				CurrentMeasure++;
			}
		}
	}
}


 
