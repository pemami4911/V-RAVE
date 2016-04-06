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
            if (obstacles.ContainsKey(24) && obstacles[24].obstacleTag.Equals(VRAVEStrings.AI_Car))
            {
                if (obstacles[24].Distance <= 10f)
                {
                    if (!obstacles.ContainsKey(26) || ((obstacles.ContainsKey(26) && obstacles[9].Distance >= 26)))
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