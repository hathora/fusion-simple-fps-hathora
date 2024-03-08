using TMPro;
using UnityEngine;

namespace SimpleFPS
{
	public class UIPlayerView : MonoBehaviour
	{
		public TextMeshProUGUI Nickname;
		public UIHealth        Health;
		public UIWeapons       Weapons;
		public UICrosshair     Crosshair;

		public void UpdatePlayer(Player player, PlayerData playerData)
		{
			Nickname.text = playerData.Nickname;

			Health.UpdateHealth(player.Health);
			Weapons.UpdateWeapons(player.Weapons);

			Crosshair.gameObject.SetActive(player.Health.IsAlive);
		}
	}
}
