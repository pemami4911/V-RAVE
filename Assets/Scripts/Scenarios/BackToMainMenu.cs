using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

namespace VRAVE
{
	public class BackToMainMenu : MonoBehaviour
	{

		// Use this for initialization
		void Start()
		{

		}

		// Update is called once per frame
		void Update()
		{
			if (Input.GetButtonDown("Cancel"))
			{
				CameraFade.StartAlphaFade(Color.black, false, 2f, 0f, () =>
				{
					SceneManager.LoadScene(VRAVEStrings.Lobby_Menu);
				});
			}
		}
	}
}
