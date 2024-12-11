# RoomsV2
(*RoomsV2*)

## Overview

### Available Operations

* [CreateRoom](#createroom) - Create a new [room](https://hathora.dev/docs/concepts/hathora-entities#room) for an existing [application](https://hathora.dev/docs/concepts/hathora-entities#application). Poll the [`GetConnectionInfo()`](https://hathora.dev/api#tag/RoomV2/operation/GetConnectionInfo) endpoint to get connection details for an active room.
* [DestroyRoom](#destroyroom) - Destroy a [room](https://hathora.dev/docs/concepts/hathora-entities#room). All associated metadata is deleted.
* [GetActiveRoomsForProcess](#getactiveroomsforprocess) - Get all active [rooms](https://hathora.dev/docs/concepts/hathora-entities#room) for a given [process](https://hathora.dev/docs/concepts/hathora-entities#process).
* [GetConnectionInfo](#getconnectioninfo) - Poll this endpoint to get connection details to a [room](https://hathora.dev/docs/concepts/hathora-entities#room). Clients can call this endpoint without authentication.
* [GetInactiveRoomsForProcess](#getinactiveroomsforprocess) - Get all inactive [rooms](https://hathora.dev/docs/concepts/hathora-entities#room) for a given [process](https://hathora.dev/docs/concepts/hathora-entities#process).
* [GetRoomInfo](#getroominfo) - Retreive current and historical allocation data for a [room](https://hathora.dev/docs/concepts/hathora-entities#room).
* [~~SuspendRoomV2Deprecated~~](#suspendroomv2deprecated) - Suspend a [room](https://hathora.dev/docs/concepts/hathora-entities#room). The room is unallocated from the process but can be rescheduled later using the same `roomId`. :warning: **Deprecated**
* [UpdateRoomConfig](#updateroomconfig)

## CreateRoom

Create a new [room](https://hathora.dev/docs/concepts/hathora-entities#room) for an existing [application](https://hathora.dev/docs/concepts/hathora-entities#application). Poll the [`GetConnectionInfo()`](https://hathora.dev/api#tag/RoomV2/operation/GetConnectionInfo) endpoint to get connection details for an active room.

### Example Usage

```csharp
using HathoraCloud;
using HathoraCloud.Models.Shared;
using HathoraCloud.Models.Operations;
using System.Collections.Generic;

var sdk = new HathoraCloudSDK(
    security: new Security() {
        HathoraDevToken = "<YOUR_BEARER_TOKEN_HERE>",
    },
    appId: "app-af469a92-5b45-4565-b3c4-b79878de67d2");

CreateRoomRequest req = new CreateRoomRequest() {
    CreateRoomParams = new CreateRoomParams() {
        Region = Region.SaoPaulo,
        RoomConfig = "{\"name\":\"my-room\"}",
    },
    RoomId = "2swovpy1fnunu",
};


using(var res = await sdk.RoomsV2.CreateRoomAsync(req))
{
    // handle response
}


```

### Parameters

| Parameter                                                         | Type                                                              | Required                                                          | Description                                                       |
| ----------------------------------------------------------------- | ----------------------------------------------------------------- | ----------------------------------------------------------------- | ----------------------------------------------------------------- |
| `request`                                                         | [CreateRoomRequest](../../Models/Operations/CreateRoomRequest.md) | :heavy_check_mark:                                                | The request object to use for the request.                        |

### Response

**[CreateRoomResponse](../../Models/Operations/CreateRoomResponse.md)**

### Errors

| Error Object                            | Status Code                             | Content Type                            |
| --------------------------------------- | --------------------------------------- | --------------------------------------- |
| HathoraCloud.Models.Errors.ApiError     | 400,401,402,404,422,429,500             | application/json                        |
| HathoraCloud.Models.Errors.SDKException | 4xx-5xx                                 | */*                                     |


## DestroyRoom

Destroy a [room](https://hathora.dev/docs/concepts/hathora-entities#room). All associated metadata is deleted.

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

DestroyRoomRequest req = new DestroyRoomRequest() {
    RoomId = "2swovpy1fnunu",
};


using(var res = await sdk.RoomsV2.DestroyRoomAsync(req))
{
    // handle response
}


```

### Parameters

| Parameter                                                           | Type                                                                | Required                                                            | Description                                                         |
| ------------------------------------------------------------------- | ------------------------------------------------------------------- | ------------------------------------------------------------------- | ------------------------------------------------------------------- |
| `request`                                                           | [DestroyRoomRequest](../../Models/Operations/DestroyRoomRequest.md) | :heavy_check_mark:                                                  | The request object to use for the request.                          |

### Response

**[DestroyRoomResponse](../../Models/Operations/DestroyRoomResponse.md)**

### Errors

| Error Object                            | Status Code                             | Content Type                            |
| --------------------------------------- | --------------------------------------- | --------------------------------------- |
| HathoraCloud.Models.Errors.ApiError     | 401,404,429,500                         | application/json                        |
| HathoraCloud.Models.Errors.SDKException | 4xx-5xx                                 | */*                                     |


## GetActiveRoomsForProcess

Get all active [rooms](https://hathora.dev/docs/concepts/hathora-entities#room) for a given [process](https://hathora.dev/docs/concepts/hathora-entities#process).

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

GetActiveRoomsForProcessRequest req = new GetActiveRoomsForProcessRequest() {
    ProcessId = "cbfcddd2-0006-43ae-996c-995fff7bed2e",
};


using(var res = await sdk.RoomsV2.GetActiveRoomsForProcessAsync(req))
{
    // handle response
}


```

### Parameters

| Parameter                                                                                     | Type                                                                                          | Required                                                                                      | Description                                                                                   |
| --------------------------------------------------------------------------------------------- | --------------------------------------------------------------------------------------------- | --------------------------------------------------------------------------------------------- | --------------------------------------------------------------------------------------------- |
| `request`                                                                                     | [GetActiveRoomsForProcessRequest](../../Models/Operations/GetActiveRoomsForProcessRequest.md) | :heavy_check_mark:                                                                            | The request object to use for the request.                                                    |

### Response

**[GetActiveRoomsForProcessResponse](../../Models/Operations/GetActiveRoomsForProcessResponse.md)**

### Errors

| Error Object                            | Status Code                             | Content Type                            |
| --------------------------------------- | --------------------------------------- | --------------------------------------- |
| HathoraCloud.Models.Errors.ApiError     | 401,404,429                             | application/json                        |
| HathoraCloud.Models.Errors.SDKException | 4xx-5xx                                 | */*                                     |


## GetConnectionInfo

Poll this endpoint to get connection details to a [room](https://hathora.dev/docs/concepts/hathora-entities#room). Clients can call this endpoint without authentication.

### Example Usage

```csharp
using HathoraCloud;
using HathoraCloud.Models.Shared;
using HathoraCloud.Models.Operations;

var sdk = new HathoraCloudSDK(appId: "app-af469a92-5b45-4565-b3c4-b79878de67d2");

GetConnectionInfoRequest req = new GetConnectionInfoRequest() {
    RoomId = "2swovpy1fnunu",
};


using(var res = await sdk.RoomsV2.GetConnectionInfoAsync(req))
{
    // handle response
}


```

### Parameters

| Parameter                                                                       | Type                                                                            | Required                                                                        | Description                                                                     |
| ------------------------------------------------------------------------------- | ------------------------------------------------------------------------------- | ------------------------------------------------------------------------------- | ------------------------------------------------------------------------------- |
| `request`                                                                       | [GetConnectionInfoRequest](../../Models/Operations/GetConnectionInfoRequest.md) | :heavy_check_mark:                                                              | The request object to use for the request.                                      |

### Response

**[GetConnectionInfoResponse](../../Models/Operations/GetConnectionInfoResponse.md)**

### Errors

| Error Object                            | Status Code                             | Content Type                            |
| --------------------------------------- | --------------------------------------- | --------------------------------------- |
| HathoraCloud.Models.Errors.ApiError     | 400,402,404,422,429,500                 | application/json                        |
| HathoraCloud.Models.Errors.SDKException | 4xx-5xx                                 | */*                                     |


## GetInactiveRoomsForProcess

Get all inactive [rooms](https://hathora.dev/docs/concepts/hathora-entities#room) for a given [process](https://hathora.dev/docs/concepts/hathora-entities#process).

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

GetInactiveRoomsForProcessRequest req = new GetInactiveRoomsForProcessRequest() {
    ProcessId = "cbfcddd2-0006-43ae-996c-995fff7bed2e",
};


using(var res = await sdk.RoomsV2.GetInactiveRoomsForProcessAsync(req))
{
    // handle response
}


```

### Parameters

| Parameter                                                                                         | Type                                                                                              | Required                                                                                          | Description                                                                                       |
| ------------------------------------------------------------------------------------------------- | ------------------------------------------------------------------------------------------------- | ------------------------------------------------------------------------------------------------- | ------------------------------------------------------------------------------------------------- |
| `request`                                                                                         | [GetInactiveRoomsForProcessRequest](../../Models/Operations/GetInactiveRoomsForProcessRequest.md) | :heavy_check_mark:                                                                                | The request object to use for the request.                                                        |

### Response

**[GetInactiveRoomsForProcessResponse](../../Models/Operations/GetInactiveRoomsForProcessResponse.md)**

### Errors

| Error Object                            | Status Code                             | Content Type                            |
| --------------------------------------- | --------------------------------------- | --------------------------------------- |
| HathoraCloud.Models.Errors.ApiError     | 401,404,429                             | application/json                        |
| HathoraCloud.Models.Errors.SDKException | 4xx-5xx                                 | */*                                     |


## GetRoomInfo

Retreive current and historical allocation data for a [room](https://hathora.dev/docs/concepts/hathora-entities#room).

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

GetRoomInfoRequest req = new GetRoomInfoRequest() {
    RoomId = "2swovpy1fnunu",
};


using(var res = await sdk.RoomsV2.GetRoomInfoAsync(req))
{
    // handle response
}


```

### Parameters

| Parameter                                                           | Type                                                                | Required                                                            | Description                                                         |
| ------------------------------------------------------------------- | ------------------------------------------------------------------- | ------------------------------------------------------------------- | ------------------------------------------------------------------- |
| `request`                                                           | [GetRoomInfoRequest](../../Models/Operations/GetRoomInfoRequest.md) | :heavy_check_mark:                                                  | The request object to use for the request.                          |

### Response

**[GetRoomInfoResponse](../../Models/Operations/GetRoomInfoResponse.md)**

### Errors

| Error Object                            | Status Code                             | Content Type                            |
| --------------------------------------- | --------------------------------------- | --------------------------------------- |
| HathoraCloud.Models.Errors.ApiError     | 401,404,422,429                         | application/json                        |
| HathoraCloud.Models.Errors.SDKException | 4xx-5xx                                 | */*                                     |


## ~~SuspendRoomV2Deprecated~~

Suspend a [room](https://hathora.dev/docs/concepts/hathora-entities#room). The room is unallocated from the process but can be rescheduled later using the same `roomId`.

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

SuspendRoomV2DeprecatedRequest req = new SuspendRoomV2DeprecatedRequest() {
    RoomId = "2swovpy1fnunu",
};


using(var res = await sdk.RoomsV2.SuspendRoomV2DeprecatedAsync(req))
{
    // handle response
}


```

### Parameters

| Parameter                                                                                   | Type                                                                                        | Required                                                                                    | Description                                                                                 |
| ------------------------------------------------------------------------------------------- | ------------------------------------------------------------------------------------------- | ------------------------------------------------------------------------------------------- | ------------------------------------------------------------------------------------------- |
| `request`                                                                                   | [SuspendRoomV2DeprecatedRequest](../../Models/Operations/SuspendRoomV2DeprecatedRequest.md) | :heavy_check_mark:                                                                          | The request object to use for the request.                                                  |

### Response

**[SuspendRoomV2DeprecatedResponse](../../Models/Operations/SuspendRoomV2DeprecatedResponse.md)**

### Errors

| Error Object                            | Status Code                             | Content Type                            |
| --------------------------------------- | --------------------------------------- | --------------------------------------- |
| HathoraCloud.Models.Errors.ApiError     | 401,404,429,500                         | application/json                        |
| HathoraCloud.Models.Errors.SDKException | 4xx-5xx                                 | */*                                     |


## UpdateRoomConfig

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

UpdateRoomConfigRequest req = new UpdateRoomConfigRequest() {
    UpdateRoomConfigParams = new UpdateRoomConfigParams() {
        RoomConfig = "{\"name\":\"my-room\"}",
    },
    RoomId = "2swovpy1fnunu",
};


using(var res = await sdk.RoomsV2.UpdateRoomConfigAsync(req))
{
    // handle response
}


```

### Parameters

| Parameter                                                                     | Type                                                                          | Required                                                                      | Description                                                                   |
| ----------------------------------------------------------------------------- | ----------------------------------------------------------------------------- | ----------------------------------------------------------------------------- | ----------------------------------------------------------------------------- |
| `request`                                                                     | [UpdateRoomConfigRequest](../../Models/Operations/UpdateRoomConfigRequest.md) | :heavy_check_mark:                                                            | The request object to use for the request.                                    |

### Response

**[UpdateRoomConfigResponse](../../Models/Operations/UpdateRoomConfigResponse.md)**

### Errors

| Error Object                            | Status Code                             | Content Type                            |
| --------------------------------------- | --------------------------------------- | --------------------------------------- |
| HathoraCloud.Models.Errors.ApiError     | 401,404,429,500                         | application/json                        |
| HathoraCloud.Models.Errors.SDKException | 4xx-5xx                                 | */*                                     |
