using UnityEngine;
using System.Collections;

public class HUDModel_Right : HUDModel {

	public HUDModel_Right() {

		HUDRotation = new Vector3(34.9585f, 180, 180);

		//Text
		centerTextPosition = new Vector3(0,0.131f, -0.777f );
		leftTextPosition = new Vector3(2.159f,0.131f, -1.802f );
		rightTextPosition = new Vector3(-0.679f,0.131f, -1.802f);
		topTextPosition = new Vector3(0,0, 0 );
		bottomTextPosition = new Vector3(0,0, 0 );

		centerText = ">Hi, Dave.";
		leftText = "Vehicle:\n100%!";
		rightText = "Alert!";
		topText = "TOP";
		bottomText = "TOP";

		centerCharSize = 0.005f;
		leftCharSize = 0.0025f;
		rightCharSize = 0.0025f;
		topCharSize = 0.005f;
		bottomCharSize = 0.005f;

		centerFontSize = 500;
		leftFontSize = 500;
		rightFontSize = 500;
		topFontSize = 500;
		bottomFontSize = 500;

		isCenterTextEnabled = false;
		isLeftTextEnabled = false;
		isRightTextEnabled = true;
		isTopTextEnabled = false;
		isBottomTextEnabled = false;

		//Image
		centerImagePosition = new Vector3(0,0, 0 );
		centerImageScale = new Vector3 (0.5f, 0.5f, 0.5f);
		isCenterImageEnabled = false;
		centerBackingMaterial = Resources.Load("schedule", typeof(Material)) as Material;

		leftImagePosition = new Vector3(2,0, 0 );
		leftImageScale = new Vector3 (0.1280507f, 0, 0.1280507f);
		isLeftImageEnabled = false;
		leftBackingMaterial = Resources.Load("alert", typeof(Material)) as Material;

		rightImagePosition = new Vector3(-2.49f,0, 0 );
		rightImageScale = new Vector3 (0.1280507f, 0, 0.1280507f);
		isRightImageEnabled = true;
		rightBackingMaterial = Resources.Load("alert", typeof(Material)) as Material;

	}
}
