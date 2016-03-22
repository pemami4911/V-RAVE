using UnityEngine;
using System.Collections;
using System.Collections.Generic;

//the model for audio used in your scenario
//its basically just a list, for your AudioModel you'll extend this class and add your own audio samples
//from there, 
public class HUDAudioModel {

	public List<AudioClip> audioClips { get; set; }
	// Use this for initialization
	public HUDAudioModel() {
		audioClips = new List<AudioClip> ();
		AudioClip dave = Resources.Load("Hey Dave", typeof(AudioClip)) as AudioClip;
		audioClips.Add (dave);
	}
}
