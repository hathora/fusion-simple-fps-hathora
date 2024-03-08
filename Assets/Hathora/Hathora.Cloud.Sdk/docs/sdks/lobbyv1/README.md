# LobbyV1
(*LobbyV1*)

## Overview

Deprecated. Use [LobbyV3](https://hathora.dev/api#tag/LobbyV3).

### Available Operations

* [~~CreatePrivateLobbyDeprecated~~](#createprivatelobbydeprecated) - :warning: **Deprecated**
* [~~CreatePublicLobbyDeprecated~~](#createpubliclobbydeprecated) - :warning: **Deprecated**
* [~~ListActivePublicLobbiesDeprecatedV1~~](#listactivepubliclobbiesdeprecatedv1) - :warning: **Deprecated**

## ~~CreatePrivateLobbyDeprecated~~

> :warning: **DEPRECATED**: This will be removed in a future release, please migrate away from it as soon as possible.

### Example Usage

```csharp
using HathoraCloud;
using HathoraCloud.Models.Operations;
using HathoraCloud.Models.Shared;

var sdk = new HathoraCloudSDK(
    appId: "app-af469a92-5b45-4565-b3c4-b79878de67d2"
);

using(var res = await sdk.LobbyV1.CreatePrivateLobbyDeprecatedAsync(new CreatePrivateLobbyDeprecatedSecurity() {
    PlayerAuth = "",
}, new CreatePrivateLobbyDeprecatedRequest() {}))
{
    // handle response
}
```

### Parameters

| Parameter                                                                                                                              | Type                                                                                                                                   | Required                                                                                                                               | Description                                                                                                                            |
| -------------------------------------------------------------------------------------------------------------------------------------- | -------------------------------------------------------------------------------------------------------------------------------------- | -------------------------------------------------------------------------------------------------------------------------------------- | -------------------------------------------------------------------------------------------------------------------------------------- |
| `request`                                                                                                                              | [CreatePrivateLobbyDeprecatedRequest](../../models/operations/CreatePrivateLobbyDeprecatedRequest.md)                                  | :heavy_check_mark:                                                                                                                     | The request object to use for the request.                                                                                             |
| `security`                                                                                                                             | [HathoraCloud.Models.Operations.CreatePrivateLobbyDeprecatedSecurity](../../models/operations/CreatePrivateLobbyDeprecatedSecurity.md) | :heavy_check_mark:                                                                                                                     | The security requirements to use for the request.                                                                                      |


### Response

**[CreatePrivateLobbyDeprecatedResponse](../../models/operations/CreatePrivateLobbyDeprecatedResponse.md)**


## ~~CreatePublicLobbyDeprecated~~

> :warning: **DEPRECATED**: This will be removed in a future release, please migrate away from it as soon as possible.

### Example Usage

```csharp
using HathoraCloud;
using HathoraCloud.Models.Operations;
using HathoraCloud.Models.Shared;

var sdk = new HathoraCloudSDK(
    appId: "app-af469a92-5b45-4565-b3c4-b79878de67d2"
);

using(var res = await sdk.LobbyV1.CreatePublicLobbyDeprecatedAsync(new CreatePublicLobbyDeprecatedSecurity() {
    PlayerAuth = "",
}, new CreatePublicLobbyDeprecatedRequest() {}))
{
    // handle response
}
```

### Parameters

| Parameter                                                                                                                            | Type                                                                                                                                 | Required                                                                                                                             | Description                                                                                                                          |
| ------------------------------------------------------------------------------------------------------------------------------------ | ------------------------------------------------------------------------------------------------------------------------------------ | ------------------------------------------------------------------------------------------------------------------------------------ | ------------------------------------------------------------------------------------------------------------------------------------ |
| `request`                                                                                                                            | [CreatePublicLobbyDeprecatedRequest](../../models/operations/CreatePublicLobbyDeprecatedRequest.md)                                  | :heavy_check_mark:                                                                                                                   | The request object to use for the request.                                                                                           |
| `security`                                                                                                                           | [HathoraCloud.Models.Operations.CreatePublicLobbyDeprecatedSecurity](../../models/operations/CreatePublicLobbyDeprecatedSecurity.md) | :heavy_check_mark:                                                                                                                   | The security requirements to use for the request.                                                                                    |


### Response

**[CreatePublicLobbyDeprecatedResponse](../../models/operations/CreatePublicLobbyDeprecatedResponse.md)**


## ~~ListActivePublicLobbiesDeprecatedV1~~

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

using(var res = await sdk.LobbyV1.ListActivePublicLobbiesDeprecatedV1Async(new ListActivePublicLobbiesDeprecatedV1Request() {}))
{
    // handle response
}
```

### Parameters

| Parameter                                                                                                           | Type                                                                                                                | Required                                                                                                            | Description                                                                                                         |
| ------------------------------------------------------------------------------------------------------------------- | ------------------------------------------------------------------------------------------------------------------- | ------------------------------------------------------------------------------------------------------------------- | ------------------------------------------------------------------------------------------------------------------- |
| `request`                                                                                                           | [ListActivePublicLobbiesDeprecatedV1Request](../../models/operations/ListActivePublicLobbiesDeprecatedV1Request.md) | :heavy_check_mark:                                                                                                  | The request object to use for the request.                                                                          |


### Response

**[ListActivePublicLobbiesDeprecatedV1Response](../../models/operations/ListActivePublicLobbiesDeprecatedV1Response.md)**

