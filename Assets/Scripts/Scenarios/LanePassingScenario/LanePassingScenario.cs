using UnityEngine;
using System.Collections;
using MonsterLove.StateMachine;
using UnityStandardAssets.Utility;
using System.Collections.Generic;
using UnityEngine.SceneManagement;

namespace VRAVE
{
    public class LanePassingScenario : StateBehaviour
    {

        [SerializeField]
        private GameObject UserCar;
        [SerializeField]
        private GameObject AIVehicle;
        [SerializeField]
        private WaypointCircuit UserTrack;
        [SerializeField]
        private WaypointCircuit AITrack;
        [SerializeField]
        private GameObject[] triggers;
        [SerializeField]
        private GameObject[] passingZones;

        private SpawnController manufacturer;
        private HUDController hudController;
        private HUDAudioController audioController;
        private Sensors userCarSensors;
        private CarAIControl userCarAI;
        private CarAIControl AIVehicleAI;
        private CarController userCarController;
        private CarController AIVehicleCarController;

        private LanePassingSensorResponseHandler lanePassingHandler;
        private FollowingSensorResponseHandler followHandler;

        private WaypointCircuit passingTrack;
        private GameObject mirror;
        private bool userMode;
        private bool canPass = false;
        private bool alreadyPassed = false;
        private bool triggerToggle = true;  //Find a better way of toggling waypoint triggers
        private int circuitProgressNum = 0;
        private float passingSpeed;



        public enum States
        {
            InitState,
            FollowingInstruction,
            Following,  //Effectively UserMode (Used only for User)
            PassingInstruction,
            WaitToPass,
            Passing,  //Effectively AIMode (Used only for AI)
            ChangeMode
        }

        void Awake()
        {
            CameraFade.StartAlphaFade(Color.black, true, 2f, 0.5f);

            Initialize<States>();

            userCarController = UserCar.GetComponent<CarController>();
            userCarController.MaxSpeed = 20f;
            userCarAI = UserCar.GetComponent<CarAIControl>();
            userCarAI.enabled = false;
            UserCar.GetComponent<CarUserControl>().enabled = false;
            userCarSensors = UserCar.GetComponent<Sensors>();


            AIVehicleCarController = AIVehicle.GetComponent<CarController>();
            AIVehicleCarController.MaxSpeed = 15;
            AIVehicleAI = AIVehicle.GetComponent<CarAIControl>();
            AIVehicleAI.enabled = false;
            (AIVehicle.GetComponent("Halo") as Behaviour).enabled = false;

            manufacturer = GetComponent<SpawnController>();
            hudController = UserCar.GetComponentInChildren<HUDController>();
            audioController = UserCar.GetComponent<HUDAudioController>();

            lanePassingHandler = UserCar.GetComponent<LanePassingSensorResponseHandler>();
            lanePassingHandler.Enable = false;
            followHandler = UserCar.GetComponent<FollowingSensorResponseHandler>();

            userMode = true;

            UserCar.SetActive(true);
            AIVehicle.SetActive(true);

            mirror = GameObject.FindWithTag(VRAVEStrings.Mirror);

            //triggers[2].gameObject.SetActive(false);

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
                    AIVehicleCarController.MaxSpeed = 15;
                    if (!userMode && !alreadyPassed)
                    {
                        userCarController.MaxSpeed = AIVehicleCarController.MaxSpeed + 0.5f;
                    }
                    break;

                case 1: //Slow Down
                    AIVehicleCarController.MaxSpeed = 10;
                    if (!userMode && !alreadyPassed)
                    {
                        userCarController.MaxSpeed = AIVehicleCarController.MaxSpeed + 0.5f;
                    }
                    break;

                case 2:  //Speed Up
                    AIVehicleCarController.MaxSpeed = 25;
                    if (!userMode && !alreadyPassed)
                    {
                        userCarController.MaxSpeed = AIVehicleCarController.MaxSpeed + 0.5f;
                    }
                    break;

                case 3: //Beginning of passing track
                    //Debug.Log("Inside Case 3");


                    break;
                case 4: //End of passing track
                    //Debug.Log("Inside Case 4");
                    //userCarController.MaxSpeed = 30;
                    //Debug.Log("Speeding up quickly!");
                    ChangeState(States.WaitToPass);


                    break;
                case 5: //Speed up quickly
                    //Debug.Log("Back to normal");
                    //userCarController.MaxSpeed = 20;
                    userCarController.FullTorqueOverAllWheels = 750;
                    userCarAI.CautiousSpeedFactor = 0.4f;
                    alreadyPassed = true;
					//triggerToggle = false;
					userCarController.MaxSpeed = 20;
                    break;

                case 10:
                    //Debug.Log("Can Pass");
                    canPass = true;
                    break;
                case 11:
                    //Debug.Log("Cannot Pass");
                    canPass = false;
                    break;

                case 20:
                    //AI Car gets to turn at 15 mph
                    AIVehicleAI.ReachTargetThreshold = 2;
                    break;
                case 21:
                    //AI Car leaves turn from 15 mph
                    AIVehicleAI.ReachTargetThreshold = 2;
                    break;

                case 30:  
                    //User Vehicle slows down before right turn
                    //Debug.Log("SLOW DOWN!");
                    if (triggerToggle && !alreadyPassed)
                    {
                        //userCarController.MaxSpeed = AIVehicleCarController.MaxSpeed - 10.0f;
                        userCarAI.AvoidOtherCarTime = Time.time + 3f;
                        userCarAI.AvoidOtherCarSlowdown = 0.5f;
                    }
                    break;
                case 31:
					//User vehicle Speeds back up
					//Debug.Log("Speed back up!");
					if (triggerToggle && !alreadyPassed)
					{
						userCarController.MaxSpeed = AIVehicleCarController.MaxSpeed + 0.5f;
					}
					break;

                case 32:
                    triggerToggle = false;
                    break;

                case 33:
                    triggerToggle = true;
                    break;
            }
        }


