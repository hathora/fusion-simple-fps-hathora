# BuildV1
(*BuildV1*)

## Overview

Operations that allow you create and manage your [builds](https://hathora.dev/docs/concepts/hathora-entities#build).

### Available Operations

* [CreateBuild](#createbuild) - Creates a new [build](https://hathora.dev/docs/concepts/hathora-entities#build). Responds with a `buildId` that you must pass to [`RunBuild()`](https://hathora.dev/api#tag/BuildV1/operation/RunBuild) to build the game server artifact. You can optionally pass in a `buildTag` to associate an external version with a build.
* [DeleteBuild](#deletebuild) - Delete a [build](https://hathora.dev/docs/concepts/hathora-entities#build). All associated metadata is deleted.
* [GetBuildInfo](#getbuildinfo) - Get details for a [build](https://hathora.dev/docs/concepts/hathora-entities#build).
* [GetBuilds](#getbuilds) - Returns an array of [builds](https://hathora.dev/docs/concepts/hathora-entities#build) for an [application](https://hathora.dev/docs/concepts/hathora-entities#application).
* [RunBuild](#runbuild) - Builds a game server artifact from a tarball you provide. Pass in the `buildId` generated from [`CreateBuild()`](https://hathora.dev/api#tag/BuildV1/operation/CreateBuild).

## CreateBuild

Creates a new [build](https://hathora.dev/docs/concepts/hathora-entities#build). Responds with a `buildId` that you must pass to [`RunBuild()`](https://hathora.dev/api#tag/BuildV1/operation/RunBuild) to build the game server artifact. You can optionally pass in a `buildTag` to associate an external version with a build.

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

using(var res = await sdk.BuildV1.CreateBuildAsync(new CreateBuildRequest() {
    CreateBuildParams = new CreateBuildParams() {
        BuildTag = "0.1.14-14c793",
    },
}))
{
    // handle response
}
```

### Parameters

| Parameter                                                           | Type                                                                | Required                                                            | Description                                                         |
| ------------------------------------------------------------------- | ------------------------------------------------------------------- | ------------------------------------------------------------------- | ------------------------------------------------------------------- |
| `request`                                                           | [CreateBuildRequest](../../models/operations/CreateBuildRequest.md) | :heavy_check_mark:                                                  | The request object to use for the request.                          |


### Response

**[CreateBuildResponse](../../models/operations/CreateBuildResponse.md)**


## DeleteBuild

Delete a [build](https://hathora.dev/docs/concepts/hathora-entities#build). All associated metadata is deleted.

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

using(var res = await sdk.BuildV1.DeleteBuildAsync(new DeleteBuildRequest() {
    BuildId = 1,
}))
{
    // handle response
}
```

### Parameters

| Parameter                                                           | Type                                                                | Required                                                            | Description                                                         |
| ------------------------------------------------------------------- | ------------------------------------------------------------------- | ------------------------------------------------------------------- | ------------------------------------------------------------------- |
| `request`                                                           | [DeleteBuildRequest](../../models/operations/DeleteBuildRequest.md) | :heavy_check_mark:                                                  | The request object to use for the request.                          |


### Response

**[DeleteBuildResponse](../../models/operations/DeleteBuildResponse.md)**


## GetBuildInfo

Get details for a [build](https://hathora.dev/docs/concepts/hathora-entities#build).

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

using(var res = await sdk.BuildV1.GetBuildInfoAsync(new GetBuildInfoRequest() {
    BuildId = 1,
}))
{
    // handle response
}
```

### Parameters

| Parameter                                                             | Type                                                                  | Required                                                              | Description                                                           |
| --------------------------------------------------------------------- | --------------------------------------------------------------------- | --------------------------------------------------------------------- | --------------------------------------------------------------------- |
| `request`                                                             | [GetBuildInfoRequest](../../models/operations/GetBuildInfoRequest.md) | :heavy_check_mark:                                                    | The request object to use for the request.                            |


### Response

**[GetBuildInfoResponse](../../models/operations/GetBuildInfoResponse.md)**


## GetBuilds

Returns an array of [builds](https://hathora.dev/docs/concepts/hathora-entities#build) for an [application](https://hathora.dev/docs/concepts/hathora-entities#application).

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

using(var res = await sdk.BuildV1.GetBuildsAsync(new GetBuildsRequest() {}))
{
    // handle response
}
```

### Parameters

| Parameter                                                       | Type                                                            | Required                                                        | Description                                                     |
| --------------------------------------------------------------- | --------------------------------------------------------------- | --------------------------------------------------------------- | --------------------------------------------------------------- |
| `request`                                                       | [GetBuildsRequest](../../models/operations/GetBuildsRequest.md) | :heavy_check_mark:                                              | The request object to use for the request.                      |


### Response

**[GetBuildsResponse](../../models/operations/GetBuildsResponse.md)**


## RunBuild

Builds a game server artifact from a tarball you provide. Pass in the `buildId` generated from [`CreateBuild()`](https://hathora.dev/api#tag/BuildV1/operation/CreateBuild).

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

using(var res = await sdk.BuildV1.RunBuildAsync(new RunBuildRequest() {
    RequestBody = new RunBuildRequestBody() {
        File = new File() {
            Content = "0xcBBBDB7B76 as bytes <<<>>>",
            FileName = "times_mini.wav",
        },
    },
    BuildId = 1,
}))
{
    // handle response
}
```

### Parameters

| Parameter                                                     | Type                                                          | Required                                                      | Description                                                   |
| ------------------------------------------------------------- | ------------------------------------------------------------- | ------------------------------------------------------------- | ------------------------------------------------------------- |
| `request`                                                     | [RunBuildRequest](../../models/operations/RunBuildRequest.md) | :heavy_check_mark:                                            | The request object to use for the request.                    |


### Response

**[RunBuildResponse](../../models/operations/RunBuildResponse.md)**

