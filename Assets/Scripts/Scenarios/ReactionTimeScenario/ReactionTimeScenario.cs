	using UnityEngine;
	using System.Collections;
	using MonsterLove.StateMachine;

	namespace VRAVE 
	{
	public class ReactionTimeScenario : StateBehaviour {

		[SerializeField] private GameObject UserCar; 

		private SpawnController manufacturer; 
		private HUDController hudController;
		private HUDAudioController audioController;

		public enum States 
		{
			IntersectionScenarioBriefing,
			HumanDrivingToIntersection,
			AIDrivingToIntersection,
			StoppedAtIntersection,
			AdvancingThroughIntersection,
			IntersectionFinish,
			HumanDrivingToCorner,
			AIDrivingToCorner,
			Turning,
			AvoidOncoming,
			DriveToCornerFinish
		}

		void Awake()
		{
			Initialize<States> ();

			UserCar.GetComponent<CarAIControl> ().enabled = true;
			UserCar.GetComponent<CarUserControl> ().enabled = false;

			manufacturer = GetComponent<SpawnController>();
			hudController = UserCar.GetComponentInChildren<HUDController>();
			audioController = UserCar.GetComponent<HUDAudioController>();

			ChangeState(States.IntersectionScenarioBriefing);
		}
			
		// Extend abstract method "ChangeState(uint id)
		//
		// This is used for reacting to "OnTriggerEnter" events, called by WaypointTrigger scripts
		public override void ChangeState (uint id)
		{
			switch (id) 
			{
			case 0: 
				ChangeState (States.IntersectionFinish);
				break;
			}
		}

		public void IntersectionFinish_Enter()
		{
			hudController.model = new DefaultHUD();

		}

	}
	}
