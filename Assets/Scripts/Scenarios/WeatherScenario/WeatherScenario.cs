using UnityEngine;
using System.Collections;
using MonsterLove.StateMachine;

namespace VRAVE {

	public class WeatherScenario : StateBehaviour {

		[SerializeField] private GameObject UserCar; 

		private SpawnController spawnController;
		private SpawnModel spawnModel;
		private HUDController hudController;
		private HUDAudioController audioController;

		private float INITIAL_FOG_DENSITY = 0.1f;

		public enum States 
		{
			UserDriveRoute,
			AIDriveRoute,
			UserApproachTraffic,
			AIApproachTraffic,
			UserWarnCollision,
			AIWarnCollision,
			UserStopped,
			AIStopped
		}

		void Awake()
		{
			Initialize<States> ();

			UserCar.GetComponent<CarAIControl> ().enabled = false;
			UserCar.GetComponent<CarUserControl> ().enabled = true;

			spawnController = GetComponent<SpawnController>();
			hudController = UserCar.GetComponentInChildren<HUDController>();
			audioController = UserCar.GetComponent<HUDAudioController>();

			spawnModel = new WeatherSpawnModel ();
			spawnController.spawnModel = spawnModel;
			//hudController.model = new DefaultHUD ();

			resetScenario ();
			ChangeState(States.UserDriveRoute);
		}

		private void resetScenario() {
			RenderSettings.fog = true; //enable fog bruh
			RenderSettings.fogMode = FogMode.Exponential;
			RenderSettings.fogDensity = INITIAL_FOG_DENSITY;
			hudController.model = new WeatherHUDModel ();
		}

		public override void TriggerCb (uint id) {
			switch (id) {
			case 0:
				if(GetState().Equals(States.UserDriveRoute)) {
					ChangeState(States.UserApproachTraffic);
				}
				else if (GetState().Equals(States.AIDriveRoute)) {
					ChangeState(States.AIApproachTraffic);
				}
				break;
			case 1: //increase fog
				if (GetState ().Equals (States.UserApproachTraffic) || GetState ().Equals (States.AIApproachTraffic)) {
					RenderSettings.fogDensity = 0.8f;
				}
				break;
			case 2:
				if (GetState ().Equals (States.UserApproachTraffic)) {
					ChangeState (States.UserWarnCollision);
				} else if (GetState().Equals (States.AIApproachTraffic)) {
					ChangeState (States.AIWarnCollision);
				}
				break;
			case 3:
				if (GetState ().Equals (States.UserDriveRoute) || GetState ().Equals (States.AIDriveRoute)) {
					hudController.model.centerText = "Turn left";
				}
				break;
			default:
				break;
			}
		}

		public void UserDriveRoute_Enter() {
			spawnController.enterScenario ();
			hudController.model.centerText = "Proceed to intersection";
		}

		public void AIDriveRoute_Enter() {
			//TODO
		}

		public void UserApproachTraffic_Enter() {
			Debug.Log("Entered State UserApproachTraffic");
			//increase fog in this state
			RenderSettings.fogDensity = 0.3f;
		}

		public void AIApproachTraffic_Enter() {
			//TODO
		}

		public void UserWarnCollision_Enter() {
			hudController.model.isLeftImageEnabled = true;
		}

		public void AIWarnCollision_Enter() {
			//TODO
			hudController.model.isLeftImageEnabled = true;
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
		