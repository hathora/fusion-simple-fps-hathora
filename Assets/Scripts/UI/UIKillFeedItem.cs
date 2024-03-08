using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace SimpleFPS
{
	public class UIKillFeedItem : MonoBehaviour
	{
		public TextMeshProUGUI Killer;
		public TextMeshProUGUI Victim;
		public Image           WeaponIcon;
		public GameObject      CriticalKillGroup;
	}
}
