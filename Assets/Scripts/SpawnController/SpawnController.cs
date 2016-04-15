using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace VRAVE {

	//Use this class with a SpawnModel, which holds spawning info for entering scenarios
	//you can also spawn with the spawn() method
	public class SpawnController : MonoBehaviour {

		public SpawnModel spawnModel {get; set;}
		private Dictionary<string, GameObject> loadedResources;
		//list of spawned items
		public Dictionary<SpawnTriple, GameObject> initialSpawnedObjects { get; protected set; }

		public SpawnController() {
			loadedResources = new Dictionary<string, GameObject> ();
			//spawnModel = new SpawnModel ();
			initialSpawnedObjects = new Dictionary<SpawnTriple, GameObject>();
		}

		public void enterScenario() {

			List<SpawnTriple> spawns = spawnModel.initialSpawns;

			foreach(SpawnTriple spawn in spawns) {
				GameObject spawnedObject = spawnObject (spawn);
				initialSpawnedObjects.Add (spawn, spawnedObject);
			}
		}


		//for spawning directly with the controller; this method does not use the SpawnModel
		public Object spawn(string resourceName, Vector3 position, Quaternion rotation) {
			return spawnObject(new SpawnTriple(resourceName, position, rotation));
		}

		public void spawnOnDemand(int index) {
			SpawnTriple spawn = spawnModel.onDemandSpawns [index];
		}


		private GameObject spawnObject(SpawnTriple spawn) {
			GameObject spawnObject;

			if (!loadedResources.ContainsKey (spawn.resourceString)) {
				//if spawn.resourceString does not exist in loaded resource, load resource
				spawnObject = Resources.Load (spawn.resourceString, typeof(GameObject)) as GameObject;
				loadedResources.Add (spawn.resourceString, spawnObject);
			}
			else //use already loaded resource
				spawnObject = loadedResources[spawn.resourceString];

			//spawn @ position & rotation
			GameObject spawnedObject = (GameObject) Instantiate (spawnObject, spawn.position, spawn.rotation);
			return spawnedObject;
		}

		public void resetInitialSpawns() {
			foreach (SpawnTriple spawn in initialSpawnedObjects.Keys) {
				GameObject spawnedObject = initialSpawnedObjects [spawn];
				spawnedObject.transform.position = spawn.position;
				spawnedObject.transform.rotation = spawn.rotation;
			}
		}
			
		public void exitScenario() {
			//will need this method... someday
		}

	}
}