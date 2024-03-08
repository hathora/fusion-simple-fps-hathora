using Fusion;
using UnityEngine;

namespace SimpleFPS
{
	/// <summary>
	/// Periodically checks for an object with Weapons component within radius and adds a specific weapon type.
	/// </summary>
	public class WeaponPickup : NetworkBehaviour
	{
		public EWeaponType Type;
		public float       Radius = 1f;
		public float       Cooldown = 30f;
		public LayerMask   LayerMask;
		public GameObject  ActiveObject;
		public GameObject  InactiveObject;

		public bool IsActive => _activationTimer.ExpiredOrNotRunning(Runner);

		[Networked]
		private TickTimer _activationTimer { get; set; }

		private static Collider[] _colliders = new Collider[8];

		public override void Spawned()
		{
			ActiveObject.SetActive(IsActive);
			InactiveObject.SetActive(IsActive == false);
		}

		public override void FixedUpdateNetwork()
		{
			if (IsActive == false)
				return;

			// Get all colliders around pickup within Radius.
			int collisions = Runner.GetPhysicsScene().OverlapSphere(transform.position + Vector3.up, Radius, _colliders, LayerMask, QueryTriggerInteraction.Ignore);
			for (int i = 0; i < collisions; i++)
			{
				// Check for Weapons component on collider game object or any parent.
				var weapons = _colliders[i].GetComponentInParent<Weapons>();
				if (weapons != null && weapons.PickupWeapon(Type))
				{
					// Pickup was successful, activating timer.
					_activationTimer = TickTimer.CreateFromSeconds(Runner, Cooldown);
					break;
				}
			}
		}

		public override void Render()
		{
			ActiveObject.SetActive(IsActive);
			InactiveObject.SetActive(IsActive == false);
		}

		private void OnDrawGizmos()
		{
			Gizmos.DrawWireSphere(transform.position + Vector3.up, Radius);
		}
	}
}
