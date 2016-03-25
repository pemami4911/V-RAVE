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

	public class VRAVESensor
	{
		private int m_ID; 
		private Vector3 m_Direction;

		public int ID {get { return m_ID; } set { m_ID = value; }}
		public Vector3 Direction { get { return m_Direction; } set { m_Direction = value; } }

		public VRAVESensor(int id, Vector3 dir)
		{
			ID = id;
			Direction = dir;
		}
	}

	// The long range sensors are for obstacles that we can steer around
	// and the short range sensors are for obstacles that we can't avoid
	public class Sensors : MonoBehaviour 
	{

		// adjustable values
		[SerializeField] private float m_sensorsStart = 1f;
		[Range(10f, 90f)][SerializeField] private float m_shortSensorAngleDelta = 30f;
		[SerializeField] private float m_longSensorLength = 45f;
		[SerializeField] private float m_shortSensorLength = 10f;
		[SerializeField] private bool m_shortRangeSensorsEnable = true;
		[SerializeField] private bool m_longRangeSensorsEnable = true;
		[SerializeField] private float m_lateralSensorShift = 1f;
		[Range(10f, 90f)][SerializeField] private float m_PassingSensorAngle = 15f;

		private readonly float shortRangeSensorsHeight = 0.25f;
		private readonly float longRangeSensorsHeight = 0.6f;

		/* IDs for look-up */

		// the reverse-facing right side sensor is sensor 0
		// short-range sensors are integers from 1 to n, n = number of short range sensors
		// long-range sensors are integers from n+1 to m, where (m - n + 1) is the number of long range sensors
		private readonly int numPassingSensors = 1;
		private readonly int numShortRangeSensors = 6;
		private readonly int numLongRangeSensors = 3;

		private VRAVESensor[] longRangeSensorsArray; 
		private VRAVESensor[] shortRangeSensorsArray;

		private void Awake()
		{
			longRangeSensorsArray = new VRAVESensor[numLongRangeSensors];
			shortRangeSensorsArray = new VRAVESensor[numShortRangeSensors + numPassingSensors];
		}

		private void InitShortRangeSensors()
		{
			// Short range sensors
			float delta = m_shortSensorAngleDelta / (numShortRangeSensors-1);
			float angle = -(m_shortSensorAngleDelta / 2);

			for (int idx = numPassingSensors; idx <= numShortRangeSensors; ++idx)
			{
				Quaternion deviation = Quaternion.AngleAxis (angle, new Vector3 (0, 1, 0));
				shortRangeSensorsArray [idx] = new VRAVESensor(idx, deviation * transform.forward);
				angle = angle + delta;
			}

			Quaternion passingSensorRot = Quaternion.AngleAxis (-m_PassingSensorAngle, new Vector3(0, 1, 0));
			// passing sensor is 0th
			shortRangeSensorsArray[0] = new VRAVESensor(0, passingSensorRot * -transform.forward);
		}

		private void InitLongRangeSensors()
		{
			// long range sensors
			longRangeSensorsArray [0] = new VRAVESensor(numPassingSensors + numShortRangeSensors, transform.forward * m_sensorsStart);
			longRangeSensorsArray [1] = new VRAVESensor(numPassingSensors + numShortRangeSensors + 1, longRangeSensorsArray [0].Direction);
			longRangeSensorsArray [2] = new VRAVESensor(numPassingSensors + numShortRangeSensors + 2, longRangeSensorsArray [0].Direction);
		}
			
		public bool Scan (out Dictionary<int, VRAVEObstacle> obstacles)
		{
			obstacles = new Dictionary<int, VRAVEObstacle> ();
				
			if (m_shortRangeSensorsEnable) 
			{
				RaycastHit shortSensorsHit;
				Vector3 shortRangeSensorsStart = transform.position; 
				shortRangeSensorsStart.y += shortRangeSensorsHeight;

				InitShortRangeSensors ();

				// Scan for nearby obstacles
				foreach (VRAVESensor scan in shortRangeSensorsArray) 
				{

					Debug.DrawRay (shortRangeSensorsStart, scan.Direction * m_shortSensorLength, Color.green);

					if (Physics.Raycast (shortRangeSensorsStart, scan.Direction, out shortSensorsHit, m_shortSensorLength)) 
					{
						if (shortSensorsHit.collider.CompareTag (VRAVEStrings.Obstacle) ||
							shortSensorsHit.collider.CompareTag (VRAVEStrings.AI_Car)) 
						{
							if (!obstacles.ContainsKey(scan.ID))
							{
								Debug.DrawLine (shortRangeSensorsStart, shortSensorsHit.point, Color.yellow);
								VRAVEObstacle vo = new VRAVEObstacle ();
								vo.obstacle = shortSensorsHit;
								vo.obstacleTag = shortSensorsHit.collider.tag;
								vo.Distance = (shortSensorsHit.point - transform.position).magnitude;
								obstacles.Add (scan.ID, vo);
							}
						}
					}
				}	
			}

			if (m_longRangeSensorsEnable) 
			{
				RaycastHit longSensorsHit;

				Vector3 longRangeSensorsStart = transform.position; 
				longRangeSensorsStart.y += longRangeSensorsHeight;

				// long sensors
				InitLongRangeSensors ();

				for (uint i = 0; i < numLongRangeSensors; ++i) 
				{
					Vector3 sensorStart = longRangeSensorsStart;

					if (i == 1) 
					{
						sensorStart += transform.right * m_lateralSensorShift;
					} 
					else if (i == 2) 
					{
						sensorStart -= transform.right * m_lateralSensorShift;
					}

					Debug.DrawRay (sensorStart, longRangeSensorsArray [i].Direction * m_longSensorLength, Color.red);

					if (Physics.Raycast (sensorStart, longRangeSensorsArray [i].Direction, out longSensorsHit, m_longSensorLength)) 
					{
						if (longSensorsHit.collider.CompareTag (VRAVEStrings.Obstacle) ||
							longSensorsHit.collider.CompareTag (VRAVEStrings.AI_Car)) 
						{
							if (!obstacles.ContainsKey (longRangeSensorsArray[i].ID)) 
							{
								Debug.DrawLine (sensorStart, longSensorsHit.point, Color.yellow);
								VRAVEObstacle vo = new VRAVEObstacle ();
								vo.obstacle = longSensorsHit;
								vo.obstacleTag = longSensorsHit.collider.tag;
								vo.Distance = (longSensorsHit.point - transform.position).magnitude;
								obstacles.Add (longRangeSensorsArray[i].ID, vo);
							}
						}
					}
				}
			}

			return obstacles.Count == 0 ? false : true;
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