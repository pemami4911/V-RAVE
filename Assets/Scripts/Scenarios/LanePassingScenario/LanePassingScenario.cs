using UnityEngine;
using System.Collections;
using MonsterLove.StateMachine;
using UnityStandardAssets.Utility;
using System.Collections.Generic;

namespace VRAVE
{
    public class LanePassingScenario : StateBehaviour
    {

        [SerializeField]
        private GameObject UserCar;
        [SerializeField]
        private GameObject AIVehicle;
        [SerializeField]
        private WaypointCircuit passingTrack;
        [SerializeField]
        private WaypointCircuit initialTrack;

        private SpawnController manufacturer;
        private HUDController hudController;
        private HUDAudioController audioController;
        private Sensors userCarSensors;
        private CarAIControl userCarAI;
        private CarAIControl AIVehicleAI;
        private CarController userCarController;
        private CarController AIVehicleCarController;

        private bool userMode = true;
        private int circuitProgressNum = 0;


        public enum States
        {
            InitState,
            FollowingInstruction,
            Following,
            PassingInstruction,
            Passing,
            PassingWait,
            Mode_Switch
        }

        void Awake()
        {
            Initialize<States>();

            userCarController = UserCar.GetComponent<CarController>();
            userCarController.MaxSpeed = 5f;
            userCarAI = UserCar.GetComponent<CarAIControl>();
            userCarAI.enabled = false;
            UserCar.GetComponent<CarUserControl>().enabled = false;
            userCarSensors = UserCar.GetComponent<Sensors>();


            AIVehicleCarController = AIVehicle.GetComponent<CarController>();
            AIVehicleCarController.MaxSpeed = 5f;
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
                case 3: //Beginning of passing track
                    Debug.Log("Inside Case 3");


                    break;
                case 4: //End of passing track
                    Debug.Log("Inside Case 4");
                    ChangeState(States.PassingInstruction);


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

            if(userMode)
            {
                //(UserCar.GetComponent<CarUserControl>() as CarUserControl).enabled = true;
                ChangeState(States.PassingInstruction);
            }
            else
            {
                //NEEDS TO BE FIXED FOR FOLLOWING HANDLING
                ChangeState(States.PassingInstruction);
            }
            
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
            //GameObject[] go = GameObject.FindGameObjectsWithTag("Path");
            //initialTrack = GameObject.Find("Figure8_North_3-22").GetComponent<WaypointCircuit>();
            AIVehicleAI.Circuit = initialTrack;
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
                userCarAI.Circuit = initialTrack;
                userCarAI.enabled = false;
            }

        }

