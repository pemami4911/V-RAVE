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
		private HUDAsyncController hudAsyncController;
		private HUDAudioController audioController;
		private AmbientAudioController ambientAudioController;
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
			hudAsyncController = UserCar.GetComponentInChildren<HUDAsyncController>();
			audioController = UserCar.GetComponentInChildren<HUDAudioController>();
			ambientAudioController = UserCar.GetComponentInChildren<AmbientAudioController>();

            lanePassingHandler = UserCar.GetComponent<LanePassingSensorResponseHandler>();
            lanePassingHandler.Enable = false;
            followHandler = UserCar.GetComponent<FollowingSensorResponseHandler>();

			// configure HUD models
			hudController.models = new HUDModel[2];
			hudController.durations = new float[2];
			hudController.models[0] = new HUDVRAVE_Default();
			hudController.model = hudController.models[0];

			// configure ASYNC controller
			hudAsyncController.Configure(audioController, hudController);

			//configure audio
			audioController.audioModel = new LanePassingAudioModel();
			ambientAudioController.Mute();

			userMode = true;

            UserCar.SetActive(true);
            AIVehicle.SetActive(true);
			
            mirror = GameObject.FindWithTag(VRAVEStrings.Mirror);

            ChangeState(States.InitState);
        }

        // Extend abstract method "ChangeState(uint id)
        //
        // This is used for reacting to "OnTriggerEnter" events, called by WaypointTrigger scripts
        public override void TriggerCb(uint id)
        {
            switch (id)
            {
                case 0:  //Back to original speed
                    AIVehicleCarController.MaxSpeed = 15;
					if (userMode)
					{
						userCarController.MaxSpeed = 20;
					}
                    else if (!userMode && !alreadyPassed)
                    {
                        userCarController.MaxSpeed = AIVehicleCarController.MaxSpeed + 0.6f;
                    }
                    break;

                case 1: //Slow Down
                    AIVehicleCarController.MaxSpeed = 10;
                    if (!userMode && !alreadyPassed)
                    {
                        userCarController.MaxSpeed = AIVehicleCarController.MaxSpeed + 0.6f;
                    }
                    break;

                case 2:  //Speed Up
                    AIVehicleCarController.MaxSpeed = 15;
                    if (!userMode && !alreadyPassed)
                    {
                        userCarController.MaxSpeed = AIVehicleCarController.MaxSpeed + 0.6f;
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
                    userCarController.FullTorqueOverAllWheels = 750;
                    userCarAI.CautiousSpeedFactor = 0.4f;
                    alreadyPassed = true;
					//triggerToggle = false;
					userCarController.MaxSpeed = 20;
                    break;

				case 6: //Speed Up North
					if(userMode)
					{
						AIVehicleCarController.MaxSpeed = 35;
						userCarController.MaxSpeed = AIVehicleCarController.MaxSpeed + 0.6f;
					}
					else if(!alreadyPassed)
					{
						AIVehicleCarController.MaxSpeed = 35;
						userCarController.MaxSpeed = AIVehicleCarController.MaxSpeed + 0.6f;
					}
					break;

				case 7: //Slow Down North
					if (userMode)
					{
						AIVehicleCarController.MaxSpeed = 5f;
					}
					else if (!userMode && !alreadyPassed)
					{
						//AIVehicleCarController.MaxSpeed = 10f;
						//userCarController.MaxSpeed = AIVehicleCarController.MaxSpeed + 0.6f;
						StartCoroutine("UserCarSlowdown");
					}
					//Turn on brake lights.
					AIVehicle.transform.GetChild(3).gameObject.SetActive(true);

					//if (userMode)
					//{
						
					//	AIVehicleCarController.MaxSpeed = 15f;
					//	//Turn on brake lights.
					//	AIVehicle.transform.GetChild(3).gameObject.SetActive(true);
					//}
					//else if(!alreadyPassed)
					//{
					//	AIVehicleCarController.MaxSpeed = 15f;
					//	userCarController.MaxSpeed = AIVehicleCarController.MaxSpeed + 0.6f;
					//	//Turn on brake lights.
					//	AIVehicle.transform.GetChild(3).gameObject.SetActive(true);
					//}
					break;

				case 8:
					//Turn of brakelights
					AIVehicle.transform.GetChild(3).gameObject.SetActive(false);
					break;

				case 10:
                    //Debug.Log("Can Pass");
                    canPass = true;
					hudController.model.bottomText = "";
					if (!alreadyPassed)
					{
						hudController.model.bottomText = VRAVEStrings.CanPass;
					}
                    break;
                case 11:
                    //Debug.Log("Cannot Pass");
                    canPass = false;
					hudController.model.bottomText = "";
					if (!alreadyPassed)
					{
						hudController.model.bottomText = VRAVEStrings.CannotPass;
					}
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
						StartCoroutine("SlowingAtTurn");
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
						userCarController.MaxSpeed = AIVehicleCarController.MaxSpeed + 0.6f;
						StartCoroutine("SpeedingAfterTurn");
					}
					break;

                case 32:
                    triggerToggle = false;
                    break;

                case 33:
                    triggerToggle = true;
                    break;

				case 101:
					StartCoroutine("SpeedChangesBriefing");
					break;

				case 102:
					// display right turn sign on HUD.
					//hudController.Clear();
					hudController.model.isLeftImageEnabled = false;
					hudController.models[1] = hudController.model;
					hudController.models[0] = hudController.model.Clone();
					hudController.durations[0] = 0.3f;
					hudController.durations[1] = 0.2f;
					hudController.models[0].leftBackingMaterial = Resources.Load(VRAVEStrings.Right_Turn, typeof(Material)) as Material;
					hudController.models[0].isLeftImageEnabled = true;
					hudController.models[0].leftImagePosition = new Vector3(1.98f, 0.19f, -0.39f);
					hudController.models[0].leftImageScale = new Vector3(0.5f * 0.1280507f, 0, 0.5f * 0.1280507f);
					hudAsyncController.DoHUDUpdates(5, 0.5f);
					break;

				case 103:
					// display left turn sign on HUD.
					//hudController.Clear();
					hudController.model.isLeftImageEnabled = false;
					hudController.models[1] = hudController.model;
					hudController.models[0] = hudController.model.Clone();
					hudController.durations[0] = 0.3f;
					hudController.durations[1] = 0.2f;
					hudController.models[0].leftBackingMaterial = Resources.Load(VRAVEStrings.Left_Turn, typeof(Material)) as Material;
					hudController.models[0].isLeftImageEnabled = true;
					hudController.models[0].leftImagePosition = new Vector3(1.98f, 0.19f, -0.39f);
					hudController.models[0].leftImageScale = new Vector3(0.5f * 0.1280507f, 0, 0.5f * 0.1280507f);
					hudAsyncController.DoHUDUpdates(5, 0.5f);
					break;

				case 104:
					//Passing Briefing
					if (userMode)
					{
						StartCoroutine("PassingBriefing");
					}
					else
					{
						StartCoroutine("AIPassingBriefing");
					}
					break;

				case 105:
					//UserMode end and Scenario end
					if(userMode)
					{
						StartCoroutine("UserConclusion");
					}
					else
					{
						StartCoroutine("ScenarioEnd");
					}
					break;

				case 106:
					//Adaptive Cruise Control
					if (!userMode)
					{
						StartCoroutine("AdaptiveCruiseControl");
					}
					break;

				case 107:
					//AI Passing Command
					if(!userMode)
					{
						StartCoroutine("AIPassingCommand");
					}
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

			hudController.Clear();

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
				(AIVehicle.GetComponent("Halo") as Behaviour).enabled = true;
				StartCoroutine("IntroBriefing");
				//Welcome to the Adaptive Cruise Control and Lane Passing Scenario.
				//Pull right trigger to continue.
			}
			//Display Scenario Information on HUD

		}

        // Wait for the user to press OK
        public void InitState_Update()
        {
			if (Input.GetButtonDown(VRAVEStrings.Right_Paddle))
            {
				StopCoroutine("IntroBriefing");
				StopCoroutine("IntroBriefAudio");
				GameObject rightPaddle = GameObject.FindGameObjectWithTag(VRAVEStrings.Right_Paddle);
				(rightPaddle.GetComponent("Halo") as Behaviour).enabled = false;
				ChangeState(States.FollowingInstruction);
            }

        }

        /* FOLLOWING_INSTRUCTION */

        public void FollowingInstruction_Enter()
        {

			//You are inside a semi-autonomous vehicle. The glowing vehicle ahead of you is a fully autonomous.
			//Please attempt to follow this vehicle at a safe, constant distance.
			//HUD UPDATE after instructions. Start driving to begin scenario. 
			mirror.SetActive(true);
			//hudController.Clear();
			hudController.model.centerText = "";
			StartCoroutine(FollowBriefing());
            if (userMode)
            {
                UserCar.GetComponent<CarUserControl>().StartCar();
                //(UserCar.GetComponent<CarUserControl>() as CarUserControl).enabled = true;
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
                //Once user begins driving, start AI vehicle
                if (userCarController.AccelInput >= 0.05f)  //Change to left trigger
                {
					//Remove HUD driving instructions.
					ambientAudioController.UnMute();
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
            
            (UserCar.GetComponent<CarUserControl>() as CarUserControl).enabled = true;
            AIVehicleAI.Circuit = AITrack;
            AIVehicleAI.enabled = true;
            AIVehicleAI.IsCircuit = true;
            (AIVehicle.GetComponent("Halo") as Behaviour).enabled = false;

            if (userMode)
            {
                (UserCar.GetComponent<CarUserControl>() as CarUserControl).enabled = true;
                userCarAI.enabled = false;
				alreadyPassed = true; //Turns off speed up and slow down triggers for user car during following.
            }
            else  //Not used
            {
                (UserCar.GetComponent<CarUserControl>() as CarUserControl).enabled = false;
				userCarController.FullTorqueOverAllWheels = 1000;
                userCarAI.Circuit = UserTrack;
                userCarAI.enabled = true;
                userCarController.MaxSpeed = AIVehicleCarController.MaxSpeed + 0.6f;
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

		public void ChangeMode_Enter()
		{
			resetScenario();
			if (userMode)
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
				//Fades out to LobbyMenu.
				SceneManager.LoadScene(VRAVEStrings.Lobby_Menu);
				//Debug.Log("End Scenario. Back to Lobby.");

				//userCarController.MaxSteeringAngle = 50f;
				//AIVehicle.SetActive(true);
				//UserCar.SetActive(true);
				//userMode = true;
				//ChangeState(States.FollowingInstruction);
			}
		}


		/* PASSING INSTRUCTION */
		//The AI part of the simulation
		public void PassingInstruction_Enter()
        {
			StartCoroutine("AIBriefing");


			//CameraFade.StartAlphaFade(Color.black, true, 1.5f, 2f);   //Causes problems.
            mirror.SetActive(true);
            UserCar.GetComponent<CarUserControl>().StartCar();
			userCarController.FullTorqueOverAllWheels = 750f;
            AIVehicleAI.enabled = false;
            userCarAI.enabled = false;
            (AIVehicle.GetComponent("Halo") as Behaviour).enabled = true;
			alreadyPassed = false; //Reset if the car has already passed
        }

        public void PassingInstruction_Update()
        {
            //Move to WaitToPass
            if (Input.GetButtonDown((VRAVEStrings.Right_Paddle)))
            {
				hudController.EngageAIMode();
				GameObject rightPaddle = GameObject.FindGameObjectWithTag(VRAVEStrings.Right_Paddle);
				(rightPaddle.GetComponent("Halo") as Behaviour).enabled = false;
				hudController.model.centerText = "";
				AIVehicleAI.enabled = true;
                AIVehicleAI.Circuit = AITrack;
                AIVehicleAI.IsCircuit = true;
                ChangeState(States.WaitToPass);
            }
        }

        /* WaitToPass */

        public void WaitToPass_Enter()
        {
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
                userCarAI.IsCircuit = true;
                userCarAI.enabled = true;
                userCarController.MaxSpeed = AIVehicleCarController.MaxSpeed + 0.6f;
            }

        }


        public void WaitToPass_Update()
        {
            //Debug.Log("Update: WaitToPass");

            if (Input.GetButtonDown(VRAVEStrings.Right_Paddle))
            {
                CameraFade.StartAlphaFade(Color.black, false, 3f, 0f, () =>
				{
					ChangeState(States.ChangeMode);
				});
                
            }

            if (Input.GetButton(VRAVEStrings.Left_Paddle) && (canPass == true))
            {
				//Debug.Log("Should start passing");
				GameObject leftPaddle = GameObject.FindGameObjectWithTag(VRAVEStrings.Left_Paddle);
				(leftPaddle.GetComponent("Halo") as Behaviour).enabled = true;
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
			hudController.model.centerText = VRAVEStrings.PassingVehicle;
            //Move passing track to 2 units ahead of the UserCar and set rotation to forward.
            passingTrack.transform.position = (UserCar.transform.position + UserCar.transform.forward * 2f);
            float newAngle = Mathf.RoundToInt(UserCar.transform.eulerAngles.y / 90f) * 90f;
            passingTrack.transform.eulerAngles = new Vector3(0, newAngle, 0);

            //Debug.Log("Enter: Passing");
            (AIVehicle.GetComponent("Halo") as Behaviour).enabled = false;  //Turn off glow as you pass.
            userCarAI.IsCircuit = false;

            circuitProgressNum = userCarAI.ProgressNum;
			Debug.Log("CircuitNum: " + circuitProgressNum);

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
			StartCoroutine("CompletedPass");
            userCarAI.IsPassing = false;
            userCarController.MaxSpeed = AIVehicleCarController.MaxSpeed + 0.6f;
            userCarAI.CautiousMaxAngle = 25f;


            if (userMode)  //Should never be called
            {
                (UserCar.GetComponent<CarUserControl>() as CarUserControl).enabled = true;
                userCarAI.enabled = false;
            }
            else
            {
                (UserCar.GetComponent<CarUserControl>() as CarUserControl).enabled = false;
                circuitProgressNum = Mathf.CeilToInt((circuitProgressNum - 3) / 4) * 4 + 3; //Rounds the waypoints up to multiples of 3+4n
                userCarAI.switchCircuit(UserTrack, circuitProgressNum);
                userCarAI.IsCircuit = true;
                userCarAI.enabled = true;
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
                    passingTrack = (WaypointCircuit)GameObject.Find(VRAVEStrings.PassingTrack25).GetComponent<WaypointCircuit>();
                    passingSpeed = 25f;
                    break;
				case "20":
					passingTrack = (WaypointCircuit)GameObject.Find(VRAVEStrings.PassingTrack25).GetComponent<WaypointCircuit>();
					passingSpeed = 25f;
					break;
				case "25":
                    passingTrack = (WaypointCircuit)GameObject.Find(VRAVEStrings.PassingTrack25).GetComponent<WaypointCircuit>();
                    passingSpeed = 30f;
                    break;
                default:
                    passingTrack = (WaypointCircuit)GameObject.Find(VRAVEStrings.PassingTrack25).GetComponent<WaypointCircuit>();
                    passingSpeed = 30f;
                    break;

            }
        }

		private IEnumerator IntroBriefing()
		{
			//Actually start introduction audio briefing.
			hudController.model.centerText = "Welcome to Bryce's Scenario!";
			yield return new WaitForSeconds(2f);
			ambientAudioController.Mute();
			audioController.playAudio (0);
			yield return new WaitForSeconds(5f);
			hudController.model.centerText = VRAVEStrings.Right_Paddle_To_Continue;
			GameObject rightPaddle = GameObject.FindGameObjectWithTag(VRAVEStrings.Right_Paddle);
			(rightPaddle.GetComponent("Halo") as Behaviour).enabled = true;
			yield return new WaitForSeconds(0.5f);
			(rightPaddle.GetComponent("Halo") as Behaviour).enabled = false;
			yield return new WaitForSeconds(0.5f);
			(rightPaddle.GetComponent("Halo") as Behaviour).enabled = true;
			yield return new WaitForSeconds(0.5f);
			(rightPaddle.GetComponent("Halo") as Behaviour).enabled = false;
			yield return new WaitForSeconds(0.5f);
			(rightPaddle.GetComponent("Halo") as Behaviour).enabled = true;

			//Ensure paddle glow is turned off.
			yield return new WaitForSeconds(3f);
			(rightPaddle.GetComponent("Halo") as Behaviour).enabled = false;
		}

		private IEnumerator FollowBriefing()
		{
			//hudController.Clear();
			hudController.model.centerText = "";
			StopCoroutine("IntroBriefing");
			StopCoroutine("IntroBriefAudio");
			//Actually start introduction audio briefing.
			ambientAudioController.Mute();
			audioController.playAudio(1);
			yield return new WaitForSeconds(7f);
			(UserCar.GetComponent<CarUserControl>() as CarUserControl).enabled = true;
			hudController.model.centerText = VRAVEStrings.Follow_Car;
			hudController.EngageManualMode();
			yield return new WaitForSeconds(12f);
			//hudController.Clear();
			hudController.model.centerText = "";
		}

		private IEnumerator SpeedChangesBriefing()
		{
			//Notify the user of speed changes.
			ambientAudioController.Mute();
			audioController.playAudio(2);
			yield return new WaitForSeconds(5f);
			ambientAudioController.UnMute();
			triggers[3].SetActive(false);
		}

		private IEnumerator PassingBriefing()
		{
			//Instruct the user to pass on the next straightaway.
			ambientAudioController.Mute();
			audioController.playAudio(3);
			userCarController.FullTorqueOverAllWheels = 1000f;
			//userCarController.MaxSpeed = 25f;
			yield return new WaitForSeconds(8f);
			hudController.model.centerText = VRAVEStrings.SafelyPassVehicle;
			ambientAudioController.UnMute();
			triggers[10].SetActive(false);
			yield return new WaitForSeconds(6f);
			//hudController.Clear();	
			hudController.model.centerText = "";
		}

		private IEnumerator UserConclusion()
		{
			//End of the user part of the scenario.
			//Allow user to enter AI scenario by pulling right trigger.

			ambientAudioController.Mute();
			audioController.playAudio(4);
			yield return new WaitForSeconds(1f);
			hudController.model.centerText = VRAVEStrings.Right_Paddle_To_Continue;
			yield return new WaitForSeconds(5f);
			ambientAudioController.UnMute();
			triggers[11].SetActive(false);
		}

		private IEnumerator AIBriefing()
		{
			//Introduction to AI scenario
			yield return new WaitForSeconds(2f);
			ambientAudioController.Mute();
			audioController.playAudio(5);
			yield return new WaitForSeconds(5f);
			hudController.model.centerText = VRAVEStrings.Right_Paddle_To_Continue;
			GameObject rightPaddle = GameObject.FindGameObjectWithTag(VRAVEStrings.Right_Paddle);
			(rightPaddle.GetComponent("Halo") as Behaviour).enabled = true;
			yield return new WaitForSeconds(0.5f);
			(rightPaddle.GetComponent("Halo") as Behaviour).enabled = false;
			yield return new WaitForSeconds(0.5f);
			(rightPaddle.GetComponent("Halo") as Behaviour).enabled = true;
			yield return new WaitForSeconds(0.5f);
			(rightPaddle.GetComponent("Halo") as Behaviour).enabled = false;
			yield return new WaitForSeconds(0.5f);
			(rightPaddle.GetComponent("Halo") as Behaviour).enabled = true;
			
			//Ensure paddle glow is turned off.
			yield return new WaitForSeconds(3f);
			(rightPaddle.GetComponent("Halo") as Behaviour).enabled = false;

			ambientAudioController.UnMute();
		}

		private IEnumerator AIPassingBriefing()
		{
			ambientAudioController.Mute();
			audioController.playAudio(6);
			yield return new WaitForSeconds(5f);
		}

		private IEnumerator AIPassingCommand()
		{
			//When allowed, pull the left trigger to initiate passing sequence.
			ambientAudioController.Mute();
			//audioController.playAudio(8);
			yield return new WaitForSeconds(2f);
			GameObject leftPaddle = GameObject.FindGameObjectWithTag(VRAVEStrings.Left_Paddle);
			(leftPaddle.GetComponent("Halo") as Behaviour).enabled = true;
			yield return new WaitForSeconds(0.5f);
			(leftPaddle.GetComponent("Halo") as Behaviour).enabled = false;
			yield return new WaitForSeconds(0.5f);
			(leftPaddle.GetComponent("Halo") as Behaviour).enabled = true;
			yield return new WaitForSeconds(0.5f);
			(leftPaddle.GetComponent("Halo") as Behaviour).enabled = false;
			yield return new WaitForSeconds(0.5f);
			(leftPaddle.GetComponent("Halo") as Behaviour).enabled = true;
			
			//Ensure paddle glow is turned off.
			yield return new WaitForSeconds(3f);
			(leftPaddle.GetComponent("Halo") as Behaviour).enabled = false;
		}

		private IEnumerator CompletedPass()
		{
			hudController.model.centerText = VRAVEStrings.PassCompleted;
			yield return new WaitForSeconds(2f);
			hudController.model.centerText = "";
		}

		private IEnumerator ScenarioEnd()
		{
			//The AI vehicle has successfully demonsrated its adaptive cruise control, 
			//following, and vehicle passing capabilities all with minimal to no human input required.
			ambientAudioController.Mute();
			//audioController.playAudio(9);
			yield return new WaitForSeconds(7f);
			hudController.model.centerText = "Pull right paddle to return to menu.";
			//Pull the right trigger to return to the lobby
		}

		private IEnumerator AdaptiveCruiseControl()
		{
			//Notice how your vehicle accelerates and decelerates as the leading vehicle varies in speed.
			yield return new WaitForSeconds(2f);
			audioController.playAudio(7);
			yield return new WaitForSeconds(1f);
			hudController.model.centerText = VRAVEStrings.Accelerating;
			yield return new WaitForSeconds(5.5f);
			hudController.model.centerText = VRAVEStrings.Decelerating;
			yield return new WaitForSeconds(2f);
			hudController.model.centerText = "";

		}

		private IEnumerator SlowingAtTurn()
		{
			hudController.model.centerText = VRAVEStrings.Decelerating;
			yield return new WaitForSeconds(1f);
			hudController.model.centerText = "";
		}

		//Not used at the moment.
		private IEnumerator SpeedingAfterTurn()
		{
			hudController.model.centerText = VRAVEStrings.Accelerating;
			yield return new WaitForSeconds(1f);
			hudController.model.centerText = "";

		}

		private IEnumerator UserCarSlowdown()
		{
			AIVehicleCarController.MaxSpeed = 10f;
			userCarController.MaxSpeed = 30f;
			while (userCarController.MaxSpeed > 11f)
			{
				yield return new WaitForFixedUpdate();
				userCarController.MaxSpeed--;
			}

		}
	}
}
