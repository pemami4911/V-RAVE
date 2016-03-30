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
                if (obstacles.ContainsKey(7) && obstacles[7].obstacleTag.Equals(VRAVEStrings.AI_Car))
                {
                    float AISpeed = obstacles[7].obstacle.collider.GetComponentInParent<CarController>().MaxSpeed;
                    controller.GetComponent<CarController>().MaxSpeed = 1.2f * AISpeed;
                    float differential = obstacles[7].Distance - followDistance;
                    Debug.Log("Distance: " + obstacles[7].Distance + "   Differential: " + differential);
                    if (Mathf.Abs(differential) > 1f)

                    {
                        if (differential > 0f)  //too far away
                        {
                            controller.AccelMultiplier++;
                            controller.TooFar = true;
                            Debug.Log("Too far : " + controller.AccelMultiplier);

                        }
                        else //(differential < 0)  //too close
                        {
                            controller.AccelMultiplier--;
                            controller.TooClose = true;
                            Debug.Log("Too close : " + controller.AccelMultiplier);
                        }
                    }
                    else
                    {
                        //In the correct following range. Do nothing.
                        controller.TooClose = false;
                        controller.TooClose = false;
                        obstacles[7].obstacle.collider.GetComponentInParent<CarController>().MaxSpeed = AISpeed;
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