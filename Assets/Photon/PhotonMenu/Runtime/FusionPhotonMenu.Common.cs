// merged ef65887

#region IPhotonMenuConfig.cs

namespace Fusion.Menu {
  using System.Collections.Generic;

  /// <summary>
  /// The menu config interface.
  /// <see cref="PhotonMenuConfig"/> can be derived or this interface implemented to change things.
  /// </summary>
  public interface IPhotonMenuConfig {
    /// <summary>
    /// List of avilable app version shown in the settings screen.
    /// </summary>
    List<string> AvailableAppVersions { get; }
    /// <summary>
    /// List of available Photon region shown in the settings screen.
    /// </summary>
    List<string> AvailableRegions { get; }
    /// <summary>
    /// List of avilable Unity scene shown in the screen selection.
    /// </summary>
    List<PhotonMeneSceneInfo> AvailableScenes { get; }
    /// <summary>
    /// The max player count for all game modes.
    /// </summary>
    int MaxPlayerCount { get; }
    /// <summary>
    /// The machine id shown in as an AppVersion option.
    /// </summary>
    string MachineId { get; }
    /// <summary>
    /// The party code generator.
    /// </summary>
    PhotonMenuPartyCodeGenerator CodeGenerator { get; }
    /// <summary>
    /// Force 60 FPS during menu animation on mobile platforms.
    /// </summary>
    bool AdaptFramerateForMobilePlatform { get; }
  }
}

#endregion


#region IPhotonMenuConnectArgs.cs

namespace Fusion.Menu {
  /// <summary>
  /// The menu connection args.
  /// For convenience they are accessible from each menu screen and the default implementaton saves the data directly into PlayerPrefs.
  /// </summary>
  public interface IPhotonMenuConnectArgs {
    /// <summary>
    /// The username / nickname.
    /// </summary>
    string Username { get; set; }
    /// <summary>
    /// The session / Photon room name. Can be null.
    /// </summary>
    string Session { get; set; }
    /// <summary>
    /// The preferred Photon region. Null = best region.
    /// </summary>
    string PreferredRegion { get; set; }
    /// <summary>
    /// The region to use for the connection.
    /// </summary>
    string Region { get; set; }
    /// <summary>
    /// The Photon AppVersion to use.
    /// </summary>
    string AppVersion { get; set; }
    /// <summary>
    /// The max player count for the connection.
    /// </summary>
    int MaxPlayerCount { get; set; }
    /// <summary>
    /// The selected scene meta information.
    /// </summary>
    PhotonMeneSceneInfo Scene { get; set; }
    /// <summary>
    /// Toggle creation then uses the supplied <see cref="Session"/>.
    /// </summary>
    bool Creating { get; set; }

    /// <summary>
    /// Set all values to their default.
    /// </summary>
    /// <param name="config">Config</param>
    void SetDefaults(IPhotonMenuConfig config);
  }
}

#endregion


#region IPhotonMenuConnection.cs

namespace Fusion.Menu {
  using System.Collections.Generic;
  using System.Threading.Tasks;

  /// <summary>
  /// The connection interface to be implemented by the SDK.
  /// </summary>
  public interface IPhotonMenuConnection {
    /// <summary>
    /// Access the session name/ Photon room name.
    /// </summary>
    string SessionName { get; }
    /// <summary>
    /// Access the max player count.
    /// </summary>
    int MaxPlayerCount { get; }
    /// <summary>
    /// Access the actual region connected to.
    /// </summary>
    string Region { get; }
    /// <summary>
    /// Access the AppVersion used.
    /// </summary>
    string AppVersion { get; }
    /// <summary>
    /// Get a list of usernames that are inside this session.
    /// </summary>
    List<string> Usernames { get; }
    /// <summary>
    /// Is connection alive.
    /// </summary>
    bool IsConnected { get; }
    /// <summary>
    /// Get current connection ping.
    /// </summary>
    public int Ping { get; }

    /// <summary>
    /// The connection task.
    /// </summary>
    /// <param name="connectArgs">Connection args.</param>
    /// <returns>When the connection is established and the game ready.</returns>
    Task<ConnectResult> ConnectAsync(IPhotonMenuConnectArgs connectArgs);
    /// <summary>
    /// Disconnect task.
    /// </summary>
    /// <param name="reason">Disconnect reason <see cref="ConnectFailReason>"/></param>
    /// <returns>When the connection has terminated gracefully.</returns>
    Task DisconnectAsync(int reason);

