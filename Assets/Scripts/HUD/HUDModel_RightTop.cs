using UnityEngine;
using System.Collections;

namespace VRAVE
{
    public class HUDModel_RightTop : HUDModel
    {

        public HUDModel_RightTop() : base()
        {

            //Text
            rightTextPosition = new Vector3(-2.35f, 0, -0.11f);

            isCenterTextEnabled = false;
            isLeftTextEnabled = false;
            isRightTextEnabled = true;

            isLeftImageEnabled = false;

            rightImagePosition = new Vector3(-1.36f, 0.129f, -1.37f);
            rightImageScale = new Vector3(0.1280507f, 0, 0.1280507f);
            isRightImageEnabled = true;
            rightBackingMaterial = Resources.Load("alert", typeof(Material)) as Material;

        }
    }
}
