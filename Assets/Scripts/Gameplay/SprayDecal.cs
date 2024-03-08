using UnityEngine;
using UnityEngine.Rendering.Universal;
using Fusion;

namespace SimpleFPS
{
	/// <summary>
	/// Switches material of spawned decal object.
	/// </summary>
	public class SprayDecal : NetworkBehaviour
	{
		public Material[]     Materials;
		public DecalProjector DecalProjector;

		[Networked]
		private int _materialIndex { get; set; }

		public override void Spawned()
		{
			if (HasStateAuthority)
			{
				_materialIndex = Random.Range(0, Materials.Length);
			}

			DecalProjector.material = Materials[_materialIndex];
		}
	}
}
