# DeploymentsV1
(*DeploymentsV1*)

## Overview

### Available Operations

* [~~CreateDeploymentV1Deprecated~~](#createdeploymentv1deprecated) - Create a new [deployment](https://hathora.dev/docs/concepts/hathora-entities#deployment). Creating a new deployment means all new rooms created will use the latest deployment configuration, but existing games in progress will not be affected. :warning: **Deprecated**
* [~~GetDeploymentInfoV1Deprecated~~](#getdeploymentinfov1deprecated) - Get details for a [deployment](https://hathora.dev/docs/concepts/hathora-entities#deployment). :warning: **Deprecated**
* [~~GetDeploymentsV1Deprecated~~](#getdeploymentsv1deprecated) - Returns an array of [deployments](https://hathora.dev/docs/concepts/hathora-entities#deployment) for an [application](https://hathora.dev/docs/concepts/hathora-entities#application). :warning: **Deprecated**
* [~~GetLatestDeploymentV1Deprecated~~](#getlatestdeploymentv1deprecated) - Get the latest [deployment](https://hathora.dev/docs/concepts/hathora-entities#deployment) for an [application](https://hathora.dev/docs/concepts/hathora-entities#application). :warning: **Deprecated**

## ~~CreateDeploymentV1Deprecated~~

Create a new [deployment](https://hathora.dev/docs/concepts/hathora-entities#deployment). Creating a new deployment means all new rooms created will use the latest deployment configuration, but existing games in progress will not be affected.

> :warning: **DEPRECATED**: This will be removed in a future release, please migrate away from it as soon as possible.

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

CreateDeploymentV1DeprecatedRequest req = new CreateDeploymentV1DeprecatedRequest() {
    DeploymentConfig = new DeploymentConfig() {
        AdditionalContainerPorts = new List<ContainerPort>() {
            new ContainerPort() {
                Name = "default",
                Port = 8000,
                TransportType = TransportType.Tcp,
            },
        },
        ContainerPort = 4000,
        Env = new List<Env>() {
            new Env() {
                Name = "EULA",
                Value = "TRUE",
            },
        },
        PlanName = PlanName.Tiny,
        RoomsPerProcess = 3,
        TransportType = TransportType.Tcp,
    },
    BuildId = 1,
};


using(var res = await sdk.DeploymentsV1.CreateDeploymentV1DeprecatedAsync(req))
{
    // handle response
}


```

### Parameters

| Parameter                                                                                             | Type                                                                                                  | Required                                                                                              | Description                                                                                           |
| ----------------------------------------------------------------------------------------------------- | ----------------------------------------------------------------------------------------------------- | ----------------------------------------------------------------------------------------------------- | ----------------------------------------------------------------------------------------------------- |
| `request`                                                                                             | [CreateDeploymentV1DeprecatedRequest](../../Models/Operations/CreateDeploymentV1DeprecatedRequest.md) | :heavy_check_mark:                                                                                    | The request object to use for the request.                                                            |

### Response

**[CreateDeploymentV1DeprecatedResponse](../../Models/Operations/CreateDeploymentV1DeprecatedResponse.md)**

### Errors

| Error Object                            | Status Code                             | Content Type                            |
| --------------------------------------- | --------------------------------------- | --------------------------------------- |
| HathoraCloud.Models.Errors.ApiError     | 400,401,404,422,429,500                 | application/json                        |
| HathoraCloud.Models.Errors.SDKException | 4xx-5xx                                 | */*                                     |


## ~~GetDeploymentInfoV1Deprecated~~

Get details for a [deployment](https://hathora.dev/docs/concepts/hathora-entities#deployment).

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

GetDeploymentInfoV1DeprecatedRequest req = new GetDeploymentInfoV1DeprecatedRequest() {
    DeploymentId = 1,
};


using(var res = await sdk.DeploymentsV1.GetDeploymentInfoV1DeprecatedAsync(req))
{
    // handle response
}


```

### Parameters

| Parameter                                                                                               | Type                                                                                                    | Required                                                                                                | Description                                                                                             |
| ------------------------------------------------------------------------------------------------------- | ------------------------------------------------------------------------------------------------------- | ------------------------------------------------------------------------------------------------------- | ------------------------------------------------------------------------------------------------------- |
| `request`                                                                                               | [GetDeploymentInfoV1DeprecatedRequest](../../Models/Operations/GetDeploymentInfoV1DeprecatedRequest.md) | :heavy_check_mark:                                                                                      | The request object to use for the request.                                                              |

### Response

**[GetDeploymentInfoV1DeprecatedResponse](../../Models/Operations/GetDeploymentInfoV1DeprecatedResponse.md)**

### Errors

| Error Object                            | Status Code                             | Content Type                            |
| --------------------------------------- | --------------------------------------- | --------------------------------------- |
| HathoraCloud.Models.Errors.ApiError     | 401,404,429                             | application/json                        |
| HathoraCloud.Models.Errors.SDKException | 4xx-5xx                                 | */*                                     |


## ~~GetDeploymentsV1Deprecated~~

Returns an array of [deployments](https://hathora.dev/docs/concepts/hathora-entities#deployment) for an [application](https://hathora.dev/docs/concepts/hathora-entities#application).

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

GetDeploymentsV1DeprecatedRequest req = new GetDeploymentsV1DeprecatedRequest() {};


using(var res = await sdk.DeploymentsV1.GetDeploymentsV1DeprecatedAsync(req))
{
    // handle response
}


```

### Parameters

| Parameter                                                                                         | Type                                                                                              | Required                                                                                          | Description                                                                                       |
| ------------------------------------------------------------------------------------------------- | ------------------------------------------------------------------------------------------------- | ------------------------------------------------------------------------------------------------- | ------------------------------------------------------------------------------------------------- |
| `request`                                                                                         | [GetDeploymentsV1DeprecatedRequest](../../Models/Operations/GetDeploymentsV1DeprecatedRequest.md) | :heavy_check_mark:                                                                                | The request object to use for the request.                                                        |

### Response

**[GetDeploymentsV1DeprecatedResponse](../../Models/Operations/GetDeploymentsV1DeprecatedResponse.md)**

### Errors

| Error Object                            | Status Code                             | Content Type                            |
| --------------------------------------- | --------------------------------------- | --------------------------------------- |
| HathoraCloud.Models.Errors.ApiError     | 401,404,429                             | application/json                        |
| HathoraCloud.Models.Errors.SDKException | 4xx-5xx                                 | */*                                     |


## ~~GetLatestDeploymentV1Deprecated~~

Get the latest [deployment](https://hathora.dev/docs/concepts/hathora-entities#deployment) for an [application](https://hathora.dev/docs/concepts/hathora-entities#application).

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

GetLatestDeploymentV1DeprecatedRequest req = new GetLatestDeploymentV1DeprecatedRequest() {};


using(var res = await sdk.DeploymentsV1.GetLatestDeploymentV1DeprecatedAsync(req))
{
    // handle response
}


```

### Parameters

| Parameter                                                                                                   | Type                                                                                                        | Required                                                                                                    | Description                                                                                                 |
| ----------------------------------------------------------------------------------------------------------- | ----------------------------------------------------------------------------------------------------------- | ----------------------------------------------------------------------------------------------------------- | ----------------------------------------------------------------------------------------------------------- |
| `request`                                                                                                   | [GetLatestDeploymentV1DeprecatedRequest](../../Models/Operations/GetLatestDeploymentV1DeprecatedRequest.md) | :heavy_check_mark:                                                                                          | The request object to use for the request.                                                                  |

### Response

**[GetLatestDeploymentV1DeprecatedResponse](../../Models/Operations/GetLatestDeploymentV1DeprecatedResponse.md)**

### Errors

| Error Object                            | Status Code                             | Content Type                            |
| --------------------------------------- | --------------------------------------- | --------------------------------------- |
| HathoraCloud.Models.Errors.ApiError     | 401,404,422,429                         | application/json                        |
| HathoraCloud.Models.Errors.SDKException | 4xx-5xx                                 | */*                                     |
