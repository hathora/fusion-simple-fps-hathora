using Fusion;
using UnityEngine;

namespace SimpleFPS
{
	/// <summary>
	/// Weapons component hold references to all player weapons
	/// and allows for weapon actions such as Fire or Reload.
	/// </summary>
	public class Weapons : NetworkBehaviour
	{
		public Animator    Animator;
	    public Transform   FireTransform;
	    public float       WeaponSwitchTime = 1f;
	    public AudioSource SwitchSound;

	    public bool IsSwitching => _switchTimer.ExpiredOrNotRunning(Runner) == false;

	    [Networked, HideInInspector]
	    public Weapon CurrentWeapon { get; set; }
	    [HideInInspector]
	    public Weapon[] AllWeapons;

	    [Networked]
	    private TickTimer _switchTimer { get; set; }
	    [Networked]
	    private Weapon _pendingWeapon { get; set; }

	    private Weapon _visibleWeapon;

	    public void Fire(bool justPressed)
		{
			if (CurrentWeapon == null || IsSwitching)
				return;

			CurrentWeapon.Fire(FireTransform.position, FireTransform.forward, justPressed);
		}

	    public void Reload()
	    {
		    if (CurrentWeapon == null || IsSwitching)
				return;

		    CurrentWeapon.Reload();
	    }

	    public void SwitchWeapon(EWeaponType weaponType)
	    {
			var newWeapon = GetWeapon(weaponType);

		    if (newWeapon == null || newWeapon.IsCollected == false)
				return;
		    if (newWeapon == CurrentWeapon && _pendingWeapon == null)
			    return;
		    if (newWeapon == _pendingWeapon)
			    return;

		    if (CurrentWeapon.IsReloading)
				return;

		    _pendingWeapon = newWeapon;
		    _switchTimer = TickTimer.CreateFromSeconds(Runner, WeaponSwitchTime);

		    // For local player start with switch animation but only
		    // in forward tick as starting animation multiple times
		    // during resimulations is not desired.
		    if (HasInputAuthority && Runner.IsForward)
		    {
			    CurrentWeapon.Animator.SetTrigger("Hide");
			    SwitchSound.Play();
		    }
	    }

	    public bool PickupWeapon(EWeaponType weaponType)
	    {
		    if (CurrentWeapon.IsReloading)
				return false;

			var weapon = GetWeapon(weaponType);
			if (weapon == null)
				return false;

			if (weapon.IsCollected)
			{
				// If the weapon is already collected at least refill the ammo.
				weapon.AddAmmo(weapon.StartAmmo - weapon.RemainingAmmo);
			}
			else
			{
				// Weapon is already present inside Player prefab,
				// marking it as IsCollected is all that is needed.
				weapon.IsCollected = true;
			}

			SwitchWeapon(weaponType);

			return true;
	    }

	    public Weapon GetWeapon(EWeaponType weaponType)
	    {
			for (int i = 0; i < AllWeapons.Length; ++i)
			{
				if (AllWeapons[i].Type == weaponType)
					return AllWeapons[i];
			}

			return default;
	    }

	    public override void Spawned()
	    {
		    if (HasStateAuthority)
		    {
			    CurrentWeapon = AllWeapons[0];
			    CurrentWeapon.IsCollected = true;
		    }
	    }

	    public override void FixedUpdateNetwork()
	    {
		    TryActivatePendingWeapon();
	    }

	    public override void Render()
	    {
		    if (_visibleWeapon == CurrentWeapon)
			    return;

			int currentWeaponID = -1;

		    // Update weapon visibility
		    for (int i = 0; i < AllWeapons.Length; i++)
		    {
			    var weapon = AllWeapons[i];
			    if (weapon == CurrentWeapon)
			    {
					currentWeaponID = i;
					weapon.ToggleVisibility(true);
			    }
				else
				{
					weapon.ToggleVisibility(false);
				}
		    }

		    _visibleWeapon = CurrentWeapon;

			Animator.SetFloat("WeaponID", currentWeaponID);
	    }

	    private void Awake()
	    {
		    // All weapons are already present inside Player prefab.
		    // This is the simplest solution when only few weapons are available in the game.
		    AllWeapons = GetComponentsInChildren<Weapon>();
	    }

	    private void TryActivatePendingWeapon()
	    {
		    if (IsSwitching == false || _pendingWeapon == null)
			    return;

		    if (_switchTimer.RemainingTime(Runner) > WeaponSwitchTime * 0.5f)
			    return; // Too soon.

		    CurrentWeapon = _pendingWeapon;
		    _pendingWeapon = null;

		    // Make the weapon immediately active.
		    CurrentWeapon.gameObject.SetActive(true);

		    if (HasInputAuthority && Runner.IsForward)
		    {
			    CurrentWeapon.Animator.SetTrigger("Show");
		    }
	    }
	}
}
