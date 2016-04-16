using UnityEngine;
using System.Collections;

namespace VRAVE
{
	public class HUDModel
    {
		public Material collisionIcon;
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

        public HUDModel()
        {
			collisionIcon = Resources.Load(VRAVEStrings.Collision_Img, typeof(Material)) as Material;

            HUDRotation = new Vector3(34.9585f, 0, 180);

            //Text
            centerTextPosition = new Vector3(0, 0.131f, -0.777f);
            leftTextPosition = new Vector3(2.159f, 0.131f, -1.802f);
            rightTextPosition = new Vector3(-0.679f, 0.131f, -1.802f);
            topTextPosition = new Vector3(0, 0, 0);
            bottomTextPosition = new Vector3(0, 0, 0);


            //		centerText = "Alert!";
            //		leftText = ">Hi, Dave.";
            //		rightText = "Vehicle:\n100%";
            //		topText = "TOP";
            //		bottomText = "BOTTOM";


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
            centerImagePosition = new Vector3(0, 0, 0);
            centerImageScale = new Vector3(0.5f, 0.5f, 0.5f);
            isCenterImageEnabled = false;
            centerBackingMaterial = Resources.Load("schedule", typeof(Material)) as Material;

            leftImagePosition = new Vector3(2, 0, 0);
            leftImageScale = new Vector3(0.1280507f, 0, 0.1280507f);
            isLeftImageEnabled = true;
            leftBackingMaterial = Resources.Load("alert", typeof(Material)) as Material;

            rightImagePosition = new Vector3(0, 0, 0);
            rightImageScale = new Vector3(0.5f, 0.5f, 0.5f);
            isRightImageEnabled = false;
            rightBackingMaterial = Resources.Load("alert", typeof(Material)) as Material;

        }

		public HUDModel Clone()
		{
			HUDModel h = new HUDModel ();

			h.HUDRotation = this.HUDRotation; 
			h.centerTextPosition = this.centerTextPosition;
			h.leftTextPosition = this.leftTextPosition;
			h.rightTextPosition = this.rightTextPosition; 
			h.topTextPosition = this.topTextPosition; 
			h.bottomTextPosition = this.bottomTextPosition;
			h.leftText = this.leftText;
			h.rightText = this.rightText;
			h.topText = this.topText;
			h.centerText = this.centerText;
			h.bottomText = this.bottomText;
			h.centerCharSize = this.centerCharSize;
			h.leftCharSize = this.leftCharSize;
			h.rightCharSize = this.rightCharSize;
			h.topCharSize = this.topCharSize;
			h.bottomCharSize = this.bottomCharSize;
			h.centerFontSize = this.centerFontSize;
			h.leftFontSize = this.leftFontSize;
			h.rightFontSize = this.rightFontSize;
			h.topFontSize = this.topFontSize;
			h.bottomFontSize = this.bottomFontSize;
			h.isCenterTextEnabled = this.isCenterTextEnabled; 
			h.isLeftTextEnabled = this.isLeftTextEnabled; 
			h.isRightTextEnabled = this.isRightTextEnabled; 
			h.isTopTextEnabled = this.isTopTextEnabled;
			h.isBottomTextEnabled = this.isBottomTextEnabled;
			h.centerImagePosition = this.centerImagePosition; 
			h.centerImageScale = this.centerImageScale; 
			h.isCenterImageEnabled = this.isCenterImageEnabled; 
			h.centerBackingMaterial = this.centerBackingMaterial; 

			h.rightImagePosition = this.rightImagePosition; 
			h.rightImageScale = this.rightImageScale; 
			h.isRightImageEnabled = this.isRightImageEnabled; 
			h.rightBackingMaterial = this.rightBackingMaterial; 

			h.leftImagePosition = this.leftImagePosition; 
			h.leftImageScale = this.leftImageScale; 
			h.isLeftImageEnabled = this.isLeftImageEnabled; 
			h.leftBackingMaterial = this.leftBackingMaterial; 
			return h;
		}
    }
}
