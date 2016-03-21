using UnityEngine;
using System.Collections.Generic;

namespace VRAVE {

	public class SpawnModel {

		//Stores the mapping between the Resource string and a mapping of coordinates to rotation
		private List<SpawnTriple> initialSpawns { get; private set;}
		private List<SpawnTriple> onDemandSpawns { get; }

		//the name of the resource to use. 
		//NOTE: the resource must be stored in the Resources folder
		public static string AI_car_resource = "AICar";

		public static Quaternion default_quaternion = Quaternion.Euler(0, 0, 0);

		public SpawnModel() {
			SpawnTriple aiCar0 = new SpawnTriple (AI_car_resource, new Vector3 (0, 0, 0), default_quaternion); 
			SpawnTriple aiCar1 = new SpawnTriple (AI_car_resource, new Vector3 (1, 1, 1), default_quaternion); 

			SpawnTriple aiCar2 = new SpawnTriple (AI_car_resource, new Vector3 (5, 5, 5), default_quaternion); 
			SpawnTriple aiCar3 = new SpawnTriple (AI_car_resource, new Vector3 (10, 10, 10), default_quaternion); 

			initialSpawns.Add (aiCar0);
			initialSpawns.Add (aiCar1);

			onDemandSpawns.Add (aiCar2);
			onDemandSpawns.Add (aiCar3);
		}

		public void addOnDemandSpawn(SpawnTriple spawn) {
			onDemandSpawns.Add (spawn);
		}
	}
}