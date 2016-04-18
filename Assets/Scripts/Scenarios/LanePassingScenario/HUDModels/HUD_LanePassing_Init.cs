using UnityEngine;
using System.Collections;

namespace VRAVE
{
    public class HUD_LanePassing_Init : HUDModel_Upright
    {

        public HUD_LanePassing_Init()
        {
            //Text
            centerTextPosition = new Vector3(0.82f, 0.38f, 0.1f);
            leftTextPosition = new Vector3(2.358f, 0.4f, -0.827f);
            rightTextPosition = new Vector3(-0.573f, 0.58f, -0.608f);
            topTextPosition = new Vector3(1.081f, 0.366f, -0.882f);           
			//Behind Glass
            //bottomTextPosition = new Vector3(2.59f, 0, 0.08f);
			//In front of glass
            bottomTextPosition = new Vector3(1.86f, 0.39f, 0.05f);

            centerFontSize = 300;
            leftFontSize = 350;
            rightFontSize = 250;
            topFontSize = 350;
            bottomFontSize = 300;

            centerText = "Welcome to Scenario #3!";
            leftText = "";
            rightText = "";
            topText = "";
            bottomText = "";

            isCenterTextEnabled = true;
            isLeftTextEnabled = true;
            isRightTextEnabled = true;
            isTopTextEnabled = true;
            isBottomTextEnabled = true;

        }

        // Use this for initialization
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {

        }
    }
}