using UnityEngine;
using System.Collections;

public class HUDModel_Upright : HUDModel {



	public HUDModel_Upright () {

		HUDRotation = new Vector3(90, 0, 180);

		//Text
		centerTextPosition = new Vector3(0,0.131f, 0.06f );
		leftTextPosition = new Vector3(3.04f,0.131f, -1.049f );
		rightTextPosition = new Vector3(-1.71f,0.131f, -0.89f);
		topTextPosition = new Vector3(0,0, 0 );
		bottomTextPosition = new Vector3(0,0, 0 );

		centerText = "Alert!";
		leftText = ">Hi, Dave.";
		rightText = "Vehicle:\n100%";
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

		isCenterTextEnabled = true;
		isLeftTextEnabled = true;
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
		isLeftImageEnabled = true;
		leftBackingMaterial = Resources.Load("alert2", typeof(Material)) as Material;

		rightImagePosition = new Vector3(0,0, 0 );
		rightImageScale = new Vector3 (0.5f, 0.5f, 0.5f);
		isRightImageEnabled = false;
		rightBackingMaterial = Resources.Load("alert2", typeof(Material)) as Material;

	}
}
