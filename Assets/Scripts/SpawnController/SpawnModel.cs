using UnityEngine;
using System.Collections.Generic;

namespace VRAVE {

	//Extend this class with your own spawn Model - it will contain the position & rotations of all the items you want to spawn
	//Set the SpawnModel in SpawnController to use your SpawnModel for your scenario
	//initial spawn items are spawned on scenario enter (call enterScenario() in SpawnController)
	//on demand spawn items are spawned by index

	public class SpawnModel {

		//Stores the mapping between the Resource string and a mapping of coordinates to rotation
		public List<SpawnTriple> initialSpawns { get; protected set;}
		public List<SpawnTriple> onDemandSpawns { get; protected set;}

		//the name of the resource to use. 
		//NOTE: the resource must be stored in the Resources folder
		public static string AI_car_resource = "AI_Vehicle_3-22";

		public static Quaternion default_quaternion = Quaternion.Euler(0, 0, 0);

		//an example SpawnModel
		public SpawnModel() {
			//example of what your SpawnModel deriving class would look like...
			/*
			SpawnTriple aiCar0 = new SpawnTriple (AI_car_resource, new Vector3 (0, 0, 0), default_quaternion); 
			SpawnTriple aiCar1 = new SpawnTriple (AI_car_resource, new Vector3 (1, 1, 1), default_quaternion); 

			SpawnTriple aiCar2 = new SpawnTriple (AI_car_resource, new Vector3 (5, 5, 5), default_quaternion); 
			SpawnTriple aiCar3 = new SpawnTriple (AI_car_resource, new Vector3 (10, 10, 10), default_quaternion); 

			initialSpawns.Add (aiCar0);
			initialSpawns.Add (aiCar1);

			onDemandSpawns.Add (aiCar2);
			onDemandSpawns.Add (aiCar3); */
		}
			
	}
}