using TMPro;
using UnityEngine;

namespace SimpleFPS
{
	/// <summary>
	/// Main gameplay info panel at the top of the screen.
	/// Also handles displaying of announcements (e.g. Lead Lost, Lead Taken)
	/// </summary>
	public class UIGameplayInfo : MonoBehaviour
	{
		[Header("Setup")]
		public TextMeshProUGUI Position;
		public TextMeshProUGUI RemainingTime;
		public TextMeshProUGUI Kills;
		public GameObject      Skirmish;

		[Header("Announcer")]
		public GameObject      GameplayStart;
		public GameObject      LeadTaken;
		public GameObject      LeadLost;
		public GameObject      DoubleDamage;
		public GameObject      RemainingTime3;
		public GameObject      RemainingTime2;
		public GameObject      RemainingTime1;

		private GameUI _gameUI;

		private int _lastTime = -1;
		private int _lastPosition;

		private void Awake()
		{
			// Make sure to turn all announcements off on start.
			GameplayStart.SetActive(false);
			LeadTaken.SetActive(false);
			LeadLost.SetActive(false);
			DoubleDamage.SetActive(false);
			RemainingTime3.SetActive(false);
			RemainingTime2.SetActive(false);
			RemainingTime1.SetActive(false);

			_gameUI = GetComponentInParent<GameUI>();
		}

		private void Update()
		{
			if (_gameUI.Runner == null)
				return;

			var gameplay = _gameUI.Gameplay;

			if (gameplay.Object == null || gameplay.Object.IsValid == false)
				return;

			int remainingTime = (int)gameplay.RemainingTime.RemainingTime(_gameUI.Runner).GetValueOrDefault();
			if (remainingTime == _lastTime)
				return;

			Skirmish.SetActive(gameplay.State == EGameplayState.Skirmish);
			GameplayStart.SetActive(gameplay.State == EGameplayState.Running);
			DoubleDamage.SetActive(gameplay.DoubleDamageActive);
			RemainingTime.gameObject.SetActive(gameplay.State > EGameplayState.Skirmish);

			ShowGameplayTime(remainingTime);

			if (gameplay.PlayerData.TryGet(_gameUI.Runner.LocalPlayer, out PlayerData playerData))
			{
				ShowPlayerData(playerData);
			}
		}

		private void ShowGameplayTime(int remainingTime)
		{
			int minutes = (remainingTime / 60);
			int seconds = (remainingTime % 60);
			RemainingTime.text = $"{minutes}:{seconds:00}";

			if (remainingTime == 3)
			{
				RemainingTime3.SetActive(true);
			}
			else if (remainingTime == 2)
			{
				RemainingTime2.SetActive(true);
			}
			else if (remainingTime == 1)
			{
				RemainingTime1.SetActive(true);
			}

			_lastTime = remainingTime;
		}

		private void ShowPlayerData(PlayerData playerData)
		{
			Position.text = playerData.StatisticPosition < int.MaxValue ? $"#{playerData.StatisticPosition}" : "-";
			Kills.text = playerData.Kills.ToString();

			if (playerData.StatisticPosition == _lastPosition)
				return;

			// Restart position animation
			Position.gameObject.SetActive(false);
			Position.gameObject.SetActive(true);

			if (playerData.StatisticPosition == 1 && playerData.Kills > 0)
			{
				LeadTaken.SetActive(false);
				LeadTaken.SetActive(true);
			}

			if (_lastPosition == 1)
			{
				LeadLost.SetActive(false);
				LeadLost.SetActive(true);
			}

			_lastPosition = playerData.StatisticPosition;
		}
	}
}