        private void resetScenario()
        {

            userCarAI.enabled = false;
            AIVehicleAI.enabled = false;

            UserCar.GetComponent<CarUserControl>().enabled = false;

            UserCar.GetComponent<CarUserControl>().StopCar();

            userCarController.ResetSpeed();
            AIVehicleCarController.ResetSpeed();

            if(mirror != null)
            {
                mirror.SetActive(false);
            }
            UserCar.SetActive(false);
            AIVehicle.SetActive(false);

            AIVehicle.transform.position = new Vector3(25.9f, 0.457f, 1f);
            AIVehicle.transform.rotation = Quaternion.Euler(0f, 0f, 0f);

            UserCar.transform.position = new Vector3(26f, 0.26f, -6f);
            UserCar.transform.rotation = Quaternion.Euler(0f, 0f, 0f);

            canPass = false;

            // reset circuits
            userCarAI.Circuit = AITrack;
            AIVehicleAI.Circuit = UserTrack;
        }

        /* INIT_STATE */

        // In this state, introduction is given
        // HUD and Audio changes
        public void InitState_Enter()
        {
            hudController.model = new HUD_LanePassing_Init();
            if(userMode)
            {
                userCarController.MaxSteeringAngle = 50f;
            }
            //Debug.Log("Enter: InitState");
            //UseCar and AI Vehicles Created
            //Display Scenario Information on HUD

        }

        // Wait for the user to press OK
        public void InitState_Update()
        {

            ChangeState(States.FollowingInstruction);

            //POSSIBLY DONT NEED THIS
            // 	Change to steering wheel paddle
            //Debug.Log("Waiting for Input");
            if (Input.GetButtonDown(VRAVEStrings.Right_Paddle))
            {
                ChangeState(States.FollowingInstruction);
            }

        }

        /* FOLLOWING_INSTRUCTION */

        public void FollowingInstruction_Enter()
        {

            //Debug.Log("Enter: FollowingInstruction");

            //Play insructions here!!!
            mirror.SetActive(true);
            (AIVehicle.GetComponent("Halo") as Behaviour).enabled = true;
            if (userMode)
            {
                UserCar.GetComponent<CarUserControl>().StartCar();
                (UserCar.GetComponent<CarUserControl>() as CarUserControl).enabled = true;
                userCarAI.enabled = false;
            }
            else
            {
                //followHandler.Enable = true;
            }

        }

