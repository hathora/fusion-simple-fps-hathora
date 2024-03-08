namespace Fusion.Menu {
  using System.Text;
  using TMPro;
  using UnityEngine;
  using UnityEngine.UI;

  /// <summary>
  /// The gameplay screen.
  /// </summary>
  public partial class PhotonMenuUIGameplay : PhotonMenuUIScreen {
    /// <summary>
    /// The session code label.
    /// </summary>
    [InlineHelp, SerializeField] protected TMP_Text _codeText;
    /// <summary>
    /// The list of players.
    /// </summary>
    [InlineHelp, SerializeField] protected TMP_Text _playersText;
    /// <summary>
    /// The current player count.
    /// </summary>
    [InlineHelp, SerializeField] protected TMP_Text _playersCountText;
    /// <summary>
    /// The max player count.
    /// </summary>
    [InlineHelp, SerializeField] protected TMP_Text _playersMaxCountText;
    /// <summary>
    /// The menu header text.
    /// </summary>
    [InlineHelp, SerializeField] protected TMP_Text _headerText;
    /// <summary>
    /// The GameObject of the session part to be toggled off.
    /// </summary>
    [InlineHelp, SerializeField] protected GameObject _sessionGameObject;
    /// <summary>
    /// The GameObject of the player part to be toggled off.
    /// </summary>
    [InlineHelp, SerializeField] protected GameObject _playersGameObject;
    /// <summary>
    /// The copy session button.
    /// </summary>
    [InlineHelp, SerializeField] protected Button _copySessionButton;
    /// <summary>
    /// The disconnect button.
    /// </summary>
    [InlineHelp, SerializeField] protected Button _disconnectButton;

    partial void AwakeUser();
    partial void InitUser();
    partial void ShowUser();
    partial void HideUser();

    /// <summary>
    /// The Unity awake method. Calls partial method <see cref="AwakeUser"/> to be implemented on the SDK side.
    /// </summary>
    public override void Awake() {
      base.Awake();
      AwakeUser();
    }

    /// <summary>
    /// The screen init method. Calls partial method <see cref="InitUser"/> to be implemented on the SDK side.
    /// </summary>
    public override void Init() {
      base.Init();
      InitUser();
    }

    /// <summary>
    /// The screen show method. Calls partial method <see cref="ShowUser"/> to be implemented on the SDK side.
    /// Will check is the session code is compatible with the party code to toggle the session UI part.
    /// </summary>
    public override void Show() {
      base.Show();
      ShowUser();

      if (Config.CodeGenerator != null && Config.CodeGenerator.IsValid(Connection.SessionName)) {
        // Only show the session UI if it is a party code
        _codeText.SetText(Connection.SessionName);
        _sessionGameObject.SetActive(true);
      } else {
        _codeText.SetText(string.Empty);
        _sessionGameObject.SetActive(false);
      }

      UpdateUsernames();
    }

    /// <summary>
    /// The screen hide method. Calls partial method <see cref="HideUser"/> to be implemented on the SDK side.
    /// </summary>
    public override void Hide() { 
      base.Hide();
      HideUser();
    }

    /// <summary>
    /// Is called when the <see cref="_disconnectButton"/> is pressed using SendMessage() from the UI object.
    /// </summary>
    protected virtual async void OnDisconnectPressed() {
      await Connection.DisconnectAsync(ConnectFailReason.UserRequest);
      Controller.Show<PhotonMenuUIMain>();
    }

    /// <summary>
    /// Is called when the <see cref="_copySessionButton"/> is pressed using SendMessage() from the UI object.
    /// </summary>
    protected virtual void OnCopySessionPressed() {
      GUIUtility.systemCopyBuffer = _codeText.text;
    }

    /// <summary>
    /// Update the usernames and toggle the UI part on/off depending on the <see cref="IPhotonMenuConnection.Usernames"/>
    /// </summary>
    protected virtual void UpdateUsernames() {
      if (Connection.Usernames != null && Connection.Usernames.Count > 0) {
        _playersGameObject.SetActive(true);
        var sBuilder = new StringBuilder();
        foreach (var username in Connection.Usernames) {
          sBuilder.AppendLine(username);
        }
        _playersText.text = sBuilder.ToString();
        _playersCountText.SetText($"{Connection.Usernames.Count}");
        _playersMaxCountText.SetText($"/{Connection.MaxPlayerCount}");
      } else {
        _playersGameObject.SetActive(false);
      }
    }
  }
}
