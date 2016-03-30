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

        public override void handle(CarAIControl controller, Dictionary<int, VRAVEObstacle> obstacles, float currentSpeed, CarAIControl.BrakeCondition brakeCondition)
        {
            if (!Enable)
            {
                return;
            }
            else
            {
                if (passingCheck(obstacles))
                {
                    Enable = false;
                    controller.IsPassing = true;
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

        private bool passingCheck(Dictionary<int, VRAVEObstacle> obstacles)
        {
            Debug.Log("Scanning!");
            if (obstacles.ContainsKey(7) && obstacles[7].obstacleTag.Equals("AI_Car"))
            {
                Debug.Log("Sensor 7 Hit");
                if (obstacles[7].Distance <= 10f)
                {
                    Debug.Log("Sensor 7 in length");
                    if (!obstacles.ContainsKey(8) || ((obstacles.ContainsKey(8) && obstacles[8].Distance >= 20)))
                    {
                        Debug.Log("Passing Conditions Met!");
                        return true;
                    }
                }
            }
            return false;
        }
    }
}