        public void FollowingInstruction_Update()
        {
            if (userMode)
            {
                //Once user begins driving, start AI
                if (userCarController.AccelInput >= 0.05f)  //Change to left trigger
                {
                    //Debug.Log("Update: FollowingInstruction");
                    ChangeState(States.Following);
                }
            }
            else  //Not used anymore
            {
                // 	Change to steering wheel paddle
                if (Input.GetKeyDown(KeyCode.Return))
                {
                    ChangeState(States.Following);
                }
            }

        }

        /* FOLLOWING */

        public void Following_Enter()
        {
            //Debug.Log("Entered: Following");
            (UserCar.GetComponent<CarUserControl>() as CarUserControl).enabled = true;
            AIVehicleAI.Circuit = AITrack;
            AIVehicleAI.enabled = true;
            AIVehicleAI.IsCircuit = true;
            (AIVehicle.GetComponent("Halo") as Behaviour).enabled = false;

            if (userMode)
            {
                (UserCar.GetComponent<CarUserControl>() as CarUserControl).enabled = true;
                userCarAI.enabled = false;
            }
            else  //Not used
            {
                (UserCar.GetComponent<CarUserControl>() as CarUserControl).enabled = false;
                userCarAI.Circuit = UserTrack;
                userCarAI.enabled = true;
                userCarController.MaxSpeed = AIVehicleCarController.MaxSpeed + 0.5f;
            }

        }

        public void Following_Update()
        {
            //followHandler.Enable = true;
            //Leave UserMode and Being AI Mode
            AIVehicle.SetActive(true);
            if (Input.GetButtonDown(VRAVEStrings.Right_Paddle))
            {
                (UserCar.GetComponent<CarUserControl>() as CarUserControl).enabled = false;
                CameraFade.StartAlphaFade(Color.black, false, 2f, 0f, () =>
                {
                    ChangeState(States.ChangeMode);
                });
            }
        }


        /* PASSING INSTRUCTION */
        //The AI part of the simulation
        public void PassingInstruction_Enter()
        {
            //CameraFade.StartAlphaFade(Color.black, true, 1.5f, 2f);   //Causes problems.
            mirror.SetActive(true);
            UserCar.GetComponent<CarUserControl>().StartCar();
            AIVehicleAI.enabled = false;
            userCarAI.enabled = false;
            (AIVehicle.GetComponent("Halo") as Behaviour).enabled = true;
        }

        public void PassingInstruction_Update()
        {
            //Move to WaitToPass
            if (Input.GetButtonDown((VRAVEStrings.Right_Paddle)))
            {
                AIVehicleAI.enabled = true;
                AIVehicleAI.Circuit = AITrack;
                AIVehicleAI.IsCircuit = true;
                ChangeState(States.WaitToPass);
            }
        }

        /* WaitToPass */

        public void WaitToPass_Enter()
        {
            
            //Debug.Log("Entered: WaitToPass");
            //(AIVehicle.GetComponent("Halo") as Behaviour).enabled = true;  Already called in PassingInstruction

            if (userMode)
            {
                (UserCar.GetComponent<CarUserControl>() as CarUserControl).enabled = true;
                userCarAI.enabled = false;
            }
            else
            {
                (UserCar.GetComponent<CarUserControl>() as CarUserControl).enabled = false;
                circuitProgressNum = Mathf.CeilToInt((userCarAI.ProgressNum - 3) / 4) * 4 + 3; //Rounds the waypoints up to multiples of 3+4n
                userCarAI.switchCircuit(UserTrack, circuitProgressNum);
                //Debug.Log(circuitProgressNum);
                //Debug.Log(initialTrack.ToString());
                userCarAI.IsCircuit = true;
                userCarAI.enabled = true;
                userCarController.MaxSpeed = AIVehicleCarController.MaxSpeed + 0.5f;
            }

        }


        public void WaitToPass_Update()
        {
            //Debug.Log("Update: WaitToPass");

            if (Input.GetButtonDown(VRAVEStrings.Right_Paddle))
            {
                CameraFade.StartAlphaFade(Color.black, false, 2f, 0f, () =>
				{
					ChangeState(States.ChangeMode);
				});
                
            }

            if (Input.GetButton(VRAVEStrings.Left_Paddle) && (canPass == true))
            {
                //Debug.Log("Should start passing");
                getPassTrack();
                lanePassingHandler.Enable = true;
            }

            if (userCarAI.IsPassing)
            {
                ChangeState(States.Passing);
            }

            //userCarController.MaxSpeed = AIVehicleCarController.MaxSpeed + 1;
        }


