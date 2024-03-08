<!-- Start SDK Example Usage -->
```csharp
using HathoraCloud;
using HathoraCloud.Models.Shared;

var sdk = new HathoraCloudSDK(
    security: new Security() {
        HathoraDevToken = "",
    },
    appId: "app-af469a92-5b45-4565-b3c4-b79878de67d2"
);

using(var res = await sdk.AppV1.CreateAppAsync(new AppConfig() {
    AppName = "minecraft",
    AuthConfiguration = new AuthConfiguration() {
        Anonymous = new RecordStringNever() {},
        Google = new Google() {
            ClientId = "string",
        },
        Nickname = new RecordStringNever() {},
    },
}))
{
    // handle response
}
```
<!-- End SDK Example Usage -->