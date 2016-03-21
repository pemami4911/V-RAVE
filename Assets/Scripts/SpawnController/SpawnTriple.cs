using System;
using UnityEngine;

namespace VRAVE
{
	public class SpawnTriple
	{
		private string resourceString { get;}
		private Vector3 position { get;}
		private Quaternion rotation { get; }

		public SpawnTriple (string resourceString, Vector3 position, Quaternion rotation)
		{
			this.resourceString = resourceString;
			this.position = position;
			this.rotation = rotation;
		}
	}
}

