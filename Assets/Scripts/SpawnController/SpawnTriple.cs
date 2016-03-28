using System;
using UnityEngine;

namespace VRAVE
{
	public class SpawnTriple
	{
		public string resourceString { get; private set;}
		public Vector3 position { get; private set;}
		public Quaternion rotation { get; private set;}

		public SpawnTriple (string resourceString, Vector3 position, Quaternion rotation)
		{
			this.resourceString = resourceString;
			this.position = position;
			this.rotation = rotation;
		}
	}
}

