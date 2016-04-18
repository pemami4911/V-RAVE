using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using MonsterLove.StateMachine;
using System;

namespace VRAVE
{
    public class FollowingSensorResponseHandler : SensorResponseHandler
    {

        [SerializeField] private float followDistance = 10;

        private StateBehaviour scenario;

        public override void handle(CarAIControl controller, Dictionary<int, VRAVEObstacle> obstacles, float currentSpeed, CarAIControl.BrakeCondition brakeCondition)
        {
            if (!Enable)
            {
                return;
            }
            else
            {
                if(obstacles.ContainsKey(9) && obstacles[9].obstacleTag.Equals(VRAVEStrings.Obstacle) && (obstacles[9].Distance <= 25))
                {
					//Don't turn
                    Enable = false;
                    controller.TooClose = false;
                    controller.TooFar = false;
                    controller.AccelMultiplier = 0f;

                    //GetComponentInParent<CarController>().MaxSpeed = AISpeed;
                    return;
                }
                else if (obstacles.ContainsKey(7) && obstacles[7].obstacleTag.Equals(VRAVEStrings.AI_Car))
                {
                    CarController userCarController = GetComponentInParent<CarController>();
                    float AISpeed = obstacles[7].obstacle.collider.GetComponentInParent<CarController>().MaxSpeed;
                    float differential = obstacles[7].Distance - followDistance;
                    Debug.Log("Distance: " + obstacles[7].Distance + "   Differential: " + differential);
                    if (Mathf.Abs(differential) > 5f)
                    {
                        if (differential > 0f)  //too far away
                        {
                            //userCarController.MaxSpeed = 1.2f * AISpeed;
                            controller.AccelMultiplier =  (1.0f + Mathf.Abs(controller.AccelMultiplier)) * 1.05f;
                            controller.TooFar = true;
                            Debug.Log("Too far : " + controller.AccelMultiplier);

                        }
                        else if (differential < 0f) //(differential < 0)  //too close
                        {
                            //userCarController.MaxSpeed = 1.2f * AISpeed;
                            controller.AccelMultiplier = -1.0f + -1f * Mathf.Abs(controller.AccelMultiplier) * (1.0f/1.05f);
                            //controller.AccelMultiplier = 0;
                            controller.TooClose = true;
                            Debug.Log("Too close : " + controller.AccelMultiplier);
                        }
                    }
                    else if (Math.Abs(differential) > 1f)
                    {
                        if (differential > 0f)  //too far away
                        {
                            userCarController.MaxSpeed = 1.5f * AISpeed;
                            //controller.AccelMultiplier = controller.AccelMultiplier / 2;
                            controller.TooFar = true;
                            Debug.Log("Too far : " + controller.AccelMultiplier);

                        }
                        else if (differential < 0f) //(differential < 0)  //too close
                        {
                            userCarController.MaxSpeed = 1.5f * AISpeed;
                            //controller.AccelMultiplier = controller.AccelMultiplier / 2;
                            controller.TooClose = true;
                            Debug.Log("Too close : " + controller.AccelMultiplier);
                        }
                    }
                    else
                    {
                        //In the correct following range. Do nothing.
                        controller.TooClose = false;
                        controller.TooFar = false;

                        GetComponentInParent<CarController>().MaxSpeed = AISpeed;
                        //obstacles[7].obstacle.collider.GetComponentInParent<CarController>().MaxSpeed = AISpeed;
                    }
                }
            }
        }
    


        // Use this for initialization
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {

        }
    }
}