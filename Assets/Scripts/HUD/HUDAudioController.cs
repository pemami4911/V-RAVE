using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityStandardAssets.CrossPlatformInput;

public class HUDAudioController : MonoBehaviour {

	public HUDAudioModel audioModel { get; set; }

	void Start () {
		audioModel = new HUDAudioModel ();
	}

	void Update() {
		bool h = CrossPlatformInputManager.GetButtonDown ("HUDTextEnable");
		if (h) {
			playAudio (0);
		}
	}

	void playAudio(int index) {
		List<AudioSource> sources = audioModel.audioSources;
		AudioSource audio = sources [index];
		audio.Play ();
	}

	void playAudio(AudioSource audio) {
		audio.Play ();
	}
}
