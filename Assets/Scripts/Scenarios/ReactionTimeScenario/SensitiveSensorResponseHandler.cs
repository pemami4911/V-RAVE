using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace VRAVE
{
public class SensitiveSensorResponseHandler : SensorResponseHandler {
		
		public override void handle (CarAIControl controller, Dictionary<int, VRAVEObstacle> obstacles,
			float currentSpeed,
			CarAIControl.BrakeCondition brakeCondition) {
			controller.Driving = false;
		}
}
}