
<div align="center">
    <img src="https://user-images.githubusercontent.com/6267663/235110661-00e586cc-7489-4daf-82e8-7ae3c2c7143e.svg" width="350px">
    <p>Serverless cloud hosting for multiplayer games</p>
    <h1>Unity SDK</h1>
    <a href="https://hathora.dev/docs"><img src="https://img.shields.io/static/v1?label=Docs&message=API Ref&color=000&style=for-the-badge" /></a>
    <a href="https://speakeasyapi.dev/"><img src="https://custom-icon-badges.demolab.com/badge/-Built%20By%20Speakeasy-212015?style=for-the-badge&logoColor=FBE331&logo=speakeasy&labelColor=545454" /></a>
    <a href="https://github.com/hathora/cloud-sdk-unity.git/actions"><img src="https://img.shields.io/github/actions/workflow/status/speakeasy-sdks/bolt-php/speakeasy_sdk_generation.yml?style=for-the-badge" /></a>
    
</div>

<!-- Start SDK Installation -->
## SDK Installation

The SDK can either be compiled using `dotnet build` and the resultant `.dll` file can be copied into your Unity project's `Assets` folder, or you can copy the source code directly into your project.

The SDK relies on Newtonsoft's JSON.NET Package which can be installed via the Unity Package Manager.

To do so open the Package Manager via `Window > Package Manager` and click the `+` button then `Add package from git URL...` and enter `com.unity.nuget.newtonsoft-json` and click `Add`.
<!-- End SDK Installation -->

## SDK Example Usage
<!-- Start SDK Example Usage -->
### Example

```csharp
using HathoraCloud;
using HathoraCloud.Models.Shared;

var sdk = new HathoraCloudSDK(
    security: new Security() {
        HathoraDevToken = "",
    },
    appId: "app-af469a92-5b45-4565-b3c4-b79878de67d2"
);

using(var res = await sdk.AppV1.CreateAppAsync(new AppConfig() {
    AppName = "minecraft",
    AuthConfiguration = new AuthConfiguration() {
        Anonymous = new RecordStringNever() {},
        Google = new Google() {
            ClientId = "string",
        },
        Nickname = new RecordStringNever() {},
    },
}))
{
    // handle response
}
```
<!-- End SDK Example Usage -->

<!-- Start SDK Available Operations -->
## Available Resources and Operations


### [AppV1](docs/sdks/appv1/README.md)

