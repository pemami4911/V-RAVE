using UnityEngine;
using System.Collections;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace VRAVE
{
public class ChangeSelected : MonoBehaviour {
	private EventSystem eventSystem;
	
		public GameObject[] buttons; 
		private int currentSelected = 0;

		// Use this for initialization
		void Start () {
			eventSystem = GetComponentInParent<EventSystem> ();
		}
		
		// Update is called once per frame
		void Update () {
			if (Input.GetButtonUp (VRAVEStrings.Right_Paddle)) {
				currentSelected++;
				if (currentSelected == buttons.Length) {
					currentSelected = 0;
				}
				eventSystem.SetSelectedGameObject (buttons [currentSelected]);
			}
		}
}
}