    /// <summary>
    /// Get available regions from Photon namemserver. Must be implemented on SDK level.
    /// The list should be sorted by code.
    /// </summary>
    /// <returns>List of Photon regions, including the last ping, that are listed on the Photon dashboard for this AppId.</returns>
    Task<List<PhotonMenuOnlineRegion>> RequestAvailableOnlineRegionsAsync(IPhotonMenuConnectArgs connectArgs);
  }
}

#endregion


#region IPhotonMenuPopup.cs

namespace Fusion.Menu {
  using System.Threading.Tasks;

  /// <summary>
  /// Popup menu is implemented by <see cref="PhotonMenuUIPopup"/> screen.
  /// </summary>
  public interface IPhotonMenuPopup {
    /// <summary>
    /// Open screen with message.
    /// </summary>
    /// <param name="msg">Message</param>
    /// <param name="header">Header</param>
    void OpenPopup(string msg, string header);
    /// <summary>
    /// Open screen and wait until it closed.
    /// </summary>
    /// <param name="msg">Message</param>
    /// <param name="header">Header</param>
    /// <returns>When the screen is closed.</returns>
    Task OpenPopupAsync(string msg, string header);
  }
}

#endregion


#region IPhotonMenuUIController.cs

namespace Fusion.Menu {
  using System.Threading.Tasks;

  /// <summary>
  /// The UI screen controller interface.
  /// </summary>
  public interface IPhotonMenuUIController {
    /// <summary>
    /// Show a screen by type.
    /// </summary>
    /// <typeparam name="S">Screentype</typeparam>
    void Show<S>() where S : PhotonMenuUIScreen;
    /// <summary>
    /// Start a popup.
    /// </summary>
    /// <param name="msg">Message</param>
    /// <param name="header">Header</param>
    void Popup(string msg, string header = default);
    /// <summary>
    /// Start and async popup.
    /// </summary>
    /// <param name="msg">Message</param>
    /// <param name="header">Header</param>
    /// <returns>When the popup is closed.</returns>
    public Task PopupAsync(string msg, string header = default);
    /// <summary>
    /// Get a screen by type.
    /// </summary>
    /// <typeparam name="S">Screentype</typeparam>
    /// <returns></returns>
    S Get<S>() where S : PhotonMenuUIScreen;
  }
}

#endregion


#region PhotonMeneSceneInfo.cs

namespace Fusion.Menu {
  using System;
  using System.IO;
  using UnityEngine;

  /// <summary>
  /// Info struct for creating configurable selectable scenes in the Photon menu.
  /// </summary>
  [Serializable]
  public partial struct PhotonMeneSceneInfo {
    /// <summary>
    /// Displayed scene name.
    /// </summary>
    public string Name;
    /// <summary>
    /// The path to the scene asset.
    /// </summary>
    [ScenePath] public string ScenePath;
    /// <summary>
    /// Gets the filename of the ScenePath to set as Unity scene to load during connection sequence.
    /// </summary>
    public string SceneName => ScenePath == null ? null : Path.GetFileNameWithoutExtension(ScenePath);
    /// <summary>
    /// The sprite displayed as scene preview in the scene selection UI.
    /// </summary>
    public Sprite Preview;
  }
}

#endregion


#region PhotonMenuConnectArgs.cs

namespace Fusion.Menu {
  using System;
  using UnityEngine;

  /// <summary>
  /// The connection options selected by the client which are operated on directly from <see cref="PlayerPrefs"/>.
  /// The menu screens all have the same instance of this object.
  /// </summary>
  public class PhotonMenuConnectArgs : IPhotonMenuConnectArgs {
    /// <summary>
    /// The username configured in the menu.
    /// </summary>
    public virtual string Username {
      get => PlayerPrefs.GetString("Photon.Menu.Username");
      set => PlayerPrefs.SetString("Photon.Menu.Username", value);
    }

    /// <summary>
    /// The session that the client wants to join. Is not persisted. Use ReconnectionInformation instead to recover it between application shutdowns.
    /// </summary>
    public virtual string Session { get; set; }

