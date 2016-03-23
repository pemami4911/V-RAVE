using UnityEngine;
using System.Collections;

namespace VRAVE {
	
public class VisualSteeringWheelController : MonoBehaviour {

    //Need to be serialized?
    //[SerializeField] private float maxSteerAngle = 60;

    // Use this for initialization
    void Start () {
        
	}
	

	// Update is called once per frame
	void Update () {
	    
	}

    public void turnSteeringWheel(float steering, float currentSteerAngle)
    {
        //clamp input values
        steering = Mathf.Clamp(steering, -1, 1);
        //transform.localEulerAngles = new Vector3(0, steering * maxSteerAngle, 0);
		Vector3 from = new Vector3(0, currentSteerAngle, 0);
		Vector3 to = new Vector3(0, steering, 0);
		/*Debug.Log("localEulerAngles.y: " + transform.localEulerAngles.y);
		Debug.Log ("Steering: " + steering);
		float newSteering = Mathf.LerpAngle (transform.localEulerAngles.y, steering, Time.deltaTime) * maxSteerAngle;
		transform.localEulerAngles = new Vector3(0, newSteering, 0);*/
		transform.localEulerAngles = Vector3.Lerp (from, to, Time.deltaTime);
	}
}
}