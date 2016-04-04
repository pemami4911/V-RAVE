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

		private Vector3 USER_STARTING_POSITION = new Vector3 (25.92f, 0.26f, 1.9f);
		private Quaternion USER_STARTING_ROTATION = Quaternion.Euler (0f, 0f, 0f);
		private float INITIAL_FOG_DENSITY = 0.1f;
		private float END_SCENARIO_WAIT_TIME = 4.0f;

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
			UserCar.transform.position = USER_STARTING_POSITION;
			UserCar.transform.rotation = USER_STARTING_ROTATION;

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
			case 4:
				Debug.Log ("CollisionTrigger triggered");
				if (GetState ().Equals (States.UserWarnCollision)) { 
					Debug.Log ("Switching to UserStopped state");
					ChangeState (States.UserStopped);
				} else if (GetState().Equals (States.AIWarnCollision)) {
					ChangeState (States.AIStopped);
				}
				break;
			default:
				break;
			}
		}

		public void UserDriveRoute_Enter() {
			spawnController.enterScenario ();
			hudController.model.centerText = "Proceed to intersection";

			UserCar.GetComponent<CarAIControl> ().enabled = false;
			UserCar.GetComponent<CarUserControl> ().enabled = true;
		}

		public void AIDriveRoute_Enter() {
			Debug.Log ("Entered AI Drive state");
			spawnController.resetInitialSpawns ();

			UserCar.GetComponent<CarAIControl> ().enabled = true;
			UserCar.GetComponent<CarAIControl> ().IsCircuit = false;
			UserCar.GetComponent<CarUserControl> ().enabled = false;
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
			Debug.Log ("Reached end of scenario for user drive");
			StartCoroutine (postStopStateChange (END_SCENARIO_WAIT_TIME));

		}

		private IEnumerator postStopStateChange(float time) {
			yield return new WaitForSeconds (time);

			CameraFade.StartAlphaFade (Color.black, false, 3f, 0f, () => {
				if(GetState().Equals(States.UserStopped)) { //fix this so I'm not checking states all the time
					Debug.Log("Preparing to reset scenario...");
					resetScenario ();
					ChangeState (States.AIDriveRoute);
				}
				else {
					Debug.Log("Else statement reached in postStopStateChange()");
				}
			});
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
		