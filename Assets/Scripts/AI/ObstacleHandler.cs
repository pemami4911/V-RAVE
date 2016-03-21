using UnityEngine;
using System.Collections;

namespace VRAVE
{
	public static class ObstacleHandler : object {

		// Defines how a vehicle should react to an obstacle hit
		public static void handleObstacle(CarAIControl controller, UnityEngine.RaycastHit obstacle, 
			UnityEngine.RaycastHit farObstacle,
			OBSTACLE_TYPE type,
			float currentSpeed,
			CarAIControl.BrakeCondition brakeCondition)
		{
			if (type == OBSTACLE_TYPE.UNAVOIDABLE &&
			    brakeCondition != CarAIControl.BrakeCondition.TargetDistance) 
			{
				handleUnavoidableObstacle (controller, obstacle);
			} 
			else if (type == OBSTACLE_TYPE.AVOIDABLE) 
			{
				handleAvoidableObstacle (controller, farObstacle);
			} 
		}

		private static void handleUnavoidableObstacle(CarAIControl controller, UnityEngine.RaycastHit obstacle)
		{
			controller.SetTarget (obstacle.transform, true);
		}

		private static void handleAvoidableObstacle(CarAIControl controller, UnityEngine.RaycastHit obstacle)
		{
			// Add logic for telling the AI Controller how to avoid "avoidable" obstacles here
		}
}
}