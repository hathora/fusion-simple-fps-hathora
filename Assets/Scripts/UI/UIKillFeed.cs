using UnityEngine;

namespace SimpleFPS
{
	public class UIKillFeed : MonoBehaviour
	{
		public UIKillFeedItem KillFeedItemPrefab;
		public float          ItemLifetime = 6f;
		public Sprite[]       WeaponIcons;

		public void ShowKill(string killer, string victim, EWeaponType weaponType, bool isCriticalKill)
		{
			var item = Instantiate(KillFeedItemPrefab, transform);

			item.Killer.text = killer;
			item.Victim.text = victim;
			item.WeaponIcon.sprite = WeaponIcons[(int)weaponType];
			item.CriticalKillGroup.SetActive(isCriticalKill);


			// Kill feed item is fading in time automatically via animation component.
			// Make sure the item gets destroyed after the animation is done.
			Destroy(item.gameObject, ItemLifetime);
		}
	}
}
