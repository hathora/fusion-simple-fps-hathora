using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.SceneManagement;
using Hathora.Core.Scripts.Runtime.Common.Utils;
using Hathora.Core.Scripts.Runtime.Server;
using Hathora.Core.Scripts.Runtime.Server.Models;
using HathoraCloud.Models.Shared;
using Fusion.Photon.Realtime;
using Fusion.Sockets;

namespace Fusion.Addons.Hathora
{
	public class HathoraServer : HathoraServerMgr
	{
		[Header(nameof(Fusion))]
		[SerializeField]
		private bool _forceMultiPeerMode;
		[SerializeField]
		private bool _enableLogs;

		private NetworkRunner    _runnerPrefab;
		private List<ServerPeer> _serverPeers = new List<ServerPeer>();

		public async Task<bool> Initialize(NetworkRunner runnerPrefab)
		{
			if (_forceMultiPeerMode == true)
			{
				NetworkProjectConfig.Global.PeerMode = NetworkProjectConfig.PeerModes.Multiple;
			}

			_runnerPrefab = runnerPrefab;

			LogInfo($"Initializing Hathora server.");

			HathoraServerContext serverContext = await GetCachedServerContextAsync();
			if (serverContext == null)
			{
				LogError($"Server initialization failed! Missing {nameof(HathoraServerContext)}.");
				return false;
			}

			RefreshServers();
			return true;
		}

		private async void RefreshServers()
		{
			NetworkRunner runnerPrefab = _runnerPrefab;

			// Run update loop until the runner prefab changes.
			while (runnerPrefab == _runnerPrefab)
			{
				// 1. Get the server context.
				HathoraServerContext serverContext = await GetCachedServerContextAsync();
				if (serverContext == null)
				{
					await Task.Delay(1000);
					continue;
				}

				// 2. Deinitialize all Fusion server instances that are still running, but their Hathora rooms has been destroyed.
				for (int i = _serverPeers.Count - 1; i >= 0; --i)
				{
					ServerPeer serverPeer = _serverPeers[i];
					if (FindActiveRoom(serverContext, serverPeer.RoomId) == default)
					{
						LogWarning($"Active room has been destroyed. RoomId: {serverPeer.RoomId}");
						await Deinitialize(serverPeer);
						await Task.Delay(1000);
					}
				}

				// 3. Initialize new Fusion server instance for first new active Hathora room.
				List<RoomWithoutAllocations> activeRooms = serverContext.ActiveRoomsForProcess;
				if (activeRooms != null)
				{
					for (int i = 0; i < activeRooms.Count; ++i)
					{
						RoomWithoutAllocations activeRoom = activeRooms[i];
						if (activeRoom != null && FindServerPeer(activeRoom.RoomId) == default)
						{
							LogWarning($"New room has been created. RoomId: {activeRoom.RoomId}");
							await Initialize(runnerPrefab, serverContext, activeRoom);
							await Task.Delay(1000);
							break;
						}
					}
				}

				// 4. Repeat checks every second.
				await Task.Delay(1000);
			}
		}

		private async Task<bool> Initialize(NetworkRunner runnerPrefab, HathoraServerContext serverContext, RoomWithoutAllocations room)
		{
			LogInfo($"Initializing server peer. RoomId: {room.RoomId}");

			if (FindUnusedPort(serverContext, out ProcessV3ExposedPort exposedPort) == false)
			{
				LogInfo($"Failed to initialize server peer, all ports are used. RoomId: {room.RoomId}");
				return false;
			}

			// 1. Get ports information and initialize the server peer.
			ushort publicPort    = (ushort)exposedPort.Port;
			ushort containerPort = (ushort)HathoraServerConfig.HathoraDeployOpts.ContainerPortSerializable.Port;
			string portName      = exposedPort.Name;

			string additionalContainer = "default-";
			if (portName.Contains(additionalContainer) == true)
			{
				if (ushort.TryParse(portName.Substring(additionalContainer.Length), out ushort containerPortOffset) == true)
				{
					containerPort += containerPortOffset;
				}
			}

			ServerPeer serverPeer = new ServerPeer(room.RoomId, publicPort, containerPort, portName);
			_serverPeers.Add(serverPeer);

			// 2. Get network addresses.
			IPAddress  ip            = await HathoraUtils.ConvertHostToIpAddress(exposedPort.Host);
			NetAddress address       = NetAddress.Any(containerPort);
			NetAddress publicAddress = NetAddress.CreateFromIpPort(ip.ToString(), publicPort);

			// 3. Set fixed Photon region based on Hathora server region.
			FusionAppSettings photonAppSettings = PhotonAppSettings.Global.AppSettings.GetCopy();
			photonAppSettings.FixedRegion = HathoraRegionUtility.HathoraToPhoton(serverContext.EnvVarRegion);

			LogInfo($"Starting server runner. RoomId: {room.RoomId}, IP: {ip}, Public Port: {serverPeer.PublicPort}, Container Port: {containerPort}, Port Name: {serverPeer.PortName}, Hathora Region: {serverContext.EnvVarRegion}, Photon Region: {photonAppSettings.FixedRegion}");

			// 4. Configure scene info => set Deathmatch gameplay scene.
			const int deathmatchSceneIndex = 1;
			NetworkSceneInfo sceneInfo = new NetworkSceneInfo();
			sceneInfo.AddSceneRef(SceneRef.FromIndex(deathmatchSceneIndex), LoadSceneMode.Additive, LocalPhysicsMode.None, true);

			// 5. Configure start game arguments.
			StartGameArgs startGameArgs = new StartGameArgs();
			startGameArgs.SessionName             = room.RoomId;
			startGameArgs.GameMode                = GameMode.Server;
			startGameArgs.Scene                   = sceneInfo;
			startGameArgs.IsVisible               = true;
			startGameArgs.IsOpen                  = true;
			startGameArgs.Address                 = address;
			startGameArgs.CustomPublicAddress     = publicAddress;
			startGameArgs.CustomPhotonAppSettings = photonAppSettings;

			// 6. Start Fusion NetworkRunner.
			serverPeer.Runner = Instantiate(runnerPrefab);
			serverPeer.Runner.name += $"_{room.RoomId}";

			StartGameResult startGameResult = await serverPeer.Runner.StartGame(startGameArgs);
			if (startGameResult.Ok == true)
			{
				UnityEngine.Application.targetFrameRate = TickRate.Resolve(serverPeer.Runner.Config.Simulation.TickRateSelection).Server;

				LogInfo($"Server runner started! Session: {startGameArgs.SessionName}, IP: {ip}, Public Port: {serverPeer.PublicPort}, Container Port: {serverPeer.ContainerPort}, Port Name: {serverPeer.PortName}, Hathora Region: {serverContext.EnvVarRegion}, Photon Region: {photonAppSettings.FixedRegion}");
				return true;
			}
			else
			{
				LogError($"Server runner start failed! Session: {startGameArgs.SessionName}, IP: {ip}, Public Port: {serverPeer.PublicPort}, Container Port: {serverPeer.ContainerPort}, Port Name: {serverPeer.PortName}, Hathora Region: {serverContext.EnvVarRegion}, Photon Region: {photonAppSettings.FixedRegion}, Result: {startGameResult}");
				await Deinitialize(serverPeer);
				return false;
			}
		}

