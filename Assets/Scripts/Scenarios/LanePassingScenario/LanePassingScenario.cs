using UnityEngine;
using System.Collections;
using MonsterLove.StateMachine;

namespace VRAVE
{
    public class LanePassingScenario : StateBehaviour
    {

        [SerializeField] private GameObject UserCar;
        [SerializeField] private GameObject AIVehicle;

        private SpawnController manufacturer;
        private HUDController hudController;
        private HUDAudioController audioController;
        private CarAIControl userCarAI;
        private CarAIControl AIVehicleAI;
        private CarController userCarController;
        private CarController AIVehicleCarController;

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

            userCarController = UserCar.GetComponent<CarController>();
            userCarController.MaxSpeed = 30f;
            userCarAI = UserCar.GetComponent<CarAIControl>();
            userCarAI.enabled = false;
            UserCar.GetComponent<CarUserControl>().enabled = false;

            AIVehicleCarController = AIVehicle.GetComponent<CarController>();
            AIVehicleCarController.MaxSpeed = 30f;
            AIVehicleAI = UserCar.GetComponent<CarAIControl>();
            AIVehicleAI.enabled = false;
            (AIVehicle.GetComponent("Halo") as Behaviour).enabled = false;

            manufacturer = GetComponent<SpawnController>();
            hudController = UserCar.GetComponentInChildren<HUDController>();
            audioController = UserCar.GetComponent<HUDAudioController>();

            ChangeState(States.Init_State);
        }

        // Extend abstract method "ChangeState(uint id)
        //
        // This is used for reacting to "OnTriggerEnter" events, called by WaypointTrigger scripts
        public override void TriggerCb(uint id)
        {
            switch (id)
            {
                case 0:
                    Debug.Log("Case 0!!!");
                    break;
                case 1:
                    Debug.Log("Case 1!!!");
                    if (checkCarSpeed())
                    {
                        //ChangeState(States.StoppedAtIntersection);
                    }
                    else {
                        //ChangeState(States.WarnAndRestart);
                    }
                    break;
            }
        }

        /* INIT_STATE */

        // In this state, introduction is given
        // HUD and Audio changes
        public void Init_State_Enter()
        {
            //UseCar and AI Vehicles Created
            //Display Scenario Information on HUD
            
            //Start when right paddle is pressed
            //ChangeState(States.Following_Instruction);
            
        }

        // Wait for the user to press OK
        public void IntersectionBriefing_Update()
        {
            // 	Change to steering wheel paddle
            if (Input.GetButtonDown("LeftPaddle"))
            {
                ChangeState(States.Following_Instruction);
            }

        }

        /* FOLLOWING_INSTRUCTION */

        public void Following_Instruction_Enter()
        {
            UserCar.GetComponent<CarUserControl>().enabled = true;      
        }

        public void Following_Instruction_Update()
        {
            UserCar.GetComponent<CarUserControl>().enabled = true;

            //Once user begins driving, start AI
            if (userCarController.AccelInput >= 0.05f)
            {
                ChangeState(States.Following);
            }
        }

        /* FOLLOWING */

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
            return (userCarController.CurrentSpeed < intersectionStopThreshold) ? true : false;
        }
    }
}
