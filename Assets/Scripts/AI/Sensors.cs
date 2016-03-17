using UnityEngine;
using System.Collections;

namespace VRAVE  
{
	public class Sensors : MonoBehaviour {

		// adjustable values
		[SerializeField] private float m_sensorsStart = 1f;
		[Range(-120f, 120f)][SerializeField] private float m_leftLongSensorAngle = -45f;
		[Range(-120f, 120f)][SerializeField] private float m_rightLongSensorAngle = 45f;
		[SerializeField] private float m_longSensorLength = 16f;
		[SerializeField] private float m_shortSensorLength = 8f;

		private readonly int numLongRangeSensors = 3;
		// 2 arrays of 13, each 10' apart for total of 120'
		// span. One array is at the front, one is at the back
		private readonly int numShortRangeSensors = 26;

		private Vector3[] longRangeSensorsArray; 
		private Vector3[] shortRangeSensorsArray;

		private void Awake()
		{
			longRangeSensorsArray = new Vector3[numLongRangeSensors];
			shortRangeSensorsArray = new Vector3[numShortRangeSensors];
		}

		public bool Scan(out RaycastHit hit) 
		{
			hit = new RaycastHit ();

			// set short range sensors
			for (int i = 0; i < 2; ++i) 
			{
				int angle = -60;
				int idx = 0;
				int direction;

				if (i == 0) {
					direction = 1;
				} else {
					direction = -1;
				}

				while (true) 
				{
					if (idx == (numShortRangeSensors / 2))
					{
						break;
					} 
						
					Quaternion deviation = Quaternion.AngleAxis (angle, new Vector3 (0, 1, 0));
					shortRangeSensorsArray [(i * numLongRangeSensors / 2) + idx] = deviation * transform.forward * direction;
					//shortRangeSensorsArray [(i * numLongRangeSensors / 2) + idx] += transform.forward * m_sensorsStart;

					++idx;
					angle = angle + 10;
				}
			}

			// long range sensors
			longRangeSensorsArray[0] = transform.position;
			//longRangeSensorsArray [0] += transform.forward * m_sensorsStart;

			longRangeSensorsArray [1] = longRangeSensorsArray [0];
			longRangeSensorsArray [2] = longRangeSensorsArray [0];

			// left angled
			longRangeSensorsArray[1] = Quaternion.AngleAxis (m_leftLongSensorAngle, new Vector3(0, 1, 0)) * longRangeSensorsArray [1];
			// Right angled
			longRangeSensorsArray[2] = Quaternion.AngleAxis (m_rightLongSensorAngle, new Vector3(0, 1, 0)) * longRangeSensorsArray[2];

			// Scan for hits, starting with short range sensors
			// returns the first object hit with a short range sensor
			foreach (Vector3 scan in shortRangeSensorsArray) 
			{
				Debug.DrawRay (transform.position * m_sensorsStart, scan, Color.green);

				if (Physics.Raycast (scan, transform.forward, out hit, m_shortSensorLength)) 
				{
					Debug.Log ("Short range sensor obstacle detection");
					Debug.DrawLine (scan, hit.point, Color.yellow);
					return true;
				}
			}		

			// long sensors
			foreach (Vector3 scan in longRangeSensorsArray) 
			{
				if (Physics.Raycast (scan, transform.forward, out hit, m_longSensorLength)) 
				{
					Debug.Log ("Long sensor obstacle detection");
					Debug.DrawLine (scan, hit.point, Color.yellow);
					return true;
				}
			}

			// no objects detected
			return false;
		}
}
}