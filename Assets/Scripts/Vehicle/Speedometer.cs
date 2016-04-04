using UnityEngine;
using System.Collections;

namespace VRAVE {
	
public class Speedometer : MonoBehaviour {

    private CarController m_Car; // the car controller we want to use

	// Use this for initialization
	void Start () {
        m_Car = GetComponentInParent<CarController>();
	}
	
	// Update is called once per frame
	void Update () {
		int speed = (int)Mathf.Round(m_Car.CurrentSpeed);
        GetComponent<TextMesh>().text = speed.ToString();
	}
}
}