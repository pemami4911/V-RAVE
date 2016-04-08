using UnityEngine;
using System;
using System.Collections;
using MonsterLove.StateMachine;

namespace VRAVE
{

	public class ReactionTimeScenario : StateBehaviour
	{

		[SerializeField] private GameObject UserCar;
		[SerializeField] private GameObject CrazyIntersectionAI;
		[SerializeField] private GameObject UnsuspectingAI;
		[SerializeField] private GameObject trashCan; 

		// 0 - unsuspecting car trigger
		// 1 - approaching intersection trigger
		// 2 - post collision trigger
		// 3 - trash trigger 
		// 4 - trashcan ai path 2 trigger
		// 5 - trashcan user ai path 2 trigger
		[SerializeField] private GameObject[] triggers; 

		// 0 - ai_intersection_path_2
		// 1 - AI_Car_TrashcanPath1
		// 2 - AI_Car_TrashcanPath2
		// 3 - UserCar_Trashcan_Path1
		// 4 - UserCar_Trashcan_Path2
		[SerializeField] private UnityStandardAssets.Utility.WaypointCircuit[] ai_paths; 

		private HUDController hudController;
		private HUDAudioController audioController;
		private AmbientAudioController ambientAudioController;
		private CarAIControl carAI;
		private CarAIControl unsuspectingCarAI;
		private CarController carController;
		private CarAIControl crazyAI;
		private CarController crazyCarController;
		private SensitiveSensorResponseHandler sensitiveSensorResponseHandler;
		private TrashcanSensorResponseHandler trashCanSensorResponseHandler;

		private bool m_humanDrivingState;

		public enum States
		{
			IntersectionBriefing,
			HumanDrivingToIntersection,
			AIDrivingToIntersection,
			AdvancingThroughIntersection,
			TrashcanBriefing,
			HumanDrivingToTrashcan,
			AIDrivingToTrashcan,
			Finish
		}

		void Awake ()
		{
			CameraFade.StartAlphaFade (Color.black, true, 2f, 0.5f);
			Initialize<States> ();
		
			carController = UserCar.GetComponent<CarController> ();
			carController.MaxSpeed = 15f;
			carAI = UserCar.GetComponent<CarAIControl> ();
			sensitiveSensorResponseHandler = UserCar.GetComponent<SensitiveSensorResponseHandler> ();
			trashCanSensorResponseHandler = UserCar.GetComponent<TrashcanSensorResponseHandler> ();

			crazyAI = CrazyIntersectionAI.GetComponent<CarAIControl> ();
			crazyCarController = CrazyIntersectionAI.GetComponent<CarController> ();

			hudController = UserCar.GetComponentInChildren<HUDController> ();
			audioController = UserCar.GetComponentInChildren<HUDAudioController> ();
			ambientAudioController = UserCar.GetComponentInChildren<AmbientAudioController> ();

			// configure audio model
			audioController.audioModel.addClip("car crashing", 6f);
				
			// configure HUD models
			hudController.model = new HUDPatrickScenario1();


			unsuspectingCarAI = UnsuspectingAI.GetComponent<CarAIControl> ();

			foreach (GameObject o2 in triggers) {
				o2.SetActive(false);
			}

			resetIntersectionScenario ();
			//resetTrashCanScenario();

			//ChangeState (States.TrashcanBriefing);
			ChangeState (States.IntersectionBriefing);
			//ChangeState(States.AIDrivingToIntersection);
		}

		/********************** RESETS *******************************/ 

		// DISABLES CAR-AI AND USER-DRIVING
		private void resetIntersectionScenario ()
		{
			carAI.enabled = false;
			UserCar.GetComponent<CarUserControl> ().enabled = false;
			UserCar.GetComponent<CarUserControl> ().StopCar();

			CrazyIntersectionAI.SetActive (false);
			UnsuspectingAI.SetActive (false);

			UserCar.transform.position = new Vector3 (26f, 0.26f, -18.3f);
			UserCar.transform.rotation = Quaternion.Euler (0f, 0f, 0f);

			carController.ResetSpeed ();

			CrazyIntersectionAI.transform.position = new Vector3 (43.6f, 0.01f, 65f);
			CrazyIntersectionAI.transform.rotation = Quaternion.Euler (0f, 270f, 0f);

			UnsuspectingAI.transform.position = new Vector3 (-34f, 0.01f, 63.07f);
			UnsuspectingAI.transform.rotation = Quaternion.Euler (0f, 90f, 0f);

			// reset circuits
			crazyAI.Circuit = crazyAI.Circuit;
			unsuspectingCarAI.Circuit = unsuspectingCarAI.Circuit;
		}
			
