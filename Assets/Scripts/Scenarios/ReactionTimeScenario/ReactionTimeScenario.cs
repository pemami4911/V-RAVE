	using UnityEngine;
	using System.Collections;
	using MonsterLove.StateMachine;

	namespace VRAVE 
	{

	public class ReactionTimeScenario : StateBehaviour {

		[SerializeField] private GameObject UserCar; 
		[SerializeField] private GameObject CrazyIntersectionAI;
		[SerializeField] private GameObject UnsuspectingAI;

		private SpawnController manufacturer; 
		private HUDController hudController;
		private HUDAudioController audioController;
		private CarAIControl carAI;
		private CarController carController;
		private CarAIControl crazyAI;
		private CarController crazyCarController;

		public enum States 
		{
			IntersectionBriefing,
			HumanDrivingToIntersection,
			AIDrivingToIntersection,
			HumanAdvancingThroughIntersection,
			AIAdvancingThroughIntersection,
			IntersectionFinish,
			HumanDrivingToCorner,
			AIDrivingToCorner,
			Turning,
			AvoidOncoming,
			DriveToCornerFinish
		}

		void Awake()
		{
			CameraFade.StartAlphaFade (Color.black, true, 2f, 0.5f);
			Initialize<States> ();

			carController = UserCar.GetComponent<CarController> ();
			carController.MaxSpeed = 15f;
			carAI = UserCar.GetComponent<CarAIControl> ();

			crazyAI = CrazyIntersectionAI.GetComponent<CarAIControl> ();
			crazyCarController = CrazyIntersectionAI.GetComponent<CarController> ();


			manufacturer = GetComponent<SpawnController>();
			hudController = UserCar.GetComponentInChildren<HUDController>();
			audioController = UserCar.GetComponent<HUDAudioController>();

			resetScenario ();

			ChangeState(States.IntersectionBriefing);
		}
			
		private void resetScenario()
		{
			carAI.enabled = false;
			UserCar.GetComponent<CarUserControl> ().enabled = false;
			CrazyIntersectionAI.SetActive (false);
			UnsuspectingAI.SetActive (false);

			UserCar.transform.position = new Vector3 (26f, 0.26f, -18.3f);
			UserCar.transform.rotation = Quaternion.Euler(0f, 0f, 0f); 
		}

		// Extend abstract method "ChangeState(uint id)
		//
		// This is used for reacting to "OnTriggerEnter" events, called by WaypointTrigger scripts
		public override void TriggerCb (uint id)
		{
			switch (id) 
			{
			case 0: 
				carAI.SetStopWhenTargetReached (true);
				break;
			case 1: 
				ChangeState (States.HumanAdvancingThroughIntersection);
				break;
			case 2:
				UnsuspectingAI.SetActive (true);
				break;
			case 3:
				StartCoroutine (PostCollisionStateChange (3f));
				break;
			}
		}
			
		/* INTERSECTION_SCENARIO_BRIEFING */

		// In this state, the user will be briefed "briefly" 
		// on what to do
		// HUD and Audio changes
		public void IntersectionBriefing_Enter()
		{
			// "Start" when paddle is hit 
		}

		// Wait for the user to press OK
		public void IntersectionBriefing_Update()
		{
			// 	Change to steering wheel paddle
			if (Input.GetButtonDown ("LeftPaddle")) 
			{
				ChangeState (States.HumanDrivingToIntersection);
			}

		}

		/* HUMAN DRIVING INTERSECTION */ 

		public void HumanDrivingToIntersection_Enter()
		{
			UserCar.GetComponent<CarUserControl> ().enabled = true;
		}

		/* Human going through intersection */

		public void HumanAdvancingThroughIntersection_Enter()
		{
			CrazyIntersectionAI.SetActive (true);
			crazyCarController.MaxSpeed = 50f;
			crazyCarController.SetSpeed = new Vector3 (-50f, 0f, 0f);
		}

		public void AIDrivingToIntersection_Enter()
		{
			CameraFade.StartAlphaFade (Color.black, true, 3f);
			carAI.enabled = true;
		}

		/* INTERSECTION_FINISH */ 

		public void IntersectionFinish_Enter()
		{
			//hudController.model = new DefaultHUD();

		}

		/* Coroutines */
		private IEnumerator PostCollisionStateChange(float time)
		{			
			yield return new WaitForSeconds (time);
			CameraFade.StartAlphaFade (Color.black, false, 3f);
			resetScenario ();
			ChangeState (States.AIDrivingToIntersection);
		}
	}
	}
