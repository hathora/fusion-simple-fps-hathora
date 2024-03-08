using Fusion;

namespace SimpleFPS
{
	public class DisableOnServer : NetworkBehaviour
	{
		public override void Spawned()
		{
			if (Runner.GameMode == GameMode.Server)
			{
				gameObject.SetActive(false);
			}
		}
	}
}
