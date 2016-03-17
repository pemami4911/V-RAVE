using UnityEngine;
using System.Collections;

namespace VRAVE
{
	public static class ObstacleHandler : object {

		// Defines how a vehicle should react to an obstacle hit
		public static void handleObstacle(CarAIControl controller, UnityEngine.RaycastHit obstacle, float currentSpeed, CarAIControl.BrakeCondition brakeCondition)
		{
			if (obstacle.collider.CompareTag("Obstacle") && 
				(currentSpeed / 2) > (obstacle.transform.position - controller.transform.position).magnitude  &&
				brakeCondition != CarAIControl.BrakeCondition.TargetDistance)
			{
				handleUnavoidableObstacle (controller, obstacle);
			}
		}

		private static void handleUnavoidableObstacle(CarAIControl controller, UnityEngine.RaycastHit obstacle)
		{
			controller.SetTarget (obstacle.transform, true);
		}

		private static void handleAvoidableObstacle()
		{
		}
}
}