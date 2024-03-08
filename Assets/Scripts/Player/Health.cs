using Fusion;
using UnityEngine;

namespace SimpleFPS
{
	/// <summary>
	/// Stores player health, triggers heal/damage effects and informs about player death.
	/// </summary>
	public class Health : NetworkBehaviour
	{
		public float      MaxHealth = 100f;
		public float      ImmortalDurationAfterSpawn = 2f;
		public GameObject ImmortalityIndicator;
		public GameObject HitEffectPrefab;

		public bool IsAlive => CurrentHealth > 0f;
		public bool IsImmortal => _immortalTimer.ExpiredOrNotRunning(Runner) == false;

		[Networked]
		public float CurrentHealth { get; private set; }

		[Networked]
		private int _hitCount { get; set; }
		[Networked]
		private Vector3 _lastHitPosition { get; set; }
		[Networked]
		private Vector3 _lastHitDirection { get; set; }
		[Networked]
		private TickTimer _immortalTimer { get; set; }

		private int _visibleHitCount;
		private SceneObjects _sceneObjects;

		public bool ApplyDamage(PlayerRef instigator, float damage, Vector3 position, Vector3 direction, EWeaponType weaponType, bool isCritical)
		{
			if (CurrentHealth <= 0f)
				return false;

			if (IsImmortal)
				return false;

			CurrentHealth -= damage;

			if (CurrentHealth <= 0f)
			{
				CurrentHealth = 0f;

				_sceneObjects.Gameplay.PlayerKilled(instigator, Object.InputAuthority, weaponType, isCritical);
			}

			// Store relative hit position.
			// Only last hit is stored. For casual gameplay this is enough, no need to store precise data for each hit.
			_lastHitPosition = position - transform.position;
			_lastHitDirection = -direction;

			_hitCount++;

			return true;
		}

		public bool AddHealth(float health)
		{
			if (CurrentHealth <= 0f)
				return false;
			if (CurrentHealth >= MaxHealth)
				return false;

			CurrentHealth = Mathf.Min(CurrentHealth + health, MaxHealth);

			if (HasInputAuthority && Runner.IsForward)
			{
				// Heal effect is shown only to local player.
				// We assume the prediction will be correct most of the time so we don't need to network anything explicitly.
				_sceneObjects.GameUI.PlayerView.Health.ShowHeal(health);
			}

			return true;
		}

		public void StopImmortality()
		{
			_immortalTimer = default;
		}

		public override void Spawned()
		{
			_sceneObjects = Runner.GetSingleton<SceneObjects>();

			if (HasStateAuthority)
			{
				CurrentHealth = MaxHealth;

				_immortalTimer = TickTimer.CreateFromSeconds(Runner, ImmortalDurationAfterSpawn);
			}

			_visibleHitCount = _hitCount;
		}

		public override void Render()
		{
			if (_visibleHitCount < _hitCount)
			{
				// Network hit counter changed in FUN, play damage effect.
				PlayDamageEffect();
			}

			ImmortalityIndicator.SetActive(IsImmortal);

			// Sync network hit counter with local.
			_visibleHitCount = _hitCount;
		}

		private void PlayDamageEffect()
		{
			if (HitEffectPrefab != null)
			{
				var hitPosition = transform.position + _lastHitPosition;
				var hitRotation = Quaternion.LookRotation(_lastHitDirection);

				Instantiate(HitEffectPrefab, hitPosition, hitRotation);
			}
		}
	}
}