* [CreateApp](docs/sdks/appv1/README.md#createapp) - Create a new [application](https://hathora.dev/docs/concepts/hathora-entities#application).
* [DeleteApp](docs/sdks/appv1/README.md#deleteapp) - Delete an [application](https://hathora.dev/docs/concepts/hathora-entities#application) using `appId`. Your organization will lose access to this application.
* [GetAppInfo](docs/sdks/appv1/README.md#getappinfo) - Get details for an [application](https://hathora.dev/docs/concepts/hathora-entities#application) using `appId`.
* [GetApps](docs/sdks/appv1/README.md#getapps) - Returns an unsorted list of your organization’s [applications](https://hathora.dev/docs/concepts/hathora-entities#application). An application is uniquely identified by an `appId`.
* [UpdateApp](docs/sdks/appv1/README.md#updateapp) - Update data for an existing [application](https://hathora.dev/docs/concepts/hathora-entities#application) using `appId`.

### [AuthV1](docs/sdks/authv1/README.md)

* [LoginAnonymous](docs/sdks/authv1/README.md#loginanonymous) - Returns a unique player token for an anonymous user.
* [LoginGoogle](docs/sdks/authv1/README.md#logingoogle) - Returns a unique player token using a Google-signed OIDC `idToken`.
* [LoginNickname](docs/sdks/authv1/README.md#loginnickname) - Returns a unique player token with a specified nickname for a user.

### [BillingV1](docs/sdks/billingv1/README.md)

* [GetBalance](docs/sdks/billingv1/README.md#getbalance)
* [GetInvoices](docs/sdks/billingv1/README.md#getinvoices)
* [GetPaymentMethod](docs/sdks/billingv1/README.md#getpaymentmethod)
* [InitStripeCustomerPortalUrl](docs/sdks/billingv1/README.md#initstripecustomerportalurl)

### [BuildV1](docs/sdks/buildv1/README.md)

* [CreateBuild](docs/sdks/buildv1/README.md#createbuild) - Creates a new [build](https://hathora.dev/docs/concepts/hathora-entities#build). Responds with a `buildId` that you must pass to [`RunBuild()`](https://hathora.dev/api#tag/BuildV1/operation/RunBuild) to build the game server artifact. You can optionally pass in a `buildTag` to associate an external version with a build.
* [DeleteBuild](docs/sdks/buildv1/README.md#deletebuild) - Delete a [build](https://hathora.dev/docs/concepts/hathora-entities#build). All associated metadata is deleted.
* [GetBuildInfo](docs/sdks/buildv1/README.md#getbuildinfo) - Get details for a [build](https://hathora.dev/docs/concepts/hathora-entities#build).
* [GetBuilds](docs/sdks/buildv1/README.md#getbuilds) - Returns an array of [builds](https://hathora.dev/docs/concepts/hathora-entities#build) for an [application](https://hathora.dev/docs/concepts/hathora-entities#application).
* [RunBuild](docs/sdks/buildv1/README.md#runbuild) - Builds a game server artifact from a tarball you provide. Pass in the `buildId` generated from [`CreateBuild()`](https://hathora.dev/api#tag/BuildV1/operation/CreateBuild).

### [DeploymentV1](docs/sdks/deploymentv1/README.md)

* [CreateDeployment](docs/sdks/deploymentv1/README.md#createdeployment) - Create a new [deployment](https://hathora.dev/docs/concepts/hathora-entities#deployment). Creating a new deployment means all new rooms created will use the latest deployment configuration, but existing games in progress will not be affected.
* [GetDeploymentInfo](docs/sdks/deploymentv1/README.md#getdeploymentinfo) - Get details for a [deployment](https://hathora.dev/docs/concepts/hathora-entities#deployment).
* [GetDeployments](docs/sdks/deploymentv1/README.md#getdeployments) - Returns an array of [deployments](https://hathora.dev/docs/concepts/hathora-entities#deployment) for an [application](https://hathora.dev/docs/concepts/hathora-entities#application).

### [DiscoveryV1](docs/sdks/discoveryv1/README.md)

* [GetPingServiceEndpoints](docs/sdks/discoveryv1/README.md#getpingserviceendpoints) - Returns an array of all regions with a host and port that a client can directly ping. Open a websocket connection to `wss://<host>:<port>/ws` and send a packet. To calculate ping, measure the time it takes to get an echo packet back.

### [LobbyV1](docs/sdks/lobbyv1/README.md)

* [~~CreatePrivateLobbyDeprecated~~](docs/sdks/lobbyv1/README.md#createprivatelobbydeprecated) - :warning: **Deprecated**
* [~~CreatePublicLobbyDeprecated~~](docs/sdks/lobbyv1/README.md#createpubliclobbydeprecated) - :warning: **Deprecated**
* [~~ListActivePublicLobbiesDeprecatedV1~~](docs/sdks/lobbyv1/README.md#listactivepubliclobbiesdeprecatedv1) - :warning: **Deprecated**

### [LobbyV2](docs/sdks/lobbyv2/README.md)

* [~~CreateLobbyDeprecated~~](docs/sdks/lobbyv2/README.md#createlobbydeprecated) - Create a new lobby for an [application](https://hathora.dev/docs/concepts/hathora-entities#application). A lobby object is a wrapper around a [room](https://hathora.dev/docs/concepts/hathora-entities#room) object. With a lobby, you get additional functionality like configuring the visibility of the room, managing the state of a match, and retrieving a list of public lobbies to display to players. :warning: **Deprecated**
* [~~CreateLocalLobby~~](docs/sdks/lobbyv2/README.md#createlocallobby) - :warning: **Deprecated**
* [~~CreatePrivateLobby~~](docs/sdks/lobbyv2/README.md#createprivatelobby) - :warning: **Deprecated**
* [~~CreatePublicLobby~~](docs/sdks/lobbyv2/README.md#createpubliclobby) - :warning: **Deprecated**
* [~~GetLobbyInfo~~](docs/sdks/lobbyv2/README.md#getlobbyinfo) - Get details for a lobby. :warning: **Deprecated**
* [~~ListActivePublicLobbiesDeprecatedV2~~](docs/sdks/lobbyv2/README.md#listactivepubliclobbiesdeprecatedv2) - Get all active lobbies for a an [application](https://hathora.dev/docs/concepts/hathora-entities#application). Filter by optionally passing in a `region`. Use this endpoint to display all public lobbies that a player can join in the game client. :warning: **Deprecated**
* [~~SetLobbyState~~](docs/sdks/lobbyv2/README.md#setlobbystate) - Set the state of a lobby. State is intended to be set by the server and must be smaller than 1MB. Use this endpoint to store match data like live player count to enforce max number of clients or persist end-game data (i.e. winner or final scores). :warning: **Deprecated**

### [LobbyV3](docs/sdks/lobbyv3/README.md)

* [CreateLobby](docs/sdks/lobbyv3/README.md#createlobby) - Create a new lobby for an [application](https://hathora.dev/docs/concepts/hathora-entities#application). A lobby object is a wrapper around a [room](https://hathora.dev/docs/concepts/hathora-entities#room) object. With a lobby, you get additional functionality like configuring the visibility of the room, managing the state of a match, and retrieving a list of public lobbies to display to players.
* [GetLobbyInfoByRoomId](docs/sdks/lobbyv3/README.md#getlobbyinfobyroomid) - Get details for a lobby.
* [GetLobbyInfoByShortCode](docs/sdks/lobbyv3/README.md#getlobbyinfobyshortcode) - Get details for a lobby. If 2 or more lobbies have the same `shortCode`, then the most recently created lobby will be returned.
* [ListActivePublicLobbies](docs/sdks/lobbyv3/README.md#listactivepubliclobbies) - Get all active lobbies for a given [application](https://hathora.dev/docs/concepts/hathora-entities#application). Filter the array by optionally passing in a `region`. Use this endpoint to display all public lobbies that a player can join in the game client.

### [LogV1](docs/sdks/logv1/README.md)

* [~~GetLogsForApp~~](docs/sdks/logv1/README.md#getlogsforapp) - Returns a stream of logs for an [application](https://hathora.dev/docs/concepts/hathora-entities#application) using `appId`. :warning: **Deprecated**
* [~~GetLogsForDeployment~~](docs/sdks/logv1/README.md#getlogsfordeployment) - Returns a stream of logs for a [deployment](https://hathora.dev/docs/concepts/hathora-entities#deployment) using `appId` and `deploymentId`. :warning: **Deprecated**
* [GetLogsForProcess](docs/sdks/logv1/README.md#getlogsforprocess) - Returns a stream of logs for a [process](https://hathora.dev/docs/concepts/hathora-entities#process) using `appId` and `processId`.

### [ManagementV1](docs/sdks/managementv1/README.md)

* [SendVerificationEmail](docs/sdks/managementv1/README.md#sendverificationemail)

### [MetricsV1](docs/sdks/metricsv1/README.md)

* [GetMetrics](docs/sdks/metricsv1/README.md#getmetrics) - Get metrics for a [process](https://hathora.dev/docs/concepts/hathora-entities#process) using `appId` and `processId`.

### [ProcessesV1](docs/sdks/processesv1/README.md)

* [GetProcessInfo](docs/sdks/processesv1/README.md#getprocessinfo) - Get details for a [process](https://hathora.dev/docs/concepts/hathora-entities#process).
* [GetRunningProcesses](docs/sdks/processesv1/README.md#getrunningprocesses) - Retrieve 10 most recently started [process](https://hathora.dev/docs/concepts/hathora-entities#process) objects for an [application](https://hathora.dev/docs/concepts/hathora-entities#application). Filter the array by optionally passing in a `region`.
* [GetStoppedProcesses](docs/sdks/processesv1/README.md#getstoppedprocesses) - Retrieve 10 most recently stopped [process](https://hathora.dev/docs/concepts/hathora-entities#process) objects for an [application](https://hathora.dev/docs/concepts/hathora-entities#application). Filter the array by optionally passing in a `region`.

### [RoomV1](docs/sdks/roomv1/README.md)

* [~~CreateRoomDeprecated~~](docs/sdks/roomv1/README.md#createroomdeprecated) - :warning: **Deprecated**
* [~~DestroyRoomDeprecated~~](docs/sdks/roomv1/README.md#destroyroomdeprecated) - :warning: **Deprecated**
* [~~GetActiveRoomsForProcessDeprecated~~](docs/sdks/roomv1/README.md#getactiveroomsforprocessdeprecated) - :warning: **Deprecated**
* [~~GetConnectionInfoDeprecated~~](docs/sdks/roomv1/README.md#getconnectioninfodeprecated) - :warning: **Deprecated**
* [~~GetInactiveRoomsForProcessDeprecated~~](docs/sdks/roomv1/README.md#getinactiveroomsforprocessdeprecated) - :warning: **Deprecated**
* [~~GetRoomInfoDeprecated~~](docs/sdks/roomv1/README.md#getroominfodeprecated) - :warning: **Deprecated**
* [~~SuspendRoomDeprecated~~](docs/sdks/roomv1/README.md#suspendroomdeprecated) - :warning: **Deprecated**

### [RoomV2](docs/sdks/roomv2/README.md)

* [CreateRoom](docs/sdks/roomv2/README.md#createroom) - Create a new [room](https://hathora.dev/docs/concepts/hathora-entities#room) for an existing [application](https://hathora.dev/docs/concepts/hathora-entities#application). Poll the [`GetConnectionInfo()`](https://hathora.dev/api#tag/RoomV2/operation/GetConnectionInfo) endpoint to get connection details for an active room.
* [DestroyRoom](docs/sdks/roomv2/README.md#destroyroom) - Destroy a [room](https://hathora.dev/docs/concepts/hathora-entities#room). All associated metadata is deleted.
* [GetActiveRoomsForProcess](docs/sdks/roomv2/README.md#getactiveroomsforprocess) - Get all active [rooms](https://hathora.dev/docs/concepts/hathora-entities#room) for a given [process](https://hathora.dev/docs/concepts/hathora-entities#process).
* [GetConnectionInfo](docs/sdks/roomv2/README.md#getconnectioninfo) - Poll this endpoint to get connection details to a [room](https://hathora.dev/docs/concepts/hathora-entities#room). Clients can call this endpoint without authentication.
* [GetInactiveRoomsForProcess](docs/sdks/roomv2/README.md#getinactiveroomsforprocess) - Get all inactive [rooms](https://hathora.dev/docs/concepts/hathora-entities#room) for a given [process](https://hathora.dev/docs/concepts/hathora-entities#process).
* [GetRoomInfo](docs/sdks/roomv2/README.md#getroominfo) - Retreive current and historical allocation data for a [room](https://hathora.dev/docs/concepts/hathora-entities#room).
* [SuspendRoom](docs/sdks/roomv2/README.md#suspendroom) - Suspend a [room](https://hathora.dev/docs/concepts/hathora-entities#room). The room is unallocated from the process but can be rescheduled later using the same `roomId`.
* [UpdateRoomConfig](docs/sdks/roomv2/README.md#updateroomconfig)
<!-- End SDK Available Operations -->



<!-- Start Dev Containers -->

<!-- End Dev Containers -->



<!-- Start Global Parameters -->
## Global Parameters

A parameter is configured globally. This parameter may be set on the SDK client instance itself during initialization. When configured as an option during SDK initialization, This global value will be used as the default on the operations that use it. When such operations are called, there is a place in each to override the global value, if needed.

For example, you can set `appId` to `"app-af469a92-5b45-4565-b3c4-b79878de67d2"` at SDK initialization and then you do not have to pass the same value on calls to operations like `DeleteApp`. But if you want to do so you may, which will locally override the global setting. See the example code below for a demonstration.


### Available Globals

The following global parameter is available.

| Name | Type | Required | Description |
| ---- | ---- |:--------:| ----------- |
| appId | string |  | The appId parameter. |


### Example

```csharp
using HathoraCloud;
using HathoraCloud.Models.Shared;
using HathoraCloud.Models.Operations;

var sdk = new HathoraCloudSDK(
    security: new Security() {
        HathoraDevToken = "",
    },
    appId: "app-af469a92-5b45-4565-b3c4-b79878de67d2"
);

using(var res = await sdk.AppV1.DeleteAppAsync(new DeleteAppRequest() {}))
{
    // handle response
}
```
<!-- End Global Parameters -->

<!-- Placeholder for Future Speakeasy SDK Sections -->



### Maturity

This SDK is in beta, and there may be breaking changes between versions without a major version update. Therefore, we recommend pinning usage
to a specific package version. This way, you can install the same version each time without breaking changes unless you are intentionally
looking for the latest version.

### Contributions

While we value open-source contributions to this SDK, this library is generated programmatically.
Feel free to open a PR or a Github issue as a proof of concept and we'll do our best to include it in a future release!

### SDK Created by [Speakeasy](https://docs.speakeasyapi.dev/docs/using-speakeasy/client-sdks)
