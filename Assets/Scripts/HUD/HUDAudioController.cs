using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityStandardAssets.CrossPlatformInput;

public class HUDAudioController : MonoBehaviour {

	public HUDAudioModel audioModel { get; set; } //set this property with your own HUDAudioModel
	private AudioSource hudAudioSource; //the component that plays the sound

	void Start () {
		audioModel = new HUDAudioModel ();
		hudAudioSource = GetComponent<AudioSource>();
	}

	//plays audio from the HUDAudioModel
	public void playAudio(int index) {
		List<AudioClip> clips = audioModel.audioClips;
		AudioClip audio = clips [index];
		hudAudioSource.clip = audio;
		hudAudioSource.Play ();
	}

	//plays audio from parameter
	public void playAudio(AudioClip audio) {
		hudAudioSource.clip = audio;
		hudAudioSource.Play ();
	}
}
