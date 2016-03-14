using UnityEngine;
using System.Collections;
using MonsterLove.StateMachine;

namespace VRAVE
{
	public class Trigger : MonoBehaviour {

			// Number the triggers in your scenario, and use this to change the 
			// state appropriately in your state machine
			[SerializeField] private uint triggerID; 
			// The scenario which contains this trigger
			[SerializeField] private StateBehaviour scenario; 
			// Tag of GameObject colliding with this collider 
			[SerializeField] private string tag; 

			// If this script is attached to a GameObject with a collider
			// that has "isTrigger" selected, this callback will fire upon collision. 
			void OnTriggerEnter(Collider other)
			{
				if (!other.CompareTag(tag))
				{
					return;
				}
					
				scenario.ChangeState (triggerID);
			}
	}
}