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

		private SpawnController manufacturer;
		private HUDController hudController;
		private HUDAudioController audioController;
		private CarAIControl carAI;
		private CarController carController;
		private CarAIControl crazyAI;
		private CarController crazyCarController;
		private SensitiveSensorResponseHandler sensitiveSensorResponseHandler;
		private UnityStandardAssets.Utility.WaypointCircuit ai_path;

		public enum States
		{
			IntersectionBriefing,
			HumanDrivingToIntersection,
			AIDrivingToIntersection,
			AdvancingThroughIntersection,
			WrongWayBriefing,
			HumanDrivingToCorner,
			AIDrivingToCorner,
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

			GameObject o = GameObject.Find (VRAVEStrings.AI_Intersection_Path2);
			ai_path = o.GetComponent<UnityStandardAssets.Utility.WaypointCircuit> ();

			resetIntersectionScenario ();

			ChangeState (States.IntersectionBriefing);
			//ChangeState(States.AIDrivingToIntersection);
		}

		/* RESETS */ 

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

		// Extend abstract method "ChangeState(uint id)
		//
		// This is used for reacting to "OnTriggerEnter" events, called by WaypointTrigger scripts
		public override void TriggerCb (uint id)
		{
			switch (id) {
			case 0: 
				if (GetState ().Equals( States.AIDrivingToIntersection)) {
					StartCoroutine (ChangeAIPaths ());
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
			}
		}
			
		/* INTERSECTION_SCENARIO_BRIEFING */

		// In this state, the user will be briefed "briefly"
		// on what to do
		// HUD and Audio changes
		public void IntersectionBriefing_Enter ()
		{
			// "Start" when paddle is hit 
		}

		// Wait for the user to press OK
		public void IntersectionBriefing_Update ()
		{
			// 	Change to steering wheel paddle
			if (Input.GetButtonDown (VRAVEStrings.Left_Paddle)) {
				ChangeState (States.HumanDrivingToIntersection);
			}
		}

		/* HUMAN DRIVING INTERSECTION */

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

		/* Wrong way briefing state */ 

		public void WrongWayBriefing_Enter() {

			// Update HUD, explain what's gonna happen
			ChangeState (States.HumanDrivingToCorner);
		}

		public void HumanDrivingToCorner_Enter() {
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
					ChangeState (States.WrongWayBriefing);
				}
			});
		}

		private IEnumerator ChangeAIPaths ()
		{
			yield return new WaitForSeconds (3f);
			carAI.switchCircuit (ai_path, 0);
		}
	}
}
