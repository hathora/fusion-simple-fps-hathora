using UnityEngine;

namespace SimpleFPS
{
	public class UICrosshair : MonoBehaviour
	{
		[Header("Hit UI")]
		public GameObject  RegularHit;
		public GameObject  CriticalHit;
		public GameObject  FatalHit;

		[Header("Hit Sounds")]
		public AudioSource RegularHitSound;
		public AudioSource CriticalHitSound;
		public AudioSource FatalHitSound;

		public void ShowHit(bool isFatal, bool isCritical)
		{
			var hitObject = isFatal ? FatalHit : (isCritical ? CriticalHit : RegularHit);

			// Restart hit animation
			hitObject.SetActive(false);
			hitObject.SetActive(true);

			var hitSound = isFatal ? FatalHitSound : (isCritical ? CriticalHitSound : RegularHitSound);
			if (hitSound != null)
			{
				hitSound.PlayOneShot(hitSound.clip);
			}
		}

		private void OnEnable()
		{
			RegularHit.SetActive(false);
			CriticalHit.SetActive(false);
			FatalHit.SetActive(false);
		}
	}
}
