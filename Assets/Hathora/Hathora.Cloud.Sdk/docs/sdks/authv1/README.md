# AuthV1
(*AuthV1*)

## Overview

Operations that allow you to generate a Hathora-signed [JSON web token (JWT)](https://jwt.io/) for [player authentication](https://hathora.dev/docs/lobbies-and-matchmaking/auth-service).

### Available Operations

* [LoginAnonymous](#loginanonymous) - Returns a unique player token for an anonymous user.
* [LoginGoogle](#logingoogle) - Returns a unique player token using a Google-signed OIDC `idToken`.
* [LoginNickname](#loginnickname) - Returns a unique player token with a specified nickname for a user.

## LoginAnonymous

Returns a unique player token for an anonymous user.

### Example Usage

```csharp
using HathoraCloud;
using HathoraCloud.Models.Shared;
using HathoraCloud.Models.Operations;

var sdk = new HathoraCloudSDK(appId: "app-af469a92-5b45-4565-b3c4-b79878de67d2");

LoginAnonymousRequest req = new LoginAnonymousRequest() {};


using(var res = await sdk.AuthV1.LoginAnonymousAsync(req))
{
    // handle response
}


```

### Parameters

| Parameter                                                                 | Type                                                                      | Required                                                                  | Description                                                               |
| ------------------------------------------------------------------------- | ------------------------------------------------------------------------- | ------------------------------------------------------------------------- | ------------------------------------------------------------------------- |
| `request`                                                                 | [LoginAnonymousRequest](../../Models/Operations/LoginAnonymousRequest.md) | :heavy_check_mark:                                                        | The request object to use for the request.                                |

### Response

**[LoginAnonymousResponse](../../Models/Operations/LoginAnonymousResponse.md)**

### Errors

| Error Object                            | Status Code                             | Content Type                            |
| --------------------------------------- | --------------------------------------- | --------------------------------------- |
| HathoraCloud.Models.Errors.ApiError     | 404,429                                 | application/json                        |
| HathoraCloud.Models.Errors.SDKException | 4xx-5xx                                 | */*                                     |


## LoginGoogle

Returns a unique player token using a Google-signed OIDC `idToken`.

### Example Usage

```csharp
using HathoraCloud;
using HathoraCloud.Models.Shared;
using HathoraCloud.Models.Operations;

var sdk = new HathoraCloudSDK(appId: "app-af469a92-5b45-4565-b3c4-b79878de67d2");

LoginGoogleRequest req = new LoginGoogleRequest() {
    GoogleIdTokenObject = new GoogleIdTokenObject() {
        IdToken = "eyJhbGciOiJSUzI1NiIsImtpZCI6ImZkNDhhNzUxMzhkOWQ0OGYwYWE2MzVlZjU2OWM0ZTE5NmY3YWU4ZDYiLCJ0eXAiOiJKV1QifQ.eyJpc3MiOiJhY2NvdW50cy5nb29nbGUuY29tIiwiYXpwIjoiODQ4NDEyODI2Nzg4LW00bXNyYjZxNDRkbTJ1ZTNrZ3Z1aTBmcTdrZGE1NWxzLmFwcHMuZ29vZ2xldXNlcmNvbnRlbnQuY29tIiwiYXVkIjoiODQ4NDEyODI2Nzg4LW00bXNyYjZxNDRkbTJ1ZTNrZ3Z1aTBmcTdrZGE1NWxzLmFwcHMuZ29vZ2xldXNlcmNvbnRlbnQuY29tIiwic3ViIjoiMTE0NTQyMzMwNzI3MTU2MTMzNzc2IiwiZW1haWwiOiJocGFdkeivmeuzQGdtYWlsLmNvbSIsImVtYWlsX3ZlcmlmaWVkIjp0cnVlLCJhdF9oYXNoIjoidno1NGhhdTNxbnVR",
    },
};


using(var res = await sdk.AuthV1.LoginGoogleAsync(req))
{
    // handle response
}


```

### Parameters

| Parameter                                                           | Type                                                                | Required                                                            | Description                                                         |
| ------------------------------------------------------------------- | ------------------------------------------------------------------- | ------------------------------------------------------------------- | ------------------------------------------------------------------- |
| `request`                                                           | [LoginGoogleRequest](../../Models/Operations/LoginGoogleRequest.md) | :heavy_check_mark:                                                  | The request object to use for the request.                          |

### Response

**[LoginGoogleResponse](../../Models/Operations/LoginGoogleResponse.md)**

### Errors

| Error Object                            | Status Code                             | Content Type                            |
| --------------------------------------- | --------------------------------------- | --------------------------------------- |
| HathoraCloud.Models.Errors.ApiError     | 401,404,429                             | application/json                        |
| HathoraCloud.Models.Errors.SDKException | 4xx-5xx                                 | */*                                     |


## LoginNickname

Returns a unique player token with a specified nickname for a user.

### Example Usage

```csharp
using HathoraCloud;
using HathoraCloud.Models.Shared;
using HathoraCloud.Models.Operations;

var sdk = new HathoraCloudSDK(appId: "app-af469a92-5b45-4565-b3c4-b79878de67d2");

LoginNicknameRequest req = new LoginNicknameRequest() {
    NicknameObject = new NicknameObject() {
        Nickname = "squiddytwoshoes",
    },
};


using(var res = await sdk.AuthV1.LoginNicknameAsync(req))
{
    // handle response
}


```

### Parameters

| Parameter                                                               | Type                                                                    | Required                                                                | Description                                                             |
| ----------------------------------------------------------------------- | ----------------------------------------------------------------------- | ----------------------------------------------------------------------- | ----------------------------------------------------------------------- |
| `request`                                                               | [LoginNicknameRequest](../../Models/Operations/LoginNicknameRequest.md) | :heavy_check_mark:                                                      | The request object to use for the request.                              |

### Response

**[LoginNicknameResponse](../../Models/Operations/LoginNicknameResponse.md)**

### Errors

| Error Object                            | Status Code                             | Content Type                            |
| --------------------------------------- | --------------------------------------- | --------------------------------------- |
| HathoraCloud.Models.Errors.ApiError     | 404,429                                 | application/json                        |
| HathoraCloud.Models.Errors.SDKException | 4xx-5xx                                 | */*                                     |
