using UnityEngine;
using System.Collections;

namespace VRAVE
{
    public class HUDModel_Left : HUDModel
    {

        public HUDModel_Left() : base()
        {

            isCenterTextEnabled = false;
            isLeftTextEnabled = true;
            isRightTextEnabled = false;
            isTopTextEnabled = false;
            isBottomTextEnabled = false;

        }
    }
}
