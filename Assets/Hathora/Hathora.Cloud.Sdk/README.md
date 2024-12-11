
<div align="center">
    <img src="https://user-images.githubusercontent.com/6267663/235110661-00e586cc-7489-4daf-82e8-7ae3c2c7143e.svg" width="350px">
    <p>Serverless cloud hosting for multiplayer games</p>
    <h1>Unity SDK</h1>
    <a href="https://hathora.dev/docs"><img src="https://img.shields.io/static/v1?label=Docs&message=API Ref&color=000&style=for-the-badge" /></a>
    <a href="https://speakeasyapi.dev/"><img src="https://custom-icon-badges.demolab.com/badge/-Built%20By%20Speakeasy-212015?style=for-the-badge&logoColor=FBE331&logo=speakeasy&labelColor=545454" /></a>    
</div>

<!-- Start Summary [summary] -->
## Summary

Hathora Cloud API: Welcome to the Hathora Cloud API documentation! Learn how to use the Hathora Cloud APIs to build and scale your game servers globally.
<!-- End Summary [summary] -->

<!-- Start Table of Contents [toc] -->
## Table of Contents

* [SDK Installation](#sdk-installation)
* [SDK Example Usage](#sdk-example-usage)
* [Available Resources and Operations](#available-resources-and-operations)
* [Global Parameters](#global-parameters)
* [Error Handling](#error-handling)
* [Server Selection](#server-selection)
* [Authentication](#authentication)
<!-- End Table of Contents [toc] -->

<!-- Start SDK Installation [installation] -->
## SDK Installation

The SDK can either be compiled using `dotnet build` and the resultant `.dll` file can be copied into your Unity project's `Assets` folder, or you can copy the source code directly into your project.

The SDK relies on Newtonsoft's JSON.NET Package which can be installed via the Unity Package Manager.

To do so open the Package Manager via `Window > Package Manager` and click the `+` button then `Add package from git URL...` and enter `com.unity.nuget.newtonsoft-json` and click `Add`.
<!-- End SDK Installation [installation] -->

<!-- Start SDK Example Usage [usage] -->
## SDK Example Usage

### Example

```csharp
using HathoraCloud;
using HathoraCloud.Models.Shared;

var sdk = new HathoraCloudSDK(
    security: new Security() {
        HathoraDevToken = "<YOUR_BEARER_TOKEN_HERE>",
    },
    appId: "app-af469a92-5b45-4565-b3c4-b79878de67d2");

AppConfig req = new AppConfig() {
    AppName = "minecraft",
    AuthConfiguration = new AuthConfiguration() {},
};


using(var res = await sdk.AppsV1.CreateAppV1DeprecatedAsync(req))
{
    // handle response
}


```
<!-- End SDK Example Usage [usage] -->

<!-- Start Available Resources and Operations [operations] -->
## Available Resources and Operations

### [AppsV1](docs/sdks/appsv1/README.md)

* [CreateAppV1Deprecated](docs/sdks/appsv1/README.md#createappv1deprecated) - Create a new [application](https://hathora.dev/docs/concepts/hathora-entities#application).
* [DeleteAppV1Deprecated](docs/sdks/appsv1/README.md#deleteappv1deprecated) - Delete an [application](https://hathora.dev/docs/concepts/hathora-entities#application) using `appId`. Your organization will lose access to this application.
* [GetAppInfoV1Deprecated](docs/sdks/appsv1/README.md#getappinfov1deprecated) - Get details for an [application](https://hathora.dev/docs/concepts/hathora-entities#application) using `appId`.
* [GetAppsV1Deprecated](docs/sdks/appsv1/README.md#getappsv1deprecated) - Returns an unsorted list of your organization’s [applications](https://hathora.dev/docs/concepts/hathora-entities#application). An application is uniquely identified by an `appId`.
* [UpdateAppV1Deprecated](docs/sdks/appsv1/README.md#updateappv1deprecated) - Update data for an existing [application](https://hathora.dev/docs/concepts/hathora-entities#application) using `appId`.

### [AppsV2](docs/sdks/appsv2/README.md)

* [CreateApp](docs/sdks/appsv2/README.md#createapp) - Create a new [application](https://hathora.dev/docs/concepts/hathora-entities#application).
* [DeleteApp](docs/sdks/appsv2/README.md#deleteapp) - Delete an [application](https://hathora.dev/docs/concepts/hathora-entities#application) using `appId`. Your organization will lose access to this application.
* [GetApp](docs/sdks/appsv2/README.md#getapp) - Get details for an [application](https://hathora.dev/docs/concepts/hathora-entities#application) using `appId`.
* [GetApps](docs/sdks/appsv2/README.md#getapps) - Returns an unsorted list of your organization’s [applications](https://hathora.dev/docs/concepts/hathora-entities#application). An application is uniquely identified by an `appId`.
* [UpdateApp](docs/sdks/appsv2/README.md#updateapp) - Update data for an existing [application](https://hathora.dev/docs/concepts/hathora-entities#application) using `appId`.

### [AuthV1](docs/sdks/authv1/README.md)

* [LoginAnonymous](docs/sdks/authv1/README.md#loginanonymous) - Returns a unique player token for an anonymous user.
* [LoginGoogle](docs/sdks/authv1/README.md#logingoogle) - Returns a unique player token using a Google-signed OIDC `idToken`.
* [LoginNickname](docs/sdks/authv1/README.md#loginnickname) - Returns a unique player token with a specified nickname for a user.

### [BillingV1](docs/sdks/billingv1/README.md)

* [GetBalance](docs/sdks/billingv1/README.md#getbalance)
* [GetInvoices](docs/sdks/billingv1/README.md#getinvoices)
* [GetPaymentMethod](docs/sdks/billingv1/README.md#getpaymentmethod)
* [GetUpcomingInvoiceItems](docs/sdks/billingv1/README.md#getupcominginvoiceitems)
* [GetUpcomingInvoiceTotal](docs/sdks/billingv1/README.md#getupcominginvoicetotal)
* [InitStripeCustomerPortalUrl](docs/sdks/billingv1/README.md#initstripecustomerportalurl)

### [BuildsV1](docs/sdks/buildsv1/README.md)

* [~~CreateBuildDeprecated~~](docs/sdks/buildsv1/README.md#createbuilddeprecated) - Creates a new [build](https://hathora.dev/docs/concepts/hathora-entities#build). Responds with a `buildId` that you must pass to [`RunBuild()`](https://hathora.dev/api#tag/BuildV1/operation/RunBuild) to build the game server artifact. You can optionally pass in a `buildTag` to associate an external version with a build. :warning: **Deprecated**
* [~~DeleteBuildDeprecated~~](docs/sdks/buildsv1/README.md#deletebuilddeprecated) - Delete a [build](https://hathora.dev/docs/concepts/hathora-entities#build). All associated metadata is deleted. :warning: **Deprecated**
* [~~GetBuildInfoDeprecated~~](docs/sdks/buildsv1/README.md#getbuildinfodeprecated) - Get details for a [build](https://hathora.dev/docs/concepts/hathora-entities#build). :warning: **Deprecated**
* [~~GetBuildsDeprecated~~](docs/sdks/buildsv1/README.md#getbuildsdeprecated) - Returns an array of [builds](https://hathora.dev/docs/concepts/hathora-entities#build) for an [application](https://hathora.dev/docs/concepts/hathora-entities#application). :warning: **Deprecated**
* [~~RunBuildDeprecated~~](docs/sdks/buildsv1/README.md#runbuilddeprecated) - Builds a game server artifact from a tarball you provide. Pass in the `buildId` generated from [`CreateBuild()`](https://hathora.dev/api#tag/BuildV1/operation/CreateBuild). :warning: **Deprecated**

### [BuildsV2](docs/sdks/buildsv2/README.md)

* [CreateBuildV2Deprecated](docs/sdks/buildsv2/README.md#createbuildv2deprecated) - Creates a new [build](https://hathora.dev/docs/concepts/hathora-entities#build). Responds with a `buildId` that you must pass to [`RunBuild()`](https://hathora.dev/api#tag/BuildV1/operation/RunBuild) to build the game server artifact. You can optionally pass in a `buildTag` to associate an external version with a build.
* [CreateBuildWithUploadUrlV2Deprecated](docs/sdks/buildsv2/README.md#createbuildwithuploadurlv2deprecated) - Creates a new [build](https://hathora.dev/docs/concepts/hathora-entities#build) with `uploadUrl` that can be used to upload the build to before calling `runBuild`. Responds with a `buildId` that you must pass to [`RunBuild()`](https://hathora.dev/api#tag/BuildV1/operation/RunBuild) to build the game server artifact. You can optionally pass in a `buildTag` to associate an external version with a build.
* [CreateWithMultipartUploadsV2Deprecated](docs/sdks/buildsv2/README.md#createwithmultipartuploadsv2deprecated) - Creates a new [build](https://hathora.dev/docs/concepts/hathora-entities#build) with optional `multipartUploadUrls` that can be used to upload larger builds in parts before calling `runBuild`. Responds with a `buildId` that you must pass to [`RunBuild()`](https://hathora.dev/api#tag/BuildV1/operation/RunBuild) to build the game server artifact. You can optionally pass in a `buildTag` to associate an external version with a build.
* [DeleteBuildV2Deprecated](docs/sdks/buildsv2/README.md#deletebuildv2deprecated) - Delete a [build](https://hathora.dev/docs/concepts/hathora-entities#build). All associated metadata is deleted.
* [GetBuildInfoV2Deprecated](docs/sdks/buildsv2/README.md#getbuildinfov2deprecated) - Get details for a [build](https://hathora.dev/docs/concepts/hathora-entities#build).
* [GetBuildsV2Deprecated](docs/sdks/buildsv2/README.md#getbuildsv2deprecated) - Returns an array of [builds](https://hathora.dev/docs/concepts/hathora-entities#build) for an [application](https://hathora.dev/docs/concepts/hathora-entities#application).
* [RunBuildV2Deprecated](docs/sdks/buildsv2/README.md#runbuildv2deprecated) - Builds a game server artifact from a tarball you provide. Pass in the `buildId` generated from [`CreateBuild()`](https://hathora.dev/api#tag/BuildV1/operation/CreateBuild).

### [BuildsV3](docs/sdks/buildsv3/README.md)

* [CreateBuild](docs/sdks/buildsv3/README.md#createbuild) - Creates a new [build](https://hathora.dev/docs/concepts/hathora-entities#build) with optional `multipartUploadUrls` that can be used to upload larger builds in parts before calling `runBuild`. Responds with a `buildId` that you must pass to [`RunBuild()`](https://hathora.dev/api#tag/BuildV1/operation/RunBuild) to build the game server artifact. You can optionally pass in a `buildTag` to associate an external version with a build.
* [DeleteBuild](docs/sdks/buildsv3/README.md#deletebuild) - Delete a [build](https://hathora.dev/docs/concepts/hathora-entities#build). All associated metadata is deleted.
Be careful which builds you delete. This endpoint does not prevent you from deleting actively used builds.
Deleting a build that is actively build used by an app's deployment will cause failures when creating rooms.
* [GetBuild](docs/sdks/buildsv3/README.md#getbuild) - Get details for a [build](https://hathora.dev/docs/concepts/hathora-entities#build).
* [GetBuilds](docs/sdks/buildsv3/README.md#getbuilds) - Returns an array of [builds](https://hathora.dev/docs/concepts/hathora-entities#build) for an [application](https://hathora.dev/docs/concepts/hathora-entities#application).
* [RunBuild](docs/sdks/buildsv3/README.md#runbuild) - Builds a game server artifact from a tarball you provide. Pass in the `buildId` generated from [`CreateBuild()`](https://hathora.dev/api#tag/BuildV1/operation/CreateBuild).

### [DeploymentsV1](docs/sdks/deploymentsv1/README.md)

* [~~CreateDeploymentV1Deprecated~~](docs/sdks/deploymentsv1/README.md#createdeploymentv1deprecated) - Create a new [deployment](https://hathora.dev/docs/concepts/hathora-entities#deployment). Creating a new deployment means all new rooms created will use the latest deployment configuration, but existing games in progress will not be affected. :warning: **Deprecated**
* [~~GetDeploymentInfoV1Deprecated~~](docs/sdks/deploymentsv1/README.md#getdeploymentinfov1deprecated) - Get details for a [deployment](https://hathora.dev/docs/concepts/hathora-entities#deployment). :warning: **Deprecated**
* [~~GetDeploymentsV1Deprecated~~](docs/sdks/deploymentsv1/README.md#getdeploymentsv1deprecated) - Returns an array of [deployments](https://hathora.dev/docs/concepts/hathora-entities#deployment) for an [application](https://hathora.dev/docs/concepts/hathora-entities#application). :warning: **Deprecated**
* [~~GetLatestDeploymentV1Deprecated~~](docs/sdks/deploymentsv1/README.md#getlatestdeploymentv1deprecated) - Get the latest [deployment](https://hathora.dev/docs/concepts/hathora-entities#deployment) for an [application](https://hathora.dev/docs/concepts/hathora-entities#application). :warning: **Deprecated**

### [DeploymentsV2](docs/sdks/deploymentsv2/README.md)

* [CreateDeploymentV2Deprecated](docs/sdks/deploymentsv2/README.md#createdeploymentv2deprecated) - Create a new [deployment](https://hathora.dev/docs/concepts/hathora-entities#deployment). Creating a new deployment means all new rooms created will use the latest deployment configuration, but existing games in progress will not be affected.
* [GetDeploymentInfoV2Deprecated](docs/sdks/deploymentsv2/README.md#getdeploymentinfov2deprecated) - Get details for a [deployment](https://hathora.dev/docs/concepts/hathora-entities#deployment).
* [GetDeploymentsV2Deprecated](docs/sdks/deploymentsv2/README.md#getdeploymentsv2deprecated) - Returns an array of [deployments](https://hathora.dev/docs/concepts/hathora-entities#deployment) for an [application](https://hathora.dev/docs/concepts/hathora-entities#application).
* [GetLatestDeploymentV2Deprecated](docs/sdks/deploymentsv2/README.md#getlatestdeploymentv2deprecated) - Get the latest [deployment](https://hathora.dev/docs/concepts/hathora-entities#deployment) for an [application](https://hathora.dev/docs/concepts/hathora-entities#application).

### [DeploymentsV3](docs/sdks/deploymentsv3/README.md)

* [CreateDeployment](docs/sdks/deploymentsv3/README.md#createdeployment) - Create a new [deployment](https://hathora.dev/docs/concepts/hathora-entities#deployment). Creating a new deployment means all new rooms created will use the latest deployment configuration, but existing games in progress will not be affected.
* [GetDeployment](docs/sdks/deploymentsv3/README.md#getdeployment) - Get details for a [deployment](https://hathora.dev/docs/concepts/hathora-entities#deployment).
* [GetDeployments](docs/sdks/deploymentsv3/README.md#getdeployments) - Returns an array of [deployments](https://hathora.dev/docs/concepts/hathora-entities#deployment) for an [application](https://hathora.dev/docs/concepts/hathora-entities#application).
* [GetLatestDeployment](docs/sdks/deploymentsv3/README.md#getlatestdeployment) - Get the latest [deployment](https://hathora.dev/docs/concepts/hathora-entities#deployment) for an [application](https://hathora.dev/docs/concepts/hathora-entities#application).

### [DiscoveryV1](docs/sdks/discoveryv1/README.md)

* [~~GetPingServiceEndpointsDeprecated~~](docs/sdks/discoveryv1/README.md#getpingserviceendpointsdeprecated) - Returns an array of V1 regions with a host and port that a client can directly ping. Open a websocket connection to `wss://<host>:<port>/ws` and send a packet. To calculate ping, measure the time it takes to get an echo packet back. :warning: **Deprecated**

### [DiscoveryV2](docs/sdks/discoveryv2/README.md)

* [GetPingServiceEndpoints](docs/sdks/discoveryv2/README.md#getpingserviceendpoints) - Returns an array of all regions with a host and port that a client can directly ping. Open a websocket connection to `wss://<host>:<port>/ws` and send a packet. To calculate ping, measure the time it takes to get an echo packet back.

### [LobbiesV1](docs/sdks/lobbiesv1/README.md)

* [~~CreatePrivateLobbyDeprecated~~](docs/sdks/lobbiesv1/README.md#createprivatelobbydeprecated) - :warning: **Deprecated**
* [~~CreatePublicLobbyDeprecated~~](docs/sdks/lobbiesv1/README.md#createpubliclobbydeprecated) - :warning: **Deprecated**
* [~~ListActivePublicLobbiesDeprecatedV1~~](docs/sdks/lobbiesv1/README.md#listactivepubliclobbiesdeprecatedv1) - :warning: **Deprecated**

### [LobbiesV2](docs/sdks/lobbiesv2/README.md)

* [~~CreateLobbyDeprecated~~](docs/sdks/lobbiesv2/README.md#createlobbydeprecated) - Create a new lobby for an [application](https://hathora.dev/docs/concepts/hathora-entities#application). A lobby object is a wrapper around a [room](https://hathora.dev/docs/concepts/hathora-entities#room) object. With a lobby, you get additional functionality like configuring the visibility of the room, managing the state of a match, and retrieving a list of public lobbies to display to players. :warning: **Deprecated**
* [~~CreateLocalLobby~~](docs/sdks/lobbiesv2/README.md#createlocallobby) - :warning: **Deprecated**
* [~~CreatePrivateLobby~~](docs/sdks/lobbiesv2/README.md#createprivatelobby) - :warning: **Deprecated**
* [~~CreatePublicLobby~~](docs/sdks/lobbiesv2/README.md#createpubliclobby) - :warning: **Deprecated**
* [~~GetLobbyInfo~~](docs/sdks/lobbiesv2/README.md#getlobbyinfo) - Get details for a lobby. :warning: **Deprecated**
* [~~ListActivePublicLobbiesDeprecatedV2~~](docs/sdks/lobbiesv2/README.md#listactivepubliclobbiesdeprecatedv2) - Get all active lobbies for a an [application](https://hathora.dev/docs/concepts/hathora-entities#application). Filter by optionally passing in a `region`. Use this endpoint to display all public lobbies that a player can join in the game client. :warning: **Deprecated**
* [~~SetLobbyState~~](docs/sdks/lobbiesv2/README.md#setlobbystate) - Set the state of a lobby. State is intended to be set by the server and must be smaller than 1MB. Use this endpoint to store match data like live player count to enforce max number of clients or persist end-game data (i.e. winner or final scores). :warning: **Deprecated**

### [LobbiesV3](docs/sdks/lobbiesv3/README.md)

* [CreateLobby](docs/sdks/lobbiesv3/README.md#createlobby) - Create a new lobby for an [application](https://hathora.dev/docs/concepts/hathora-entities#application). A lobby object is a wrapper around a [room](https://hathora.dev/docs/concepts/hathora-entities#room) object. With a lobby, you get additional functionality like configuring the visibility of the room, managing the state of a match, and retrieving a list of public lobbies to display to players.
* [GetLobbyInfoByRoomId](docs/sdks/lobbiesv3/README.md#getlobbyinfobyroomid) - Get details for a lobby.
* [GetLobbyInfoByShortCode](docs/sdks/lobbiesv3/README.md#getlobbyinfobyshortcode) - Get details for a lobby. If 2 or more lobbies have the same `shortCode`, then the most recently created lobby will be returned.
* [ListActivePublicLobbies](docs/sdks/lobbiesv3/README.md#listactivepubliclobbies) - Get all active lobbies for a given [application](https://hathora.dev/docs/concepts/hathora-entities#application). Filter the array by optionally passing in a `region`. Use this endpoint to display all public lobbies that a player can join in the game client.

### [LogsV1](docs/sdks/logsv1/README.md)

* [DownloadLogForProcess](docs/sdks/logsv1/README.md#downloadlogforprocess) - Download entire log file for a stopped process.
* [~~GetLogsForApp~~](docs/sdks/logsv1/README.md#getlogsforapp) - Returns a stream of logs for an [application](https://hathora.dev/docs/concepts/hathora-entities#application) using `appId`. :warning: **Deprecated**
* [~~GetLogsForDeployment~~](docs/sdks/logsv1/README.md#getlogsfordeployment) - Returns a stream of logs for a [deployment](https://hathora.dev/docs/concepts/hathora-entities#deployment) using `appId` and `deploymentId`. :warning: **Deprecated**
* [GetLogsForProcess](docs/sdks/logsv1/README.md#getlogsforprocess) - Returns a stream of logs for a [process](https://hathora.dev/docs/concepts/hathora-entities#process) using `appId` and `processId`.

### [ManagementV1](docs/sdks/managementv1/README.md)

* [SendVerificationEmail](docs/sdks/managementv1/README.md#sendverificationemail)

### [MetricsV1](docs/sdks/metricsv1/README.md)

* [GetMetrics](docs/sdks/metricsv1/README.md#getmetrics) - Get metrics for a [process](https://hathora.dev/docs/concepts/hathora-entities#process) using `appId` and `processId`.

### [OrganizationsV1](docs/sdks/organizationsv1/README.md)

* [AcceptInvite](docs/sdks/organizationsv1/README.md#acceptinvite)
* [GetOrgMembers](docs/sdks/organizationsv1/README.md#getorgmembers)
* [GetOrgPendingInvites](docs/sdks/organizationsv1/README.md#getorgpendinginvites)
* [GetOrgs](docs/sdks/organizationsv1/README.md#getorgs) - Returns an unsorted list of all organizations that you are a member of (an accepted membership invite). An organization is uniquely identified by an `orgId`.
* [GetUserPendingInvites](docs/sdks/organizationsv1/README.md#getuserpendinginvites)
* [InviteUser](docs/sdks/organizationsv1/README.md#inviteuser)
* [RejectInvite](docs/sdks/organizationsv1/README.md#rejectinvite)
* [RescindInvite](docs/sdks/organizationsv1/README.md#rescindinvite)

### [ProcessesV1](docs/sdks/processesv1/README.md)

* [~~GetProcessInfoDeprecated~~](docs/sdks/processesv1/README.md#getprocessinfodeprecated) - Get details for a [process](https://hathora.dev/docs/concepts/hathora-entities#process). :warning: **Deprecated**
* [~~GetRunningProcesses~~](docs/sdks/processesv1/README.md#getrunningprocesses) - Retrieve 10 most recently started [process](https://hathora.dev/docs/concepts/hathora-entities#process) objects for an [application](https://hathora.dev/docs/concepts/hathora-entities#application). Filter the array by optionally passing in a `region`. :warning: **Deprecated**
* [~~GetStoppedProcesses~~](docs/sdks/processesv1/README.md#getstoppedprocesses) - Retrieve 10 most recently stopped [process](https://hathora.dev/docs/concepts/hathora-entities#process) objects for an [application](https://hathora.dev/docs/concepts/hathora-entities#application). Filter the array by optionally passing in a `region`. :warning: **Deprecated**

### [ProcessesV2](docs/sdks/processesv2/README.md)

* [CreateProcessV2Deprecated](docs/sdks/processesv2/README.md#createprocessv2deprecated) - Creates a [process](https://hathora.dev/docs/concepts/hathora-entities#process) without a room. Use this to pre-allocate processes ahead of time so that subsequent room assignment via [CreateRoom()](https://hathora.dev/api#tag/RoomV2/operation/CreateRoom) can be instant.
* [GetLatestProcessesV2Deprecated](docs/sdks/processesv2/README.md#getlatestprocessesv2deprecated) - Retrieve the 10 most recent [processes](https://hathora.dev/docs/concepts/hathora-entities#process) objects for an [application](https://hathora.dev/docs/concepts/hathora-entities#application). Filter the array by optionally passing in a `status` or `region`.
* [GetProcessInfoV2Deprecated](docs/sdks/processesv2/README.md#getprocessinfov2deprecated) - Get details for a [process](https://hathora.dev/docs/concepts/hathora-entities#process).
* [GetProcessesCountExperimentalV2Deprecated](docs/sdks/processesv2/README.md#getprocessescountexperimentalv2deprecated) - Count the number of [processes](https://hathora.dev/docs/concepts/hathora-entities#process) objects for an [application](https://hathora.dev/docs/concepts/hathora-entities#application). Filter by optionally passing in a `status` or `region`.
* [StopProcessV2Deprecated](docs/sdks/processesv2/README.md#stopprocessv2deprecated) - Stops a [process](https://hathora.dev/docs/concepts/hathora-entities#process) immediately.

### [ProcessesV3](docs/sdks/processesv3/README.md)

* [CreateProcess](docs/sdks/processesv3/README.md#createprocess) - Creates a [process](https://hathora.dev/docs/concepts/hathora-entities#process) without a room. Use this to pre-allocate processes ahead of time so that subsequent room assignment via [CreateRoom()](https://hathora.dev/api#tag/RoomV2/operation/CreateRoom) can be instant.
* [GetLatestProcesses](docs/sdks/processesv3/README.md#getlatestprocesses) - Retrieve the 10 most recent [processes](https://hathora.dev/docs/concepts/hathora-entities#process) objects for an [application](https://hathora.dev/docs/concepts/hathora-entities#application). Filter the array by optionally passing in a `status` or `region`.
* [GetProcess](docs/sdks/processesv3/README.md#getprocess) - Get details for a [process](https://hathora.dev/docs/concepts/hathora-entities#process).
* [GetProcessesCountExperimental](docs/sdks/processesv3/README.md#getprocessescountexperimental) - Count the number of [processes](https://hathora.dev/docs/concepts/hathora-entities#process) objects for an [application](https://hathora.dev/docs/concepts/hathora-entities#application). Filter by optionally passing in a `status` or `region`.
* [StopProcess](docs/sdks/processesv3/README.md#stopprocess) - Stops a [process](https://hathora.dev/docs/concepts/hathora-entities#process) immediately.

### [RoomsV1](docs/sdks/roomsv1/README.md)

* [~~CreateRoomDeprecated~~](docs/sdks/roomsv1/README.md#createroomdeprecated) - :warning: **Deprecated**
* [~~DestroyRoomDeprecated~~](docs/sdks/roomsv1/README.md#destroyroomdeprecated) - :warning: **Deprecated**
* [~~GetActiveRoomsForProcessDeprecated~~](docs/sdks/roomsv1/README.md#getactiveroomsforprocessdeprecated) - :warning: **Deprecated**
* [~~GetConnectionInfoDeprecated~~](docs/sdks/roomsv1/README.md#getconnectioninfodeprecated) - :warning: **Deprecated**
* [~~GetInactiveRoomsForProcessDeprecated~~](docs/sdks/roomsv1/README.md#getinactiveroomsforprocessdeprecated) - :warning: **Deprecated**
* [~~GetRoomInfoDeprecated~~](docs/sdks/roomsv1/README.md#getroominfodeprecated) - :warning: **Deprecated**
* [~~SuspendRoomDeprecated~~](docs/sdks/roomsv1/README.md#suspendroomdeprecated) - :warning: **Deprecated**

### [RoomsV2](docs/sdks/roomsv2/README.md)

* [CreateRoom](docs/sdks/roomsv2/README.md#createroom) - Create a new [room](https://hathora.dev/docs/concepts/hathora-entities#room) for an existing [application](https://hathora.dev/docs/concepts/hathora-entities#application). Poll the [`GetConnectionInfo()`](https://hathora.dev/api#tag/RoomV2/operation/GetConnectionInfo) endpoint to get connection details for an active room.
* [DestroyRoom](docs/sdks/roomsv2/README.md#destroyroom) - Destroy a [room](https://hathora.dev/docs/concepts/hathora-entities#room). All associated metadata is deleted.
* [GetActiveRoomsForProcess](docs/sdks/roomsv2/README.md#getactiveroomsforprocess) - Get all active [rooms](https://hathora.dev/docs/concepts/hathora-entities#room) for a given [process](https://hathora.dev/docs/concepts/hathora-entities#process).
* [GetConnectionInfo](docs/sdks/roomsv2/README.md#getconnectioninfo) - Poll this endpoint to get connection details to a [room](https://hathora.dev/docs/concepts/hathora-entities#room). Clients can call this endpoint without authentication.
* [GetInactiveRoomsForProcess](docs/sdks/roomsv2/README.md#getinactiveroomsforprocess) - Get all inactive [rooms](https://hathora.dev/docs/concepts/hathora-entities#room) for a given [process](https://hathora.dev/docs/concepts/hathora-entities#process).
* [GetRoomInfo](docs/sdks/roomsv2/README.md#getroominfo) - Retreive current and historical allocation data for a [room](https://hathora.dev/docs/concepts/hathora-entities#room).
* [~~SuspendRoomV2Deprecated~~](docs/sdks/roomsv2/README.md#suspendroomv2deprecated) - Suspend a [room](https://hathora.dev/docs/concepts/hathora-entities#room). The room is unallocated from the process but can be rescheduled later using the same `roomId`. :warning: **Deprecated**
* [UpdateRoomConfig](docs/sdks/roomsv2/README.md#updateroomconfig)

### [TokensV1](docs/sdks/tokensv1/README.md)

* [CreateOrgToken](docs/sdks/tokensv1/README.md#createorgtoken) - Create a new organization token.
* [GetOrgTokens](docs/sdks/tokensv1/README.md#getorgtokens) - List all organization tokens for a given org.
* [RevokeOrgToken](docs/sdks/tokensv1/README.md#revokeorgtoken) - Revoke an organization token.
<!-- End Available Resources and Operations [operations] -->





<!-- Start Global Parameters [global-parameters] -->
## Global Parameters

## Global Parameters

A parameter is configured globally. This parameter may be set on the SDK client instance itself during initialization. When configured as an option during SDK initialization, This global value will be used as the default on the operations that use it. When such operations are called, there is a place in each to override the global value, if needed.

For example, you can set `appId` to `"app-af469a92-5b45-4565-b3c4-b79878de67d2"` at SDK initialization and then you do not have to pass the same value on calls to operations like `DeleteAppV1Deprecated`. But if you want to do so you may, which will locally override the global setting. See the example code below for a demonstration.


### Available Globals

The following global parameter is available.

| Name | Type | Required | Description |
| ---- | ---- |:--------:| ----------- |
| appId | string |  | The AppId parameter. |


### Example

```csharp
using HathoraCloud;
using HathoraCloud.Models.Shared;
using HathoraCloud.Models.Operations;

var sdk = new HathoraCloudSDK(
    security: new Security() {
        HathoraDevToken = "<YOUR_BEARER_TOKEN_HERE>",
    },
    appId: "app-af469a92-5b45-4565-b3c4-b79878de67d2");

DeleteAppV1DeprecatedRequest req = new DeleteAppV1DeprecatedRequest() {};


using(var res = await sdk.AppsV1.DeleteAppV1DeprecatedAsync(req))
{
    // handle response
}


```
<!-- End Global Parameters [global-parameters] -->

<!-- Start Server Selection [server] -->
## Server Selection

## Server Selection

### Select Server by Index

You can override the default server globally by passing a server index to the `serverIndex: number` optional parameter when initializing the SDK client instance. The selected server will then be used as the default on the operations that use it. This table lists the indexes associated with the available servers:

| # | Server | Variables |
| - | ------ | --------- |
| 0 | `https://api.hathora.dev` | None |
| 1 | `https:///` | None |




### Override Server URL Per-Client

The default server can also be overridden globally by passing a URL to the `serverUrl: str` optional parameter when initializing the SDK client instance. For example:
<!-- End Server Selection [server] -->

<!-- Start Error Handling [errors] -->
## Error Handling

Handling errors in this SDK should largely match your expectations.  All operations return a response object or thow an exception.  If Error objects are specified in your OpenAPI Spec, the SDK will raise the appropriate type.

| Error Object                            | Status Code                             | Content Type                            |
| --------------------------------------- | --------------------------------------- | --------------------------------------- |
| HathoraCloud.Models.Errors.ApiError     | 401,422,429,500                         | application/json                        |
| HathoraCloud.Models.Errors.SDKException | 4xx-5xx                                 | */*                                     |

### Example

```csharp
using HathoraCloud;
using HathoraCloud.Models.Shared;
using System;
using HathoraCloud.Models.Errors;

var sdk = new HathoraCloudSDK(
    security: new Security() {
        HathoraDevToken = "<YOUR_BEARER_TOKEN_HERE>",
    },
    appId: "app-af469a92-5b45-4565-b3c4-b79878de67d2");

AppConfig req = new AppConfig() {
    AppName = "minecraft",
    AuthConfiguration = new AuthConfiguration() {},
};

try
{
    using(var res = await sdk.AppsV1.CreateAppV1DeprecatedAsync(req))
    {
            // handle response
    }
}
catch (Exception ex)
{
    if (ex is ApiError)
    {
        // handle exception
    }
    else if (ex is HathoraCloud.Models.Errors.SDKException)
    {
        // handle exception
    }
}

```
<!-- End Error Handling [errors] -->

<!-- Start Authentication [security] -->
## Authentication

### Per-Client Security Schemes

This SDK supports the following security scheme globally:

| Name              | Type              | Scheme            |
| ----------------- | ----------------- | ----------------- |
| `HathoraDevToken` | http              | HTTP Bearer       |

You can set the security parameters through the `security` optional parameter when initializing the SDK client instance. For example:
```csharp
using HathoraCloud;
using HathoraCloud.Models.Shared;

var sdk = new HathoraCloudSDK(
    security: new Security() {
        HathoraDevToken = "<YOUR_BEARER_TOKEN_HERE>",
    },
    appId: "app-af469a92-5b45-4565-b3c4-b79878de67d2");

AppConfig req = new AppConfig() {
    AppName = "minecraft",
    AuthConfiguration = new AuthConfiguration() {},
};


using(var res = await sdk.AppsV1.CreateAppV1DeprecatedAsync(req))
{
    // handle response
}


```

### Per-Operation Security Schemes

Some operations in this SDK require the security scheme to be specified at the request level. For example:
```csharp
using HathoraCloud;
using HathoraCloud.Models.Operations;
using HathoraCloud.Models.Shared;

var sdk = new HathoraCloudSDK(appId: "app-af469a92-5b45-4565-b3c4-b79878de67d2");

CreatePrivateLobbyDeprecatedRequest req = new CreatePrivateLobbyDeprecatedRequest() {};


using(var res = await sdk.LobbiesV1.CreatePrivateLobbyDeprecatedAsync(
    security: new CreatePrivateLobbyDeprecatedSecurity() {
        PlayerAuth = "<YOUR_BEARER_TOKEN_HERE>",
    },
    req))
{
    // handle response
}


```
<!-- End Authentication [security] -->

<!-- Placeholder for Future Speakeasy SDK Sections -->



### Maturity

This SDK is in beta, and there may be breaking changes between versions without a major version update. Therefore, we recommend pinning usage
to a specific package version. This way, you can install the same version each time without breaking changes unless you are intentionally
looking for the latest version.

### Contributions

While we value open-source contributions to this SDK, this library is generated programmatically.
Feel free to open a PR or a Github issue as a proof of concept and we'll do our best to include it in a future release!

### SDK Created by [Speakeasy](https://docs.speakeasyapi.dev/docs/using-speakeasy/client-sdks)