    /// <summary>
    /// The preferred region the user selected in the menu.
    /// </summary>
    public virtual string PreferredRegion {
      get => PlayerPrefs.GetString("Photon.Menu.Region");
      set => PlayerPrefs.SetString("Photon.Menu.Region", string.IsNullOrEmpty(value) ? value : value.ToLower());
    }

    /// <summary>
    /// The actual region that the client will connect to.
    /// </summary>
    public virtual string Region { get; set; }

    /// <summary>
    /// The app version used for the Photon connection.
    /// </summary>
    public virtual string AppVersion {
      get => PlayerPrefs.GetString("Photon.Menu.AppVersion");
      set => PlayerPrefs.SetString("Photon.Menu.AppVersion", value);
    }

    /// <summary>
    /// The max player count that the user selected in the menu.
    /// </summary>
    public virtual int MaxPlayerCount {
      get => PlayerPrefs.GetInt("Photon.Menu.MaxPlayerCount");
      set => PlayerPrefs.SetInt("Photon.Menu.MaxPlayerCount", value);
    }

    /// <summary>
    /// The map or scene information that the user selected in the menu.
    /// </summary>
    public virtual PhotonMeneSceneInfo Scene {
      get {
        try {
          return JsonUtility.FromJson<PhotonMeneSceneInfo>(PlayerPrefs.GetString("Photon.Menu.Scene"));
        }
        catch {
          return default(PhotonMeneSceneInfo);
        }
      }
      set => PlayerPrefs.SetString("Photon.Menu.Scene", JsonUtility.ToJson(value));
    }

    /// <summary>
    /// Toggle to create or join-only game sessions/rooms.
    /// </summary>
    public virtual bool Creating { get; set; }

    /// <summary>
    /// Make sure that all configuration have a default settings.
    /// </summary>
    /// <param name="config">The menu config.</param>
    public virtual void SetDefaults(IPhotonMenuConfig config) {
      Session = null;
      Creating = false;

      if (AppVersion == null || (AppVersion != config.MachineId && config.AvailableAppVersions.Contains(AppVersion) == false)) {
        AppVersion = config.MachineId;
      }

      if (PreferredRegion != null && config.AvailableRegions.Contains(PreferredRegion) == false) {
        PreferredRegion = string.Empty;
      }

      if (MaxPlayerCount <= 0 || MaxPlayerCount > config.MaxPlayerCount) {
        MaxPlayerCount = config.MaxPlayerCount;
      }

      if (string.IsNullOrEmpty(Username)) {
        Username = $"Player{config.CodeGenerator.Create(3)}";
      }

      if (string.IsNullOrEmpty(Scene.Name)) {
        Scene = config.AvailableScenes[0];
      }
      else {
        var index = config.AvailableScenes.FindIndex(s => s.Name == Scene.Name);
        // Overwrite anything in storage with fresh information from the config
        Scene = config.AvailableScenes[Mathf.Clamp(index, 0, config.AvailableScenes.Count - 1)];
      }
    }
  }
}

#endregion


#region PhotonMenuConnectFailReason.cs

namespace Fusion.Menu {
  /// <summary>
  /// Is used to convey some information about a connection error back to the caller.
  /// </summary>
  public partial class ConnectFailReason {
    /// <summary>
    /// User requested cancellation or disconnect.
    /// </summary>
    public const int UserRequest = 1;
    /// <summary>
    /// Disconnect
    /// </summary>
    public const int Disconnect = 2;
  }
}

#endregion


#region PhotonMenuConnectionBehaviour.cs

namespace Fusion.Menu {
  using System.Collections.Generic;
  using System.Threading.Tasks;

