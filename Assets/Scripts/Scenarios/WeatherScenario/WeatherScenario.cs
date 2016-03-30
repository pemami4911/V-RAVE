using UnityEngine;
using System.Collections;
using MonsterLove.StateMachine;

namespace VRAVE {

	public class WeatherScenario : StateBehaviour {

		[SerializeField] private GameObject UserCar; 

		private SpawnController manufacturer; 
		private HUDController hudController;
		private HUDAudioController audioController;

		private float FOG_DENSITY = 0.8f;

		public enum States 
		{
			UserDriveRoute,
			AIDriveRoute,
			UserApproachTraffic,
			AIApproachTraffic,
			UserStopped,
			AIStopped
		}

		void Awake()
		{
			Initialize<States> ();

			UserCar.GetComponent<CarAIControl> ().enabled = false;
			UserCar.GetComponent<CarUserControl> ().enabled = true;

			manufacturer = GetComponent<SpawnController>();
			hudController = UserCar.GetComponentInChildren<HUDController>();
			audioController = UserCar.GetComponent<HUDAudioController>();

			//hudController.model = new DefaultHUD ();

			resetScenario ();
			ChangeState(States.UserDriveRoute);
		}

		private void resetScenario() {
			RenderSettings.fog = true; //enable fog bruh
			RenderSettings.fogMode = FogMode.ExponentialSquared;
			RenderSettings.fogDensity = FOG_DENSITY;
		}

		public override void TriggerCb (uint id) {
			switch (id) {
			//will fill out later
			default:
				break;
			}
		}

		public void UserDriveRoute_Enter() {
			//RenderSettings.fog = true;
		}

		public void AIDriveRoute_Enter() {
			//TODO
		}

		public void UserApproachTraffic_Enter() {
			//TODO
		}

		public void AIApproachTraffic_Enter() {
			//TODO
		}

		public void UserStopped_Enter() {
			//TODO
		}

		public void AIStopped_Enter() {
			//TODO
		}

		// Extend abstract method "ChangeState(uint id)
		//
		// This is used for reacting to "OnTriggerEnter" events, called by WaypointTrigger scripts
		/*public override void ChangeState (uint id)
		{
			switch (id) 
			{
			case 0: 
				break;
			}
		}*/
	}
}
		