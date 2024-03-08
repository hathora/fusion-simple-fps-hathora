# DeploymentV1
(*DeploymentV1*)

## Overview

Operations that allow you configure and manage an application's [build](https://hathora.dev/docs/concepts/hathora-entities#build) at runtime.

### Available Operations

* [CreateDeployment](#createdeployment) - Create a new [deployment](https://hathora.dev/docs/concepts/hathora-entities#deployment). Creating a new deployment means all new rooms created will use the latest deployment configuration, but existing games in progress will not be affected.
* [GetDeploymentInfo](#getdeploymentinfo) - Get details for a [deployment](https://hathora.dev/docs/concepts/hathora-entities#deployment).
* [GetDeployments](#getdeployments) - Returns an array of [deployments](https://hathora.dev/docs/concepts/hathora-entities#deployment) for an [application](https://hathora.dev/docs/concepts/hathora-entities#application).

## CreateDeployment

Create a new [deployment](https://hathora.dev/docs/concepts/hathora-entities#deployment). Creating a new deployment means all new rooms created will use the latest deployment configuration, but existing games in progress will not be affected.

### Example Usage

```csharp
using HathoraCloud;
using HathoraCloud.Models.Shared;
using HathoraCloud.Models.Operations;
using System.Collections.Generic;

var sdk = new HathoraCloudSDK(
    security: new Security() {
        HathoraDevToken = "",
    },
    appId: "app-af469a92-5b45-4565-b3c4-b79878de67d2"
);

using(var res = await sdk.DeploymentV1.CreateDeploymentAsync(new CreateDeploymentRequest() {
    DeploymentConfig = new DeploymentConfig() {
        AdditionalContainerPorts = new List<ContainerPort>() {
            new ContainerPort() {
                Name = "default",
                Port = 8000,
                TransportType = TransportType.Udp,
            },
        },
        ContainerPort = 4000,
        Env = new List<DeploymentConfigEnv>() {
            new DeploymentConfigEnv() {
                Name = "EULA",
                Value = "TRUE",
            },
        },
        PlanName = PlanName.Tiny,
        RoomsPerProcess = 3,
        TransportType = TransportType.Tcp,
    },
    BuildId = 1,
}))
{
    // handle response
}
```

### Parameters

| Parameter                                                                     | Type                                                                          | Required                                                                      | Description                                                                   |
| ----------------------------------------------------------------------------- | ----------------------------------------------------------------------------- | ----------------------------------------------------------------------------- | ----------------------------------------------------------------------------- |
| `request`                                                                     | [CreateDeploymentRequest](../../models/operations/CreateDeploymentRequest.md) | :heavy_check_mark:                                                            | The request object to use for the request.                                    |


### Response

**[CreateDeploymentResponse](../../models/operations/CreateDeploymentResponse.md)**


## GetDeploymentInfo

Get details for a [deployment](https://hathora.dev/docs/concepts/hathora-entities#deployment).

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

using(var res = await sdk.DeploymentV1.GetDeploymentInfoAsync(new GetDeploymentInfoRequest() {
    DeploymentId = 1,
}))
{
    // handle response
}
```

### Parameters

| Parameter                                                                       | Type                                                                            | Required                                                                        | Description                                                                     |
| ------------------------------------------------------------------------------- | ------------------------------------------------------------------------------- | ------------------------------------------------------------------------------- | ------------------------------------------------------------------------------- |
| `request`                                                                       | [GetDeploymentInfoRequest](../../models/operations/GetDeploymentInfoRequest.md) | :heavy_check_mark:                                                              | The request object to use for the request.                                      |


### Response

**[GetDeploymentInfoResponse](../../models/operations/GetDeploymentInfoResponse.md)**


## GetDeployments

Returns an array of [deployments](https://hathora.dev/docs/concepts/hathora-entities#deployment) for an [application](https://hathora.dev/docs/concepts/hathora-entities#application).

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

using(var res = await sdk.DeploymentV1.GetDeploymentsAsync(new GetDeploymentsRequest() {}))
{
    // handle response
}
```

### Parameters

| Parameter                                                                 | Type                                                                      | Required                                                                  | Description                                                               |
| ------------------------------------------------------------------------- | ------------------------------------------------------------------------- | ------------------------------------------------------------------------- | ------------------------------------------------------------------------- |
| `request`                                                                 | [GetDeploymentsRequest](../../models/operations/GetDeploymentsRequest.md) | :heavy_check_mark:                                                        | The request object to use for the request.                                |


### Response

**[GetDeploymentsResponse](../../models/operations/GetDeploymentsResponse.md)**

