using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace VRAVE {
	public class WeatherSpawnModel : SpawnModel {

		public static string Dark_AI_car_resource = "AI_Vehicle_DARK_4-4";
		private static float stoppedCarX = 15.445f;
		private static float stoppedCarY = 0.266f;
		private static float stoppedCarZ = -20.697f;
		private static Quaternion stoppedCarRotation = Quaternion.Euler(360.0f, 90.0f, 360.0f);

		public WeatherSpawnModel() {

			SpawnTriple stoppedCar0 = new SpawnTriple (Dark_AI_car_resource,
				new Vector3 (stoppedCarX, stoppedCarY, stoppedCarZ), stoppedCarRotation); 
			SpawnTriple stoppedCar1 = new SpawnTriple (Dark_AI_car_resource,
				new Vector3 (stoppedCarX-6, stoppedCarY, stoppedCarZ), stoppedCarRotation);
			SpawnTriple stoppedCar2 = new SpawnTriple (Dark_AI_car_resource,
				new Vector3 (stoppedCarX-12, stoppedCarY, stoppedCarZ), stoppedCarRotation); 
			SpawnTriple stoppedCar3 = new SpawnTriple (Dark_AI_car_resource,
				new Vector3 (stoppedCarX-18, stoppedCarY, stoppedCarZ), stoppedCarRotation);
			
			initialSpawns = new List<SpawnTriple> ();

			initialSpawns.Add (stoppedCar0);
			initialSpawns.Add (stoppedCar1);
			initialSpawns.Add (stoppedCar2);
			initialSpawns.Add (stoppedCar3);

		}

	}

}
