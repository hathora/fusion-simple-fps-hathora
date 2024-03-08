namespace Fusion.Menu {
  using System;
  using TMPro;
  using UnityEngine;
  using UnityEngine.UI;

  /// <summary>
  /// The ping plugin can display the current <see cref="IPhotonMenuConnection.Ping"/> and/or show a color code.
  /// </summary>
  public class PhotonMenuScreenPluginPing : PhotonMenuScreenPlugin {
    /// <summary>
    /// Saves a maximum ping number and the related color code.
    /// </summary>
    [Serializable]
    public struct ColorThresholds {
      /// <summary>
      /// Max ping number that this color code is valid for.
      /// </summary>
      public int MaxPing;
      /// <summary>
      /// Color code symbolizing the connection quality.
      /// </summary>
      public Color Color;
    }

    /// <summary>
    /// The ping text. Can be null.
    /// </summary>
    [InlineHelp, SerializeField] protected TMP_Text _pingText;
    /// <summary>
    /// The ping color. Can be null.
    /// </summary>
    [InlineHelp, SerializeField] protected Image _coloredImage;
    /// <summary>
    /// The color thresholds. Must be set if <see cref="_coloredImage"/> is set.
    /// </summary>
    [InlineHelp, SerializeField] protected ColorThresholds[] _colorsThresholds;

    private IPhotonMenuConnection _connection;

    /// <summary>
    /// The parent screen is shown. Cache the connection object.
    /// </summary>
    /// <param name="screen">Parent screen</param>
    public override void Show(PhotonMenuUIScreen screen) {
      base.Show(screen);

      _connection = screen.Connection;
    }

    /// <summary>
    /// The parent screen is hidden. Clear the connection object.
    /// </summary>
    /// <param name="screen">Parent screen</param>
    public override void Hide(PhotonMenuUIScreen screen) {
      base.Hide(screen);

      _connection = null;
    }

    /// <summary>
    /// Unity update method to update text and/or color code.
    /// </summary>
    public virtual void Update() {
      if (_connection == null) {
        return;
      }

      if (_pingText != null) {
        _pingText.text = _connection.Ping.ToString();
      }

      if (_coloredImage != null) {
        for (int i = 0; i < _colorsThresholds.Length; i++) {
          if (_connection.Ping <= _colorsThresholds[i].MaxPing || i == _colorsThresholds.Length - 1) {
            if (_coloredImage.color != _colorsThresholds[i].Color) {
              _coloredImage.color = _colorsThresholds[i].Color;
            }
            break;
          }
        }
      }
    }
  }
}
