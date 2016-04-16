using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace VRAVE
{
	public class TrashcanSensorResponseHandler : SensorResponseHandler {
	
		private int scanningRange_Left = 0;
		private int scanningRange_Right = 23;

		// check sensors 8 - 17, ignore otherwise
		// get the distance to the trash can and angle to the trash can
		// if angle is +, steer left
		// if angle is -, steer right ?
		// compute steer amount as fn of distance, angle, and base steer amount (10 degrees?)
		public override void handle (CarAIControl controller, Dictionary<int, VRAVEObstacle> obstacles,
			float currentSpeed,
			CarAIControl.BrakeCondition brakeCondition) {

			if (!Enable) {
				return;
			}
			controller.IsAvoidingObstacle = false;
			VRAVEObstacle obs;

			for (int i = scanningRange_Left; i <= scanningRange_Right; ++i) {

				if (obstacles.TryGetValue(i, out obs)) {
					if (obs.obstacleTag.Equals (VRAVEStrings.Obstacle)) {
						// found trash can
						controller.IsAvoidingObstacle = true;

						//float distance = obs.Distance;

						// calculate the local-relative position of the target, to steer away from
						Vector3 localTarget = transform.InverseTransformPoint (obs.obstacle.point);

						// work out the local angle towards the target
						float targetAngle = Mathf.Atan2 (localTarget.x, localTarget.z) * Mathf.Rad2Deg;

						if (targetAngle <= 0)
							targetAngle = -90 + targetAngle;
						else
							targetAngle = 90 - targetAngle;
						
						controller.ObstacleAvoidanceSteerAmount = -targetAngle * 2f;

						return;						
					}
					
				}
			}
		}
}
}