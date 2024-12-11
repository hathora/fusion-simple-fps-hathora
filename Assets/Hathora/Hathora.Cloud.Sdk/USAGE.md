<!-- Start SDK Example Usage [usage] -->
```csharp
using HathoraCloud;
using HathoraCloud.Models.Shared;

var sdk = new HathoraCloudSDK(
    security: new Security() {
        HathoraDevToken = "<YOUR_BEARER_TOKEN_HERE>",
    },
    appId: "app-af469a92-5b45-4565-b3c4-b79878de67d2");

AppConfig req = new AppConfig() {
    AppName = "minecraft",
    AuthConfiguration = new AuthConfiguration() {},
};


using(var res = await sdk.AppsV1.CreateAppV1DeprecatedAsync(req))
{
    // handle response
}


```
<!-- End SDK Example Usage [usage] -->