		private void resetTrashCanScenario() 
		{
			carAI.enabled = false;
			trashCan.SetActive (false);
			UserCar.GetComponent<CarUserControl> ().enabled = false;
			UserCar.GetComponent<CarUserControl> ().StopCar();

			CrazyIntersectionAI.SetActive (false);
			UnsuspectingAI.SetActive (false);
			trashCanSensorResponseHandler.Enable = false;

			UserCar.transform.position = new Vector3 (26f, 0.26f, -18.3f);
			UserCar.transform.rotation = Quaternion.Euler (0f, 0f, 0f);

			carController.ResetSpeed ();

			UnsuspectingAI.transform.position = new Vector3 (26.09f, 0.01f, -2.24f);
			UnsuspectingAI.transform.rotation = Quaternion.Euler (0f, 0f, 0f);
			UnsuspectingAI.GetComponent<CarController> ().ResetSpeed ();

			trashCan.transform.position = new Vector3 (47.663f, 0.4394f, 121.95f);
			trashCan.transform.rotation = Quaternion.Euler (90f, 310.44f, 133f);
			trashCan.GetComponent<Rigidbody> ().velocity = Vector3.zero;
			trashCan.GetComponent<Rigidbody> ().angularVelocity = Vector3.zero;

			triggers [0].SetActive(false);
			triggers [1].SetActive(false);
			triggers [2].SetActive(false);
			triggers [3].SetActive(true);
			triggers [4].SetActive(true);
			triggers [5].SetActive(false);
		}

		/********************** TRIGGERS *****************************/

		// Extend abstract method "ChangeState(uint id)
		//
		// This is used for reacting to "OnTriggerEnter" events, called by WaypointTrigger scripts
		public override void TriggerCb (uint id)
		{
			switch (id) {
			case 0: 
				if (GetState ().Equals (States.AIDrivingToIntersection)) {
					StartCoroutine (ChangeAIPaths (3f, ai_paths [0], carAI, () => { 
						carController.MaxSpeed = 20f;
					}));
				}
				break;
			case 1: 
				ChangeState (States.AdvancingThroughIntersection);
				break;
			case 2:
				UnsuspectingAI.SetActive (true);
				break;
			case 3:
				UserCar.GetComponent<CarUserControl> ().enabled = false;
				StartCoroutine (PostCollisionStateChange (3f));
				break;
			case 4: 
				trashCan.SetActive (true);
				trashCan.GetComponent<TrashCanAnimator> ().roll ();
				trashCanSensorResponseHandler.Enable = true;
				StartCoroutine (PostTrashcanStateChange (6f));
				break;
			case 5: 
				StartCoroutine (ChangeAIPaths (4f, ai_paths [2], unsuspectingCarAI, () => {
					unsuspectingCarAI.ReachTargetThreshold = 2f;
					UnsuspectingAI.GetComponent<CarController> ().MaxSpeed = 30f;
				}));
				break;
			case 6:
				unsuspectingCarAI.CautiousSpeedFactor = 0.8f;
				break;
			case 7: 
				StartCoroutine (ChangeAIPaths (4f, ai_paths [4], carAI, () => {
					carAI.ReachTargetThreshold = 2f;
					carController.MaxSpeed = 25f;
				}));
				break;
			case 8:
				carAI.CautiousSpeedFactor = 0.8f;
				break;
			}
		}
			
		/*************** INTERSECTION_SCENARIO_BRIEFING ***********************/

		// In this state, the user will be briefed "briefly"
		// on what to do
		// HUD and Audio changes
		public void IntersectionBriefing_Enter ()
		{
			// "Start" when paddle is hit 
			triggers [0].SetActive(true);
			triggers [1].SetActive(true);
			triggers [2].SetActive(true);
		}

		// Wait for the user to press OK
		public void IntersectionBriefing_Update ()
		{
			// 	Change to steering wheel paddle
			if (Input.GetButtonDown (VRAVEStrings.Left_Paddle)) {
				ambientAudioController.StartLooping ();
				ChangeState (States.HumanDrivingToIntersection);
			}
		}
			
