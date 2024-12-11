# ProcessExposedPort

Connection details for an active process.


## Fields

| Field                                                                               | Type                                                                                | Required                                                                            | Description                                                                         |
| ----------------------------------------------------------------------------------- | ----------------------------------------------------------------------------------- | ----------------------------------------------------------------------------------- | ----------------------------------------------------------------------------------- |
| `Host`                                                                              | *string*                                                                            | :heavy_check_mark:                                                                  | N/A                                                                                 |
| `Name`                                                                              | *string*                                                                            | :heavy_check_mark:                                                                  | N/A                                                                                 |
| `Port`                                                                              | *int*                                                                               | :heavy_check_mark:                                                                  | N/A                                                                                 |
| `TransportType`                                                                     | [TransportType](../../Models/Shared/TransportType.md)                               | :heavy_check_mark:                                                                  | Transport type specifies the underlying communication protocol to the exposed port. |