  /// <summary>
  /// A wrapper for a <see cref="IPhotonMenuConnection"/> that is owned by a Unity game object and visible in the inspector this way.
  /// </summary>
  public abstract class PhotonMenuConnectionBehaviour : Fusion.Behaviour, IPhotonMenuConnection {
    /// <summary>
    /// The actual <see cref="IPhotonMenuConnection"/> object underneath. Is lazily created during either <see cref="ConnectAsync(IPhotonMenuConnectArgs)"/> or <see cref="RequestAvailableOnlineRegionsAsync"/>.
    /// </summary>
    public IPhotonMenuConnection Connection;
    /// <summary>
    /// The session name (Photon room) that the client is connected to.
    /// </summary>
    public string SessionName => Connection.SessionName;
    /// <summary>
    /// The maximum number of clients for this connection.
    /// </summary>
    public int MaxPlayerCount => Connection.MaxPlayerCount;
    /// <summary>
    /// The region of the game server that the client is connected to.
    /// </summary>
    public string Region => Connection.Region;
    /// <summary>
    /// The AppVersion used.
    /// </summary>
    public string AppVersion => Connection.AppVersion;
    /// <summary>
    /// A list of user names that are also connected to the session.
    /// </summary>
    public List<string> Usernames => Connection.Usernames;
    /// <summary>
    /// Is the connection object valid and is the connection alive.
    /// </summary>
    public bool IsConnected => Connection != null && Connection.IsConnected;
    /// <summary>
    /// The current ping.
    /// </summary>
    public int Ping => Connection.Ping;

    /// <summary>
    /// Factory method to be implemented in a derived class.
    /// </summary>
    /// <returns>Concrete <see cref="IPhotonMenuConnection"/> object.</returns>
    public abstract IPhotonMenuConnection Create();

    /// <summary>
    /// Connect using <see cref="IPhotonMenuConnectArgs"/>.
    /// </summary>
    /// <param name="connectionArgs">Connection arguments.</param>
    /// <returns>When the connection is established</returns>
    public Task<ConnectResult> ConnectAsync(IPhotonMenuConnectArgs connectionArgs) {
      if (Connection == null) {
        Connection = Create();
      }

      return Connection.ConnectAsync(connectionArgs);
    }

    /// <summary>
    /// Disconnect the current connection.
    /// </summary>
    /// <param name="reason">The disconnect reason <see cref="ConnectFailReason"/></param>
    /// <returns></returns>
    public Task DisconnectAsync(int reason) {
      if (Connection != null) {
        return Connection.DisconnectAsync(reason);
      }

      return Task.CompletedTask;
    }

    /// <summary>
    /// Requests a list of available regions from the name server.
    /// </summary>
    /// <param name="connectionArgs">Connection arguments</param>
    /// <returns>List of available region configured in the dashboard for this app.</returns>
    public Task<List<PhotonMenuOnlineRegion>> RequestAvailableOnlineRegionsAsync(IPhotonMenuConnectArgs connectionArgs) {
      if (Connection == null) {
        Connection = Create();
      }

      return Connection.RequestAvailableOnlineRegionsAsync(connectionArgs);
    }
  }
}

#endregion


#region PhotonMenuConnectResult.cs

namespace Fusion.Menu {
  using System.Threading.Tasks;

  /// <summary>
  /// Connection result info object.
  /// </summary>
  public partial class ConnectResult {
    /// <summary>
    /// Is successful
    /// </summary>
    public bool Success;
    /// <summary>
    /// The fail reason code <see cref="ConnectFailReason"/>
    /// </summary>
    public int FailReason;
    /// <summary>
    /// A debug message.
    /// </summary>
    public string DebugMessage;
    /// <summary>
    /// Set to true to disable all error handling by the menu.
    /// </summary>
    public bool CustomResultHandling;
    /// <summary>
    /// An optional task to signal the menu to wait until cleanup operation have completed (e.g. level unloading).
    /// </summary>
    public Task WaitForCleanup;
  }
}

#endregion


#region PhotonMenuGraphicsSettings.cs

namespace Fusion.Menu {
  using System;
  using System.Collections.Generic;
  using System.Linq;
  using UnityEngine;

  /// <summary>
  /// Graphics settings that can be changed in the settings screen.
  /// Selected values are stored in <see cref="PlayerPrefs"/>
  /// Use <see cref="Apply()"/> to apply all after starting the app.
  /// </summary>
  public partial class PhotonMenuGraphicsSettings {
    /// <summary>
    /// Available framerates.
    /// -1 = platform default
    /// </summary>
    protected static int[] PossibleFramerates = new int[] { -1, 30, 60, 75, 90, 120, 144, 165, 240, 360 };

    /// <summary>
    /// Target framerate
    /// </summary>
    public virtual int Framerate {
      get {
        var f = PlayerPrefs.GetInt("Photon.Menu.Framerate", -1);
        if (PossibleFramerates.Contains(f) == false) {
          return PossibleFramerates[0];
        }
        return f;
      }
      set => PlayerPrefs.SetInt("Photon.Menu.Framerate", value);
    }

