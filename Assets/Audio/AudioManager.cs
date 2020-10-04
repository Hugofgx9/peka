using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class AudioManager : MonoBehaviour
{

		[SerializeField] private AudioMixer generalMixer;
		[SerializeField] private ThirdPersonMovement mainCharacter;

		//sawCutoff
		float sawCutoff = 200f;
		float SmoothVelocitySaw;

		//leadCutoff
		float leadCutoff = 3500f;
		float SmoothVelocityLead;

		//masterCutoff
		float masterCutoff = 20000f;
		float SmoothVelocityMaster;

		//masterHipass
		float masterHiPass = 10f;
		float SmoothVelocityMasterHiPass;


		//reverb Send
		float reverbVolume = 4.84f;
		bool isGrounded = true;



	// Start is called before the first frame update
	void Start()
	{

		//getfloat ??? 

	}

	// Update is called once per frame
	void Update()
	{
		//cutoff
		SetSawCutoff();
		SetLeadCutoff();
		SetGlobalCutoff();


		//lowpass when not grounded


	}

	void SetSawCutoff() {
		float speed = mainCharacter.getSpeed();
		float targetSawCutoff = 200 + (speed * speed * 1000);
		sawCutoff = Mathf.SmoothDamp(sawCutoff, targetSawCutoff, ref SmoothVelocitySaw, 1f);
		//float Cutoff = ((Mathf.Sin(Time.time) + 1) / 2 * 15000 ) + 200;
		generalMixer.SetFloat("SawCutoff", sawCutoff);
	}

	void SetLeadCutoff() {
		float speed = mainCharacter.getSpeed();
		float targetLeadCutoff = 3500 + (speed * speed * 16500);
		leadCutoff = Mathf.SmoothDamp(leadCutoff, targetLeadCutoff, ref SmoothVelocityLead, 1f);
		//float Cutoff = ((Mathf.Sin(Time.time) + 1) / 2 * 15000 ) + 200;
		generalMixer.SetFloat("LeadCutoff", leadCutoff);
	}

	void SetGlobalCutoff() {
		//lowpass when not grounded
		isGrounded = mainCharacter.getIsGrounded();
		if (!isGrounded) {
			bool isJumping = mainCharacter.getIsJumping();

			if (isJumping) {
				generalMixer.SetFloat("ReverbVolume", 15f);

			} else { 
				generalMixer.SetFloat("ReverbVolume", reverbVolume); 

				float targetCutoffMaster = 300f;
				//masterCutoff = 300f;
				masterCutoff = Mathf.SmoothDamp(masterCutoff, targetCutoffMaster, ref SmoothVelocityMaster, 0.6f);
			}
		} else {
			masterCutoff = 20000f;
		}
		generalMixer.SetFloat("MasterCutoff", masterCutoff);
	}

	public void StartTrack2(){
		// AudioSource[] allChildren = gameObject.GetComponentsInChildren<AudioSource>();
		// foreach (AudioSource audio in allChildren) {
		// 	audio.Play();
		// }
  
		Transform[] directChild = new Transform[gameObject.transform.childCount];
		 
		for (int i = 0; i < gameObject.transform.childCount; ++i) {
	  	directChild[i] = gameObject.transform.GetChild (i);
		}

		foreach(Transform child in directChild) {
			if (child.name != "Leads") {
				child.GetComponent<AudioSource>().Play();
			}
		}

	}

	public void StartLead2() {
		AudioSource[] leads = gameObject.transform.Find("Leads").GetComponents<AudioSource>();
		leads[0].Stop();
		leads[1].Play();
	}



}
