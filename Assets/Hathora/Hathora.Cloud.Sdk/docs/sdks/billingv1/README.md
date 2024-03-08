# BillingV1
(*BillingV1*)

### Available Operations

* [GetBalance](#getbalance)
* [GetInvoices](#getinvoices)
* [GetPaymentMethod](#getpaymentmethod)
* [InitStripeCustomerPortalUrl](#initstripecustomerportalurl)

## GetBalance

### Example Usage

```csharp
using HathoraCloud;
using HathoraCloud.Models.Shared;

var sdk = new HathoraCloudSDK(
    security: new Security() {
        HathoraDevToken = "",
    },
    appId: "app-af469a92-5b45-4565-b3c4-b79878de67d2"
);

using(var res = await sdk.BillingV1.GetBalanceAsync())
{
    // handle response
}
```


### Response

**[GetBalanceResponse](../../models/operations/GetBalanceResponse.md)**


## GetInvoices

### Example Usage

```csharp
using HathoraCloud;
using HathoraCloud.Models.Shared;

var sdk = new HathoraCloudSDK(
    security: new Security() {
        HathoraDevToken = "",
    },
    appId: "app-af469a92-5b45-4565-b3c4-b79878de67d2"
);

using(var res = await sdk.BillingV1.GetInvoicesAsync())
{
    // handle response
}
```


### Response

**[GetInvoicesResponse](../../models/operations/GetInvoicesResponse.md)**


## GetPaymentMethod

### Example Usage

```csharp
using HathoraCloud;
using HathoraCloud.Models.Shared;

var sdk = new HathoraCloudSDK(
    security: new Security() {
        HathoraDevToken = "",
    },
    appId: "app-af469a92-5b45-4565-b3c4-b79878de67d2"
);

using(var res = await sdk.BillingV1.GetPaymentMethodAsync())
{
    // handle response
}
```


### Response

**[GetPaymentMethodResponse](../../models/operations/GetPaymentMethodResponse.md)**


## InitStripeCustomerPortalUrl

### Example Usage

```csharp
using HathoraCloud;
using HathoraCloud.Models.Shared;

var sdk = new HathoraCloudSDK(
    security: new Security() {
        HathoraDevToken = "",
    },
    appId: "app-af469a92-5b45-4565-b3c4-b79878de67d2"
);

using(var res = await sdk.BillingV1.InitStripeCustomerPortalUrlAsync(new CustomerPortalUrl() {
    ReturnUrl = "string",
}))
{
    // handle response
}
```

### Parameters

| Parameter                                                     | Type                                                          | Required                                                      | Description                                                   |
| ------------------------------------------------------------- | ------------------------------------------------------------- | ------------------------------------------------------------- | ------------------------------------------------------------- |
| `request`                                                     | [CustomerPortalUrl](../../models/shared/CustomerPortalUrl.md) | :heavy_check_mark:                                            | The request object to use for the request.                    |


### Response

**[InitStripeCustomerPortalUrlResponse](../../models/operations/InitStripeCustomerPortalUrlResponse.md)**

