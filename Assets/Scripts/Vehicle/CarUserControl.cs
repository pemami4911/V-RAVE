using System;
using UnityEngine;
using UnityStandardAssets.CrossPlatformInput;

namespace VRAVE
{
    //Necessary requirement?
    //[RequireComponent(typeof (CarController))]
    public class CarUserControl : MonoBehaviour
    {
        private CarController m_Car; // the car controller we want to use
        private VisualSteeringWheelController m_SteeringWheel; //SteeringWheelController

        private void Awake()
        {
            // get the car controller
            m_Car = GetComponent<CarController>();


            //get the steering wheel controller
            m_SteeringWheel = GetComponentInChildren<VisualSteeringWheelController>();
        }

        private void onEnable()
        {
            //When switched to UserControl mode, expand steeringAngle
            //m_Car.MaxSteeringAngle = 60f;
        }


        private void FixedUpdate()
        {
            // pass the input to the car!
            //double h = gain * CrossPlatformInputManager.GetAxis("Horizontal");
            double hh =Input.GetAxis("Horizontal");
            //double v = gain * CrossPlatformInputManager.GetAxis("Vertical");
            double vv = Input.GetAxis("Vertical");

            //double h_raw = Input.GetAxisRaw("Horizontal");
            //double v_raw = Input.GetAxisRaw("Vertical");

            //Debug.Log("Horizontal: " + hh.ToString());
            //Debug.Log("Vertical: " + vv.ToString());


            /*
            #if !MOBILE_INPUT
                        float handbrake = CrossPlatformInputManager.GetAxis("Jump");
            */
            //m_Car.Move(h, v, v, handbrake);

            /*#else*/

            m_Car.Move((float)hh, (float)vv, (float)vv, 0f);
			m_SteeringWheel.turnSteeringWheel((float)hh, m_Car.CurrentSteerAngle);
/*#endif*/
        }
    }
}
