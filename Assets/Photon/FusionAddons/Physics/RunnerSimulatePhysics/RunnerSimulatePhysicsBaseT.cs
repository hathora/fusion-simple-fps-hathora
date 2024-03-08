using System;
using System.Collections.Generic;
using UnityEngine;

namespace Fusion.Addons.Physics {

  /// <summary>
  /// This base class exists to allow for additional Physics Scenes to be accounted for in Physics simulation,
  /// in addition to the Physics Scenes associated with the NetworkRunner.
  /// </summary>
  /// <typeparam name="TPhysicsScene"></typeparam>
  public abstract class RunnerSimulatePhysicsBase<TPhysicsScene> : RunnerSimulatePhysicsBase where TPhysicsScene : struct, IEquatable <TPhysicsScene> {
    
    /// <summary>
    /// Wrapper for physics scene reference.
    /// </summary>
    protected struct AdditionalScene {
      public TPhysicsScene PhysicsScene;
      public bool ForwardOnly;
    }
    
    /// <summary>
    /// List of additional physics scenes that should be simulated by Fusion.
    /// </summary>
    protected List<AdditionalScene> _additionalScenes;
    
    /// <summary>
    /// Register a Physics Scene to be simulated by Fusion.
    /// </summary>
    /// <param name="scene">The Physics Scene to include in simulation.</param>
    /// <param name="forwardOnly">Indicate if this additional scene should not resimulate.
    /// Typically this will be Forward, if you want to simulate physics locally for non-networked objects (such as rag dolls)</param>
    public void RegisterAdditionalScene(TPhysicsScene scene, bool forwardOnly = false) {
      if (_additionalScenes == null) {
        _additionalScenes = new List<AdditionalScene>();
      } else {
        foreach (var entry in _additionalScenes) {
          if (entry.PhysicsScene.Equals(scene)) {
            Debug.LogWarning("Scene already registered.");
            return;
          }
        }
      }
      _additionalScenes.Add(new AdditionalScene(){PhysicsScene = scene, ForwardOnly = forwardOnly});
    }

    /// <summary>
    /// Unregister a Physics Scene, and it will not longer have calls made to Simulate() by this component.
    /// </summary>
    /// <param name="scene"></param>
    public void UnregisterAdditionalScene(TPhysicsScene scene) {
      if (_additionalScenes == null) {
        Debug.LogWarning("Scene was never registered, cannot unregister.");
        return;
      }

      int? found = null;
      for (int i = 0; i < _additionalScenes.Count; i++) {
        if (_additionalScenes[i].PhysicsScene.Equals(scene)) {
          found = i;
          break;
        }
      }

      if (found.HasValue) {
        _additionalScenes.RemoveAt(found.Value);
      }
    }
  }
}
