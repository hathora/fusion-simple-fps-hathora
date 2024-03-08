namespace Fusion.Menu {
  using System.Threading.Tasks;
  using System.Linq;
  using TMPro;
  using UnityEngine;
  using UnityEngine.UI;

  /// <summary>
  /// The scene selection screen.
  /// </summary>
  public partial class PhotonMenuUIScenes : PhotonMenuUIScreen {
    /// <summary>
    /// The dropdown element for the scene selection.
    /// </summary>
    [InlineHelp, SerializeField] protected TMP_Dropdown _availableScenes;
    /// <summary>
    /// The image element for the screenshot preview.
    /// </summary>
    [InlineHelp, SerializeField] protected Image _preview;
    /// <summary>
    /// The default sprite to use when there is no scene preview.
    /// </summary>
    [InlineHelp, SerializeField] protected Sprite _defaultSprite;
    /// <summary>
    /// The back button.
    /// </summary>
    [InlineHelp, SerializeField] protected Button _backButton;
    /// <summary>
    /// Set this to automatically transition to main menu after selecting a map.
    /// </summary>
    public int WaitAfterSelectionAndReturnToMainMenuInMs = 0;

    partial void AwakeUser();
    partial void InitUser();
    partial void ShowUser();
    partial void HideUser();
    partial void SaveChangesUser();

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

      _availableScenes.ClearOptions();
      _availableScenes.AddOptions(Config.AvailableScenes.Select(m => m.Name).ToList());

      var sceneIndex = Config.AvailableScenes.FindIndex(m => m.ScenePath == ConnectionArgs.Scene.ScenePath);
      if (sceneIndex >= 0) {
        _availableScenes.SetValueWithoutNotify(sceneIndex);
        RefreshPreviewSprite();
      } else if (Config.AvailableScenes.Count > 0) {
        _availableScenes.SetValueWithoutNotify(0);
        ConnectionArgs.Scene = Config.AvailableScenes[0];
      }

      ShowUser();
    }

    /// <summary>
    /// The screen hide method. Calls partial method <see cref="HideUser"/> to be implemented on the SDK side.
    /// </summary>
    public override void Hide() {
      base.Hide();
      HideUser();
    }

    /// <summary>
    /// Is called when the <see cref="_backButton"/> is pressed using SendMessage() from the UI object.
    /// </summary>
    public virtual void OnBackButtonPressed() {
      Controller.Show<PhotonMenuUIMain>();
    }


    /// <summary>
    /// Is called when <see cref="_availableScenes"/> has changed it's value from the UI using SendMessage().
    /// </summary>
    protected virtual async void OnSaveChanges() {
      RefreshPreviewSprite();

      ConnectionArgs.Scene = Config.AvailableScenes[_availableScenes.value];

      SaveChangesUser();

      if (WaitAfterSelectionAndReturnToMainMenuInMs > 0) {
        await Task.Delay(WaitAfterSelectionAndReturnToMainMenuInMs);
        Controller.Show<PhotonMenuUIMain>();
      }
    }

    /// <summary>
    /// Updates the preview sprite and calls <see cref="PhotonMenuImageFitter.OnResolutionChanged"/> via SendMessage().
    /// </summary>
    protected void RefreshPreviewSprite() {
      if (Config.AvailableScenes[_availableScenes.value].Preview == null) {
        _preview.sprite = _defaultSprite;
      } else {
        _preview.sprite = Config.AvailableScenes[_availableScenes.value].Preview;
      }

      _preview.gameObject.SendMessage("OnResolutionChanged");
    }
  }
}