    /// <summary>
    /// Fullscreen mode.
    /// Is not shown for mobile platforms.
    /// </summary>
    public virtual bool Fullscreen {
      get => PlayerPrefs.GetInt("Photon.Menu.Fullscreen", Screen.fullScreen ? 1 : 0) == 1;
      set => PlayerPrefs.SetInt("Photon.Menu.Fullscreen", value ? 1 : 0);
    }

    /// <summary>
    /// Selected resolution index based on Screen.resolutions.
    /// Is not shown for mobile platforms.
    /// </summary>
    public virtual int Resolution {
      get => Math.Clamp(PlayerPrefs.GetInt("Photon.Menu.Resolution", GetCurrentResolutionIndex()), 0, Screen.resolutions.Length - 1);
      set => PlayerPrefs.SetInt("Photon.Menu.Resolution", value);
    }

    /// <summary>
    /// Select VSync.
    /// </summary>
    public virtual bool VSync {
      get => PlayerPrefs.GetInt("Photon.Menu.VSync", Math.Clamp(QualitySettings.vSyncCount, 0, 1)) == 1;
      set => PlayerPrefs.SetInt("Photon.Menu.VSync", value ? 1 : 0);
    }

    /// <summary>
    /// Select Unity quality level index based on QualitySettings.names.
    /// </summary>
    public virtual int QualityLevel {
      get {
        var q = PlayerPrefs.GetInt("Photon.Menu.QualityLevel", QualitySettings.GetQualityLevel());
        q = Math.Clamp(q, 0, QualitySettings.names.Length - 1);
        return q;
      }
      set => PlayerPrefs.SetInt("Photon.Menu.QualityLevel", value);
    }

    /// <summary>
    /// Return a list of possible framerates filtered by Screen.currentResolution.refreshRate.
    /// </summary>
    public virtual List<int> CreateFramerateOptions => PossibleFramerates.Where(f => f <= Mathf.RoundToInt((float)Screen.currentResolution.refreshRateRatio.value)).ToList();

    /// <summary>
    /// Returns a list of resolution option indices based on Screen.resolutions.
    /// </summary>
    public virtual List<int> CreateResolutionOptions => Enumerable.Range(0, Screen.resolutions.Length).ToList();

    /// <summary>
    /// Returns a list of graphics quality indices based on QualitySettings.names.
    /// </summary>
    public virtual List<int> CreateGraphicsQualityOptions => Enumerable.Range(0, QualitySettings.names.Length).ToList();

    /// <summary>
    /// A partial method to be implemented on the SDK level.
    /// </summary>
    partial void ApplyUser();

    /// <summary>
    /// Applies all graphics settings.
    /// </summary>
    public virtual void Apply() {
#if !UNITY_IOS && !UNITY_ANDROID
      var resolution = Screen.resolutions[Resolution < 0 ? Screen.resolutions.Length - 1 : Resolution];
      if (Screen.currentResolution.width != resolution.width ||
        Screen.currentResolution.height != resolution.height ||
        Screen.fullScreen != Fullscreen) {
        Screen.SetResolution(resolution.width, resolution.height, Fullscreen);
      }
#endif

      if (QualitySettings.GetQualityLevel() != QualityLevel) {
        QualitySettings.SetQualityLevel(QualityLevel);
      }

      if (QualitySettings.vSyncCount != (VSync ? 1 : 0)) {
        QualitySettings.vSyncCount = VSync ? 1 : 0;
      }

      if (Application.targetFrameRate != Framerate) {
        Application.targetFrameRate = Framerate;
      }

      ApplyUser();
    }

    /// <summary>
    /// Return the current selected resolution index based on Screen.resolutions.
    /// </summary>
    /// <returns>Index into Screen.resolutions</returns>
    private int GetCurrentResolutionIndex() {
      var resolutions = Screen.resolutions;
      if (resolutions == null || resolutions.Length == 0)
        return -1;

      int currentWidth = Mathf.RoundToInt(Screen.width);
      int currentHeight = Mathf.RoundToInt(Screen.height);
      int defaultRefreshRate = Mathf.RoundToInt((float)resolutions[^1].refreshRateRatio.value);

      for (int i = 0; i < resolutions.Length; i++) {
        var resolution = resolutions[i];

        if (resolution.width == currentWidth && resolution.height == currentHeight && Mathf.RoundToInt((float)resolution.refreshRateRatio.value) == defaultRefreshRate)
          return i;
      }

      return -1;
    }
  }
}

