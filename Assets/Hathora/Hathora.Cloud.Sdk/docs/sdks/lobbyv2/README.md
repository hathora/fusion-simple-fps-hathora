# LobbyV2
(*LobbyV2*)

## Overview

Deprecated. Use [LobbyV3](https://hathora.dev/api#tag/LobbyV3).

### Available Operations

* [~~CreateLobbyDeprecated~~](#createlobbydeprecated) - Create a new lobby for an [application](https://hathora.dev/docs/concepts/hathora-entities#application). A lobby object is a wrapper around a [room](https://hathora.dev/docs/concepts/hathora-entities#room) object. With a lobby, you get additional functionality like configuring the visibility of the room, managing the state of a match, and retrieving a list of public lobbies to display to players. :warning: **Deprecated**
* [~~CreateLocalLobby~~](#createlocallobby) - :warning: **Deprecated**
* [~~CreatePrivateLobby~~](#createprivatelobby) - :warning: **Deprecated**
* [~~CreatePublicLobby~~](#createpubliclobby) - :warning: **Deprecated**
* [~~GetLobbyInfo~~](#getlobbyinfo) - Get details for a lobby. :warning: **Deprecated**
* [~~ListActivePublicLobbiesDeprecatedV2~~](#listactivepubliclobbiesdeprecatedv2) - Get all active lobbies for a an [application](https://hathora.dev/docs/concepts/hathora-entities#application). Filter by optionally passing in a `region`. Use this endpoint to display all public lobbies that a player can join in the game client. :warning: **Deprecated**
* [~~SetLobbyState~~](#setlobbystate) - Set the state of a lobby. State is intended to be set by the server and must be smaller than 1MB. Use this endpoint to store match data like live player count to enforce max number of clients or persist end-game data (i.e. winner or final scores). :warning: **Deprecated**

## ~~CreateLobbyDeprecated~~

Create a new lobby for an [application](https://hathora.dev/docs/concepts/hathora-entities#application). A lobby object is a wrapper around a [room](https://hathora.dev/docs/concepts/hathora-entities#room) object. With a lobby, you get additional functionality like configuring the visibility of the room, managing the state of a match, and retrieving a list of public lobbies to display to players.

> :warning: **DEPRECATED**: This will be removed in a future release, please migrate away from it as soon as possible.

### Example Usage

```csharp
using HathoraCloud;
using HathoraCloud.Models.Operations;
using HathoraCloud.Models.Shared;

var sdk = new HathoraCloudSDK(
    appId: "app-af469a92-5b45-4565-b3c4-b79878de67d2"
);

using(var res = await sdk.LobbyV2.CreateLobbyDeprecatedAsync(new CreateLobbyDeprecatedSecurity() {
    PlayerAuth = "",
}, new CreateLobbyDeprecatedRequest() {
    CreateLobbyParams = new CreateLobbyParams() {
        InitialConfig = new LobbyInitialConfig() {},
        Region = Region.Tokyo,
        Visibility = LobbyVisibility.Private,
    },
    RoomId = "2swovpy1fnunu",
}))
{
    // handle response
}
```

### Parameters

| Parameter                                                                                                                | Type                                                                                                                     | Required                                                                                                                 | Description                                                                                                              |
| ------------------------------------------------------------------------------------------------------------------------ | ------------------------------------------------------------------------------------------------------------------------ | ------------------------------------------------------------------------------------------------------------------------ | ------------------------------------------------------------------------------------------------------------------------ |
| `request`                                                                                                                | [CreateLobbyDeprecatedRequest](../../models/operations/CreateLobbyDeprecatedRequest.md)                                  | :heavy_check_mark:                                                                                                       | The request object to use for the request.                                                                               |
| `security`                                                                                                               | [HathoraCloud.Models.Operations.CreateLobbyDeprecatedSecurity](../../models/operations/CreateLobbyDeprecatedSecurity.md) | :heavy_check_mark:                                                                                                       | The security requirements to use for the request.                                                                        |


### Response

**[CreateLobbyDeprecatedResponse](../../models/operations/CreateLobbyDeprecatedResponse.md)**


## ~~CreateLocalLobby~~

> :warning: **DEPRECATED**: This will be removed in a future release, please migrate away from it as soon as possible.

### Example Usage

```csharp
using HathoraCloud;
using HathoraCloud.Models.Operations;
using HathoraCloud.Models.Shared;

var sdk = new HathoraCloudSDK(
    appId: "app-af469a92-5b45-4565-b3c4-b79878de67d2"
);

using(var res = await sdk.LobbyV2.CreateLocalLobbyAsync(new CreateLocalLobbySecurity() {
    PlayerAuth = "",
}, new CreateLocalLobbyRequest() {
    RequestBody = new CreateLocalLobbyRequestBody() {
        InitialConfig = new LobbyInitialConfig() {},
        Region = Region.Sydney,
    },
    RoomId = "2swovpy1fnunu",
}))
{
    // handle response
}
```

### Parameters

| Parameter                                                                                                      | Type                                                                                                           | Required                                                                                                       | Description                                                                                                    |
| -------------------------------------------------------------------------------------------------------------- | -------------------------------------------------------------------------------------------------------------- | -------------------------------------------------------------------------------------------------------------- | -------------------------------------------------------------------------------------------------------------- |
| `request`                                                                                                      | [CreateLocalLobbyRequest](../../models/operations/CreateLocalLobbyRequest.md)                                  | :heavy_check_mark:                                                                                             | The request object to use for the request.                                                                     |
| `security`                                                                                                     | [HathoraCloud.Models.Operations.CreateLocalLobbySecurity](../../models/operations/CreateLocalLobbySecurity.md) | :heavy_check_mark:                                                                                             | The security requirements to use for the request.                                                              |


### Response

**[CreateLocalLobbyResponse](../../models/operations/CreateLocalLobbyResponse.md)**


## ~~CreatePrivateLobby~~

> :warning: **DEPRECATED**: This will be removed in a future release, please migrate away from it as soon as possible.

### Example Usage

```csharp
using HathoraCloud;
using HathoraCloud.Models.Operations;
using HathoraCloud.Models.Shared;

var sdk = new HathoraCloudSDK(
    appId: "app-af469a92-5b45-4565-b3c4-b79878de67d2"
);

using(var res = await sdk.LobbyV2.CreatePrivateLobbyAsync(new CreatePrivateLobbySecurity() {
    PlayerAuth = "",
}, new CreatePrivateLobbyRequest() {
    RequestBody = new CreatePrivateLobbyRequestBody() {
        InitialConfig = new LobbyInitialConfig() {},
        Region = Region.Chicago,
    },
    RoomId = "2swovpy1fnunu",
}))
{
    // handle response
}
```

### Parameters

| Parameter                                                                                                          | Type                                                                                                               | Required                                                                                                           | Description                                                                                                        |
| ------------------------------------------------------------------------------------------------------------------ | ------------------------------------------------------------------------------------------------------------------ | ------------------------------------------------------------------------------------------------------------------ | ------------------------------------------------------------------------------------------------------------------ |
| `request`                                                                                                          | [CreatePrivateLobbyRequest](../../models/operations/CreatePrivateLobbyRequest.md)                                  | :heavy_check_mark:                                                                                                 | The request object to use for the request.                                                                         |
| `security`                                                                                                         | [HathoraCloud.Models.Operations.CreatePrivateLobbySecurity](../../models/operations/CreatePrivateLobbySecurity.md) | :heavy_check_mark:                                                                                                 | The security requirements to use for the request.                                                                  |


### Response

**[CreatePrivateLobbyResponse](../../models/operations/CreatePrivateLobbyResponse.md)**


## ~~CreatePublicLobby~~

> :warning: **DEPRECATED**: This will be removed in a future release, please migrate away from it as soon as possible.

### Example Usage

```csharp
using HathoraCloud;
using HathoraCloud.Models.Operations;
using HathoraCloud.Models.Shared;

var sdk = new HathoraCloudSDK(
    appId: "app-af469a92-5b45-4565-b3c4-b79878de67d2"
);

using(var res = await sdk.LobbyV2.CreatePublicLobbyAsync(new CreatePublicLobbySecurity() {
    PlayerAuth = "",
}, new CreatePublicLobbyRequest() {
    RequestBody = new CreatePublicLobbyRequestBody() {
        InitialConfig = new LobbyInitialConfig() {},
        Region = Region.Sydney,
    },
    RoomId = "2swovpy1fnunu",
}))
{
    // handle response
}
```

### Parameters

| Parameter                                                                                                        | Type                                                                                                             | Required                                                                                                         | Description                                                                                                      |
| ---------------------------------------------------------------------------------------------------------------- | ---------------------------------------------------------------------------------------------------------------- | ---------------------------------------------------------------------------------------------------------------- | ---------------------------------------------------------------------------------------------------------------- |
| `request`                                                                                                        | [CreatePublicLobbyRequest](../../models/operations/CreatePublicLobbyRequest.md)                                  | :heavy_check_mark:                                                                                               | The request object to use for the request.                                                                       |
| `security`                                                                                                       | [HathoraCloud.Models.Operations.CreatePublicLobbySecurity](../../models/operations/CreatePublicLobbySecurity.md) | :heavy_check_mark:                                                                                               | The security requirements to use for the request.                                                                |


### Response

**[CreatePublicLobbyResponse](../../models/operations/CreatePublicLobbyResponse.md)**


## ~~GetLobbyInfo~~

Get details for a lobby.

> :warning: **DEPRECATED**: This will be removed in a future release, please migrate away from it as soon as possible.

### Example Usage

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

using(var res = await sdk.LobbyV2.GetLobbyInfoAsync(new GetLobbyInfoRequest() {
    RoomId = "2swovpy1fnunu",
}))
{
    // handle response
}
```

### Parameters

| Parameter                                                             | Type                                                                  | Required                                                              | Description                                                           |
| --------------------------------------------------------------------- | --------------------------------------------------------------------- | --------------------------------------------------------------------- | --------------------------------------------------------------------- |
| `request`                                                             | [GetLobbyInfoRequest](../../models/operations/GetLobbyInfoRequest.md) | :heavy_check_mark:                                                    | The request object to use for the request.                            |


### Response

**[GetLobbyInfoResponse](../../models/operations/GetLobbyInfoResponse.md)**


## ~~ListActivePublicLobbiesDeprecatedV2~~

Get all active lobbies for a an [application](https://hathora.dev/docs/concepts/hathora-entities#application). Filter by optionally passing in a `region`. Use this endpoint to display all public lobbies that a player can join in the game client.

> :warning: **DEPRECATED**: This will be removed in a future release, please migrate away from it as soon as possible.

### Example Usage

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

using(var res = await sdk.LobbyV2.ListActivePublicLobbiesDeprecatedV2Async(new ListActivePublicLobbiesDeprecatedV2Request() {}))
{
    // handle response
}
```

### Parameters

| Parameter                                                                                                           | Type                                                                                                                | Required                                                                                                            | Description                                                                                                         |
| ------------------------------------------------------------------------------------------------------------------- | ------------------------------------------------------------------------------------------------------------------- | ------------------------------------------------------------------------------------------------------------------- | ------------------------------------------------------------------------------------------------------------------- |
| `request`                                                                                                           | [ListActivePublicLobbiesDeprecatedV2Request](../../models/operations/ListActivePublicLobbiesDeprecatedV2Request.md) | :heavy_check_mark:                                                                                                  | The request object to use for the request.                                                                          |


### Response

**[ListActivePublicLobbiesDeprecatedV2Response](../../models/operations/ListActivePublicLobbiesDeprecatedV2Response.md)**


## ~~SetLobbyState~~

Set the state of a lobby. State is intended to be set by the server and must be smaller than 1MB. Use this endpoint to store match data like live player count to enforce max number of clients or persist end-game data (i.e. winner or final scores).

> :warning: **DEPRECATED**: This will be removed in a future release, please migrate away from it as soon as possible.

### Example Usage

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

using(var res = await sdk.LobbyV2.SetLobbyStateAsync(new SetLobbyStateRequest() {
    SetLobbyStateParams = new SetLobbyStateParams() {
        State = new SetLobbyStateParamsState() {},
    },
    RoomId = "2swovpy1fnunu",
}))
{
    // handle response
}
```

### Parameters

| Parameter                                                               | Type                                                                    | Required                                                                | Description                                                             |
| ----------------------------------------------------------------------- | ----------------------------------------------------------------------- | ----------------------------------------------------------------------- | ----------------------------------------------------------------------- |
| `request`                                                               | [SetLobbyStateRequest](../../models/operations/SetLobbyStateRequest.md) | :heavy_check_mark:                                                      | The request object to use for the request.                              |


### Response

**[SetLobbyStateResponse](../../models/operations/SetLobbyStateResponse.md)**

