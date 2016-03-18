using UnityEngine;
using System.Collections;

namespace VRAVE  
{
	public enum OBSTACLE_TYPE 
	{
		NONE = 0,
		AVOIDABLE = 1,
		UNAVOIDABLE = 2,
	}

	public class Sensors : MonoBehaviour {

		// adjustable values
		[SerializeField] private float m_sensorsStart = 1f;
		[Range(10f, 60f)][SerializeField] private float m_longSensorAngleDelta = 10f;
		[Range(10f, 60f)][SerializeField] private float m_shortSensorAngleDelta = 30f;
		[SerializeField] private float m_longSensorLength = 45f;
		[SerializeField] private float m_shortSensorLength = 10f;
		[SerializeField] private bool m_shortRangeSensorsEnable = true;
		[SerializeField] private bool m_longRangeSensorsEnable = true;

		private readonly float shortRangeSensorsHeight = 0.25f;
		private readonly float longRangeSensorsHeight = 0.6f;
		private readonly int numLongRangeSensors = 5;
		private readonly int numShortRangeSensors = 12;
		private readonly float lateralSensorShift = 2f;

		private Vector3[] longRangeSensorsArray; 
		private Vector3[] shortRangeSensorsArray;
		private string obstacleLabel = "Obstacle";

		private void Awake()
		{
			longRangeSensorsArray = new Vector3[numLongRangeSensors];
			shortRangeSensorsArray = new Vector3[numShortRangeSensors];
		}

		public bool Scan(out RaycastHit shortSensorsHit, out RaycastHit longSensorsHit, out OBSTACLE_TYPE o) 
		{
			shortSensorsHit = new RaycastHit ();
			longSensorsHit = new RaycastHit ();
			o = OBSTACLE_TYPE.NONE;

			if (m_shortRangeSensorsEnable) 
			{
				Vector3 shortRangeSensorsStart = transform.position; 
				shortRangeSensorsStart.y += shortRangeSensorsHeight;

				float delta = m_shortSensorAngleDelta / numShortRangeSensors;
				float angle = -(m_shortSensorAngleDelta / 2);

				for (int idx = 0; idx < numShortRangeSensors; ++idx)
				{
						
					Quaternion deviation = Quaternion.AngleAxis (angle, new Vector3 (0, 1, 0));
					shortRangeSensorsArray [idx] = deviation * transform.forward;
					angle = angle + delta;
				}
					
				// Scan for nearby obstacles
				foreach (Vector3 scan in shortRangeSensorsArray) {
	
					Debug.DrawRay (shortRangeSensorsStart, scan * m_shortSensorLength, Color.green);

					if (Physics.Raycast (shortRangeSensorsStart, scan, out shortSensorsHit, m_shortSensorLength)) {
						if (shortSensorsHit.collider.CompareTag (obstacleLabel)) 
						{
							Debug.DrawLine (shortRangeSensorsStart, shortSensorsHit.point, Color.yellow);
							o = OBSTACLE_TYPE.UNAVOIDABLE;
							return true;
						}
					}
				}	
			}

			if (m_longRangeSensorsEnable) 
			{
				Vector3 longRangeSensorsStart = transform.position; 
				longRangeSensorsStart.y += longRangeSensorsHeight;

				// long range sensors
				longRangeSensorsArray [0] = transform.forward * m_sensorsStart;
				longRangeSensorsArray [1] = longRangeSensorsArray [0];
				longRangeSensorsArray [2] = longRangeSensorsArray [0];
				longRangeSensorsArray [3] = longRangeSensorsArray [0];
				longRangeSensorsArray [4] = longRangeSensorsArray [0];

				// left shift 
				longRangeSensorsArray [3] -= transform.right * lateralSensorShift;
				// right shift
				longRangeSensorsArray [4] += transform.right * lateralSensorShift;

				// left angled
				longRangeSensorsArray [1] = Quaternion.AngleAxis (-m_longSensorAngleDelta / 2, new Vector3 (0, 1, 0)) * longRangeSensorsArray [1];
				// Right angled
				longRangeSensorsArray [2] = Quaternion.AngleAxis (m_longSensorAngleDelta / 2, new Vector3 (0, 1, 0)) * longRangeSensorsArray [2];



				// long sensors
				foreach (Vector3 scan in longRangeSensorsArray) {
					Debug.DrawRay (longRangeSensorsStart, scan * m_longSensorLength, Color.red);

					if (Physics.Raycast (longRangeSensorsStart, scan, out longSensorsHit, m_longSensorLength)) 
					{
						if (longSensorsHit.collider.CompareTag (obstacleLabel)) 
						{
							Debug.DrawLine (longRangeSensorsStart, longSensorsHit.point, Color.yellow);
							o = OBSTACLE_TYPE.AVOIDABLE;
							return true;
						}
					}
				}
			}

			return false;
		}
}
}