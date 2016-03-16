using UnityEngine;
using System.Collections;
using UnityStandardAssets.CrossPlatformInput;

public class HUDController : MonoBehaviour {


	public Transform centerTextField;
	public Transform leftTextField;
	public Transform rightTextField;
	public Transform topTextField;
	public Transform bottomTextField;

	public Transform centerImageField;
	public Transform leftImageField;
	public Transform rightImageField;

	public TextMesh centerText;
	public TextMesh leftText;
	public TextMesh rightText;
	public TextMesh topText;
	public TextMesh bottomText;

	private bool isHUDImageEnabled = true;
	private bool isHUDTextEnabled = true;


	public TextMesh newMesh;

	private HUDModel model;

	// Use this for initialization
	void Start () {

		model = new HUDModel_Upright ();
		transform.eulerAngles = model.HUDRotation;

		centerImageField = transform.Find ("HUDImage_Center");
		leftImageField = transform.Find ("HUDImage_Left");
		rightImageField = transform.Find ("HUDImage_Right");

		centerTextField = transform.Find("HUDText_Center");
		leftTextField = transform.Find("HUDText_Left");
		rightTextField = transform.Find("HUDText_Right");
		topTextField = transform.Find("HUDText_Top");
		bottomTextField = transform.Find("HUDText_Bottom");

		centerText = centerTextField.GetComponent<TextMesh>();
		leftText = leftTextField.GetComponent<TextMesh>();
		rightText = rightTextField.GetComponent<TextMesh>();
		topText = topTextField.GetComponent<TextMesh>();
		bottomText = bottomTextField.GetComponent<TextMesh>();

	}
	
	// Update is called once per frame
	void Update () {
		
		updateCenterImageField ();
		updateLeftImageField ();
		updateRightImageField ();

		updateCenterTextField ();
		updateLeftTextField ();
		updateRightTextField ();
		updateBottomTextField ();
		updateTopTextField ();

		bool g = CrossPlatformInputManager.GetButtonDown ("HUDImageEnable");
		if (g) {
			isHUDImageEnabled = !isHUDImageEnabled;
		}

		bool h = CrossPlatformInputManager.GetButtonDown ("HUDTextEnable");
		if (h) {
			isHUDTextEnabled = !isHUDTextEnabled;
		}
			
	}

	void updateCenterImageField() {
		centerImageField.transform.localPosition = model.centerImagePosition;
		centerImageField.transform.localScale = model.centerImageScale;
		centerImageField.GetComponent<MeshRenderer> ().enabled = model.isCenterImageEnabled && isHUDImageEnabled;
		centerImageField.GetComponent<MeshRenderer> ().material = model.centerBackingMaterial;
	}

	void updateLeftImageField() {
		leftImageField.transform.localPosition = model.leftImagePosition;
		leftImageField.transform.localScale = model.leftImageScale;
		leftImageField.GetComponent<MeshRenderer> ().enabled = model.isLeftImageEnabled && isHUDImageEnabled;
		leftImageField.GetComponent<MeshRenderer> ().material = model.leftBackingMaterial;
	}

	void updateRightImageField() {
		rightImageField.transform.localPosition = model.rightImagePosition;
		rightImageField.transform.localScale = model.rightImageScale;
		rightImageField.GetComponent<MeshRenderer> ().enabled = model.isRightImageEnabled && isHUDImageEnabled;
		rightImageField.GetComponent<MeshRenderer> ().material = model.rightBackingMaterial;
	}

	void updateCenterTextField() {
		centerText.text = model.centerText;
		centerTextField.transform.localPosition = model.centerTextPosition;
		centerText.characterSize = model.centerCharSize;
		centerText.fontSize = model.centerFontSize;
		centerTextField.GetComponent<MeshRenderer> ().enabled = model.isCenterTextEnabled && isHUDTextEnabled;
	}

	void updateLeftTextField() {
		leftText.text = model.leftText;
		leftTextField.transform.localPosition = model.leftTextPosition;
		leftText.characterSize = model.leftCharSize;
		leftText.fontSize = model.leftFontSize;
		leftTextField.GetComponent<MeshRenderer> ().enabled = model.isLeftTextEnabled && isHUDTextEnabled;
	}

	void updateRightTextField() {
		rightText.text = model.rightText;
		rightTextField.transform.localPosition = model.rightTextPosition;
		rightText.characterSize = model.rightCharSize;
		rightText.fontSize = model.rightFontSize;
		rightTextField.GetComponent<MeshRenderer> ().enabled = model.isRightTextEnabled && isHUDTextEnabled;
	}

	void updateBottomTextField() {
		bottomText.text = model.bottomText;
		bottomTextField.transform.localPosition = model.bottomTextPosition;
		bottomText.characterSize = model.bottomCharSize;
		bottomText.fontSize = model.bottomFontSize;
		bottomTextField.GetComponent<MeshRenderer> ().enabled = model.isBottomTextEnabled && isHUDTextEnabled;

	}

	void updateTopTextField() {
		topText.text = model.topText;
		topTextField.transform.localPosition = model.topTextPosition;
		topText.characterSize = model.topCharSize;
		topText.fontSize = model.topFontSize;
		topTextField.GetComponent<MeshRenderer> ().enabled = model.isTopTextEnabled && isHUDTextEnabled;
	}
}
