using UnityEngine;
using System.Collections;

namespace VRAVE
{
    public class HUDModel_Right : HUDModel
    {

        public HUDModel_Right() : base()
        {

            isCenterTextEnabled = false;
            isLeftTextEnabled = false;
            isRightTextEnabled = true;
            isTopTextEnabled = false;
            isBottomTextEnabled = false;

            isLeftImageEnabled = false;

            rightImagePosition = new Vector3(-2.49f, 0, 0);
            rightImageScale = new Vector3(0.1280507f, 0, 0.1280507f);
            isRightImageEnabled = true;
            rightBackingMaterial = Resources.Load("alert", typeof(Material)) as Material;

        }
    }
}