        /* Passing */
        /* Wait while passing other vehicle as to not attempt to pass again until finished. */
        public void Passing_Enter()
        {
            //Move passing track to 2 units ahead of the UserCar and set rotation to forward.
            passingTrack.transform.position = (UserCar.transform.position + UserCar.transform.forward * 2f);
            float newAngle = Mathf.RoundToInt(UserCar.transform.eulerAngles.y / 90f) * 90f;
            passingTrack.transform.eulerAngles = new Vector3(0, newAngle, 0);

            //Debug.Log("Enter: Passing");
            (AIVehicle.GetComponent("Halo") as Behaviour).enabled = false;  //Turn off glow as you pass.
            userCarAI.IsCircuit = false;

            circuitProgressNum = userCarAI.ProgressNum;
            userCarAI.switchCircuit(passingTrack, 0);

            (UserCar.GetComponent<CarUserControl>() as CarUserControl).enabled = false;
            userCarController.FullTorqueOverAllWheels = 1500;
            userCarController.MaxSpeed = passingSpeed;
            userCarAI.CautiousMaxAngle = 75f;
            userCarAI.CautiousSpeedFactor = 0.7f;
            userCarAI.enabled = true;
            

        }

        public void Passing_Exit()
        {
            //Debug.Log("Exit: Passing");
            userCarAI.IsPassing = false;
            userCarController.MaxSpeed = AIVehicleCarController.MaxSpeed + 0.5f;
            userCarAI.CautiousMaxAngle = 25f;
            //float AISpeed = AIVehicleCarController.CurrentSpeed;
            //userCarController.SetSpeed = new Vector3(0,0,40f);


            if (userMode)  //Should never be called
            {
                (UserCar.GetComponent<CarUserControl>() as CarUserControl).enabled = true;
                userCarAI.enabled = false;
            }
            else
            {
                (UserCar.GetComponent<CarUserControl>() as CarUserControl).enabled = false;
                circuitProgressNum = Mathf.CeilToInt((userCarAI.ProgressNum - 3) / 4) * 4 + 3; //Rounds the waypoints up to multiples of 3+4n
                userCarAI.switchCircuit(UserTrack, circuitProgressNum);
                //Debug.Log(circuitProgressNum);
                userCarAI.IsCircuit = true;
                userCarAI.enabled = true;
            }

        }

        public void ChangeMode_Enter()
        {
            resetScenario();
            if(userMode)
            {
                userMode = false;
                userCarController.MaxSteeringAngle = 35f;
                AIVehicle.SetActive(true);
                UserCar.SetActive(true);
                //Show AI passing and following
                ChangeState(States.PassingInstruction);
            }
            else  //End scenario
            {
				//Fade out.
				//SceneManager.LoadScene(0);
                //Debug.Log("End Scenario. Back to Lobby.");
                userCarController.MaxSteeringAngle = 50f;
                AIVehicle.SetActive(true);
                UserCar.SetActive(true);
                userMode = true;
                ChangeState(States.FollowingInstruction);
            }
        }

        private void getPassTrack()
        {
            switch (Mathf.RoundToInt(AIVehicleCarController.MaxSpeed).ToString())
            {
                case "5":
                    //string track = "PassingTrack" + userCarController.MaxSpeed.ToString();
                    passingTrack = (WaypointCircuit)GameObject.Find(VRAVEStrings.PassingTrack5).GetComponent<WaypointCircuit>();
                    passingSpeed = 15f;
                    break;
                case "10":
                case "15":
                    passingTrack = (WaypointCircuit)GameObject.Find(VRAVEStrings.PassingTrack10).GetComponent<WaypointCircuit>();
                    passingSpeed = 25f;
                    break;
                case "25":
                    passingTrack = (WaypointCircuit)GameObject.Find(VRAVEStrings.PassingTrack25).GetComponent<WaypointCircuit>();
                    passingSpeed = 30f;
                    break;
                default:
                    passingTrack = (WaypointCircuit)GameObject.Find(VRAVEStrings.PassingTrack10).GetComponent<WaypointCircuit>();
                    passingSpeed = 25f;
                    break;

            }
        }
    }
}
