# BuildsV3
(*BuildsV3*)

## Overview

### Available Operations

* [CreateBuild](#createbuild) - Creates a new [build](https://hathora.dev/docs/concepts/hathora-entities#build) with optional `multipartUploadUrls` that can be used to upload larger builds in parts before calling `runBuild`. Responds with a `buildId` that you must pass to [`RunBuild()`](https://hathora.dev/api#tag/BuildV1/operation/RunBuild) to build the game server artifact. You can optionally pass in a `buildTag` to associate an external version with a build.
* [DeleteBuild](#deletebuild) - Delete a [build](https://hathora.dev/docs/concepts/hathora-entities#build). All associated metadata is deleted.
Be careful which builds you delete. This endpoint does not prevent you from deleting actively used builds.
Deleting a build that is actively build used by an app's deployment will cause failures when creating rooms.
* [GetBuild](#getbuild) - Get details for a [build](https://hathora.dev/docs/concepts/hathora-entities#build).
* [GetBuilds](#getbuilds) - Returns an array of [builds](https://hathora.dev/docs/concepts/hathora-entities#build) for an [application](https://hathora.dev/docs/concepts/hathora-entities#application).
* [RunBuild](#runbuild) - Builds a game server artifact from a tarball you provide. Pass in the `buildId` generated from [`CreateBuild()`](https://hathora.dev/api#tag/BuildV1/operation/CreateBuild).

## CreateBuild

Creates a new [build](https://hathora.dev/docs/concepts/hathora-entities#build) with optional `multipartUploadUrls` that can be used to upload larger builds in parts before calling `runBuild`. Responds with a `buildId` that you must pass to [`RunBuild()`](https://hathora.dev/api#tag/BuildV1/operation/RunBuild) to build the game server artifact. You can optionally pass in a `buildTag` to associate an external version with a build.

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

CreateBuildRequest req = new CreateBuildRequest() {
    CreateMultipartBuildParams = new CreateMultipartBuildParams() {
        BuildSizeInBytes = 5387.85D,
        BuildTag = "0.1.14-14c793",
    },
    OrgId = "org-6f706e83-0ec1-437a-9a46-7d4281eb2f39",
};


using(var res = await sdk.BuildsV3.CreateBuildAsync(req))
{
    // handle response
}


```

### Parameters

| Parameter                                                           | Type                                                                | Required                                                            | Description                                                         |
| ------------------------------------------------------------------- | ------------------------------------------------------------------- | ------------------------------------------------------------------- | ------------------------------------------------------------------- |
| `request`                                                           | [CreateBuildRequest](../../Models/Operations/CreateBuildRequest.md) | :heavy_check_mark:                                                  | The request object to use for the request.                          |

### Response

**[CreateBuildResponse](../../Models/Operations/CreateBuildResponse.md)**

### Errors

| Error Object                            | Status Code                             | Content Type                            |
| --------------------------------------- | --------------------------------------- | --------------------------------------- |
| HathoraCloud.Models.Errors.ApiError     | 400,401,404,422,429,500                 | application/json                        |
| HathoraCloud.Models.Errors.SDKException | 4xx-5xx                                 | */*                                     |


## DeleteBuild

Delete a [build](https://hathora.dev/docs/concepts/hathora-entities#build). All associated metadata is deleted.
Be careful which builds you delete. This endpoint does not prevent you from deleting actively used builds.
Deleting a build that is actively build used by an app's deployment will cause failures when creating rooms.

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

DeleteBuildRequest req = new DeleteBuildRequest() {
    BuildId = "<value>",
    OrgId = "org-6f706e83-0ec1-437a-9a46-7d4281eb2f39",
};


using(var res = await sdk.BuildsV3.DeleteBuildAsync(req))
{
    // handle response
}


```

### Parameters

| Parameter                                                           | Type                                                                | Required                                                            | Description                                                         |
| ------------------------------------------------------------------- | ------------------------------------------------------------------- | ------------------------------------------------------------------- | ------------------------------------------------------------------- |
| `request`                                                           | [DeleteBuildRequest](../../Models/Operations/DeleteBuildRequest.md) | :heavy_check_mark:                                                  | The request object to use for the request.                          |

### Response

**[DeleteBuildResponse](../../Models/Operations/DeleteBuildResponse.md)**

### Errors

| Error Object                            | Status Code                             | Content Type                            |
| --------------------------------------- | --------------------------------------- | --------------------------------------- |
| HathoraCloud.Models.Errors.ApiError     | 401,404,422,429,500                     | application/json                        |
| HathoraCloud.Models.Errors.SDKException | 4xx-5xx                                 | */*                                     |


## GetBuild

Get details for a [build](https://hathora.dev/docs/concepts/hathora-entities#build).

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

GetBuildRequest req = new GetBuildRequest() {
    BuildId = "<value>",
    OrgId = "org-6f706e83-0ec1-437a-9a46-7d4281eb2f39",
};


using(var res = await sdk.BuildsV3.GetBuildAsync(req))
{
    // handle response
}


```

### Parameters

| Parameter                                                     | Type                                                          | Required                                                      | Description                                                   |
| ------------------------------------------------------------- | ------------------------------------------------------------- | ------------------------------------------------------------- | ------------------------------------------------------------- |
| `request`                                                     | [GetBuildRequest](../../Models/Operations/GetBuildRequest.md) | :heavy_check_mark:                                            | The request object to use for the request.                    |

### Response

**[GetBuildResponse](../../Models/Operations/GetBuildResponse.md)**

### Errors

| Error Object                            | Status Code                             | Content Type                            |
| --------------------------------------- | --------------------------------------- | --------------------------------------- |
| HathoraCloud.Models.Errors.ApiError     | 401,404,429                             | application/json                        |
| HathoraCloud.Models.Errors.SDKException | 4xx-5xx                                 | */*                                     |


## GetBuilds

Returns an array of [builds](https://hathora.dev/docs/concepts/hathora-entities#build) for an [application](https://hathora.dev/docs/concepts/hathora-entities#application).

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

GetBuildsRequest req = new GetBuildsRequest() {
    OrgId = "org-6f706e83-0ec1-437a-9a46-7d4281eb2f39",
};


using(var res = await sdk.BuildsV3.GetBuildsAsync(req))
{
    // handle response
}


```

### Parameters

| Parameter                                                       | Type                                                            | Required                                                        | Description                                                     |
| --------------------------------------------------------------- | --------------------------------------------------------------- | --------------------------------------------------------------- | --------------------------------------------------------------- |
| `request`                                                       | [GetBuildsRequest](../../Models/Operations/GetBuildsRequest.md) | :heavy_check_mark:                                              | The request object to use for the request.                      |

### Response

**[GetBuildsResponse](../../Models/Operations/GetBuildsResponse.md)**

### Errors

| Error Object                            | Status Code                             | Content Type                            |
| --------------------------------------- | --------------------------------------- | --------------------------------------- |
| HathoraCloud.Models.Errors.ApiError     | 401,404,429                             | application/json                        |
| HathoraCloud.Models.Errors.SDKException | 4xx-5xx                                 | */*                                     |


## RunBuild

Builds a game server artifact from a tarball you provide. Pass in the `buildId` generated from [`CreateBuild()`](https://hathora.dev/api#tag/BuildV1/operation/CreateBuild).

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

RunBuildRequest req = new RunBuildRequest() {
    BuildId = "<value>",
    OrgId = "org-6f706e83-0ec1-437a-9a46-7d4281eb2f39",
};


using(var res = await sdk.BuildsV3.RunBuildAsync(req))
{
    // handle response
}


```

### Parameters

| Parameter                                                     | Type                                                          | Required                                                      | Description                                                   |
| ------------------------------------------------------------- | ------------------------------------------------------------- | ------------------------------------------------------------- | ------------------------------------------------------------- |
| `request`                                                     | [RunBuildRequest](../../Models/Operations/RunBuildRequest.md) | :heavy_check_mark:                                            | The request object to use for the request.                    |

### Response

**[RunBuildResponse](../../Models/Operations/RunBuildResponse.md)**

### Errors

| Error Object                            | Status Code                             | Content Type                            |
| --------------------------------------- | --------------------------------------- | --------------------------------------- |
| HathoraCloud.Models.Errors.ApiError     | 400,401,404,429,500                     | application/json                        |
| HathoraCloud.Models.Errors.SDKException | 4xx-5xx                                 | */*                                     |
