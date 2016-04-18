using UnityEngine;
using System.Collections;
using System.Collections.Generic;

//the Model for audio used in your scenario
//its basically just a list, for your AudioModel you'll extend this class and add your own audio samples
//from there, 
namespace VRAVE
{
	public class HUDAudioModel : MonoBehaviour {

		public List<AudioClip> audioClips = new List<AudioClip> ();

		// for storing how much time between switching between clips
		// should have same num of elements as audioClips and be in the same order 
		public List<float> durations = new List<float>(); 

		public void addClip(string name, float duration) {
			audioClips.Add(Resources.Load (name, typeof(AudioClip)) as AudioClip);
			durations.Add (duration);
		}
	}
}