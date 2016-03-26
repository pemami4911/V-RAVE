using UnityEngine;
using System.Collections;
using MonsterLove.StateMachine;

namespace VRAVE
{
    public class LanePassingScenario : StateBehaviour
    {

        [SerializeField]
        private GameObject UserCar;

        private SpawnController manufacturer;
        private HUDController hudController;
        private HUDAudioController audioController;
        private CarAIControl carAI;
        private CarController carController;

        private float intersectionStopThreshold = 5f;

        public enum States
        {
            Init_State,
            Following_Instruction,
            Following,
            Passing_Instruction,
            Passing,
            Mode_Switch
        }

        void Awake()
        {
            Initialize<States>();

            carController = UserCar.GetComponent<CarController>();
            carController.MaxSpeed = 15f;
            carAI = UserCar.GetComponent<CarAIControl>();
            carAI.enabled = false;
            UserCar.GetComponent<CarUserControl>().enabled = false;

            manufacturer = GetComponent<SpawnController>();
            hudController = UserCar.GetComponentInChildren<HUDController>();
            audioController = UserCar.GetComponent<HUDAudioController>();

            ChangeState(States.IntersectionBriefing);
        }

        // Extend abstract method "ChangeState(uint id)
        //
        // This is used for reacting to "OnTriggerEnter" events, called by WaypointTrigger scripts
        public override void TriggerCb(uint id)
        {
            switch (id)
            {
                case 0:
                    carAI.SetStopWhenTargetReached(true);
                    break;
                case 1:
                    if (checkCarSpeed())
                    {
                        ChangeState(States.StoppedAtIntersection);
                    }
                    else {
                        ChangeState(States.WarnAndRestart);
                    }
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
            if (Input.GetButtonDown("LeftPaddle"))
            {
                ChangeState(States.HumanDrivingToIntersection);
            }

        }

        /* HUMAN DRIVING INTERSECTION */

        public void HumanDrivingToIntersection_Enter()
        {
            UserCar.GetComponent<CarUserControl>().enabled = true;
        }

        /* STOPPED_AT_INTERSECTION */

        public void StoppedAtIntersection_Enter()
        {
            Debug.Log("Stopped At Intersection!");
        }

        /* INTERSECTION_FINISH */

        public void IntersectionFinish_Enter()
        {
            //hudController.model = new DefaultHUD();

        }

        /* Internal scenario methods */
        private bool checkCarSpeed()
        {
            return (carController.CurrentSpeed < intersectionStopThreshold) ? true : false;
        }
    }
}
