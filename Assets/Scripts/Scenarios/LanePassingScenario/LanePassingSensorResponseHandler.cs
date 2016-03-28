using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using MonsterLove.StateMachine;
using System;

namespace VRAVE
{
    public class LanePassingSensorResponseHandler : SensorResponseHandler
    {

        private StateBehaviour scenario;

        private bool handlerEnabled = false;
        

        public override void handle(CarAIControl controller, Dictionary<int, VRAVEObstacle> obstacles, float currentSpeed, CarAIControl.BrakeCondition brakeCondition)
        {
            if (!handlerEnabled)
            {
                return;
            }

            Debug.Log("Scanning!");
            if (obstacles.ContainsKey(7) && obstacles[7].obstacleTag.Equals("AI_Car"))
            {
                Debug.Log("Sensor 7 Hit");
                if (obstacles[7].Distance <= 10f)
                {
                    Debug.Log("Sensor 7 in length");
                    if (!obstacles.ContainsKey(8) || ((obstacles.ContainsKey(8) && obstacles[8].Distance >= 10)))
                    {
                        Debug.Log("Sensor 8 good!");

                        Debug.Log("Passing Conditions Met!");

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

        void PassingCheck(Dictionary<int, VRAVEObstacle>.ValueCollection obstacles)
        {
            Debug.Log("Scanning!");
            if (obstacles.ContainsKey(7) && obstacles[7].obstacleTag.Equals("AI_Car"))
            {
                Debug.Log("Sensor 7 Hit");
                if (obstacles[7].Distance <= 10f)
                {
                    Debug.Log("Sensor 7 in length");
                    if (!obstacles.ContainsKey(8) || ((obstacles.ContainsKey(8) && obstacles[8].Distance >= 10)))
                    {
                        Debug.Log("Sensor 8 good!");

                        Debug.Log("Passing Conditions Met!");

                    }
                }
            }
        }
    }
}