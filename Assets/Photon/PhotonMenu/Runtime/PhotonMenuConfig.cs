namespace Fusion.Menu {
  using System.Collections.Generic;
  using UnityEngine;

  /// <summary>
  /// Photon menu config file implements <see cref="IPhotonMenuConfig"/>.
  /// Stores static options that affect parts of the menu behavior and selectable configurations.
  /// </summary>
  [ScriptHelp(BackColor = ScriptHeaderBackColor.Blue)]
  [CreateAssetMenu(menuName = "Photon/Menu/Menu Config")]
  public class PhotonMenuConfig : ScriptableObject, IPhotonMenuConfig {
    /// <summary>
    /// The maximum player count allowed for all game modes.
    /// </summary>
    [InlineHelp, SerializeField] protected int _maxPlayers = 6;
    /// <summary>
    /// Force 60 FPS during menu animations.
    /// </summary>
    [InlineHelp, SerializeField] protected bool _adaptFramerateForMobilePlatform = true;
    /// <summary>
    /// The available Photon AppVersions to be selecteable by the user.
    /// An empty list will hide the related dropdown on the settings screen.
    /// </summary>
    [InlineHelp, SerializeField] protected List<string> _availableAppVersions;
    /// <summary>
    /// Static list of regions available in the settings.
    /// An empty entry symbolizes best region option.
    /// An empty list will hide the related dropdown on the settings screen.
    /// </summary>
    [InlineHelp, SerializeField] protected List<string> _availableRegions;
    /// <summary>
    /// Static list of scenes available in the scenes menu.
    /// An empty list will hide the related button in the main screen.
    /// PhotonMeneSceneInfo.Name = displayed name
    /// PhotonMeneSceneInfo.ScenePath = the actual Unity scene (must be included in BuildSettings)
    /// PhotonMeneSceneInfo.Preview = a sprite with a preview of the scene (screenshot) that is displayed in the main menu and scene selection screen (can be null)
    /// </summary>
    [InlineHelp, SerializeField] protected List<PhotonMeneSceneInfo> _availableScenes;
    /// <summary>
    /// The <see cref="PhotonMenuMachineId"/> ScriptableObject that stores local ids to use as an option in for AppVersion.
    /// Designed as a convenient development feature.
    /// Can be null.
    /// </summary>
    [InlineHelp, SerializeField] protected PhotonMenuMachineId _machineId;
    /// <summary>
    /// The <see cref="PhotonMenuPartyCodeGenerator"/> ScriptableObject that is required for party code generation.
    /// Also used to create random player names.
    /// </summary>
    [InlineHelp, SerializeField] protected PhotonMenuPartyCodeGenerator _codeGenerator;

    public List<string> AvailableAppVersions => _availableAppVersions;
    public List<string> AvailableRegions => _availableRegions;
    public List<PhotonMeneSceneInfo> AvailableScenes => _availableScenes;
    public int MaxPlayerCount => _maxPlayers;
    public virtual string MachineId => _machineId?.Id;
    public PhotonMenuPartyCodeGenerator CodeGenerator => _codeGenerator;
    public bool AdaptFramerateForMobilePlatform => _adaptFramerateForMobilePlatform;
  }
}
