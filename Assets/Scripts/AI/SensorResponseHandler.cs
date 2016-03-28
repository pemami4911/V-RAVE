using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace VRAVE
{
public abstract class SensorResponseHandler : MonoBehaviour 
{
		private bool m_enable = false; 
		public bool Enable { get { return m_enable; } set { m_enable = value; } }

		// Defines how a vehicle should react to an obstacle hit
		//
		// In most cases, you will want to disable the sensor response handler at the end of handle()
		public abstract void handle (CarAIControl controller, Dictionary<int, VRAVEObstacle> obstacles,
	                                  float currentSpeed,
	                                  CarAIControl.BrakeCondition brakeCondition);
}
}