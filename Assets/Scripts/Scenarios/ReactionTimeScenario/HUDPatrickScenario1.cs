using UnityEngine;
using System.Collections;

namespace VRAVE 
{
public class HUDPatrickScenario1 : HUDModel_Upright 
{
		private float imgScale = 0.5f;

		public HUDPatrickScenario1() : base()
		{
			centerText = "Reaction Time Scenario";
			leftBackingMaterial = Resources.Load("stop", typeof(Material)) as Material;
			isLeftImageEnabled = true;

			leftImagePosition = new Vector3(1.98f, 0.19f, -0.39f);
			leftImageScale = new Vector3 (imgScale * 0.1280507f, 0, imgScale * 0.1280507f);
		}

}

}