		public void HumanDrivingToIntersection_Enter ()
		{
			m_humanDrivingState = true;
			UserCar.GetComponent<CarUserControl> ().enabled = true;
			UserCar.GetComponent<CarUserControl> ().StartCar();
		}

		public void AIDrivingToIntersection_Enter ()
		{
			m_humanDrivingState = false;
			carAI.enabled = true;
			sensitiveSensorResponseHandler.Enable = true;
		}

		/* going through intersection */

		public void AdvancingThroughIntersection_Enter ()
		{
			CrazyIntersectionAI.SetActive (true);
			crazyCarController.MaxSpeed = 40f;
			crazyCarController.SetSpeed = new Vector3 (-40f, 0f, 0f);
			if (m_humanDrivingState) {
				audioController.playAudio (0);
			}
		}

		// disable hard stopping 
		public void AdvancingThroughIntersection_Exit ()
		{
			if (sensitiveSensorResponseHandler.Enable) {
				sensitiveSensorResponseHandler.Enable = false;
			}
		}
			
		/**************************** Trash can states *********************/ 

		public void TrashcanBriefing_Enter() {
			resetTrashCanScenario ();
			unsuspectingCarAI.enabled = false;
			UnsuspectingAI.SetActive (true);
			UnsuspectingAI.GetComponent<CarController> ().MaxSpeed = 15f;
			unsuspectingCarAI.ReachTargetThreshold = 5f;

			unsuspectingCarAI.Circuit = ai_paths[1];
			unsuspectingCarAI.enabled = true;

			// Update HUD, explain what's gonna happen
			ChangeState (States.HumanDrivingToTrashcan);
		}

		public void HumanDrivingToTrashcan_Enter() {
			m_humanDrivingState = true;
			carController.MaxSpeed = 15f;
			UserCar.GetComponent<CarUserControl> ().enabled = true;
			UserCar.GetComponent<CarUserControl> ().StartCar();
		}

		// change sensor angle to 100
		public void AIDrivingToTrashcan_Enter() {
			m_humanDrivingState = false;

			unsuspectingCarAI.enabled = false;
			UnsuspectingAI.SetActive (true);
			UnsuspectingAI.GetComponent<CarController> ().MaxSpeed = 15f;
			carController.MaxSpeed = 15f;
			unsuspectingCarAI.ReachTargetThreshold = 5f;
			carAI.ReachTargetThreshold = 5f;
			UserCar.GetComponent<Sensors> ().M_shortSensorAngleDelta = 100f;

			// enable the trigger for changing the path of the user AI
			triggers [5].SetActive(true);

			unsuspectingCarAI.Circuit = ai_paths[1];
			carAI.GetComponent<CarAIControl> ().Circuit = ai_paths[3];
			carAI.enabled = true;
			unsuspectingCarAI.enabled = true;
		}

		public void Finish_Enter() {
			Debug.Log ("Finish!");
			carController.MaxSpeed = 0f;
			// Application.LoadLevel(Lobby)
		}

		/************* Coroutines *****************/

		private IEnumerator PostCollisionStateChange (float time)
		{			
			yield return new WaitForSeconds (time);
		
			// use a lambda expression to define the callback
			CameraFade.StartAlphaFade (Color.black, false, 3f, 0f, () => {
				if (m_humanDrivingState) {
					resetIntersectionScenario ();
					ChangeState (States.AIDrivingToIntersection);
				} else {					
					ChangeState (States.TrashcanBriefing);
				}
			});
		}

		private IEnumerator ChangeAIPaths (float time, UnityStandardAssets.Utility.WaypointCircuit wc, CarAIControl ai)
		{
			yield return new WaitForSeconds (time);
			ai.switchCircuit(wc, 0);
		}

		private IEnumerator ChangeAIPaths (float time, UnityStandardAssets.Utility.WaypointCircuit wc, CarAIControl ai, Action postPathChange)
		{
			yield return new WaitForSeconds (time);
			ai.switchCircuit(wc, 0);
			if (postPathChange != null) {
				postPathChange ();					
			}
		}

		private IEnumerator PostTrashcanStateChange( float time )
		{
			yield return new WaitForSeconds (time); 

			// use a lambda expression to define the callback
			CameraFade.StartAlphaFade (Color.black, false, 3f, 0f, () => {
				if (m_humanDrivingState) {
					resetTrashCanScenario ();
					ChangeState (States.AIDrivingToTrashcan);
				} else {					
					ChangeState (States.Finish);
				}
			});
		}
	}
}