		private async Task Deinitialize(ServerPeer serverPeer)
		{
			LogInfo($"Denitializing server peer. RoomId: {serverPeer.RoomId}, Public Port: {serverPeer.PublicPort}, Container Port: {serverPeer.ContainerPort}, Port Name: {serverPeer.PortName}.");

			_serverPeers.Remove(serverPeer);

			if (serverPeer.Runner != null)
			{
				await serverPeer.Runner.Shutdown();
				serverPeer.Runner = null;
			}
		}

		private RoomWithoutAllocations FindActiveRoom(HathoraServerContext serverContext, string roomId)
		{
			List<RoomWithoutAllocations> activeRooms = serverContext.ActiveRoomsForProcess;
			if (activeRooms == null)
				return default;

			for (int i = 0, count = activeRooms.Count; i < count; ++i)
			{
				RoomWithoutAllocations activeRoom = activeRooms[i];
				if (activeRoom != null && activeRoom.RoomId == roomId)
					return activeRoom;
			}

			return default;
		}

		private ServerPeer FindServerPeer(string roomId)
		{
			for (int i = 0, count = _serverPeers.Count; i < count; ++i)
			{
				ServerPeer serverPeer = _serverPeers[i];
				if (serverPeer.RoomId == roomId)
					return serverPeer;
			}

			return default;
		}

		private ServerPeer FindServerPeer(ProcessV3ExposedPort port)
		{
			for (int i = 0, count = _serverPeers.Count; i < count; ++i)
			{
				ServerPeer serverPeer = _serverPeers[i];
				if (serverPeer.PortName == port.Name)
					return serverPeer;
			}

			return default;
		}
		private ServerPeer FindServerPeer(ExposedPort port)
		{
			for (int i = 0, count = _serverPeers.Count; i < count; ++i)
			{
				ServerPeer serverPeer = _serverPeers[i];
				if (serverPeer.PortName == port.Name)
					return serverPeer;
			}

			return default;
		}

		private bool FindUnusedPort(HathoraServerContext serverContext, out ProcessV3ExposedPort port)
		{
			port = default;

			if (serverContext == null || serverContext.ProcessInfo == null)
				return false;

			if (FindServerPeer(serverContext.ProcessInfo.ExposedPort) == null)
			{
				port = serverContext.ProcessInfo.ExposedPort;
				return true;
			}

			foreach (ExposedPort additionalPort in serverContext.ProcessInfo.AdditionalExposedPorts)
			{
				if (FindServerPeer(additionalPort) == null)
				{
					port = ExposedPortToProcessV3ExposedPort(additionalPort);
					return true;
				}
			}

			return false;
		}

		private ProcessV3ExposedPort ExposedPortToProcessV3ExposedPort(ExposedPort port)
		{
			ProcessV3ExposedPort newPort = new ProcessV3ExposedPort();
			newPort.Host = port.Host;
			newPort.Port = port.Port;
			newPort.Name = port.Name;
			newPort.TransportType = port.TransportType;
			return newPort;
		}

		private void LogInfo   (object message) { if (_enableLogs == true) Debug.Log       ($"[{nameof(HathoraServer)}][{Time.realtimeSinceStartup:F3}] {message}", gameObject); }
		private void LogWarning(object message) { if (_enableLogs == true) Debug.LogWarning($"[{nameof(HathoraServer)}][{Time.realtimeSinceStartup:F3}] {message}", gameObject); }
		private void LogError  (object message) { if (_enableLogs == true) Debug.LogError  ($"[{nameof(HathoraServer)}][{Time.realtimeSinceStartup:F3}] {message}", gameObject); }

		private sealed class ServerPeer
		{
			public readonly string RoomId;
			public readonly ushort PublicPort;
			public readonly ushort ContainerPort;
			public readonly string PortName;

			public NetworkRunner Runner;

			public ServerPeer(string roomId, ushort publicPort, ushort containerPort, string portName)
			{
				RoomId        = roomId;
				PublicPort    = publicPort;
				ContainerPort = containerPort;
				PortName      = portName;
			}
		}
	}
}
