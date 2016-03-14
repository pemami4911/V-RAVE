using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace VRAVE {
	
	public class SpawnController : MonoBehaviour {

		private SpawnModel spawnModel {get; set;}

		public SpawnController() {
			spawnModel = new SpawnModel ();
		}

		void enterScenario() {
			
			ICollection<string> resources = spawnModel.getResourceStrings ();
			foreach (string resource in resources) {
				GameObject spawnObject = Resources.Load (resource, typeof(GameObject)) as GameObject;
					
				ICollection<KeyValuePair<Vector3, Quaternion>> positionPairs = spawnModel.getCoordinateRotationPairs (resource);

				//get coordinates at which to spawn this resource
				foreach (KeyValuePair<Vector3, Quaternion> positionPair in positionPairs) {
						
					Instantiate (spawnObject, positionPair.Key, positionPair.Value);
				}
			}
		}
			
		void exitScenario() {
			//will need this method... someday
		}

	}
}