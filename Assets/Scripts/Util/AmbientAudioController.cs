using UnityEngine;
using System.Collections;

namespace VRAVE
{
	public class AmbientAudioController : MonoBehaviour
	{

		[SerializeField] private AudioSource carDrivingAmbient;
		[SerializeField] private AudioSource citySounds; 

		private HUDAsyncController carAsyncController; 
		private HUDAsyncController cityAsyncController; 
		private HUDAudioController carAudioController;
		private HUDAudioController cityAudioController; 
		private HUDAudioModel carAudioModel;
		private HUDAudioModel cityAudioModel;

		// Use this for initialization
		void Start ()
		{
			carAudioModel = new HUDAudioModel ();
			carAudioModel.addClip ("Car-Driving", 30f);

			cityAudioModel = new HUDAudioModel ();
			cityAudioModel.addClip ("Urban-Ambient", 25f);

			HUDAsyncController[] asyncControllers = GetComponents<HUDAsyncController> ();
			carAsyncController = asyncControllers [0];
			cityAsyncController = asyncControllers [1];

			HUDAudioController[] audioControllers = GetComponents<HUDAudioController> ();
			carAudioController = audioControllers [0];
			carAudioController.HudAudioSource = CarDrivingAmbient;

			cityAudioController = audioControllers [1];
			cityAudioController.HudAudioSource = CitySounds;

			carAudioController.audioModel = carAudioModel;
			cityAudioController.audioModel = cityAudioModel;

			carAsyncController.Configure (carAudioController, null);
			cityAsyncController.Configure (cityAudioController, null);
		}

		public void StartLooping() 
		{
			//carAsyncController.RepeatAudio (0, int.MaxValue);
			cityAsyncController.RepeatAudio (0, int.MaxValue);
		}


		public AudioSource CarDrivingAmbient {
			get {
				return carDrivingAmbient;
			}
		}

		public AudioSource CitySounds {
			get {
				return citySounds;
			}
		}
	}
}