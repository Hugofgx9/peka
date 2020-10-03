using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class AudioManager : MonoBehaviour
{

		[SerializeField] private AudioMixer generalMixer;
		[SerializeField] private ThirdPersonMovement mainCharacter;
		float SmoothVelocity;
		float cutoff = 200f;



    // Start is called before the first frame update
    void Start()
    {


        
    }

    // Update is called once per frame
    void Update()
    {

    	float speed = mainCharacter.getSpeed();

    	float targetCutoff = 200 + (speed * speed * 1000);

    	cutoff = Mathf.SmoothDamp(cutoff, targetCutoff, ref SmoothVelocity, 1f);

    	//float Cutoff = ((Mathf.Sin(Time.time) + 1) / 2 * 15000 ) + 200;

    	SetSawCutoff(cutoff);

    }

    public void SetSawCutoff(float setCutoff) {
    	generalMixer.SetFloat("SawCutoff", setCutoff);
    }
}
