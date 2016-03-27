using UnityEngine;
using System.Collections;
using MonsterLove.StateMachine;
using UnityStandardAssets.Utility;
using System.Collections.Generic;

namespace VRAVE
{
    public class LanePassingScenario : StateBehaviour
    {

        [SerializeField] private GameObject UserCar;
        [SerializeField] private GameObject AIVehicle;

        private SpawnController manufacturer;
        private HUDController hudController;
        private HUDAudioController audioController;
        private Sensors userCarSensors;
        private CarAIControl userCarAI;
        private CarAIControl AIVehicleAI;
        private CarController userCarController;
        private CarController AIVehicleCarController;

        private bool userMode = true;

        private float intersectionStopThreshold = 5f;

        public enum States
        {
            InitState,
            FollowingInstruction,
            Following,
            PassingInstruction,
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
            userCarSensors = UserCar.GetComponent<Sensors>();


            AIVehicleCarController = AIVehicle.GetComponent<CarController>();
            AIVehicleCarController.MaxSpeed = 30f;
            AIVehicleAI = AIVehicle.GetComponent<CarAIControl>();
            AIVehicleAI.enabled = false;
            (AIVehicle.GetComponent("Halo") as Behaviour).enabled = false;

            manufacturer = GetComponent<SpawnController>();
            hudController = UserCar.GetComponentInChildren<HUDController>();
            audioController = UserCar.GetComponent<HUDAudioController>();

            ChangeState(States.InitState);
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
                    WaypointCircuit wc = GameObject.Find("LeftTurn_Circuit").GetComponent<WaypointCircuit>();
                    userCarAI.switchCircuit(wc, 0);
                    userCarAI.IsCircuit = false;
                    break;
                case 1:
                    Debug.Log("Case 1!!!");
                    if (true)
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
        public void InitState_Enter()
        {
            Debug.Log("Enter: InitState");
            //UseCar and AI Vehicles Created
            //Display Scenario Information on HUD
            
            //Start when right paddle is pressed
            //ChangeState(States.FollowingInstruction);
            
        }

        // Wait for the user to press OK
        public void InitState_Update()
        {
            // 	Change to steering wheel paddle
            if (Input.GetKeyDown(KeyCode.Return))
            {
                ChangeState(States.FollowingInstruction);
            }

        }

        /* FOLLOWING_INSTRUCTION */

        public void FollowingInstruction_Enter()
        {
            Debug.Log("Enter: FollowingInstruction");
            (UserCar.GetComponent<CarUserControl>() as CarUserControl).enabled = true;      
        }

        public void FollowingInstruction_Update()
        {

            //Once user begins driving, start AI
            if (userCarController.AccelInput >= 0.05f)
            {
                Debug.Log("Update: FollowingInstruction");
                ChangeState(States.Following);
            }
        }

        /* FOLLOWING */

        public void Following_Enter()
        {
            Debug.Log("Entered: Following");
            WaypointCircuit wc = GameObject.Find("Figure8_North_3-22").GetComponent<WaypointCircuit>();
            AIVehicleAI.Circuit = wc;
            AIVehicleAI.enabled = true;
            AIVehicleAI.IsCircuit = true;
            (AIVehicle.GetComponent("Halo") as Behaviour).enabled = true;

            if (userMode)
            {
                (UserCar.GetComponent<CarUserControl>() as CarUserControl).enabled = true;       
                userCarAI.enabled = false;
            }
            else
            {
                (UserCar.GetComponent<CarUserControl>() as CarUserControl).enabled = false;
                userCarAI.Circuit = wc;
                userCarAI.enabled = false;
            }

        }

        public void Following_Update()
        {
            Debug.Log("Update: Following");
            WaypointCircuit wc = GameObject.Find("Figure8_North_3-22").GetComponent<WaypointCircuit>();
            Dictionary<int, VRAVE.VRAVEObstacle> vo;
            bool scanned = userCarSensors.Scan(out vo);
            Debug.Log(scanned);
            if (scanned)
            {
                Debug.Log("Scanning!");
                if (vo.ContainsKey(7) && vo[7].obstacleTag.Equals("AI_Car"))
                {
                    Debug.Log("Sensor 7 Hit");
                    if (vo[7].Distance <= 10f)
                    {
                        Debug.Log("Sensor 7 in length");
                        if(!vo.ContainsKey(8) || ((vo.ContainsKey(8) && vo[8].Distance >= 10)))
                        {
                            Debug.Log("Sensor 8 good!");

                            Debug.Log("Passing Conditions Met!");
                            userCarAI.IsPassing = true;
                        }
                    }
                }
            }

            //AIVehicleAI.enabled = true;
            //AIVehicleAI.IsCircuit = true;
            //(AIVehicle.GetComponent("Halo") as Behaviour).enabled = true;

            //userCarAI.Circuit = wc;
            //userCarAI.enabled = true;

            //RECORD DISTANCES!

        }

        /* PASSING INSTRUCTION */

        public void PassingInstruction_Enter()
        {
            //hudController.model = new DefaultHUD();

        }

        ///* Internal scenario methods */
        //private bool checkCarSpeed()
        //{
        //    return (userCarController.CurrentSpeed < intersectionStopThreshold) ? true : false;
        //}
    }
}
