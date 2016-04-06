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
            leftTextPosition = new Vector3(2.159f, 0.131f, -1.802f);
            rightTextPosition = new Vector3(-0.679f, 0.131f, -1.802f);
            topTextPosition = new Vector3(0, 0, 0);
            //Behind Glass
            bottomTextPosition = new Vector3(2.59f, 0, 0.08f);
            //In front of glass
            bottomTextPosition = new Vector3(1.86f, 0.39f, 0.08f);

            centerFontSize = 300;
            leftFontSize = 500;
            rightFontSize = 500;
            topFontSize = 500;
            bottomFontSize = 250;

            centerText = "Welcome to Scenario #3!";
            leftText = "Left Text";
            rightText = "Right Text";
            topText = "Top Text";
            bottomText = "Bottom Text";

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