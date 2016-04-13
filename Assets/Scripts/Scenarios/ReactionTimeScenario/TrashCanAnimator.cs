using UnityEngine;
using System.Collections;

namespace VRAVE
{
	public class TrashCanAnimator : MonoBehaviour
	{

		public float trashCanForce;

		// Use this for initialization
		public void roll ()
		{
			transform.GetComponent<Rigidbody> ().AddForce (-Vector3.right * trashCanForce, ForceMode.Acceleration);
		}
	}
}