using UnityEngine;

namespace SimpleFPS
{
	/// <summary>
	/// Component for spawn points lookup.
	/// </summary>
	public class SpawnPoint : MonoBehaviour
	{
		private void OnDrawGizmos()
		{
			Gizmos.DrawWireSphere(transform.position, 0.1f);
		}
	}
}
