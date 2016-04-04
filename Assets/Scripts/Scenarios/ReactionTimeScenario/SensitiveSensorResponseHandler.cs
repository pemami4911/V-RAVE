using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace VRAVE
{
public class SensitiveSensorResponseHandler : SensorResponseHandler {
		
		public override void handle (CarAIControl controller, Dictionary<int, VRAVEObstacle> obstacles,
			float currentSpeed,
			CarAIControl.BrakeCondition brakeCondition) {

			if (!Enable) {
				return;
			}

			foreach (VRAVEObstacle vo in obstacles.Values) {
				if (vo.obstacleTag.Equals(VRAVEStrings.Crazy_AI_Car)) {
					controller.Driving = false;
					return;
				}
			}
		}
}
}