using UnityEngine;
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
		[SerializeField] private GameObject[] triggers; 

		// 0 - ai_intersection_path_2
		// 1 - AI_Car_TrashcanPath1
		// 2 - AI_Car_TrashcanPath2
		[SerializeField] private UnityStandardAssets.Utility.WaypointCircuit[] ai_paths; 

		private SpawnController manufacturer;
		private HUDController hudController;
		private HUDAudioController audioController;
		private CarAIControl carAI;
		private CarController carController;
		private CarAIControl crazyAI;
		private CarController crazyCarController;
		private SensitiveSensorResponseHandler sensitiveSensorResponseHandler;


		public enum States
		{
			IntersectionBriefing,
			HumanDrivingToIntersection,
			AIDrivingToIntersection,
			AdvancingThroughIntersection,
			TrashcanBriefing,
			HumanDrivingToTrashcan,
			AIDrivingToTrashcan,
			AvoidOncoming,
			DriveToCornerFinish
		}

		void Awake ()
		{
			CameraFade.StartAlphaFade (Color.black, true, 2f, 0.5f);
			Initialize<States> ();
		
			carController = UserCar.GetComponent<CarController> ();
			carController.MaxSpeed = 15f;
			carAI = UserCar.GetComponent<CarAIControl> ();
			sensitiveSensorResponseHandler = UserCar.GetComponent<SensitiveSensorResponseHandler> ();

			crazyAI = CrazyIntersectionAI.GetComponent<CarAIControl> ();
			crazyCarController = CrazyIntersectionAI.GetComponent<CarController> ();

			manufacturer = GetComponent<SpawnController> ();
			hudController = UserCar.GetComponentInChildren<HUDController> ();
			audioController = UserCar.GetComponent<HUDAudioController> ();

			//resetIntersectionScenario ();
			resetTrashCanScenario();

			foreach (GameObject o2 in triggers) {
				o2.SetActive(false);
			}

			ChangeState (States.TrashcanBriefing);
			//ChangeState (States.IntersectionBriefing);
			//ChangeState(States.AIDrivingToIntersection);
		}

		/********************** RESETS *******************************/ 

		// DISABLES CAR-AI AND USER-DRIVING
		private void resetIntersectionScenario ()
		{
			carAI.enabled = false;
			UserCar.GetComponent<CarUserControl> ().enabled = false;

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
			UnsuspectingAI.GetComponent<CarAIControl> ().Circuit = UnsuspectingAI.GetComponent<CarAIControl> ().Circuit;
		}
			
		private void resetTrashCanScenario() 
		{
			carAI.enabled = false;
			UserCar.GetComponent<CarUserControl> ().enabled = false;
			CrazyIntersectionAI.SetActive (false);
			UnsuspectingAI.SetActive (false);

			UserCar.transform.position = new Vector3 (26f, 0.26f, -18.3f);
			UserCar.transform.rotation = Quaternion.Euler (0f, 0f, 0f);

			carController.ResetSpeed ();

			UnsuspectingAI.transform.position = new Vector3 (26.09f, 0.01f, -2.24f);
			UnsuspectingAI.transform.rotation = Quaternion.Euler (0f, 0f, 0f);

			// reset circuits 
			carAI.Circuit = carAI.Circuit;
		}

		/********************** TRIGGERS *****************************/

		// Extend abstract method "ChangeState(uint id)
		//
		// This is used for reacting to "OnTriggerEnter" events, called by WaypointTrigger scripts
		public override void TriggerCb (uint id)
		{
			switch (id) {
			case 0: 
				if (GetState ().Equals( States.AIDrivingToIntersection)) {
					StartCoroutine (ChangeAIPaths (3f, ai_paths[0], carAI));
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
				StartCoroutine (PostCollisionStateChange (2f));
				break;
			case 4: 
				trashCan.SetActive (true);
				trashCan.GetComponent<TrashCanAnimator> ().roll ();
				break;
			case 5: 
				StartCoroutine (ChangeAIPaths (5f, ai_paths[2], UnsuspectingAI.GetComponent<CarAIControl>()));
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
				ChangeState (States.HumanDrivingToIntersection);
			}
		}
			
		public void HumanDrivingToIntersection_Enter ()
		{
			UserCar.GetComponent<CarUserControl> ().enabled = true;
		}

		public void AIDrivingToIntersection_Enter ()
		{
			CameraFade.StartAlphaFade (Color.black, true, 3f, 0f, () => {
				carAI.enabled = true;
				sensitiveSensorResponseHandler.Enable = true;
			});
		}

		/* going through intersection */

		public void AdvancingThroughIntersection_Enter ()
		{
			CrazyIntersectionAI.SetActive (true);
			crazyCarController.MaxSpeed = 40f;
			crazyCarController.SetSpeed = new Vector3 (-40f, 0f, 0f);
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

			UnsuspectingAI.GetComponent<CarAIControl> ().switchCircuit (ai_paths [1], 0);
			UnsuspectingAI.GetComponent<CarAIControl> ().enabled = false;

			UnsuspectingAI.SetActive (true);
			UnsuspectingAI.GetComponent<CarController> ().MaxSpeed = 30f;

			CameraFade.StartAlphaFade (Color.black, true, 3f, 0f, () => {
				triggers [0].SetActive(false);
				triggers [1].SetActive(false);
				triggers [2].SetActive(false);
				triggers [3].SetActive(false);
				triggers [4].SetActive(true);
				UnsuspectingAI.GetComponent<CarAIControl> ().enabled = true;

				// Update HUD, explain what's gonna happen
				ChangeState (States.HumanDrivingToTrashcan);
			});
		}

		public void HumanDrivingToTrashcan_Enter() {
			UserCar.GetComponent<CarUserControl> ().enabled = true;
		}


		/************* Coroutines *****************/

		private IEnumerator PostCollisionStateChange (float time)
		{			
			yield return new WaitForSeconds (time);
		
			// use a lambda expression to define the callback
			CameraFade.StartAlphaFade (Color.black, false, 3f, 0f, () => {
				resetIntersectionScenario ();
				if (!carAI.enabled) {
					ChangeState (States.AIDrivingToIntersection);
				} else {					
					ChangeState (States.TrashcanBriefing);
				}
			});
		}

		private IEnumerator ChangeAIPaths (float time, UnityStandardAssets.Utility.WaypointCircuit wc, CarAIControl ai)
		{
			yield return new WaitForSeconds (time);
			ai.Circuit = wc;
		}
	}
}