        public void Following_Update()
        {
            Debug.Log("Update: Following");

            //TODO: REMOVE!!!
            //WaypointCircuit wc = GameObject.Find("Figure8_North_3-22").GetComponent<WaypointCircuit>();
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
                        if (!vo.ContainsKey(8) || ((vo.ContainsKey(8) && vo[8].Distance >= 10)))
                        {
                            Debug.Log("Sensor 8 good!");

                            Debug.Log("Passing Conditions Met!");

                            if (Input.GetKey(KeyCode.Return))
                            {
                                Debug.Log("Passing Track Created!");
                                Transform carTransform = UserCar.transform;
                                WaypointCircuit passTrack = (WaypointCircuit)(Instantiate(passingTrack, carTransform.position + carTransform.forward * 2f, carTransform.rotation));
                                Debug.Log("UserCar: " + carTransform.position.ToString());





                            }
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
            Debug.Log("Entered: PassingInstruction");
            //GameObject[] go = GameObject.FindGameObjectsWithTag("Path");
            //WaypointCircuit wc = GameObject.Find("Figure8_North_3-22").GetComponent<WaypointCircuit>();
            AIVehicleAI.Circuit = initialTrack;
            (AIVehicle.GetComponent("Halo") as Behaviour).enabled = true;

            //Move to update
            if (Input.GetKey(KeyCode.Return))
            {
                ChangeState(States.Passing);
            }
        }

        /* PASSING */

        public void Passing_Enter()
        {
            Debug.Log("Entered: Passing");
            AIVehicleAI.enabled = true;
            AIVehicleAI.IsCircuit = true;


            if (userMode)
            {
                (UserCar.GetComponent<CarUserControl>() as CarUserControl).enabled = true;
                userCarAI.enabled = false;
            }
            else
            {
                (UserCar.GetComponent<CarUserControl>() as CarUserControl).enabled = false;
                circuitProgressNum = Mathf.CeilToInt((userCarAI.ProgressNum - 3) / 4) * 4 + 3; //Rounds the waypoints up to multiples of 3+4n
                userCarAI.switchCircuit(initialTrack, circuitProgressNum);
                userCarAI.IsCircuit = true;
                userCarAI.enabled = true;
            }
        }


        public void Passing_Update()
        {
            Debug.Log("Update: Passing");

            if (Input.GetKey(KeyCode.Return))
            {
                //Enable SensorHandlerEnable from CarAIConrol
            }

            if (userCarAI.IsPassing)
            {
                ChangeState(States.PassingWait);
            }


                if (scanned)
            {
                Debug.Log("Scanning!");
                if (vo.ContainsKey(7) && vo[7].obstacleTag.Equals("AI_Car"))
                {
                    Debug.Log("Sensor 7 Hit");
                    if (vo[7].Distance <= 10f)
                    {
                        Debug.Log("Sensor 7 in length");
                        if (!vo.ContainsKey(8) || ((vo.ContainsKey(8) && vo[8].Distance >= 10)))
                        {
                            Debug.Log("Sensor 8 good!");

                            Debug.Log("Passing Conditions Met!");
                            //userCarAI.IsPassing = true; //Should be set in the first trigger of the passing path
                            if (Input.GetKey(KeyCode.Return))
                            {
                                Debug.Log("Passing Track Created!");
                                Transform carTransform = UserCar.transform;
                                //Instantiate in awake() and just translate and enable when needed. REMOVE!
                                WaypointCircuit passTrack = (WaypointCircuit)(Instantiate(passingTrack, carTransform.position + carTransform.forward * 2f, carTransform.rotation));
                                Debug.Log("UserCar: " + carTransform.position.ToString());
                                ChangeState(States.PassingWait);
                            }
                        }
                    }
                }
            }
        }


        /* Passing Wait */
        /* Wait while passing other vehicle as to not attempt to pass again until finished. */
        public void PassingWait_Enter()
        {
            Debug.Log("Enter: PassingWait");
            (AIVehicle.GetComponent("Halo") as Behaviour).enabled = false;  //Turn off glow as you pass.
            userCarAI.IsPassing = true;
            userCarAI.IsCircuit = false;

            circuitProgressNum = userCarAI.ProgressNum;
            //Get reference from private variable made in awake() then set location/rotation
            WaypointCircuit passTemp = (WaypointCircuit)GameObject.FindGameObjectWithTag("PassingPath").GetComponent<WaypointCircuit>();
            userCarAI.switchCircuit(passTemp, 0);

            (UserCar.GetComponent<CarUserControl>() as CarUserControl).enabled = false;
            userCarController.MaxSpeed = 30;
            userCarAI.enabled = true;
            

        }

        public void PassingWait_Exit()
        {
            Debug.Log("Exit: PassingWait");
            userCarAI.IsPassing = false;
           

            if (userMode)
            {
                (UserCar.GetComponent<CarUserControl>() as CarUserControl).enabled = true;
                userCarAI.enabled = false;
            }
            else
            {
                (UserCar.GetComponent<CarUserControl>() as CarUserControl).enabled = false;
                circuitProgressNum = Mathf.CeilToInt((userCarAI.ProgressNum - 3) / 4) * 4 + 3; //Rounds the waypoints up to multiples of 3+4n
                userCarAI.switchCircuit(initialTrack, circuitProgressNum);
                userCarAI.IsCircuit = true;
                userCarAI.enabled = true;
            }

        }
    }
}
