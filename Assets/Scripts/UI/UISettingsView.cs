using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

namespace SimpleFPS
{
	public class UISettingsView : MonoBehaviour
	{
		public Slider          Volume;
		public Slider          Sensitivity;
		public TextMeshProUGUI SensitivityValue;

		public void LoadSettings()
		{
			float volume = PlayerPrefs.GetFloat("Volume", 1f);
			Volume.value = volume;

			float sensitivity = PlayerPrefs.GetFloat("Sensitivity", 3f);
			Sensitivity.value = sensitivity;

			AudioListener.volume = volume;
			PlayerInput.LookSensitivity = sensitivity;
		}

		// Called from slider OnChanged event.
		public void SensitivityChanged(float value)
		{
			PlayerInput.LookSensitivity = value;
			PlayerPrefs.SetFloat("Sensitivity", value);

			SensitivityValue.text = $"{value:F1}";
		}

		// Called from slider OnChanged event.
		public void VolumeChanged(float value)
		{
			AudioListener.volume = value;
			PlayerPrefs.SetFloat("Volume", value);
		}

		// Called from button OnClick event.
		public void CloseView()
		{
			gameObject.SetActive(false);
			GetComponentInParent<GameUI>().MenuView.SetActive(false);
		}

		private void Update()
		{
			if (Keyboard.current.escapeKey.wasPressedThisFrame)
			{
				gameObject.SetActive(false);
			}
		}
	}
}
