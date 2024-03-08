using TMPro;
using UnityEngine;

namespace SimpleFPS
{
	/// <summary>
	/// View showed at the end of the match.
	/// </summary>
	public class UIGameOverView : MonoBehaviour
	{
		public TextMeshProUGUI Winner;
		public GameObject      VictoryGroup;
		public GameObject      DefeatGroup;
		public AudioSource     GameOverMusic;

		private GameUI _gameUI;
		private EGameplayState _lastState;

		// Called from button OnClick event.
		public void GoToMenu()
		{
			_gameUI.GoToMenu();
		}

		private void Awake()
		{
			_gameUI = GetComponentInParent<GameUI>();
		}

		private void Update()
		{
			if (_gameUI.Runner == null)
				return;

			// Unlock cursor.
			Cursor.lockState = CursorLockMode.None;
			Cursor.visible = true;

			if (_gameUI.Gameplay.Object == null || _gameUI.Gameplay.Object.IsValid == false)
				return;

			if (_lastState == _gameUI.Gameplay.State)
				return;

			GameOverMusic.PlayDelayed(1f);

			_lastState = _gameUI.Gameplay.State;

			bool localPlayerIsWinner = false;
			Winner.text = string.Empty;

			foreach (var playerPair in _gameUI.Gameplay.PlayerData)
			{
				if (playerPair.Value.StatisticPosition != 1)
					continue;

				Winner.text = $"Winner is {playerPair.Value.Nickname}";
				localPlayerIsWinner = playerPair.Key == _gameUI.Runner.LocalPlayer;
			}

			VictoryGroup.SetActive(localPlayerIsWinner);
			DefeatGroup.SetActive(localPlayerIsWinner == false);
		}
	}
}
