using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;
using VRStandardAssets.Utils;

namespace VRAVE
{
	public class BackToMainMenu : MonoBehaviour
	{
		[SerializeField] 
		private VRCameraFade cameraFade; 

		// Use this for initialization
		void Start()
		{

		}

		// Update is called once per frame
		void Update()
		{
			if (Input.GetButtonDown("Cancel"))
			{
				cameraFade.StartAlphaFade(Color.black, false, 2f, () =>
				{
					SceneManager.LoadScene(VRAVEStrings.Lobby_Menu);
				});
			}
		}
	}
}
