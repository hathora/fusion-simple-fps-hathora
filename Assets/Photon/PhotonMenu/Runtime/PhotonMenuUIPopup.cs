namespace Fusion.Menu {
  using System.Threading.Tasks;
  using TMPro;
  using UnityEngine;
  using UnityEngine.UI;

  /// <summary>
  /// The popup screen handles notificaction.
  /// The screen has be <see cref="PhotonMenuUIScreen.IsModal"/> true.
  /// </summary>
  public partial class PhotonMenuUIPopup : PhotonMenuUIScreen, IPhotonMenuPopup {
    /// <summary>
    /// The text field for the message.
    /// </summary>
    [InlineHelp, SerializeField] protected TMP_Text _text;
    /// <summary>
    /// The text field for the header.
    /// </summary>
    [InlineHelp, SerializeField] protected TMP_Text _header;
    /// <summary>
    /// The okay button.
    /// </summary>
    [InlineHelp, SerializeField] protected Button _button;

    /// <summary>
    /// The completion source will be triggered when the screen has been hidden.
    /// </summary>
    protected TaskCompletionSource<bool> _taskCompletionSource;

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
    /// </summary>
    public override void Show() {
      base.Show();
      ShowUser();
    }

    /// <summary>
    /// The screen hide method. Calls partial method <see cref="HideUser"/> to be implemented on the SDK side.
    /// </summary>
    public override void Hide() {
      base.Hide();

      _taskCompletionSource?.SetResult(true);
      _taskCompletionSource = null;

      HideUser();
    }

    /// <summary>
    /// Open the screen in overlay mode
    /// </summary>
    /// <param name="msg">Message</param>
    /// <param name="header">Header, can be null</param>
    public virtual void OpenPopup(string msg, string header) {
      _header.text = header;
      _text.text = msg;

      Show();
    }

    /// <summary>
    /// Open the screen and wait for it being hidden
    /// </summary>
    /// <param name="msg">Message</param>
    /// <param name="header">Header, can be null</param>
    /// <returns>When the screen is hidden.</returns>
    public virtual Task OpenPopupAsync(string msg, string header) {
      _taskCompletionSource?.SetResult(true);
      _taskCompletionSource = new TaskCompletionSource<bool>();

      OpenPopup(msg, header);

      return _taskCompletionSource.Task;
    }
  }
}
