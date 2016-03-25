using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace VRAVE
{
public abstract class SensorResponseHandler : MonoBehaviour 
{

	// Defines how a vehicle should react to an obstacle hit
	public abstract void handle (CarAIControl controller, Dictionary<int, VRAVEObstacle>.ValueCollection obstacles,
	                                  float currentSpeed,
	                                  CarAIControl.BrakeCondition brakeCondition);
}
}