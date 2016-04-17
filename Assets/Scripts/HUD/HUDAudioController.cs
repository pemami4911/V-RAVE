using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityStandardAssets.CrossPlatformInput;

namespace VRAVE
{
	public class HUDAudioController : MonoBehaviour
	{

		public HUDAudioModel audioModel { get; set; }
		//set this property with your own HUDAudioModel

		[SerializeField] private int numberOfAudioSources = 2;

		private AudioSource[] hudAudioSources;
		//the component that plays the sound

		private void Awake ()
		{
			audioModel = new HUDAudioModel ();
			hudAudioSources = new AudioSource[2];
			hudAudioSources[0] = GetComponentInParent<AudioSource> ();

			// add other audio sources
			for (int i = 1; i < numberOfAudioSources; ++i) {
				hudAudioSources[i] = transform.parent.gameObject.AddComponent<AudioSource>();
				hudAudioSources [i].playOnAwake = false;
				hudAudioSources [i].volume = 0.5f;
			}
		}


		//plays audio from the HUDAudioModel
		public void playAudio (int index)
		{
			List<AudioClip> clips = audioModel.audioClips;
			AudioClip audio = clips [index];
			hudAudioSources[0].clip = audio;
			hudAudioSources[0].Play ();
		}

		public void PlayAudio (int index, int source)
		{
			AudioClip audio = audioModel.audioClips [index];
			hudAudioSources [source].clip = audio;
			hudAudioSources [source].Play ();
		}

		//plays audio from parameter
		public void playAudio (AudioClip audio)
		{
			hudAudioSources[0].clip = audio;
			hudAudioSources[0].Play ();
		}

		public AudioSource HudAudioSource {
			get {
				return hudAudioSources[0];
			}
			set {
				hudAudioSources[0] = value;
			}
		}

		public int NumberOfAudioSources {
			get {
				return numberOfAudioSources;
			}
		}

			
	}
}
