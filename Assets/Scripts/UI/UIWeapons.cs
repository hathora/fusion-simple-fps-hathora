using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace SimpleFPS
{
	public class UIWeapons : MonoBehaviour
	{
	    public Image           WeaponIcon;
	    public Image           WeaponIconShadow;
	    public TextMeshProUGUI WeaponName;
		public TextMeshProUGUI ClipAmmo;
		public TextMeshProUGUI RemainingAmmo;
	    public Image           AmmoProgress;
	    public GameObject      NoAmmoGroup;
	    public CanvasGroup[]   WeaponThumbnails;

	    private Weapon _weapon;
	    private int _lastClipAmmo;
	    private int _lastRemainingAmmo;

	    public void UpdateWeapons(Weapons weapons)
	    {
		    SetWeapon(weapons.CurrentWeapon);

		    // Update weapon thumbnails.
		    for (int i = 0; i < weapons.AllWeapons.Length; i++)
		    {
			    var weapon = weapons.AllWeapons[i];
			    WeaponThumbnails[i].alpha = weapon.IsCollected && weapon.HasAmmo ? 1f : 0.2f;
		    }

		    if (_weapon == null)
			    return;

		    UpdateAmmoProgress();

		    // Modify UI text only when value changed.
		    if (_weapon.ClipAmmo == _lastClipAmmo && _weapon.RemainingAmmo == _lastRemainingAmmo)
			    return;

		    ClipAmmo.text = _weapon.ClipAmmo.ToString();
		    RemainingAmmo.text = _weapon.RemainingAmmo < 1000 ? _weapon.RemainingAmmo.ToString() : "-";

		    NoAmmoGroup.SetActive(_weapon.ClipAmmo == 0 && _weapon.RemainingAmmo == 0);

		    _lastClipAmmo = _weapon.ClipAmmo;
		    _lastRemainingAmmo = _weapon.RemainingAmmo;
	    }

	    private void SetWeapon(Weapon weapon)
	    {
		    if (weapon == _weapon)
			    return;

		    _weapon = weapon;

		    if (weapon == null)
			    return;

		    WeaponIcon.sprite = weapon.Icon;
		    WeaponIconShadow.sprite = weapon.Icon;
			WeaponName.text = weapon.Name;
	    }

	    private void UpdateAmmoProgress()
	    {
		    if (_weapon.IsReloading)
		    {
			    // During reloading the ammo progress bar slowly fills.
			    AmmoProgress.fillAmount = _weapon.GetReloadProgress();
		    }
		    else
		    {
			    AmmoProgress.fillAmount = _weapon.ClipAmmo / (float)_weapon.MaxClipAmmo;
		    }
	    }
	}
}
