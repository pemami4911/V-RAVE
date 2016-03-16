using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections;

public class StartMenu : MonoBehaviour {

	void Update() {
		if (Input.GetKeyDown (KeyCode.Return)) {
			SceneManager.LoadScene ("Lobby");
		}
	}

}
