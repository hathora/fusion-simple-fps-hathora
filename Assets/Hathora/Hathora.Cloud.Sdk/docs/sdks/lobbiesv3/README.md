<<<<<<<< HEAD:Assets/Hathora/Hathora.Cloud.Sdk/docs/sdks/lobbiesv3/README.md
# LobbiesV3
(*LobbiesV3*)
========
# LobbyV3SDK
(*LobbyV3SDK*)
>>>>>>>> d69f8f9ceaa2932e7b27d50d6633da753c68582d:Assets/Hathora/Hathora.Cloud.Sdk/docs/sdks/lobbyv3sdk/README.md

## Overview

### Available Operations

* [CreateLobby](#createlobby) - Create a new lobby for an [application](https://hathora.dev/docs/concepts/hathora-entities#application). A lobby object is a wrapper around a [room](https://hathora.dev/docs/concepts/hathora-entities#room) object. With a lobby, you get additional functionality like configuring the visibility of the room, managing the state of a match, and retrieving a list of public lobbies to display to players.
* [GetLobbyInfoByRoomId](#getlobbyinfobyroomid) - Get details for a lobby.
* [GetLobbyInfoByShortCode](#getlobbyinfobyshortcode) - Get details for a lobby. If 2 or more lobbies have the same `shortCode`, then the most recently created lobby will be returned.
* [ListActivePublicLobbies](#listactivepubliclobbies) - Get all active lobbies for a given [application](https://hathora.dev/docs/concepts/hathora-entities#application). Filter the array by optionally passing in a `region`. Use this endpoint to display all public lobbies that a player can join in the game client.

## CreateLobby

Create a new lobby for an [application](https://hathora.dev/docs/concepts/hathora-entities#application). A lobby object is a wrapper around a [room](https://hathora.dev/docs/concepts/hathora-entities#room) object. With a lobby, you get additional functionality like configuring the visibility of the room, managing the state of a match, and retrieving a list of public lobbies to display to players.

### Example Usage

```csharp
using HathoraCloud;
using HathoraCloud.Models.Operations;
using HathoraCloud.Models.Shared;

<<<<<<<< HEAD:Assets/Hathora/Hathora.Cloud.Sdk/docs/sdks/lobbiesv3/README.md
var sdk = new HathoraCloudSDK(appId: "app-af469a92-5b45-4565-b3c4-b79878de67d2");
========
var sdk = new HathoraCloudSDK(
    appId: "app-af469a92-5b45-4565-b3c4-b79878de67d2");
>>>>>>>> d69f8f9ceaa2932e7b27d50d6633da753c68582d:Assets/Hathora/Hathora.Cloud.Sdk/docs/sdks/lobbyv3sdk/README.md

CreateLobbyRequest req = new CreateLobbyRequest() {
    CreateLobbyV3Params = new CreateLobbyV3Params() {
        Region = Region.Seattle,
        RoomConfig = "{\"name\":\"my-room\"}",
        Visibility = LobbyVisibility.Private,
    },
    RoomId = "2swovpy1fnunu",
    ShortCode = "LFG4",
};

<<<<<<<< HEAD:Assets/Hathora/Hathora.Cloud.Sdk/docs/sdks/lobbiesv3/README.md

using(var res = await sdk.LobbiesV3.CreateLobbyAsync(
    security: new CreateLobbySecurity() {
        PlayerAuth = "<YOUR_BEARER_TOKEN_HERE>",
    },
    req))
========
using(var res = await sdk.LobbyV3SDK.CreateLobbyAsync(new CreateLobbySecurity() {
    PlayerAuth = "<YOUR_BEARER_TOKEN_HERE>",
}, req))
>>>>>>>> d69f8f9ceaa2932e7b27d50d6633da753c68582d:Assets/Hathora/Hathora.Cloud.Sdk/docs/sdks/lobbyv3sdk/README.md
{

    // handle response
}


```

### Parameters

| Parameter                                                                                            | Type                                                                                                 | Required                                                                                             | Description                                                                                          |
| ---------------------------------------------------------------------------------------------------- | ---------------------------------------------------------------------------------------------------- | ---------------------------------------------------------------------------------------------------- | ---------------------------------------------------------------------------------------------------- |
| `request`                                                                                            | [CreateLobbyRequest](../../Models/Operations/CreateLobbyRequest.md)                                  | :heavy_check_mark:                                                                                   | The request object to use for the request.                                                           |
| `security`                                                                                           | [HathoraCloud.Models.Operations.CreateLobbySecurity](../../models/operations/CreateLobbySecurity.md) | :heavy_check_mark:                                                                                   | The security requirements to use for the request.                                                    |

### Response

**[CreateLobbyResponse](../../Models/Operations/CreateLobbyResponse.md)**

### Errors

| Error Object                            | Status Code                             | Content Type                            |
| --------------------------------------- | --------------------------------------- | --------------------------------------- |
| HathoraCloud.Models.Errors.ApiError     | 400,401,402,404,422,429,500             | application/json                        |
| HathoraCloud.Models.Errors.SDKException | 4xx-5xx                                 | */*                                     |


## GetLobbyInfoByRoomId

Get details for a lobby.

### Example Usage

```csharp
using HathoraCloud;
using HathoraCloud.Models.Shared;
using HathoraCloud.Models.Operations;

<<<<<<<< HEAD:Assets/Hathora/Hathora.Cloud.Sdk/docs/sdks/lobbiesv3/README.md
var sdk = new HathoraCloudSDK(appId: "app-af469a92-5b45-4565-b3c4-b79878de67d2");
========
var sdk = new HathoraCloudSDK(
    security: new Security() {
        HathoraDevToken = "<YOUR_BEARER_TOKEN_HERE>",
    },
    appId: "app-af469a92-5b45-4565-b3c4-b79878de67d2");
>>>>>>>> d69f8f9ceaa2932e7b27d50d6633da753c68582d:Assets/Hathora/Hathora.Cloud.Sdk/docs/sdks/lobbyv3sdk/README.md

GetLobbyInfoByRoomIdRequest req = new GetLobbyInfoByRoomIdRequest() {
    RoomId = "2swovpy1fnunu",
};

<<<<<<<< HEAD:Assets/Hathora/Hathora.Cloud.Sdk/docs/sdks/lobbiesv3/README.md

using(var res = await sdk.LobbiesV3.GetLobbyInfoByRoomIdAsync(req))
========
using(var res = await sdk.LobbyV3SDK.GetLobbyInfoByRoomIdAsync(req))
>>>>>>>> d69f8f9ceaa2932e7b27d50d6633da753c68582d:Assets/Hathora/Hathora.Cloud.Sdk/docs/sdks/lobbyv3sdk/README.md
{

    // handle response
}


```

### Parameters

| Parameter                                                                             | Type                                                                                  | Required                                                                              | Description                                                                           |
| ------------------------------------------------------------------------------------- | ------------------------------------------------------------------------------------- | ------------------------------------------------------------------------------------- | ------------------------------------------------------------------------------------- |
| `request`                                                                             | [GetLobbyInfoByRoomIdRequest](../../Models/Operations/GetLobbyInfoByRoomIdRequest.md) | :heavy_check_mark:                                                                    | The request object to use for the request.                                            |
<<<<<<<< HEAD:Assets/Hathora/Hathora.Cloud.Sdk/docs/sdks/lobbiesv3/README.md
========

>>>>>>>> d69f8f9ceaa2932e7b27d50d6633da753c68582d:Assets/Hathora/Hathora.Cloud.Sdk/docs/sdks/lobbyv3sdk/README.md

### Response

**[GetLobbyInfoByRoomIdResponse](../../Models/Operations/GetLobbyInfoByRoomIdResponse.md)**

### Errors

| Error Object                            | Status Code                             | Content Type                            |
| --------------------------------------- | --------------------------------------- | --------------------------------------- |
| HathoraCloud.Models.Errors.ApiError     | 404,422,429                             | application/json                        |
| HathoraCloud.Models.Errors.SDKException | 4xx-5xx                                 | */*                                     |


## GetLobbyInfoByShortCode

Get details for a lobby. If 2 or more lobbies have the same `shortCode`, then the most recently created lobby will be returned.

### Example Usage

```csharp
using HathoraCloud;
using HathoraCloud.Models.Shared;
using HathoraCloud.Models.Operations;

<<<<<<<< HEAD:Assets/Hathora/Hathora.Cloud.Sdk/docs/sdks/lobbiesv3/README.md
var sdk = new HathoraCloudSDK(appId: "app-af469a92-5b45-4565-b3c4-b79878de67d2");
========
var sdk = new HathoraCloudSDK(
    security: new Security() {
        HathoraDevToken = "<YOUR_BEARER_TOKEN_HERE>",
    },
    appId: "app-af469a92-5b45-4565-b3c4-b79878de67d2");
>>>>>>>> d69f8f9ceaa2932e7b27d50d6633da753c68582d:Assets/Hathora/Hathora.Cloud.Sdk/docs/sdks/lobbyv3sdk/README.md

GetLobbyInfoByShortCodeRequest req = new GetLobbyInfoByShortCodeRequest() {
    ShortCode = "LFG4",
};

<<<<<<<< HEAD:Assets/Hathora/Hathora.Cloud.Sdk/docs/sdks/lobbiesv3/README.md

using(var res = await sdk.LobbiesV3.GetLobbyInfoByShortCodeAsync(req))
========
using(var res = await sdk.LobbyV3SDK.GetLobbyInfoByShortCodeAsync(req))
>>>>>>>> d69f8f9ceaa2932e7b27d50d6633da753c68582d:Assets/Hathora/Hathora.Cloud.Sdk/docs/sdks/lobbyv3sdk/README.md
{

    // handle response
}


```

### Parameters

| Parameter                                                                                   | Type                                                                                        | Required                                                                                    | Description                                                                                 |
| ------------------------------------------------------------------------------------------- | ------------------------------------------------------------------------------------------- | ------------------------------------------------------------------------------------------- | ------------------------------------------------------------------------------------------- |
| `request`                                                                                   | [GetLobbyInfoByShortCodeRequest](../../Models/Operations/GetLobbyInfoByShortCodeRequest.md) | :heavy_check_mark:                                                                          | The request object to use for the request.                                                  |
<<<<<<<< HEAD:Assets/Hathora/Hathora.Cloud.Sdk/docs/sdks/lobbiesv3/README.md
========

>>>>>>>> d69f8f9ceaa2932e7b27d50d6633da753c68582d:Assets/Hathora/Hathora.Cloud.Sdk/docs/sdks/lobbyv3sdk/README.md

### Response

**[GetLobbyInfoByShortCodeResponse](../../Models/Operations/GetLobbyInfoByShortCodeResponse.md)**

### Errors

| Error Object                            | Status Code                             | Content Type                            |
| --------------------------------------- | --------------------------------------- | --------------------------------------- |
| HathoraCloud.Models.Errors.ApiError     | 404,429                                 | application/json                        |
| HathoraCloud.Models.Errors.SDKException | 4xx-5xx                                 | */*                                     |


## ListActivePublicLobbies

Get all active lobbies for a given [application](https://hathora.dev/docs/concepts/hathora-entities#application). Filter the array by optionally passing in a `region`. Use this endpoint to display all public lobbies that a player can join in the game client.

### Example Usage

```csharp
using HathoraCloud;
using HathoraCloud.Models.Shared;
using HathoraCloud.Models.Operations;

<<<<<<<< HEAD:Assets/Hathora/Hathora.Cloud.Sdk/docs/sdks/lobbiesv3/README.md
var sdk = new HathoraCloudSDK(appId: "app-af469a92-5b45-4565-b3c4-b79878de67d2");

ListActivePublicLobbiesRequest req = new ListActivePublicLobbiesRequest() {};


using(var res = await sdk.LobbiesV3.ListActivePublicLobbiesAsync(req))
========
var sdk = new HathoraCloudSDK(
    security: new Security() {
        HathoraDevToken = "<YOUR_BEARER_TOKEN_HERE>",
    },
    appId: "app-af469a92-5b45-4565-b3c4-b79878de67d2");

ListActivePublicLobbiesRequest req = new ListActivePublicLobbiesRequest() {};

using(var res = await sdk.LobbyV3SDK.ListActivePublicLobbiesAsync(req))
>>>>>>>> d69f8f9ceaa2932e7b27d50d6633da753c68582d:Assets/Hathora/Hathora.Cloud.Sdk/docs/sdks/lobbyv3sdk/README.md
{

    // handle response
}


```

### Parameters

| Parameter                                                                                   | Type                                                                                        | Required                                                                                    | Description                                                                                 |
| ------------------------------------------------------------------------------------------- | ------------------------------------------------------------------------------------------- | ------------------------------------------------------------------------------------------- | ------------------------------------------------------------------------------------------- |
| `request`                                                                                   | [ListActivePublicLobbiesRequest](../../Models/Operations/ListActivePublicLobbiesRequest.md) | :heavy_check_mark:                                                                          | The request object to use for the request.                                                  |
<<<<<<<< HEAD:Assets/Hathora/Hathora.Cloud.Sdk/docs/sdks/lobbiesv3/README.md
========

>>>>>>>> d69f8f9ceaa2932e7b27d50d6633da753c68582d:Assets/Hathora/Hathora.Cloud.Sdk/docs/sdks/lobbyv3sdk/README.md

### Response

**[ListActivePublicLobbiesResponse](../../Models/Operations/ListActivePublicLobbiesResponse.md)**

### Errors

| Error Object                            | Status Code                             | Content Type                            |
| --------------------------------------- | --------------------------------------- | --------------------------------------- |
| HathoraCloud.Models.Errors.ApiError     | 401,429                                 | application/json                        |
| HathoraCloud.Models.Errors.SDKException | 4xx-5xx                                 | */*                                     |