#endregion


#region PhotonMenuOnlineRegion.cs

namespace Fusion.Menu {
  using System;

  /// <summary>
  /// Includes Photon ping regions result used by the Party menu to pre select the best region and encode the region into the party code.
  /// </summary>
  [Serializable]
  public struct PhotonMenuOnlineRegion {
    /// <summary>
    /// Photon region code.
    /// </summary>
    public string Code;
    /// <summary>
    /// Last ping result.
    /// </summary>
    public int Ping;
  }
}

#endregion


#region PhotonMenuSettingsEntry.cs

namespace Fusion.Menu {
  using System;
  using System.Collections.Generic;
  using System.Linq;
  using TMPro;

  /// <summary>
  /// A helper class that maps a option name into the actual value.
  /// Is used to simplifiy dropdown UI code in <see cref="PhotonMenuUISettings"/>.
  /// </summary>
  /// <typeparam name="T">The option value type</typeparam>
  public class PhotonMenuSettingsEntry<T> where T : IEquatable<T> {
    private TMP_Dropdown _dropdown;
    private List<T> _options;

    /// <summary>
    /// Returns the value of this option.
    /// </summary>
    public T Value => _options == null || _options.Count == 0 ? default(T) : _options[_dropdown.value];

    /// <summary>
    /// Creates an option for this dropdown element.
    /// </summary>
    /// <param name="dropdown">Dropdown UI element</param>
    /// <param name="onValueChanged">Forward the value chaged callback</param>
    public PhotonMenuSettingsEntry(TMP_Dropdown dropdown, Action onValueChanged) {
      _dropdown = dropdown;
      _dropdown.onValueChanged.RemoveAllListeners();
      _dropdown.onValueChanged.AddListener(_ => onValueChanged.Invoke());
    }

    /// <summary>
    /// Clear all options and set new.
    /// </summary>
    /// <param name="options">List of options</param>
    /// <param name="current">The current selected option</param>
    /// <param name="ToString">A callback to format the option text</param>
    public void SetOptions(List<T> options, T current, Func<T, string> ToString = null) {
      _options = options;
      _dropdown.ClearOptions();
      _dropdown.AddOptions(options.Select(o => ToString != null ? ToString(o) : o.ToString()).ToList());

      var index = _options.FindIndex(0, o => o.Equals(current));
      if (index >= 0) {
        _dropdown.SetValueWithoutNotify(index);
      } else {
        _dropdown.SetValueWithoutNotify(0);
      }
    }
  }
}

#endregion


#region PhotonMenuUIController.cs

namespace Fusion.Menu {
  using System;
  using System.Collections.Generic;
  using System.Threading.Tasks;
  using UnityEngine;

  /// <summary>
  /// All screens are registed by this controller in the <see cref="_screens"/> list.
  /// Every screen gets a reference of this controller, the assigned <see cref="_config"/> and <see cref="_connection"/> wrapper.
  /// The first screen in the <see cref="_screens"/> list is the screen that is shown on app start.
  /// Controller is used to progress from one screen to another <see cref="Show{S}()"/>.
  /// E.g. Show&lt;PhotonMenuUILoading&gt;().
  /// When deriving a screen the base type will still be functionally to use for Get() and Show(). But only the derived type or the base are useable.
  /// </summary>
  /// <typeparam name="T"></typeparam>
  public class PhotonMenuUIController<T> : Fusion.Behaviour, IPhotonMenuUIController where T : IPhotonMenuConnectArgs, new() {
    /// <summary>
    /// The menu config.
    /// </summary>
    [InlineHelp, SerializeField] protected PhotonMenuConfig _config;
    /// <summary>
    /// The connection wrapper.
    /// </summary>
    [InlineHelp, SerializeField] protected PhotonMenuConnectionBehaviour _connection;
    /// <summary>
    /// The list of screens. The first one is the default screen shown on start.
    /// </summary>
    [InlineHelp, SerializeField] protected PhotonMenuUIScreen[] _screens;

