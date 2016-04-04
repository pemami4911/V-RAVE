using UnityEngine;
using System.Collections;

public class HUDModel_Upright : HUDModel {

	public HUDModel_Upright () : base() {

		HUDRotation = new Vector3(90, 0, 180);

		//Text
		centerTextPosition = new Vector3(0,0.131f, 0.06f );
		leftTextPosition = new Vector3(3.04f,0.131f, -1.049f );
		rightTextPosition = new Vector3(-1.71f,0.131f, -0.89f);

		isLeftImageEnabled = false;
		isRightTextEnabled = false;

		centerCharSize = 0.005f;
		leftCharSize = 0.005f;
		rightCharSize = 0.005f;
		topCharSize = 0.005f;
		bottomCharSize = 0.005f;

		centerFontSize = 500;
		leftFontSize = 500;
		rightFontSize = 500;
		topFontSize = 500;
		bottomFontSize = 500;
	}
}
