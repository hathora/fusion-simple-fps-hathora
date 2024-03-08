namespace Fusion.Menu {
  using UnityEngine;

  /// <summary>
  /// A scriptable object that has an id used by the PhotonMenu as appversion.
  /// Mostly a developement feature to ensure to only meet compatible clients in the Photon matchmaking.
  /// </summary>
  //[CreateAssetMenu(menuName = "Photon/Menu/MachineId")]
  [ScriptHelp(BackColor = ScriptHeaderBackColor.Blue)]
  public class PhotonMenuMachineId : ScriptableObject {
    /// <summary>
    /// An id that should be unique to this machine, used by the PhotonMenu as AppVersion.
    /// An explicit asset importer is used to create local ids during import (see PhotonMenuMachineIdImporter).
    /// </summary>
    [InlineHelp] public string Id;
  }
}
