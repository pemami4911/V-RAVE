using UnityEngine;
using System.Collections;

namespace VRAVE{ 
	public class WeatherHUDModel : HUDVRAVE_Default {

		public static Material collisionWarning = Resources.Load("warning", typeof(Material)) as Material;

		public WeatherHUDModel () : base() {
			leftImagePosition = new Vector3(2.228f,-0.011f, -0.02f );
			leftImageScale = new Vector3 (0.07f, 0.07f, 0.07f);
			leftBackingMaterial = collisionWarning;

			centerText = "SEVERE WEATHER ALERT";

			//centerText = "DRIVE WITH CAUTION";
			//centerTextPosition = new Vector3 (0.438f, 0.109f, 0.059f);
		}
	}
}
