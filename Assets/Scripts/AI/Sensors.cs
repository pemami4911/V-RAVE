using UnityEngine;
using System.Collections;

namespace VRAVE  
{
	public class Sensors : MonoBehaviour {

		// adjustable values
		[SerializeField] private float m_sensorsStart = 1f;
		[Range(10f, 60f)][SerializeField] private float m_longSensorAngleDelta = 45f;
		[Range(10f, 60f)][SerializeField] private float m_shortSensorAngleDelta = 25f;
		[SerializeField] private float m_longSensorLength = 16f;
		[SerializeField] private float m_shortSensorLength = 8f;
		[SerializeField] private float m_sensorHeight = 0.5f;
		[SerializeField] private bool m_shortRangeSensorsEnable = true;
		[SerializeField] private bool m_longRangeSensorsEnable = true;


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
			Vector3 sensorsStart = transform.position; 
			sensorsStart.y += m_sensorHeight;

			hit = new RaycastHit ();

			if (m_shortRangeSensorsEnable) {
				// set short range sensors
				for (int i = 0; i < 2; ++i) {
					float delta = m_shortSensorAngleDelta / (numShortRangeSensors / 2);
					float angle = -(m_shortSensorAngleDelta / 2);
					int idx = 0;
					int direction;

					if (i == 0) {
						direction = -1;
					} else {
						direction = 1;
					}

					while (true) {
						if (idx == (numShortRangeSensors / 2)) {
							break;
						} 
							
						Quaternion deviation = Quaternion.AngleAxis (angle, new Vector3 (0, 1, 0));
						shortRangeSensorsArray [(i * numShortRangeSensors / 2) + idx] = deviation * transform.forward * direction;

						++idx;
						angle = angle + delta;
					}
				}

				// Scan for hits, starting with short range sensors
				// returns the first object hit with a short range sensor
				foreach (Vector3 scan in shortRangeSensorsArray) {
	
					Debug.DrawRay (sensorsStart, scan * m_shortSensorLength, Color.green);

					if (Physics.Raycast (sensorsStart, scan, out hit, m_shortSensorLength)) {
						Debug.Log ("Short range sensor obstacle detection");
						Debug.DrawLine (sensorsStart, hit.point, Color.yellow);
						return true;
					}
				}	
			}

			if (m_longRangeSensorsEnable) 
			{
				// long range sensors
				longRangeSensorsArray [0] = transform.forward * m_sensorsStart;
				//longRangeSensorsArray [0] += transform.forward * m_sensorsStart;

				longRangeSensorsArray [1] = longRangeSensorsArray [0];
				longRangeSensorsArray [2] = longRangeSensorsArray [0];

				// left angled
				longRangeSensorsArray [1] = Quaternion.AngleAxis (-m_longSensorAngleDelta / 2, new Vector3 (0, 1, 0)) * longRangeSensorsArray [1];
				// Right angled
				longRangeSensorsArray [2] = Quaternion.AngleAxis (m_longSensorAngleDelta / 2, new Vector3 (0, 1, 0)) * longRangeSensorsArray [2];

				// long sensors
				foreach (Vector3 scan in longRangeSensorsArray) {
					Debug.DrawRay (sensorsStart, scan * m_longSensorLength, Color.red);

					if (Physics.Raycast (sensorsStart, scan, out hit, m_longSensorLength)) {
						Debug.Log ("Long sensor obstacle detection");
						Debug.DrawLine (sensorsStart, hit.point, Color.yellow);
						return true;
					}
				}
			}

			// no objects detected
			return false;
		}
}
}