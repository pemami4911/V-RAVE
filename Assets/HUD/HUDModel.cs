using UnityEngine;
using System.Collections;

public class HUDModel {

	//plane rotation
	public Vector3 HUDRotation { get; set; }

	//Text
	public Vector3 centerTextPosition { get; set; }
	public Vector3 leftTextPosition { get; set; }
	public Vector3 rightTextPosition { get; set; }
	public Vector3 topTextPosition { get; set; }
	public Vector3 bottomTextPosition { get; set; }

	public string centerText { get; set; }
	public string leftText { get; set; }
	public string rightText { get; set; }
	public string topText { get; set; }
	public string bottomText { get; set; }

	public float centerCharSize { get; set; }
	public float topCharSize { get; set; }
	public float leftCharSize { get; set; }
	public float rightCharSize { get; set; }
	public float bottomCharSize { get; set; }

	public int centerFontSize { get; set; }
	public int topFontSize { get; set; }
	public int leftFontSize { get; set; }
	public int rightFontSize { get; set; }
	public int bottomFontSize { get; set; }

	public bool isCenterTextEnabled { get; set; }
	public bool isLeftTextEnabled { get; set; }
	public bool isRightTextEnabled { get; set; }
	public bool isTopTextEnabled { get; set; }
	public bool isBottomTextEnabled { get; set; }

	//Image
	public Vector3 centerImagePosition { get; set; }
	public Vector3 centerImageScale { get; set; }
	public bool isCenterImageEnabled { get; set; }
	public Material centerBackingMaterial;

	public Vector3 leftImagePosition { get; set; }
	public Vector3 leftImageScale { get; set; }
	public bool isLeftImageEnabled { get; set; }
	public Material leftBackingMaterial;

	public Vector3 rightImagePosition { get; set; }
	public Vector3 rightImageScale { get; set; }
	public bool isRightImageEnabled { get; set; }
	public Material rightBackingMaterial;
	
	public HUDModel() {

		HUDRotation = new Vector3(34.9585f, 0, 180);

		//Text
		centerTextPosition = new Vector3(0,0.131f, -0.777f );
		leftTextPosition = new Vector3(2.159f,0.131f, -1.802f );
		rightTextPosition = new Vector3(-0.679f,0.131f, -1.802f);
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
