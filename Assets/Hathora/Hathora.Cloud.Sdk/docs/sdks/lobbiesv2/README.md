# LobbiesV2
(*LobbiesV2*)

## Overview

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

var sdk = new HathoraCloudSDK(appId: "app-af469a92-5b45-4565-b3c4-b79878de67d2");

CreateLobbyDeprecatedRequest req = new CreateLobbyDeprecatedRequest() {
    CreateLobbyParams = new CreateLobbyParams() {
        InitialConfig = "<value>",
        Region = Region.Tokyo,
        Visibility = LobbyVisibility.Private,
    },
    RoomId = "2swovpy1fnunu",
};


using(var res = await sdk.LobbiesV2.CreateLobbyDeprecatedAsync(
    security: new CreateLobbyDeprecatedSecurity() {
        PlayerAuth = "<YOUR_BEARER_TOKEN_HERE>",
    },
    req))
{
    // handle response
}


```

### Parameters

| Parameter                                                                                                                | Type                                                                                                                     | Required                                                                                                                 | Description                                                                                                              |
| ------------------------------------------------------------------------------------------------------------------------ | ------------------------------------------------------------------------------------------------------------------------ | ------------------------------------------------------------------------------------------------------------------------ | ------------------------------------------------------------------------------------------------------------------------ |
| `request`                                                                                                                | [CreateLobbyDeprecatedRequest](../../Models/Operations/CreateLobbyDeprecatedRequest.md)                                  | :heavy_check_mark:                                                                                                       | The request object to use for the request.                                                                               |
| `security`                                                                                                               | [HathoraCloud.Models.Operations.CreateLobbyDeprecatedSecurity](../../models/operations/CreateLobbyDeprecatedSecurity.md) | :heavy_check_mark:                                                                                                       | The security requirements to use for the request.                                                                        |

### Response

**[CreateLobbyDeprecatedResponse](../../Models/Operations/CreateLobbyDeprecatedResponse.md)**

### Errors

| Error Object                            | Status Code                             | Content Type                            |
| --------------------------------------- | --------------------------------------- | --------------------------------------- |
| HathoraCloud.Models.Errors.ApiError     | 400,401,402,404,422,429,500             | application/json                        |
| HathoraCloud.Models.Errors.SDKException | 4xx-5xx                                 | */*                                     |


## ~~CreateLocalLobby~~

> :warning: **DEPRECATED**: This will be removed in a future release, please migrate away from it as soon as possible.

### Example Usage

```csharp
using HathoraCloud;
using HathoraCloud.Models.Operations;
using HathoraCloud.Models.Shared;

var sdk = new HathoraCloudSDK(appId: "app-af469a92-5b45-4565-b3c4-b79878de67d2");

CreateLocalLobbyRequest req = new CreateLocalLobbyRequest() {
    RequestBody = new CreateLocalLobbyRequestBody() {
        InitialConfig = "<value>",
        Region = Region.SaoPaulo,
    },
    RoomId = "2swovpy1fnunu",
};


using(var res = await sdk.LobbiesV2.CreateLocalLobbyAsync(
    security: new CreateLocalLobbySecurity() {
        PlayerAuth = "<YOUR_BEARER_TOKEN_HERE>",
    },
    req))
{
    // handle response
}


```

### Parameters

| Parameter                                                                                                      | Type                                                                                                           | Required                                                                                                       | Description                                                                                                    |
| -------------------------------------------------------------------------------------------------------------- | -------------------------------------------------------------------------------------------------------------- | -------------------------------------------------------------------------------------------------------------- | -------------------------------------------------------------------------------------------------------------- |
| `request`                                                                                                      | [CreateLocalLobbyRequest](../../Models/Operations/CreateLocalLobbyRequest.md)                                  | :heavy_check_mark:                                                                                             | The request object to use for the request.                                                                     |
| `security`                                                                                                     | [HathoraCloud.Models.Operations.CreateLocalLobbySecurity](../../models/operations/CreateLocalLobbySecurity.md) | :heavy_check_mark:                                                                                             | The security requirements to use for the request.                                                              |

### Response

**[CreateLocalLobbyResponse](../../Models/Operations/CreateLocalLobbyResponse.md)**

### Errors

| Error Object                            | Status Code                             | Content Type                            |
| --------------------------------------- | --------------------------------------- | --------------------------------------- |
| HathoraCloud.Models.Errors.ApiError     | 400,401,402,404,422,429,500             | application/json                        |
| HathoraCloud.Models.Errors.SDKException | 4xx-5xx                                 | */*                                     |


## ~~CreatePrivateLobby~~

> :warning: **DEPRECATED**: This will be removed in a future release, please migrate away from it as soon as possible.

### Example Usage

```csharp
using HathoraCloud;
using HathoraCloud.Models.Operations;
using HathoraCloud.Models.Shared;

