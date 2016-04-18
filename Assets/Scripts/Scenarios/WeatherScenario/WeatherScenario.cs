using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using MonsterLove.StateMachine;
using UnityEngine.SceneManagement;

namespace VRAVE {

	public class WeatherScenario : StateBehaviour {

		[SerializeField] private GameObject UserCar;
		[SerializeField] private GameObject movingAI0;
		[SerializeField] private GameObject movingAI1;
		[SerializeField] private GameObject movingAI2;
		[SerializeField] private GameObject movingAI3;

		/*[SerializeField] private GameObject staticAI0;
		[SerializeField] private GameObject staticAI1;
		[SerializeField] private GameObject staticAI2;
		[SerializeField] private GameObject staticAI3;*/
		

		private SpawnController spawnController;
		private SpawnModel spawnModel;
		private HUDController hudController;
		private HUDAudioController audioController;
		private HUDAsyncController hudAsyncController;
		private AmbientAudioController ambientAudioController;

		private Vector3 USER_STARTING_POSITION = new Vector3 (25.92f, 0.26f, 1.9f);
		private Quaternion USER_STARTING_ROTATION = Quaternion.Euler (0f, 0f, 0f);
		private float INITIAL_FOG_DENSITY = 0.1f;
		private float END_SCENARIO_WAIT_TIME = 6.0f;

		private KeyValuePair<Vector3, Quaternion> movingAIStart0;
		private KeyValuePair<Vector3, Quaternion> movingAIStart1;
		private KeyValuePair<Vector3, Quaternion> movingAIStart2;
		private KeyValuePair<Vector3, Quaternion> movingAIStart3;

		/*private Vector3 staticAIStart0;
		private Vector3 staticAIStart1;
		private Vector3 staticAIStart2;
		private Vector3 staticAIStart3;*/


		public enum States 
		{
			ScenarioBriefing,
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
			audioController = UserCar.GetComponentInChildren<HUDAudioController> ();


			ambientAudioController = UserCar.GetComponentInChildren<AmbientAudioController> ();
			ambientAudioController.Mute();

			hudAsyncController = UserCar.GetComponentInChildren<HUDAsyncController> ();
			hudAsyncController.Configure(audioController, hudController);

			audioController.audioModel = new WeatherAudioModel ();

			spawnModel = new WeatherSpawnModel ();
			spawnController.spawnModel = spawnModel;

			//hudController.model = new DefaultHUD ();

			UserCar.GetComponent<CarAIControl> ().enabled = false;
			UserCar.GetComponent<CarUserControl> ().enabled = false;

			movingAI0.GetComponent<CarAIControl> ().enabled = false;
			movingAI1.GetComponent<CarAIControl> ().enabled = false;
			movingAI2.GetComponent<CarAIControl> ().enabled = false;
			movingAI3.GetComponent<CarAIControl> ().enabled = false;

			movingAIStart0 = new KeyValuePair<Vector3, Quaternion> (movingAI0.transform.position, movingAI0.transform.rotation);
			movingAIStart1 = new KeyValuePair<Vector3, Quaternion> (movingAI1.transform.position, movingAI1.transform.rotation);
			movingAIStart2 = new KeyValuePair<Vector3, Quaternion> (movingAI2.transform.position, movingAI2.transform.rotation);
			movingAIStart3 = new KeyValuePair<Vector3, Quaternion> (movingAI3.transform.position, movingAI3.transform.rotation);

			spawnController.enterScenario ();

			/*foreach(GameObject nonMovingAICar in spawnController.initialSpawnedObjects.Values) {
				nonMovingAICar.GetComponent<CarAIControl> ().enabled = false;
				nonMovingAICar.GetComponent<CarAIControl> ().Circuit = movingAI0.GetComponent<CarAIControl> ().Circuit;
			}*/

			resetScenario ();
			ChangeState(States.ScenarioBriefing);
		}

