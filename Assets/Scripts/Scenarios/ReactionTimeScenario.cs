	using UnityEngine;
	using System.Collections;
	using MonsterLove.StateMachine;

	namespace VRAVE 
	{
	public class ReactionTimeScenario : StateBehaviour {

		// Each scenario will want a reference to the user car
		[SerializeField] private GameObject UserCar; 

		private SpawnController manufacturer; 

		// You'll need to specify all of the states for your scenario here
		// I recommend drawing an FSM to give you an idea about how state transitions
		// will happen in your scenario
		public enum States 
		{
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

		// Utilize the Vehicle Factory here to create any vehicles that need to be made at the beginning of the scenario
		// It is probably a good idea to instantiate all of the vehicles that will be used at this time, and just disable
		// the ones that shouldn't be visible yet
		void Awake()
		{
			Initialize<States> ();
			//Instantiate (UserCar, new Vector3 (21.11f, 0.14f, 14.4f), Quaternion.identity);
			UserCar.SetActive (false);

			manufacturer = GetComponent<SpawnController> ();

			// Set the initial state here
			ChangeState (States.AIDriving);
		}

		// Extend abstract method "ChangeState(uint id)
		//
		// This is used for reacting to "OnTriggerEnter" events, called by WaypointTrigger scripts
		public override void ChangeState (uint id)
		{
			switch (id) 
			{
			case 0: 
				ChangeState (States.Obstacle);
				break;
			}
		}


	}
	}
