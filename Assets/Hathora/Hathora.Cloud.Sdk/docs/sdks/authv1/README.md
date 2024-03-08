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

var sdk = new HathoraCloudSDK(
    security: new Security() {
        HathoraDevToken = "",
    },
    appId: "app-af469a92-5b45-4565-b3c4-b79878de67d2"
);

using(var res = await sdk.AuthV1.LoginAnonymousAsync(new LoginAnonymousRequest() {}))
{
    // handle response
}
```

### Parameters

| Parameter                                                                 | Type                                                                      | Required                                                                  | Description                                                               |
| ------------------------------------------------------------------------- | ------------------------------------------------------------------------- | ------------------------------------------------------------------------- | ------------------------------------------------------------------------- |
| `request`                                                                 | [LoginAnonymousRequest](../../models/operations/LoginAnonymousRequest.md) | :heavy_check_mark:                                                        | The request object to use for the request.                                |


### Response

**[LoginAnonymousResponse](../../models/operations/LoginAnonymousResponse.md)**


## LoginGoogle

Returns a unique player token using a Google-signed OIDC `idToken`.

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

using(var res = await sdk.AuthV1.LoginGoogleAsync(new HathoraCloud.Models.Operations.LoginGoogleRequest() {
    LoginGoogleRequest = new HathoraCloud.Models.Shared.LoginGoogleRequest() {
        IdToken = "eyJhbGciOiJSUzI1NiIsImtpZCI6ImZkNDhhNzUxMzhkOWQ0OGYwYWE2MzVlZjU2OWM0ZTE5NmY3YWU4ZDYiLCJ0eXAiOiJKV1QifQ.eyJpc3MiOiJhY2NvdW50cy5nb29nbGUuY29tIiwiYXpwIjoiODQ4NDEyODI2Nzg4LW00bXNyYjZxNDRkbTJ1ZTNrZ3Z1aTBmcTdrZGE1NWxzLmFwcHMuZ29vZ2xldXNlcmNvbnRlbnQuY29tIiwiYXVkIjoiODQ4NDEyODI2Nzg4LW00bXNyYjZxNDRkbTJ1ZTNrZ3Z1aTBmcTdrZGE1NWxzLmFwcHMuZ29vZ2xldXNlcmNvbnRlbnQuY29tIiwic3ViIjoiMTE0NTQyMzMwNzI3MTU2MTMzNzc2IiwiZW1haWwiOiJocGFdkeivmeuzQGdtYWlsLmNvbSIsImVtYWlsX3ZlcmlmaWVkIjp0cnVlLCJhdF9oYXNoIjoidno1NGhhdTNxbnVR",
    },
}))
{
    // handle response
}
```

### Parameters

| Parameter                                                                                          | Type                                                                                               | Required                                                                                           | Description                                                                                        |
| -------------------------------------------------------------------------------------------------- | -------------------------------------------------------------------------------------------------- | -------------------------------------------------------------------------------------------------- | -------------------------------------------------------------------------------------------------- |
| `request`                                                                                          | [HathoraCloud.Models.Operations.LoginGoogleRequest](../../models/operations/LoginGoogleRequest.md) | :heavy_check_mark:                                                                                 | The request object to use for the request.                                                         |


### Response

**[LoginGoogleResponse](../../models/operations/LoginGoogleResponse.md)**


## LoginNickname

Returns a unique player token with a specified nickname for a user.

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

using(var res = await sdk.AuthV1.LoginNicknameAsync(new HathoraCloud.Models.Operations.LoginNicknameRequest() {
    LoginNicknameRequest = new HathoraCloud.Models.Shared.LoginNicknameRequest() {
        Nickname = "squiddytwoshoes",
    },
}))
{
    // handle response
}
```

### Parameters

| Parameter                                                                                              | Type                                                                                                   | Required                                                                                               | Description                                                                                            |
| ------------------------------------------------------------------------------------------------------ | ------------------------------------------------------------------------------------------------------ | ------------------------------------------------------------------------------------------------------ | ------------------------------------------------------------------------------------------------------ |
| `request`                                                                                              | [HathoraCloud.Models.Operations.LoginNicknameRequest](../../models/operations/LoginNicknameRequest.md) | :heavy_check_mark:                                                                                     | The request object to use for the request.                                                             |


### Response

**[LoginNicknameResponse](../../models/operations/LoginNicknameResponse.md)**

