using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace VRAVE  
{
	public class VRAVEObstacle 
	{
		public string obstacleTag {get; set;}
		public RaycastHit obstacle { get; set; }
		// Distance from the car to the obstacle
		public float Distance { get; set; }
	}

	// The long range sensors are for obstacles that we can steer around
	// and the short range sensors are for obstacles that we can't avoid
	public class Sensors : MonoBehaviour 
	{

		// adjustable values
		[SerializeField] private float m_sensorsStart = 1f;
		[Range(10f, 60f)][SerializeField] private float m_shortSensorAngleDelta = 30f;
		[SerializeField] private float m_longSensorLength = 45f;
		[SerializeField] private float m_shortSensorLength = 10f;
		[SerializeField] private bool m_shortRangeSensorsEnable = true;
		[SerializeField] private bool m_longRangeSensorsEnable = true;
		[SerializeField] private float m_lateralSensorShift = 1f;

		private readonly float shortRangeSensorsHeight = 0.25f;
		private readonly float longRangeSensorsHeight = 0.6f;
		private readonly int numLongRangeSensors = 3;
		private readonly int numShortRangeSensors = 6;

		private Vector3[] longRangeSensorsArray; 
		private Vector3[] shortRangeSensorsArray;
		private Vector3 passingSensor; 


		private void Awake()
		{
			longRangeSensorsArray = new Vector3[numLongRangeSensors];
			shortRangeSensorsArray = new Vector3[numShortRangeSensors];
			passingSensor = new Vector3();
		}

		private void InitShortRangeSensors()
		{
			// Short range sensors
			float delta = m_shortSensorAngleDelta / (numShortRangeSensors-1);
			float angle = -(m_shortSensorAngleDelta / 2);

			for (int idx = 0; idx < numShortRangeSensors; ++idx)
			{
				Quaternion deviation = Quaternion.AngleAxis (angle, new Vector3 (0, 1, 0));
				shortRangeSensorsArray [idx] = deviation * transform.forward;
				angle = angle + delta;
			}
		}

		private void InitLongRangeSensors()
		{
			// long range sensors
			longRangeSensorsArray [0] = transform.forward * m_sensorsStart;
			longRangeSensorsArray [1] = longRangeSensorsArray [0];
			longRangeSensorsArray [2] = longRangeSensorsArray [0];
		}


		public bool Scan (out Dictionary<int, VRAVEObstacle> obstacles)
		{
			obstacles = new Dictionary<int, VRAVEObstacle> ();
				
			if (m_shortRangeSensorsEnable) {
				RaycastHit shortSensorsHit;

				Vector3 shortRangeSensorsStart = transform.position; 
				shortRangeSensorsStart.y += shortRangeSensorsHeight;

				InitShortRangeSensors ();

				// Scan for nearby obstacles
				foreach (Vector3 scan in shortRangeSensorsArray) {

					Debug.DrawRay (shortRangeSensorsStart, scan * m_shortSensorLength, Color.green);

					if (Physics.Raycast (shortRangeSensorsStart, scan, out shortSensorsHit, m_shortSensorLength)) {

						if (shortSensorsHit.collider.CompareTag (VRAVEStrings.Obstacle) ||
							shortSensorsHit.collider.CompareTag (VRAVEStrings.AI_Car)) {
							if (!obstacles.ContainsKey(shortSensorsHit.collider.GetHashCode()))
							{
								Debug.DrawLine (shortRangeSensorsStart, shortSensorsHit.point, Color.yellow);
								VRAVEObstacle vo = new VRAVEObstacle ();
								vo.obstacle = shortSensorsHit;
								vo.obstacleTag = shortSensorsHit.collider.tag;
								vo.Distance = (shortSensorsHit.point - transform.position).magnitude;
								obstacles.Add (shortSensorsHit.collider.GetHashCode (), vo);
							}
						}
					}
				}	
			}

			if (m_longRangeSensorsEnable) {
				RaycastHit longSensorsHit;

				Vector3 longRangeSensorsStart = transform.position; 
				longRangeSensorsStart.y += longRangeSensorsHeight;

				// long sensors
				InitLongRangeSensors ();

				for (uint i = 0; i < numLongRangeSensors; ++i) {

					Vector3 sensorStart = longRangeSensorsStart;

					if (i == 1) {
						sensorStart += transform.right * m_lateralSensorShift;
					} else if (i == 2) {
						sensorStart -= transform.right * m_lateralSensorShift;
					}

					Debug.DrawRay (sensorStart, longRangeSensorsArray [i] * m_longSensorLength, Color.red);

					if (Physics.Raycast (sensorStart, longRangeSensorsArray [i], out longSensorsHit, m_longSensorLength)) {
						if (longSensorsHit.collider.CompareTag (VRAVEStrings.Obstacle) ||
							longSensorsHit.collider.CompareTag (VRAVEStrings.AI_Car)) 
						{
							if (!obstacles.ContainsKey (longSensorsHit.collider.GetHashCode ())) 
							{
								Debug.DrawLine (sensorStart, longSensorsHit.point, Color.yellow);
								VRAVEObstacle vo = new VRAVEObstacle ();
								vo.obstacle = longSensorsHit;
								vo.obstacleTag = longSensorsHit.collider.tag;
								vo.Distance = (longSensorsHit.point - transform.position).magnitude;
								obstacles.Add (longSensorsHit.collider.GetHashCode(), vo);
							}
						}
					}
				}
			}

			return obstacles.Count == 0 ? false : true;
		}

		public bool PassingSensor(out VRAVEObstacle obs)
		{
			obs = new VRAVEObstacle ();
			RaycastHit shortSensorsHit;

			Quaternion passingSensorAngle = Quaternion.AngleAxis (-(m_shortSensorAngleDelta / 2), new Vector3(0, 1, 0));
			passingSensor = passingSensorAngle * -transform.forward;

			Vector3 shortRangeSensorsStart = transform.position; 
			shortRangeSensorsStart.y += shortRangeSensorsHeight;

			// Passing sensor
			Debug.DrawRay(shortRangeSensorsStart, passingSensor * m_shortSensorLength, Color.green);

			if (Physics.Raycast (shortRangeSensorsStart, passingSensor, out shortSensorsHit, m_shortSensorLength))
			{
				if (shortSensorsHit.collider.CompareTag (VRAVEStrings.AI_Car)) 
				{
					Debug.DrawLine (shortRangeSensorsStart, shortSensorsHit.point, Color.yellow);
					obs.obstacle = shortSensorsHit;
					obs.obstacleTag = shortSensorsHit.collider.tag;	// this sensor is used to check whether you've passed a car
					obs.Distance = (shortSensorsHit.point - transform.position).magnitude;	
					return true;
				}
			}
			return false;
		}

		// can be used to find the nearest obstacle being tracked
		public static bool nearestObstacle(Dictionary<int, VRAVEObstacle>.ValueCollection obs, out VRAVEObstacle nearest) 
		{
			nearest = new VRAVEObstacle ();

			if (obs.Count == 0)
				return false;
		
			float min = Mathf.Infinity;

			foreach (VRAVEObstacle o in obs) {
				if (o.Distance < min) {
					min = o.Distance;
					nearest = o;
				}
			}

			return true;
		}
	}
}