		private void resetScenario() {
			UserCar.transform.position = USER_STARTING_POSITION;
			UserCar.transform.rotation = USER_STARTING_ROTATION;

			movingAI0.transform.position = movingAIStart0.Key;
			movingAI0.transform.rotation = movingAIStart0.Value;
			movingAI0.GetComponent<CarAIControl> ().Circuit = movingAI0.GetComponent<CarAIControl> ().Circuit;

			movingAI1.transform.position = movingAIStart1.Key;
			movingAI1.transform.rotation = movingAIStart1.Value;
			movingAI1.GetComponent<CarAIControl> ().Circuit = movingAI1.GetComponent<CarAIControl> ().Circuit;

			movingAI2.transform.position = movingAIStart2.Key;
			movingAI2.transform.rotation = movingAIStart2.Value;
			movingAI2.GetComponent<CarAIControl> ().Circuit = movingAI2.GetComponent<CarAIControl> ().Circuit;

			movingAI3.transform.position = movingAIStart3.Key;
			movingAI3.transform.rotation = movingAIStart3.Value;
			movingAI3.GetComponent<CarAIControl> ().Circuit = movingAI3.GetComponent<CarAIControl> ().Circuit;

			/*staticAI0.transform.position = staticAIStart0;
			staticAI1.transform.position = staticAIStart1;
			staticAI2.transform.position = staticAIStart2;
			staticAI3.transform.position = staticAIStart3;*/

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
					hudController.model.centerText = "Go straight";
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
					hudController.model.centerText = "Turn right";
				}
				break;
			case 4:
				//Debug.Log ("CollisionTrigger triggered");
				if (GetState ().Equals (States.UserWarnCollision)) { 
					//Debug.Log ("Switching to UserStopped state");
					ChangeState (States.UserStopped);
				} else if (GetState ().Equals (States.AIWarnCollision)) {
					ChangeState (States.AIStopped);
				}
				break;
			case 5:
				if (GetState ().Equals (States.UserDriveRoute) || GetState ().Equals (States.AIDriveRoute)) {
					hudController.model.centerText = "Proceed through intersection";
				}
				break;
			case 6:
				if (GetState ().Equals (States.UserApproachTraffic) || GetState ().Equals (States.AIApproachTraffic)) {
					hudController.model.centerText = "Turn left";
				}
				break;
			case 7:
				if (GetState ().Equals (States.UserStopped)) { 
					audioController.playAudio (2);
				}
				else if (GetState ().Equals (States.AIStopped)) {
					audioController.playAudio (4);
				}
				break;
			default:
				break;
			}
		}

		public void ScenarioBriefing_Enter() {
			ambientAudioController.Mute ();
			hudController.EngageManualMode();
			audioController.playAudio (3);

			StartCoroutine (PostIntersectionScenarioBriefingHUDChange ());
		}

		public void ScenarioBriefing_Update() {
			if (Input.GetButtonDown (VRAVEStrings.Left_Paddle)) {
				ambientAudioController.UnMute ();
				ChangeState (States.UserDriveRoute);
			}
		}

		public void UserDriveRoute_Enter() {
			
			movingAI0.GetComponent<CarAIControl> ().enabled = true;
			movingAI1.GetComponent<CarAIControl> ().enabled = true;
			movingAI2.GetComponent<CarAIControl> ().enabled = true;
			movingAI3.GetComponent<CarAIControl> ().enabled = true;

			hudController.model.centerText = "Proceed through intersection";

			UserCar.GetComponent<CarAIControl> ().enabled = false;
			UserCar.GetComponent<CarUserControl> ().enabled = true;

			//UserCar.GetComponent<CarAIControl> ().enabled = true;
			//UserCar.GetComponent<CarUserControl> ().enabled = false;
		}

		public void AIDriveRoute_Enter() {
			//Debug.Log ("Entered AI Drive state");
			spawnController.resetInitialSpawns ();
			hudController.EngageAIMode();
			audioController.playAudio (0);

			movingAI0.SetActive (true);
			movingAI1.SetActive (true);
			movingAI2.SetActive (true);
			movingAI3.SetActive (true);

			hudController.model.centerText = "Proceed through intersection";

			UserCar.GetComponent<CarAIControl> ().enabled = true;
			UserCar.GetComponent<CarAIControl> ().IsCircuit = false;
			UserCar.GetComponent<CarUserControl> ().enabled = false;
		}

		public void UserApproachTraffic_Enter() {
			//Debug.Log("Entered State UserApproachTraffic");
			//increase fog in this state
			RenderSettings.fogDensity = 0.3f;
		}

		public void AIApproachTraffic_Enter() {
			RenderSettings.fogDensity = 0.3f;
		}

		public void UserWarnCollision_Enter() {
			hudController.model.leftBackingMaterial = Resources.Load (VRAVEStrings.Warning_Img, typeof(Material)) as Material;
			hudController.model.isLeftImageEnabled = true;
			hudAsyncController.RepeatAudio (5, -1, 3);
		}

		public void AIWarnCollision_Enter() {
			hudController.model.leftBackingMaterial = Resources.Load (VRAVEStrings.Warning_Img, typeof(Material)) as Material;
			hudController.model.isLeftImageEnabled = true;
			hudAsyncController.RepeatAudio (5, -1, 3);
		}

		public void UserStopped_Enter() {
			//Debug.Log ("Reached end of scenario for user drive");
			StartCoroutine (postStopStateChange (END_SCENARIO_WAIT_TIME));

		}

		public void UserStopped_Exit() {
			//Debug.Log ("Exiting user stopped state");
			movingAI0.SetActive (false);
			movingAI1.SetActive (false);
			movingAI2.SetActive (false);
			movingAI3.SetActive (false);

		}

		private IEnumerator postStopStateChange(float time) {
			yield return new WaitForSeconds (time);

			CameraFade.StartAlphaFade (Color.black, false, 3f, 0f, () => {
				if(GetState().Equals(States.UserStopped)) { //fix this so I'm not checking states all the time
					//Debug.Log("Preparing to reset scenario...");
					resetScenario ();
					ChangeState (States.AIDriveRoute);
				}
				else {
					//Debug.Log("Else statement reached in postStopStateChange()");
				}
			});
		}

		private IEnumerator postStopExitScenario(float time) {
			yield return new WaitForSeconds (time);

			CameraFade.StartAlphaFade (Color.black, false, 3f, 0f, () => {
				if(GetState().Equals(States.AIStopped)) { //fix this so I'm not checking states all the time
					//Debug.Log("Preparing to reset scenario...");
					resetScenario ();
					SceneManager.LoadScene (VRAVEStrings.Lobby_Menu);
				}
				else {
					//Debug.Log("Else statement reached in postStopStateChange()");
				}
			});
		}

		public void AIStopped_Enter() {
			
			StartCoroutine (postStopExitScenario (END_SCENARIO_WAIT_TIME));
		}

		private IEnumerator PostIntersectionScenarioBriefingHUDChange() 
		{
			yield return new WaitForSeconds (14f);

			if (!GetState ().Equals(States.UserDriveRoute)) {
				hudController.model.centerText = VRAVEStrings.Left_Paddle_To_Continue; 
			}
		}
	}
}
		