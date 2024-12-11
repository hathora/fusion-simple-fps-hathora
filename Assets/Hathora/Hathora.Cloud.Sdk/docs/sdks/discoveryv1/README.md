# DiscoveryV1
(*DiscoveryV1*)

## Overview

Deprecated. Does not include latest Regions (missing Dallas region). Use [DiscoveryV2](https://hathora.dev/api#tag/DiscoveryV2).

### Available Operations

* [~~GetPingServiceEndpointsDeprecated~~](#getpingserviceendpointsdeprecated) - Returns an array of V1 regions with a host and port that a client can directly ping. Open a websocket connection to `wss://<host>:<port>/ws` and send a packet. To calculate ping, measure the time it takes to get an echo packet back. :warning: **Deprecated**

## ~~GetPingServiceEndpointsDeprecated~~

Returns an array of V1 regions with a host and port that a client can directly ping. Open a websocket connection to `wss://<host>:<port>/ws` and send a packet. To calculate ping, measure the time it takes to get an echo packet back.

> :warning: **DEPRECATED**: This will be removed in a future release, please migrate away from it as soon as possible.

### Example Usage

```csharp
using HathoraCloud;
using HathoraCloud.Models.Shared;

var sdk = new HathoraCloudSDK(appId: "app-af469a92-5b45-4565-b3c4-b79878de67d2");


using(var res = await sdk.DiscoveryV1.GetPingServiceEndpointsDeprecatedAsync())
{
    // handle response
}


```

### Response

**[GetPingServiceEndpointsDeprecatedResponse](../../Models/Operations/GetPingServiceEndpointsDeprecatedResponse.md)**

### Errors

| Error Object                            | Status Code                             | Content Type                            |
| --------------------------------------- | --------------------------------------- | --------------------------------------- |
| HathoraCloud.Models.Errors.SDKException | 4xx-5xx                                 | */*                                     |
