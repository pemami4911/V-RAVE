using UnityEngine;
using System.Collections;
using UnityStandardAssets.CrossPlatformInput;

public class HUDAudioController : MonoBehaviour {

	//private bool playAudio = false;
	AudioSource dave;

	// Use this for initialization
	void Start () {
		dave = GetComponent<AudioSource>();
	
	}
	
	// Update is called once per frame
	void Update () {
		bool h = CrossPlatformInputManager.GetButtonDown ("HUDAudioEnable");
		if (h) {
			dave.Play ();
		}
	}
}
