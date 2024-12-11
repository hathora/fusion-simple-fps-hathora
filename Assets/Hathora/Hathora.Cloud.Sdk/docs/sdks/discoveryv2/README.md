# DiscoveryV2
(*DiscoveryV2*)

## Overview

Service that allows clients to directly ping all Hathora regions to get latency information

### Available Operations

* [GetPingServiceEndpoints](#getpingserviceendpoints) - Returns an array of all regions with a host and port that a client can directly ping. Open a websocket connection to `wss://<host>:<port>/ws` and send a packet. To calculate ping, measure the time it takes to get an echo packet back.

## GetPingServiceEndpoints

Returns an array of all regions with a host and port that a client can directly ping. Open a websocket connection to `wss://<host>:<port>/ws` and send a packet. To calculate ping, measure the time it takes to get an echo packet back.

### Example Usage

```csharp
using HathoraCloud;
using HathoraCloud.Models.Shared;

var sdk = new HathoraCloudSDK(appId: "app-af469a92-5b45-4565-b3c4-b79878de67d2");


using(var res = await sdk.DiscoveryV2.GetPingServiceEndpointsAsync())
{
    // handle response
}


```

### Response

**[GetPingServiceEndpointsResponse](../../Models/Operations/GetPingServiceEndpointsResponse.md)**

### Errors

| Error Object                            | Status Code                             | Content Type                            |
| --------------------------------------- | --------------------------------------- | --------------------------------------- |
| HathoraCloud.Models.Errors.SDKException | 4xx-5xx                                 | */*                                     |
