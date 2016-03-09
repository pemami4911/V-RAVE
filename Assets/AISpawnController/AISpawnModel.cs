using UnityEngine;
using System.Collections.Generic;

public class AISpawnModel {

	public List<Vector3> coordinates {get; set;}

	public AISpawnModel() {
		coordinates = new List<Vector3> ();
		Vector3 aiCar0 = new Vector3 (0, 0, 0); 


		coordinates.Add(aiCar0);
	}
}
