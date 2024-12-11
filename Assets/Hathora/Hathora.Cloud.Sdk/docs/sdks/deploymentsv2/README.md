# DeploymentsV2
(*DeploymentsV2*)

## Overview

### Available Operations

* [CreateDeploymentV2Deprecated](#createdeploymentv2deprecated) - Create a new [deployment](https://hathora.dev/docs/concepts/hathora-entities#deployment). Creating a new deployment means all new rooms created will use the latest deployment configuration, but existing games in progress will not be affected.
* [GetDeploymentInfoV2Deprecated](#getdeploymentinfov2deprecated) - Get details for a [deployment](https://hathora.dev/docs/concepts/hathora-entities#deployment).
* [GetDeploymentsV2Deprecated](#getdeploymentsv2deprecated) - Returns an array of [deployments](https://hathora.dev/docs/concepts/hathora-entities#deployment) for an [application](https://hathora.dev/docs/concepts/hathora-entities#application).
* [GetLatestDeploymentV2Deprecated](#getlatestdeploymentv2deprecated) - Get the latest [deployment](https://hathora.dev/docs/concepts/hathora-entities#deployment) for an [application](https://hathora.dev/docs/concepts/hathora-entities#application).

## CreateDeploymentV2Deprecated

Create a new [deployment](https://hathora.dev/docs/concepts/hathora-entities#deployment). Creating a new deployment means all new rooms created will use the latest deployment configuration, but existing games in progress will not be affected.

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

CreateDeploymentV2DeprecatedRequest req = new CreateDeploymentV2DeprecatedRequest() {
    DeploymentConfigV2 = new DeploymentConfigV2() {
        AdditionalContainerPorts = new List<ContainerPort>() {
            new ContainerPort() {
                Name = "default",
                Port = 8000,
                TransportType = TransportType.Tls,
            },
        },
        ContainerPort = 4000,
        Env = new List<DeploymentConfigV2Env>() {
            new DeploymentConfigV2Env() {
                Name = "EULA",
                Value = "TRUE",
            },
        },
        IdleTimeoutEnabled = false,
        RequestedCPU = 0.5D,
        RequestedMemoryMB = 1024D,
        RoomsPerProcess = 3,
        TransportType = TransportType.Tcp,
    },
    BuildId = 1,
};


using(var res = await sdk.DeploymentsV2.CreateDeploymentV2DeprecatedAsync(req))
{
    // handle response
}


```

### Parameters

| Parameter                                                                                             | Type                                                                                                  | Required                                                                                              | Description                                                                                           |
| ----------------------------------------------------------------------------------------------------- | ----------------------------------------------------------------------------------------------------- | ----------------------------------------------------------------------------------------------------- | ----------------------------------------------------------------------------------------------------- |
| `request`                                                                                             | [CreateDeploymentV2DeprecatedRequest](../../Models/Operations/CreateDeploymentV2DeprecatedRequest.md) | :heavy_check_mark:                                                                                    | The request object to use for the request.                                                            |

### Response

**[CreateDeploymentV2DeprecatedResponse](../../Models/Operations/CreateDeploymentV2DeprecatedResponse.md)**

### Errors

| Error Object                            | Status Code                             | Content Type                            |
| --------------------------------------- | --------------------------------------- | --------------------------------------- |
| HathoraCloud.Models.Errors.ApiError     | 400,401,404,422,429,500                 | application/json                        |
| HathoraCloud.Models.Errors.SDKException | 4xx-5xx                                 | */*                                     |


## GetDeploymentInfoV2Deprecated

Get details for a [deployment](https://hathora.dev/docs/concepts/hathora-entities#deployment).

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

GetDeploymentInfoV2DeprecatedRequest req = new GetDeploymentInfoV2DeprecatedRequest() {
    DeploymentId = 1,
};


using(var res = await sdk.DeploymentsV2.GetDeploymentInfoV2DeprecatedAsync(req))
{
    // handle response
}


```

### Parameters

| Parameter                                                                                               | Type                                                                                                    | Required                                                                                                | Description                                                                                             |
| ------------------------------------------------------------------------------------------------------- | ------------------------------------------------------------------------------------------------------- | ------------------------------------------------------------------------------------------------------- | ------------------------------------------------------------------------------------------------------- |
| `request`                                                                                               | [GetDeploymentInfoV2DeprecatedRequest](../../Models/Operations/GetDeploymentInfoV2DeprecatedRequest.md) | :heavy_check_mark:                                                                                      | The request object to use for the request.                                                              |

### Response

**[GetDeploymentInfoV2DeprecatedResponse](../../Models/Operations/GetDeploymentInfoV2DeprecatedResponse.md)**

### Errors

| Error Object                            | Status Code                             | Content Type                            |
| --------------------------------------- | --------------------------------------- | --------------------------------------- |
| HathoraCloud.Models.Errors.ApiError     | 401,404,429                             | application/json                        |
| HathoraCloud.Models.Errors.SDKException | 4xx-5xx                                 | */*                                     |


## GetDeploymentsV2Deprecated

Returns an array of [deployments](https://hathora.dev/docs/concepts/hathora-entities#deployment) for an [application](https://hathora.dev/docs/concepts/hathora-entities#application).

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

GetDeploymentsV2DeprecatedRequest req = new GetDeploymentsV2DeprecatedRequest() {};


using(var res = await sdk.DeploymentsV2.GetDeploymentsV2DeprecatedAsync(req))
{
    // handle response
}


```

### Parameters

| Parameter                                                                                         | Type                                                                                              | Required                                                                                          | Description                                                                                       |
| ------------------------------------------------------------------------------------------------- | ------------------------------------------------------------------------------------------------- | ------------------------------------------------------------------------------------------------- | ------------------------------------------------------------------------------------------------- |
| `request`                                                                                         | [GetDeploymentsV2DeprecatedRequest](../../Models/Operations/GetDeploymentsV2DeprecatedRequest.md) | :heavy_check_mark:                                                                                | The request object to use for the request.                                                        |

### Response

**[GetDeploymentsV2DeprecatedResponse](../../Models/Operations/GetDeploymentsV2DeprecatedResponse.md)**

### Errors

| Error Object                            | Status Code                             | Content Type                            |
| --------------------------------------- | --------------------------------------- | --------------------------------------- |
| HathoraCloud.Models.Errors.ApiError     | 401,404,429                             | application/json                        |
| HathoraCloud.Models.Errors.SDKException | 4xx-5xx                                 | */*                                     |


## GetLatestDeploymentV2Deprecated

Get the latest [deployment](https://hathora.dev/docs/concepts/hathora-entities#deployment) for an [application](https://hathora.dev/docs/concepts/hathora-entities#application).

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

GetLatestDeploymentV2DeprecatedRequest req = new GetLatestDeploymentV2DeprecatedRequest() {};


using(var res = await sdk.DeploymentsV2.GetLatestDeploymentV2DeprecatedAsync(req))
{
    // handle response
}


```

### Parameters

| Parameter                                                                                                   | Type                                                                                                        | Required                                                                                                    | Description                                                                                                 |
| ----------------------------------------------------------------------------------------------------------- | ----------------------------------------------------------------------------------------------------------- | ----------------------------------------------------------------------------------------------------------- | ----------------------------------------------------------------------------------------------------------- |
| `request`                                                                                                   | [GetLatestDeploymentV2DeprecatedRequest](../../Models/Operations/GetLatestDeploymentV2DeprecatedRequest.md) | :heavy_check_mark:                                                                                          | The request object to use for the request.                                                                  |

### Response

**[GetLatestDeploymentV2DeprecatedResponse](../../Models/Operations/GetLatestDeploymentV2DeprecatedResponse.md)**

### Errors

| Error Object                            | Status Code                             | Content Type                            |
| --------------------------------------- | --------------------------------------- | --------------------------------------- |
| HathoraCloud.Models.Errors.ApiError     | 401,404,422,429                         | application/json                        |
| HathoraCloud.Models.Errors.SDKException | 4xx-5xx                                 | */*                                     |
