using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class HUDAudioModel {

	public List<AudioSource> audioSources { get; set; }
	// Use this for initialization
	public HUDAudioModel() {
		audioSources = new List<AudioSource> ();
		AudioSource dave = Resources.Load("Hey Dave", typeof(AudioSource)) as AudioSource;
		audioSources.Add (dave);
	}
}
