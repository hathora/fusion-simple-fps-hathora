namespace Fusion.Menu {
  using System;
  using System.Threading.Tasks;
  using TMPro;
  using UnityEngine;
  using UnityEngine.UI;

  /// <summary>
  /// The main menu.
  /// </summary>
  public partial class PhotonMenuUIMain : PhotonMenuUIScreen {
    /// <summary>
    /// The username label.
    /// </summary>
    [InlineHelp, SerializeField] protected TMP_Text _usernameLabel;
    /// <summary>
    /// The scene thumbnail. Can be null.
    /// </summary>
    [InlineHelp, SerializeField] protected Image _sceneThumbnail;
    /// <summary>
    /// The username input UI part.
    /// </summary>
    [InlineHelp, SerializeField] protected GameObject _usernameView;
    /// <summary>
    /// The actual username input field.
    /// </summary>
    [InlineHelp, SerializeField] protected TMP_InputField _usernameInput;
    /// <summary>
    /// The username confirmation button (background).
    /// </summary>
    [InlineHelp, SerializeField] protected Button _usernameConfirmButton;
    /// <summary>
    /// The username change button.
    /// </summary>
    [InlineHelp, SerializeField] protected Button _usernameButton;
    /// <summary>
    /// The open character selection button.
    /// </summary>
    [InlineHelp, SerializeField] protected Button _characterButton;
    /// <summary>
    /// The open party screen button.
    /// </summary>
    [InlineHelp, SerializeField] protected Button _partyButton;
    /// <summary>
    /// The quikc play button.
    /// </summary>
    [InlineHelp, SerializeField] protected Button _playButton;
    /// <summary>
    /// The quit button.
    /// </summary>
    [InlineHelp, SerializeField] protected Button _quitButton;
    /// <summary>
    /// The open scene screen button.
    /// </summary>
    [InlineHelp, SerializeField] protected Button _sceneButton;
    /// <summary>
    /// The open setting button.
    /// </summary>
    [InlineHelp, SerializeField] protected Button _settingsButton;
    /// <summary>
    /// Callback fired before the connection is created.
    /// This can stop the connection attempt with an <see cref="ConnectResult"/>.
    /// </summary>
    public Func<IPhotonMenuConnectArgs, Task<ConnectResult>> OnBeforeConnection;

    partial void AwakeUser();
    partial void InitUser();
    partial void ShowUser();
    partial void HideUser();
    partial void BeforeConnectUser();


    /// <summary>
    /// The Unity awake method. Calls partial method <see cref="AwakeUser"/> to be implemented on the SDK side.
    /// Applies the current selected graphics settings (loaded from PlayerPrefs)
    /// </summary>
    public override void Awake() {
      base.Awake();

      new PhotonMenuGraphicsSettings().Apply();

#if UNITY_STANDALONE
      _quitButton.gameObject.SetActive(true);
#else 
      _quitButton.gameObject.SetActive(false);
#endif

      AwakeUser();
    }

    /// <summary>
    /// The screen init method. Calls partial method <see cref="InitUser"/> to be implemented on the SDK side.
    /// Initialized the default arguments.
    /// </summary>
    public override void Init() {
      base.Init();

      ConnectionArgs.SetDefaults(Config);

      InitUser();
    }

    /// <summary>
    /// The screen show method. Calls partial method <see cref="ShowUser"/> to be implemented on the SDK side.
    /// </summary>
    public override void Show() {
      base.Show();

      _usernameView.SetActive(false);
      _usernameLabel.SetText(ConnectionArgs.Username);

      if (Config.AvailableScenes.Count > 0) {
        _sceneButton.gameObject.SetActive(true);
      } else {
        _sceneButton.gameObject.SetActive(false);
      }

      if (_sceneThumbnail != null) {
        if (ConnectionArgs.Scene.Preview != null) {
          _sceneThumbnail.transform.parent.gameObject.SetActive(true);
          _sceneThumbnail.sprite = ConnectionArgs.Scene.Preview;
          _sceneThumbnail.gameObject.SendMessage("OnResolutionChanged");
        } else {
          _sceneThumbnail.transform.parent.gameObject.SetActive(false);
          _sceneThumbnail.sprite = null;
        }
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
    /// Is called when the sceen background is pressed using SendMessage() from the UI object.
    /// </summary>
    protected virtual void OnFinishUsernameEdit() {
      OnFinishUsernameEdit(_usernameInput.text);
    }

    /// <summary>
    /// Is called when the <see cref="_usernameInput"/> has finished editing using SendMessage() from the UI object.
    /// </summary>
    protected virtual void OnFinishUsernameEdit(string username) {
      _usernameView.SetActive(false);

      if (string.IsNullOrEmpty(username) == false) {
        _usernameLabel.SetText(username);
        ConnectionArgs.Username = username;
      }
    }

    /// <summary>
    /// Is called when the <see cref="_usernameButton"/> is pressed using SendMessage() from the UI object.
    /// </summary>
    protected virtual void OnUsernameButtonPressed() {
      _usernameView.SetActive(true);
      _usernameInput.text = _usernameLabel.text;
    }

    /// <summary>
    /// Is called when the <see cref="_playButton"/> is pressed using SendMessage() from the UI object.
    /// Intitiates the connection and expects the connection object to set further screen states.
    /// </summary>
    protected virtual async void OnPlayButtonPressed() {
      ConnectionArgs.Session = null;
      ConnectionArgs.Creating = false;
      ConnectionArgs.Region = ConnectionArgs.PreferredRegion;

      BeforeConnectUser();

      Controller.Show<PhotonMenuUILoading>();

      var result = new ConnectResult { Success = true };
      if (OnBeforeConnection != null) {
        result = await OnBeforeConnection.Invoke(ConnectionArgs);
      }

      if (result.Success) {
        result = await Connection.ConnectAsync(ConnectionArgs);
      }

      if (result.CustomResultHandling == false) {
        if (result.Success) {
          Controller.Show<PhotonMenuUIGameplay>();
        } else {
          var popup = Controller.PopupAsync(result.DebugMessage, "Connection Failed");
          if (result.WaitForCleanup != null) {
            await Task.WhenAll(result.WaitForCleanup, popup);
          } else {
            await popup;
          }
          Controller.Show<PhotonMenuUIMain>();
        }
      }
    }

    /// <summary>
    /// Is called when the <see cref="_partyButton"/> is pressed using SendMessage() from the UI object.
    /// </summary>
    protected virtual void OnPartyButtonPressed() {
      Controller.Show<PhotonMenuUIParty>();
    }

    /// <summary>
    /// Is called when the <see cref="_sceneButton"/> is pressed using SendMessage() from the UI object.
    /// </summary>
    protected virtual void OnScenesButtonPressed() {
      Controller.Show<PhotonMenuUIScenes>();
    }

    /// <summary>
    /// Is called when the <see cref="_settingsButton"/> is pressed using SendMessage() from the UI object.
    /// </summary>
    protected virtual void OnSettingsButtonPressed() {
      Controller.Show<PhotonMenuUISettings>();
    }

    /// <summary>
    /// Is called when the <see cref="_characterButton"/> is pressed using SendMessage() from the UI object.
    /// </summary>
    protected virtual void OnCharacterButtonPressed() {
    }

    /// <summary>
    /// Is called when the <see cref="_quitButton"/> is pressed using SendMessage() from the UI object.
    /// </summary>
    protected virtual void OnQuitButtonPressed() {
      Application.Quit();
    }
  }
}
