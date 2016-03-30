using UnityEngine;
using System.Collections;

public class VRManager : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		if(Input.GetKeyDown("escape")) {
			Debug.Log ("excape key pressed");
			Application.Quit(); // Quits the game
		}
	}
}
