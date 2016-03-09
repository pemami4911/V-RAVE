using UnityEngine;
using System.Collections;

public class AISpawnController : MonoBehaviour {

	public AISpawnModel spawnModel;

	// Use this for initialization
	void Start () {

		spawnModel = new AISpawnModel ();
		GameObject AICar = Resources.Load ("AICar", typeof(GameObject)) as GameObject;

		foreach (Vector3 coordinate in spawnModel.coordinates) {
			Instantiate(AICar, coordinate, transform.rotation);
		}
		
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
