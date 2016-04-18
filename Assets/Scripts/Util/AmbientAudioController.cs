using UnityEngine;
using System.Collections;

namespace VRAVE
{
	public class AmbientAudioController : MonoBehaviour
	{

		public AudioSource citySounds; 

		private HUDAudioController cityAudioController; 
		private HUDAudioModel cityAudioModel;

		// Use this for initialization
		void Start ()
		{
			cityAudioModel = new HUDAudioModel ();
			cityAudioModel.addClip ("Urban-Ambient", 25f);

			cityAudioController = GetComponent<HUDAudioController> ();

			cityAudioController.primaryAudioSource = citySounds;

			cityAudioController.audioModel = cityAudioModel;

			cityAudioController.playAudio (0);
		}
			
		public void Mute() 
		{
			citySounds.mute = true;
		}

		public void UnMute()
		{
			citySounds.mute = false;
		}
	}
}