    /// <summary>
    /// A type to screen lookup to support <see cref="Get{S}()"/>
    /// </summary>
    protected Dictionary<Type, PhotonMenuUIScreen> _screenLookup;
    /// <summary>
    /// The popup handler is automatically set if present based on the interface <see cref="IPhotonMenuPopup"/>.
    /// </summary>
    protected IPhotonMenuPopup _popupHandler;
    /// <summary>
    /// The current active screen.
    /// </summary>
    protected PhotonMenuUIScreen _activeScreen;

    /// <summary>
    /// A factory to create SDK dependend derived connection args.
    /// </summary>
    public virtual T CreateConnectArgs => new T();

    /// <summary>
    /// Unity awake method. Populates internal structures based on the <see cref="_screens"/> list.
    /// </summary>
    protected virtual void Awake() {
      var connectionArgs = CreateConnectArgs;
      _screenLookup = new Dictionary<Type, PhotonMenuUIScreen>();

      foreach (var screen in _screens) {
        screen.Config = _config;
        screen.Connection = _connection;
        screen.ConnectionArgs = connectionArgs;
        screen.Controller = this;

        var t = screen.GetType();
        while (true) {
          _screenLookup.Add(t, screen);
          if (t.BaseType == null || typeof(PhotonMenuUIScreen).IsAssignableFrom(t) == false || t.BaseType == typeof(PhotonMenuUIScreen)) {
            break;
          }

          t = t.BaseType;
        }

        if (typeof(IPhotonMenuPopup).IsAssignableFrom(t)) {
          _popupHandler = (IPhotonMenuPopup)screen;
        }
      }

      foreach (var screen in _screens) {
        screen.Init();
      }
    }

    /// <summary>
    /// The Unity start method to enable the default screen.
    /// </summary>
    protected virtual void Start() {
      if (_screens != null && _screens.Length > 0) {
        // First screen is displayed by default
        _screens[0].Show();
        _activeScreen = _screens[0];
      }
    }

    /// <summary>
    /// Show a sreen will automaticall disable the current active screen and call animations.
    /// </summary>
    /// <typeparam name="S">Screen type</typeparam>
    public virtual void Show<S>() where S : PhotonMenuUIScreen {
      if (_screenLookup.TryGetValue(typeof(S), out var result)) {
        if (result.IsModal == false && _activeScreen != this && _activeScreen) {
          _activeScreen.Hide();
        }
        result.Show();
        if (result.IsModal == false) {
          _activeScreen = result;
        }
      } else {
        Debug.LogError($"Show() - Screen type '{typeof(S).Name}' not found");
      }
    }

    /// <summary>
    /// Get a screen based on type.
    /// </summary>
    /// <typeparam name="S">Screen type</typeparam>
    /// <returns>Screen object</returns>
    public virtual S Get<S>() where S : PhotonMenuUIScreen {
      if (_screenLookup.TryGetValue(typeof(S), out var result)) {
        return result as S;
      } else {
        Debug.LogError($"Show() - Screen type '{typeof(S).Name}' not found");
        return null;
      }
    }

    /// <summary>
    /// Show the popup/notification.
    /// </summary>
    /// <param name="msg">Popup message</param>
    /// <param name="header">Popup header</param>
    public void Popup(string msg, string header = default) {
      if (_popupHandler == null) {
        Debug.LogError("Popup() - no popup handler found");
      } else {
        _popupHandler.OpenPopup(msg, header);
      }
    }

    /// <summary>
    /// Show the popup but wait until it hides.
    /// </summary>
    /// <param name="msg">Popup message</param>
    /// <param name="header">Popup header</param>
    /// <returns>When the user clicked okay.</returns>
    public Task PopupAsync(string msg, string header = default) {
      if (_popupHandler == null) {
        Debug.LogError("Popup() - no popup handler found");
        return Task.CompletedTask;
      } else {
        return _popupHandler.OpenPopupAsync(msg, header);
      }
    }
  }
}

#endregion


#region PhotonMenuUIScreen.cs

namespace Fusion.Menu {
  using System.Collections;
  using System.Collections.Generic;
  using UnityEngine;

