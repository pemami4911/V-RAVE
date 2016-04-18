using UnityEngine;
using System.Collections;

namespace VRAVE {

	public class AICarCollisionChecker : MonoBehaviour {

		private GameObject UserCar;
		private HUDController hudController;
		//private HUDAudioController audioController;
		[SerializeField] private float displayTime = 6.0f;

		// Use this for initialization
		void Start () {
			UserCar = gameObject;
			hudController = UserCar.GetComponentInChildren<HUDController>();
			//audioController = UserCar.GetComponentInChildren<HUDAudioController>();
		}

		void OnCollisionEnter(Collision collision) {
			//Debug.Log ("Collision detected!");
			if (collision.gameObject.tag.Equals(VRAVEStrings.AI_Car) ||
				collision.gameObject.tag.Equals(VRAVEStrings.Obstacle)) { 
				//collect old values
				bool oldIsEnabled = hudController.model.isLeftImageEnabled;
				Material oldMaterial = hudController.model.leftBackingMaterial;

				//set collision icon
				hudController.model.leftBackingMaterial = hudController.model.collisionIcon;
				hudController.model.leftImageScale = new Vector3 (0.6f * 0.1280507f, 0, 0.6f * 0.1280507f);
				hudController.model.isLeftImageEnabled = true;

				//audioController.playAudio((AudioClip)Resources.Load("beep"));

				//restore old values after a waiting period
				StartCoroutine(resetCollisionIcon (displayTime, oldIsEnabled, oldMaterial));

				}
		}

		private IEnumerator resetCollisionIcon (float time, bool oldIsEnabled, Material oldMaterial) {
			yield return new WaitForSeconds(time);

			//hudController.model.leftBackingMaterial = oldMaterial;
			//hudController.model.isLeftImageEnabled = oldIsEnabled;
			hudController.model.isLeftImageEnabled = false;
		}
	}
}
