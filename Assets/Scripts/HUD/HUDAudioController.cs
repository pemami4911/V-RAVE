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

		public AudioSource primaryAudioSource;
		public AudioSource secondaryAudioSource;

		//the component that plays the sound


		//plays audio from the HUDAudioModel
		public void playAudio (int index)
		{
			List<AudioClip> clips = audioModel.audioClips;
			AudioClip audio = clips [index];
			primaryAudioSource.clip = audio;
			primaryAudioSource.Play ();
		}

		public void playSecondaryAudio (int index)
		{
			AudioClip audio = audioModel.audioClips [index];
			secondaryAudioSource.clip = audio;
			secondaryAudioSource.Play ();
		}

		//plays audio from parameter
		public void playAudio (AudioClip audio)
		{
			primaryAudioSource.clip = audio;
			primaryAudioSource.Play ();
		}
	}
}
