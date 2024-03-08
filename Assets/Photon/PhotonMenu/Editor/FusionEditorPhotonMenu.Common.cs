// merged ef65887

#region PhotonMenuUIScreenEditor.cs

namespace Fusion.Menu.Editor {
  using UnityEditor;

  /// <summary>
  /// Debug PhotonMenuUIScreen content.
  /// </summary>
  [CustomEditor(typeof(PhotonMenuUIScreen), true)]
  public class PhotonMenuUIScreenEditor : Editor {
    public override void OnInspectorGUI() {
      base.OnInspectorGUI();

      var data = (PhotonMenuUIScreen)target;

      if (data.ConnectionArgs != null) {
        EditorGUILayout.Space();
        EditorGUILayout.LabelField("Connect Args", EditorStyles.boldLabel);
        using (new EditorGUI.DisabledScope(true)) {
          EditorGUILayout.TextField("Username", data.ConnectionArgs.Username);
          EditorGUILayout.TextField("Session", data.ConnectionArgs.Session);
          EditorGUILayout.TextField("PreferredRegion", data.ConnectionArgs.PreferredRegion);
          EditorGUILayout.TextField("Region", data.ConnectionArgs.Region);
          EditorGUILayout.TextField("AppVersion", data.ConnectionArgs.AppVersion);
          EditorGUILayout.TextField("Scene", data.ConnectionArgs.Scene.ScenePath);
          EditorGUILayout.IntField("MaxPlayerCount", data.ConnectionArgs.MaxPlayerCount);
          EditorGUILayout.Toggle("Creating", data.ConnectionArgs.Creating);
        }
      }
    }
  }
}

#endregion

