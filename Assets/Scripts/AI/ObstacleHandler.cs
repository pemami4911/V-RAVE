using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace VRAVE
{
public abstract class ObstacleHandler : MonoBehaviour 
{

	// Defines how a vehicle should react to an obstacle hit
	public abstract void handleObstacles (CarAIControl controller, Dictionary<int, VRAVEObstacle>.ValueCollection obstacles,
	                                  float currentSpeed,
	                                  CarAIControl.BrakeCondition brakeCondition);
}
}