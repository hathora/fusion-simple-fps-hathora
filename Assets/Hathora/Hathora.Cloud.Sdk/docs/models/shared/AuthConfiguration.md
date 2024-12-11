# AuthConfiguration

Configure [player authentication](https://hathora.dev/docs/lobbies-and-matchmaking/auth-service) for your application. Use Hathora's built-in auth providers or use your own [custom authentication](https://hathora.dev/docs/lobbies-and-matchmaking/auth-service#custom-auth-provider).


## Fields

| Field                                                         | Type                                                          | Required                                                      | Description                                                   |
| ------------------------------------------------------------- | ------------------------------------------------------------- | ------------------------------------------------------------- | ------------------------------------------------------------- |
| `Anonymous`                                                   | [RecordStringNever](../../Models/Shared/RecordStringNever.md) | :heavy_minus_sign:                                            | Construct a type with a set of properties K of type T         |
| `Google`                                                      | [Google](../../Models/Shared/Google.md)                       | :heavy_minus_sign:                                            | Enable google auth for your application.                      |
| `Nickname`                                                    | [RecordStringNever](../../Models/Shared/RecordStringNever.md) | :heavy_minus_sign:                                            | Construct a type with a set of properties K of type T         |