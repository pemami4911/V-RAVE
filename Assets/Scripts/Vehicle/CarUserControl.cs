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
		private double gain = 0.5; 

        private void Awake()
        {
            // get the car controller
            m_Car = GetComponent<CarController>();

            //get the steering wheel controller
            m_SteeringWheel = GetComponentInChildren<VisualSteeringWheelController>();
        }


        private void FixedUpdate()
        {
            // pass the input to the car!
            double h = gain * CrossPlatformInputManager.GetAxis("Horizontal");
            double v = gain * CrossPlatformInputManager.GetAxis("Vertical");

/*
#if !MOBILE_INPUT
            float handbrake = CrossPlatformInputManager.GetAxis("Jump");
*/
            //m_Car.Move(h, v, v, handbrake);
            
/*#else*/
            
			m_Car.Move((float)h, (float)v, (float)v, 0f);
			m_SteeringWheel.turnSteeringWheel((float)h, m_Car.CurrentSteerAngle);
/*#endif*/
        }
    }
}
