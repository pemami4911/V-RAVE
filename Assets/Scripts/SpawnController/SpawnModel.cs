using UnityEngine;
using System.Collections.Generic;

namespace VRAVE {

public class SpawnModel {

	//Stores the mapping between the Resource string and a mapping of coordinates to rotation
	//at which to spawn that resource
	//NOTE: may want to upgrade the List to a HashSet in the future
	private Dictionary<string, ICollection<KeyValuePair<Vector3, Quaternion>>> spawnMap;

	//the name of the resource to use. 
	//NOTE: the resource must be stored in the Resources folder
	public static string AI_car_resource = "AICar";

	public static Quaternion default_quaternion = Quaternion.Euler(0, 0, 0);

	public SpawnModel() {
		KeyValuePair<Vector3, Quaternion> aiCar0 = new KeyValuePair<Vector3, Quaternion>(new Vector3 (0, 0, 0), default_quaternion); 
		KeyValuePair<Vector3, Quaternion> aiCar1 = new KeyValuePair<Vector3, Quaternion>(new Vector3 (1, 1, 1), default_quaternion); 

		ICollection<KeyValuePair<Vector3, Quaternion>> AIcoordinateRotationPairs = new List<KeyValuePair<Vector3, Quaternion>> ();
		AIcoordinateRotationPairs.Add (aiCar0);
		AIcoordinateRotationPairs.Add (aiCar1);

		spawnMap.Add (AI_car_resource, AIcoordinateRotationPairs);
	}

	public ICollection<string> getResourceStrings() {
		return spawnMap.Keys;
	}
		
	public ICollection<KeyValuePair<Vector3, Quaternion>> getCoordinateRotationPairs(string resource) {
		return spawnMap [resource];

	}
}

}