var sdk = new HathoraCloudSDK(appId: "app-af469a92-5b45-4565-b3c4-b79878de67d2");

CreatePrivateLobbyRequest req = new CreatePrivateLobbyRequest() {
    RequestBody = new CreatePrivateLobbyRequestBody() {
        InitialConfig = "<value>",
        Region = Region.Chicago,
    },
    RoomId = "2swovpy1fnunu",
};


using(var res = await sdk.LobbiesV2.CreatePrivateLobbyAsync(
    security: new CreatePrivateLobbySecurity() {
        PlayerAuth = "<YOUR_BEARER_TOKEN_HERE>",
    },
    req))
{
    // handle response
}


```

### Parameters

| Parameter                                                                                                          | Type                                                                                                               | Required                                                                                                           | Description                                                                                                        |
| ------------------------------------------------------------------------------------------------------------------ | ------------------------------------------------------------------------------------------------------------------ | ------------------------------------------------------------------------------------------------------------------ | ------------------------------------------------------------------------------------------------------------------ |
| `request`                                                                                                          | [CreatePrivateLobbyRequest](../../Models/Operations/CreatePrivateLobbyRequest.md)                                  | :heavy_check_mark:                                                                                                 | The request object to use for the request.                                                                         |
| `security`                                                                                                         | [HathoraCloud.Models.Operations.CreatePrivateLobbySecurity](../../models/operations/CreatePrivateLobbySecurity.md) | :heavy_check_mark:                                                                                                 | The security requirements to use for the request.                                                                  |

### Response

**[CreatePrivateLobbyResponse](../../Models/Operations/CreatePrivateLobbyResponse.md)**

### Errors

| Error Object                            | Status Code                             | Content Type                            |
| --------------------------------------- | --------------------------------------- | --------------------------------------- |
| HathoraCloud.Models.Errors.ApiError     | 400,401,402,404,422,429,500             | application/json                        |
| HathoraCloud.Models.Errors.SDKException | 4xx-5xx                                 | */*                                     |


## ~~CreatePublicLobby~~

> :warning: **DEPRECATED**: This will be removed in a future release, please migrate away from it as soon as possible.

### Example Usage

```csharp
using HathoraCloud;
using HathoraCloud.Models.Operations;
using HathoraCloud.Models.Shared;

var sdk = new HathoraCloudSDK(appId: "app-af469a92-5b45-4565-b3c4-b79878de67d2");

CreatePublicLobbyRequest req = new CreatePublicLobbyRequest() {
    RequestBody = new CreatePublicLobbyRequestBody() {
        InitialConfig = "<value>",
        Region = Region.SaoPaulo,
    },
    RoomId = "2swovpy1fnunu",
};


using(var res = await sdk.LobbiesV2.CreatePublicLobbyAsync(
    security: new CreatePublicLobbySecurity() {
        PlayerAuth = "<YOUR_BEARER_TOKEN_HERE>",
    },
    req))
{
    // handle response
}


```

### Parameters

| Parameter                                                                                                        | Type                                                                                                             | Required                                                                                                         | Description                                                                                                      |
| ---------------------------------------------------------------------------------------------------------------- | ---------------------------------------------------------------------------------------------------------------- | ---------------------------------------------------------------------------------------------------------------- | ---------------------------------------------------------------------------------------------------------------- |
| `request`                                                                                                        | [CreatePublicLobbyRequest](../../Models/Operations/CreatePublicLobbyRequest.md)                                  | :heavy_check_mark:                                                                                               | The request object to use for the request.                                                                       |
| `security`                                                                                                       | [HathoraCloud.Models.Operations.CreatePublicLobbySecurity](../../models/operations/CreatePublicLobbySecurity.md) | :heavy_check_mark:                                                                                               | The security requirements to use for the request.                                                                |

### Response

**[CreatePublicLobbyResponse](../../Models/Operations/CreatePublicLobbyResponse.md)**

### Errors

| Error Object                            | Status Code                             | Content Type                            |
| --------------------------------------- | --------------------------------------- | --------------------------------------- |
| HathoraCloud.Models.Errors.ApiError     | 400,401,402,404,422,429,500             | application/json                        |
| HathoraCloud.Models.Errors.SDKException | 4xx-5xx                                 | */*                                     |


## ~~GetLobbyInfo~~

Get details for a lobby.

> :warning: **DEPRECATED**: This will be removed in a future release, please migrate away from it as soon as possible.

### Example Usage

```csharp
using HathoraCloud;
using HathoraCloud.Models.Shared;
using HathoraCloud.Models.Operations;