  /// <summary>
  /// The screen base class contains a lot of accessors (e.g. Config, Connection, ConnectArgs) for convenient access.
  /// </summary>
  public abstract class PhotonMenuUIScreen : Fusion.Behaviour {
    /// <summary>
    /// Cached Hide animation hash.
    /// </summary>
    protected static readonly int HideAnimHash = Animator.StringToHash("Hide");
    /// <summary>
    /// Cached Show animation hash.
    /// </summary>
    protected static readonly int ShowAnimHash = Animator.StringToHash("Show");

    /// <summary>
    /// Is modal flag must be set for overlay screens.
    /// </summary>
    [InlineHelp, SerializeField] private bool _isModal;
    /// <summary>
    /// The list of screen plugins for the screen. The actual plugin scripts can be distributed insde the UI hierarchy but must be liked here.
    /// </summary>
    [InlineHelp, SerializeField] private List<PhotonMenuScreenPlugin> _plugins;
    /// <summary>
    /// The animator object.
    /// </summary>
    private Animator _animator;
    /// <summary>
    /// The hide animation coroutine.
    /// </summary>
    private Coroutine _hideCoroutine;

    /// <summary>
    /// The list of screen plugins.
    /// </summary>
    public List<PhotonMenuScreenPlugin> Plugins => _plugins;
    /// <summary>
    /// Is modal property.
    /// </summary>
    public bool IsModal => _isModal;
    /// <summary>
    /// Is the screen currently showing.
    /// </summary>
    public bool IsShowing { get; private set; }
    /// <summary>
    /// The menu config, assigned by the <see cref="IPhotonMenuUIController"/>.
    /// </summary>
    public IPhotonMenuConfig Config { get; set; }
    /// <summary>
    /// The menu connection object, The menu config, assigned by the <see cref="IPhotonMenuUIController"/>.
    /// </summary>
    public IPhotonMenuConnection Connection { get; set; }
    /// <summary>
    /// The menu connection args.
    /// </summary>
    public IPhotonMenuConnectArgs ConnectionArgs { get; set; }
    /// <summary>
    /// The menu UI controller that owns this screen.
    /// </summary>
    public IPhotonMenuUIController Controller { get; set; }

    /// <summary>
    /// Unity start method to find the animator.
    /// </summary>
    public virtual void Start() {
      TryGetComponent(out _animator);
    }

    /// <summary>
    /// Unit awake method to be overwritten by derived screens.
    /// </summary>
    public virtual void Awake() {
    }

    /// <summary>
    /// The screen init method is called during <see cref="PhotonMenuUIController{T}.Awake()"/> after all screen have been assigned and configured.
    /// </summary>
    public virtual void Init() {
    }

    /// <summary>
    /// The screen hide method.
    /// </summary>
    public virtual void Hide() {
      if (_animator) {
        if (_hideCoroutine != null) {
          StopCoroutine(_hideCoroutine);
        }

        _hideCoroutine = StartCoroutine(HideAnimCoroutine());
        return;
      }

      IsShowing = false;

      foreach (var p in _plugins) {
        p.Hide(this);
      }

      gameObject.SetActive(false);
    }

    /// <summary>
    /// The screen show method.
    /// </summary>
    public virtual void Show() {
      if (_hideCoroutine != null) {
        StopCoroutine(_hideCoroutine);
      }

      gameObject.SetActive(true);

      IsShowing = true;

      foreach (var p in _plugins) {
        p.Show(this);
      }
    }

    /// <summary>
    /// Play the hide animation wrapped in a coroutine.
    /// Forces the target framerate to 60 during the transition animations.
    /// </summary>
    /// <returns>When done</returns>
    private IEnumerator HideAnimCoroutine() {
#if UNITY_IOS || UNITY_ANDROID
      var changedFramerate = false;
      if (Config.AdaptFramerateForMobilePlatform) {
        if (Application.targetFrameRate < 60) {
          Application.targetFrameRate = 60;
          changedFramerate = true;
        }
      }
#endif

      _animator.Play(HideAnimHash);
      yield return null;
      while (_animator.GetCurrentAnimatorStateInfo(0).normalizedTime < 1) {
        yield return null;
      }

#if UNITY_IOS || UNITY_ANDROID
      if (changedFramerate) {
        new PhotonMenuGraphicsSettings().Apply();
      }
#endif

      gameObject.SetActive(false);
    }
  }
}

#endregion

