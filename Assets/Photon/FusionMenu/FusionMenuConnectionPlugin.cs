using UnityEngine;

namespace Fusion.Menu
{
	public abstract class FusionMenuConnectionPlugin : MonoBehaviour
	{
		public abstract IPhotonMenuConnection Create(FusionMenuConnectionBehaviour connectionBehaviour);
	}
}
