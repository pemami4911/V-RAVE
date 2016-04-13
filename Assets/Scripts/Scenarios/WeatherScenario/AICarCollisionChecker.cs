using UnityEngine;
using System.Collections;

namespace VRAVE {

	public class AICarCollisionChecker : MonoBehaviour {

		private GameObject UserCar;
		private HUDController hudController;
		private float displayTime = 2.0f;

		// Use this for initialization
		void Start () {
			UserCar = gameObject;
			hudController = UserCar.GetComponentInChildren<HUDController>();
		}

		void OnCollisionEnter(Collision collision) {
			//Debug.Log ("Collision detected!");
			if (collision.gameObject.tag.Equals("AI_Car")) { 
				//collect old values
				bool oldIsEnabled = hudController.model.isLeftImageEnabled;
				Material oldMaterial = hudController.model.leftBackingMaterial;

				//set collision icon
				hudController.model.leftBackingMaterial = hudController.model.collisionIcon;
				hudController.model.isLeftImageEnabled = true;

				//restore old values after a waiting period
				StartCoroutine(resetCollisionIcon (displayTime, oldIsEnabled, oldMaterial));

				}
		}

		private IEnumerator resetCollisionIcon (float time, bool oldIsEnabled, Material oldMaterial) {
			yield return new WaitForSeconds(time);

			hudController.model.leftBackingMaterial = oldMaterial;
			hudController.model.isLeftImageEnabled = oldIsEnabled;
		}
	}
}
