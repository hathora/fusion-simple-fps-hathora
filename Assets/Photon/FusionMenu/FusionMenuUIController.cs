namespace Fusion.Menu
{
	public class FusionMenuUIController : PhotonMenuUIController<PhotonMenuConnectArgs>
	{
		public PhotonMenuConfig Config => _config;

		public GameMode SelectedGameMode { get; protected set; } = GameMode.AutoHostOrClient;

		public virtual void OnGameStarted() {}
		public virtual void OnGameStopped() {}
	}
}
