
using UnityEngine;

namespace Fusion.Addons.Physics
{
  /// <summary>
  /// Options for whether Unity will auto-simulate or Fusion will call Physics.Simulate().
  /// Auto will make Fusion the simulation authority in all cases except Single-Peer Shared Mode.
  /// </summary>
  public enum PhysicsAuthorities {
    /// <summary>
    /// Automatically determine if Unity or Physics should be calling Physics.Simulate.
    ///  Will make Fusion the simulation authority in all cases except Single-Peer Shared Mode.
    /// </summary>
    Auto,
    /// <summary>
    /// Physics will always be auto-simulated by Unity Physics.
    /// </summary>
    Unity,
    /// <summary>
    /// Physics.Simulate() will be called by a <see cref="RunnerSimulatePhysicsBase"/> derived component on the Runner.
    /// </summary>
    Fusion,
  }
  
  /// <summary>
  /// Timing segment options for when Physics.Simulate() occurs.
  /// These enum values align with Unity's SimulationMode and SimulationMode2D enums, and have FixedUpdateNetwork added.
  /// </summary>
  public enum PhysicsTimings {
    /// <summary>
    /// Calls to Physics.Simulate() are automatically called every Unity FixedUpdate()
    /// </summary>
    FixedUpdate = SimulationMode2D.FixedUpdate, 
    /// <summary>
    /// Calls to Physics.Simulate() are automatically called every Update()
    /// </summary>
    Update = SimulationMode2D.Update,  
    /// <summary>
    /// Calls to Physics.Simulate() are handled by user code
    /// </summary>
    Script = SimulationMode2D.Script, 
    /// <summary>
    /// Calls to Physics.Simulate() are automatically called every Unity FixedUpdateNetwork()
    /// </summary>
    FixedUpdateNetwork,
  }
}