var sdk = new HathoraCloudSDK(appId: "app-af469a92-5b45-4565-b3c4-b79878de67d2");

GetLobbyInfoRequest req = new GetLobbyInfoRequest() {
    RoomId = "2swovpy1fnunu",
};


using(var res = await sdk.LobbiesV2.GetLobbyInfoAsync(req))
{
    // handle response
}


```

### Parameters

| Parameter                                                             | Type                                                                  | Required                                                              | Description                                                           |
| --------------------------------------------------------------------- | --------------------------------------------------------------------- | --------------------------------------------------------------------- | --------------------------------------------------------------------- |
| `request`                                                             | [GetLobbyInfoRequest](../../Models/Operations/GetLobbyInfoRequest.md) | :heavy_check_mark:                                                    | The request object to use for the request.                            |

### Response

**[GetLobbyInfoResponse](../../Models/Operations/GetLobbyInfoResponse.md)**

### Errors

| Error Object                            | Status Code                             | Content Type                            |
| --------------------------------------- | --------------------------------------- | --------------------------------------- |
| HathoraCloud.Models.Errors.ApiError     | 404,429                                 | application/json                        |
| HathoraCloud.Models.Errors.SDKException | 4xx-5xx                                 | */*                                     |


## ~~ListActivePublicLobbiesDeprecatedV2~~

Get all active lobbies for a an [application](https://hathora.dev/docs/concepts/hathora-entities#application). Filter by optionally passing in a `region`. Use this endpoint to display all public lobbies that a player can join in the game client.

> :warning: **DEPRECATED**: This will be removed in a future release, please migrate away from it as soon as possible.

### Example Usage

```csharp
using HathoraCloud;
using HathoraCloud.Models.Shared;
using HathoraCloud.Models.Operations;

var sdk = new HathoraCloudSDK(appId: "app-af469a92-5b45-4565-b3c4-b79878de67d2");

ListActivePublicLobbiesDeprecatedV2Request req = new ListActivePublicLobbiesDeprecatedV2Request() {};


using(var res = await sdk.LobbiesV2.ListActivePublicLobbiesDeprecatedV2Async(req))
{
    // handle response
}


```

### Parameters

| Parameter                                                                                                           | Type                                                                                                                | Required                                                                                                            | Description                                                                                                         |
| ------------------------------------------------------------------------------------------------------------------- | ------------------------------------------------------------------------------------------------------------------- | ------------------------------------------------------------------------------------------------------------------- | ------------------------------------------------------------------------------------------------------------------- |
| `request`                                                                                                           | [ListActivePublicLobbiesDeprecatedV2Request](../../Models/Operations/ListActivePublicLobbiesDeprecatedV2Request.md) | :heavy_check_mark:                                                                                                  | The request object to use for the request.                                                                          |

### Response

**[ListActivePublicLobbiesDeprecatedV2Response](../../Models/Operations/ListActivePublicLobbiesDeprecatedV2Response.md)**

### Errors

| Error Object                            | Status Code                             | Content Type                            |
| --------------------------------------- | --------------------------------------- | --------------------------------------- |
| HathoraCloud.Models.Errors.ApiError     | 401,429                                 | application/json                        |
| HathoraCloud.Models.Errors.SDKException | 4xx-5xx                                 | */*                                     |


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
        HathoraDevToken = "<YOUR_BEARER_TOKEN_HERE>",
    },
    appId: "app-af469a92-5b45-4565-b3c4-b79878de67d2");

SetLobbyStateRequest req = new SetLobbyStateRequest() {
    SetLobbyStateParams = new SetLobbyStateParams() {
        State = "<value>",
    },
    RoomId = "2swovpy1fnunu",
};


using(var res = await sdk.LobbiesV2.SetLobbyStateAsync(req))
{
    // handle response
}


```

### Parameters

| Parameter                                                               | Type                                                                    | Required                                                                | Description                                                             |
| ----------------------------------------------------------------------- | ----------------------------------------------------------------------- | ----------------------------------------------------------------------- | ----------------------------------------------------------------------- |
| `request`                                                               | [SetLobbyStateRequest](../../Models/Operations/SetLobbyStateRequest.md) | :heavy_check_mark:                                                      | The request object to use for the request.                              |

### Response

**[SetLobbyStateResponse](../../Models/Operations/SetLobbyStateResponse.md)**

### Errors

| Error Object                            | Status Code                             | Content Type                            |
| --------------------------------------- | --------------------------------------- | --------------------------------------- |
| HathoraCloud.Models.Errors.ApiError     | 401,404,422,429                         | application/json                        |
| HathoraCloud.Models.Errors.SDKException | 4xx-5xx                                 | */*                                     |
