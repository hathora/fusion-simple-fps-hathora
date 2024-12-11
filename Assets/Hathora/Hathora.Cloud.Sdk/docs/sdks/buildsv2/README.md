# BuildsV2
(*BuildsV2*)

## Overview

### Available Operations

* [CreateBuildV2Deprecated](#createbuildv2deprecated) - Creates a new [build](https://hathora.dev/docs/concepts/hathora-entities#build). Responds with a `buildId` that you must pass to [`RunBuild()`](https://hathora.dev/api#tag/BuildV1/operation/RunBuild) to build the game server artifact. You can optionally pass in a `buildTag` to associate an external version with a build.
* [CreateBuildWithUploadUrlV2Deprecated](#createbuildwithuploadurlv2deprecated) - Creates a new [build](https://hathora.dev/docs/concepts/hathora-entities#build) with `uploadUrl` that can be used to upload the build to before calling `runBuild`. Responds with a `buildId` that you must pass to [`RunBuild()`](https://hathora.dev/api#tag/BuildV1/operation/RunBuild) to build the game server artifact. You can optionally pass in a `buildTag` to associate an external version with a build.
* [CreateWithMultipartUploadsV2Deprecated](#createwithmultipartuploadsv2deprecated) - Creates a new [build](https://hathora.dev/docs/concepts/hathora-entities#build) with optional `multipartUploadUrls` that can be used to upload larger builds in parts before calling `runBuild`. Responds with a `buildId` that you must pass to [`RunBuild()`](https://hathora.dev/api#tag/BuildV1/operation/RunBuild) to build the game server artifact. You can optionally pass in a `buildTag` to associate an external version with a build.
* [DeleteBuildV2Deprecated](#deletebuildv2deprecated) - Delete a [build](https://hathora.dev/docs/concepts/hathora-entities#build). All associated metadata is deleted.
* [GetBuildInfoV2Deprecated](#getbuildinfov2deprecated) - Get details for a [build](https://hathora.dev/docs/concepts/hathora-entities#build).
* [GetBuildsV2Deprecated](#getbuildsv2deprecated) - Returns an array of [builds](https://hathora.dev/docs/concepts/hathora-entities#build) for an [application](https://hathora.dev/docs/concepts/hathora-entities#application).
* [RunBuildV2Deprecated](#runbuildv2deprecated) - Builds a game server artifact from a tarball you provide. Pass in the `buildId` generated from [`CreateBuild()`](https://hathora.dev/api#tag/BuildV1/operation/CreateBuild).

## CreateBuildV2Deprecated

Creates a new [build](https://hathora.dev/docs/concepts/hathora-entities#build). Responds with a `buildId` that you must pass to [`RunBuild()`](https://hathora.dev/api#tag/BuildV1/operation/RunBuild) to build the game server artifact. You can optionally pass in a `buildTag` to associate an external version with a build.

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

CreateBuildV2DeprecatedRequest req = new CreateBuildV2DeprecatedRequest() {
    CreateBuildParams = new CreateBuildParams() {
        BuildTag = "0.1.14-14c793",
    },
};


using(var res = await sdk.BuildsV2.CreateBuildV2DeprecatedAsync(req))
{
    // handle response
}


```

### Parameters

| Parameter                                                                                   | Type                                                                                        | Required                                                                                    | Description                                                                                 |
| ------------------------------------------------------------------------------------------- | ------------------------------------------------------------------------------------------- | ------------------------------------------------------------------------------------------- | ------------------------------------------------------------------------------------------- |
| `request`                                                                                   | [CreateBuildV2DeprecatedRequest](../../Models/Operations/CreateBuildV2DeprecatedRequest.md) | :heavy_check_mark:                                                                          | The request object to use for the request.                                                  |

### Response

**[CreateBuildV2DeprecatedResponse](../../Models/Operations/CreateBuildV2DeprecatedResponse.md)**

### Errors

| Error Object                            | Status Code                             | Content Type                            |
| --------------------------------------- | --------------------------------------- | --------------------------------------- |
| HathoraCloud.Models.Errors.ApiError     | 401,404,429,500                         | application/json                        |
| HathoraCloud.Models.Errors.SDKException | 4xx-5xx                                 | */*                                     |


## CreateBuildWithUploadUrlV2Deprecated

Creates a new [build](https://hathora.dev/docs/concepts/hathora-entities#build) with `uploadUrl` that can be used to upload the build to before calling `runBuild`. Responds with a `buildId` that you must pass to [`RunBuild()`](https://hathora.dev/api#tag/BuildV1/operation/RunBuild) to build the game server artifact. You can optionally pass in a `buildTag` to associate an external version with a build.

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

CreateBuildWithUploadUrlV2DeprecatedRequest req = new CreateBuildWithUploadUrlV2DeprecatedRequest() {
    CreateBuildParams = new CreateBuildParams() {
        BuildTag = "0.1.14-14c793",
    },
};


using(var res = await sdk.BuildsV2.CreateBuildWithUploadUrlV2DeprecatedAsync(req))
{
    // handle response
}


```

### Parameters

| Parameter                                                                                                             | Type                                                                                                                  | Required                                                                                                              | Description                                                                                                           |
| --------------------------------------------------------------------------------------------------------------------- | --------------------------------------------------------------------------------------------------------------------- | --------------------------------------------------------------------------------------------------------------------- | --------------------------------------------------------------------------------------------------------------------- |
| `request`                                                                                                             | [CreateBuildWithUploadUrlV2DeprecatedRequest](../../Models/Operations/CreateBuildWithUploadUrlV2DeprecatedRequest.md) | :heavy_check_mark:                                                                                                    | The request object to use for the request.                                                                            |

### Response

**[CreateBuildWithUploadUrlV2DeprecatedResponse](../../Models/Operations/CreateBuildWithUploadUrlV2DeprecatedResponse.md)**

### Errors

| Error Object                            | Status Code                             | Content Type                            |
| --------------------------------------- | --------------------------------------- | --------------------------------------- |
| HathoraCloud.Models.Errors.ApiError     | 401,404,429,500                         | application/json                        |
| HathoraCloud.Models.Errors.SDKException | 4xx-5xx                                 | */*                                     |


## CreateWithMultipartUploadsV2Deprecated

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

CreateWithMultipartUploadsV2DeprecatedRequest req = new CreateWithMultipartUploadsV2DeprecatedRequest() {
    CreateMultipartBuildParams = new CreateMultipartBuildParams() {
        BuildSizeInBytes = 3146.66D,
        BuildTag = "0.1.14-14c793",
    },
};


using(var res = await sdk.BuildsV2.CreateWithMultipartUploadsV2DeprecatedAsync(req))
{
    // handle response
}


```

### Parameters

| Parameter                                                                                                                 | Type                                                                                                                      | Required                                                                                                                  | Description                                                                                                               |
| ------------------------------------------------------------------------------------------------------------------------- | ------------------------------------------------------------------------------------------------------------------------- | ------------------------------------------------------------------------------------------------------------------------- | ------------------------------------------------------------------------------------------------------------------------- |
| `request`                                                                                                                 | [CreateWithMultipartUploadsV2DeprecatedRequest](../../Models/Operations/CreateWithMultipartUploadsV2DeprecatedRequest.md) | :heavy_check_mark:                                                                                                        | The request object to use for the request.                                                                                |

### Response

**[CreateWithMultipartUploadsV2DeprecatedResponse](../../Models/Operations/CreateWithMultipartUploadsV2DeprecatedResponse.md)**

### Errors

| Error Object                            | Status Code                             | Content Type                            |
| --------------------------------------- | --------------------------------------- | --------------------------------------- |
| HathoraCloud.Models.Errors.ApiError     | 400,401,404,429,500                     | application/json                        |
| HathoraCloud.Models.Errors.SDKException | 4xx-5xx                                 | */*                                     |


## DeleteBuildV2Deprecated

Delete a [build](https://hathora.dev/docs/concepts/hathora-entities#build). All associated metadata is deleted.

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

DeleteBuildV2DeprecatedRequest req = new DeleteBuildV2DeprecatedRequest() {
    BuildId = 1,
};


using(var res = await sdk.BuildsV2.DeleteBuildV2DeprecatedAsync(req))
{
    // handle response
}


```

### Parameters

| Parameter                                                                                   | Type                                                                                        | Required                                                                                    | Description                                                                                 |
| ------------------------------------------------------------------------------------------- | ------------------------------------------------------------------------------------------- | ------------------------------------------------------------------------------------------- | ------------------------------------------------------------------------------------------- |
| `request`                                                                                   | [DeleteBuildV2DeprecatedRequest](../../Models/Operations/DeleteBuildV2DeprecatedRequest.md) | :heavy_check_mark:                                                                          | The request object to use for the request.                                                  |

### Response

**[DeleteBuildV2DeprecatedResponse](../../Models/Operations/DeleteBuildV2DeprecatedResponse.md)**

### Errors

| Error Object                            | Status Code                             | Content Type                            |
| --------------------------------------- | --------------------------------------- | --------------------------------------- |
| HathoraCloud.Models.Errors.ApiError     | 401,404,422,429,500                     | application/json                        |
| HathoraCloud.Models.Errors.SDKException | 4xx-5xx                                 | */*                                     |


## GetBuildInfoV2Deprecated

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

GetBuildInfoV2DeprecatedRequest req = new GetBuildInfoV2DeprecatedRequest() {
    BuildId = 1,
};


using(var res = await sdk.BuildsV2.GetBuildInfoV2DeprecatedAsync(req))
{
    // handle response
}


```

### Parameters

| Parameter                                                                                     | Type                                                                                          | Required                                                                                      | Description                                                                                   |
| --------------------------------------------------------------------------------------------- | --------------------------------------------------------------------------------------------- | --------------------------------------------------------------------------------------------- | --------------------------------------------------------------------------------------------- |
| `request`                                                                                     | [GetBuildInfoV2DeprecatedRequest](../../Models/Operations/GetBuildInfoV2DeprecatedRequest.md) | :heavy_check_mark:                                                                            | The request object to use for the request.                                                    |

### Response

**[GetBuildInfoV2DeprecatedResponse](../../Models/Operations/GetBuildInfoV2DeprecatedResponse.md)**

### Errors

| Error Object                            | Status Code                             | Content Type                            |
| --------------------------------------- | --------------------------------------- | --------------------------------------- |
| HathoraCloud.Models.Errors.ApiError     | 401,404,429                             | application/json                        |
| HathoraCloud.Models.Errors.SDKException | 4xx-5xx                                 | */*                                     |


## GetBuildsV2Deprecated

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

GetBuildsV2DeprecatedRequest req = new GetBuildsV2DeprecatedRequest() {};


using(var res = await sdk.BuildsV2.GetBuildsV2DeprecatedAsync(req))
{
    // handle response
}


```

### Parameters

| Parameter                                                                               | Type                                                                                    | Required                                                                                | Description                                                                             |
| --------------------------------------------------------------------------------------- | --------------------------------------------------------------------------------------- | --------------------------------------------------------------------------------------- | --------------------------------------------------------------------------------------- |
| `request`                                                                               | [GetBuildsV2DeprecatedRequest](../../Models/Operations/GetBuildsV2DeprecatedRequest.md) | :heavy_check_mark:                                                                      | The request object to use for the request.                                              |

### Response

**[GetBuildsV2DeprecatedResponse](../../Models/Operations/GetBuildsV2DeprecatedResponse.md)**

### Errors

| Error Object                            | Status Code                             | Content Type                            |
| --------------------------------------- | --------------------------------------- | --------------------------------------- |
| HathoraCloud.Models.Errors.ApiError     | 401,404,429                             | application/json                        |
| HathoraCloud.Models.Errors.SDKException | 4xx-5xx                                 | */*                                     |


## RunBuildV2Deprecated

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

RunBuildV2DeprecatedRequest req = new RunBuildV2DeprecatedRequest() {
    RequestBody = new RunBuildV2DeprecatedRequestBody() {},
    BuildId = 1,
};


using(var res = await sdk.BuildsV2.RunBuildV2DeprecatedAsync(req))
{
    // handle response
}


```

### Parameters

| Parameter                                                                             | Type                                                                                  | Required                                                                              | Description                                                                           |
| ------------------------------------------------------------------------------------- | ------------------------------------------------------------------------------------- | ------------------------------------------------------------------------------------- | ------------------------------------------------------------------------------------- |
| `request`                                                                             | [RunBuildV2DeprecatedRequest](../../Models/Operations/RunBuildV2DeprecatedRequest.md) | :heavy_check_mark:                                                                    | The request object to use for the request.                                            |

### Response

**[RunBuildV2DeprecatedResponse](../../Models/Operations/RunBuildV2DeprecatedResponse.md)**

### Errors

| Error Object                            | Status Code                             | Content Type                            |
| --------------------------------------- | --------------------------------------- | --------------------------------------- |
| HathoraCloud.Models.Errors.ApiError     | 400,401,404,429,500                     | application/json                        |
| HathoraCloud.Models.Errors.SDKException | 4xx-5xx                                 | */*                                     |
