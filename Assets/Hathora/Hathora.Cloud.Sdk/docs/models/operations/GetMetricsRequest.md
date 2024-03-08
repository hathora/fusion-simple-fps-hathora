# GetMetricsRequest


## Fields

| Field                                                 | Type                                                  | Required                                              | Description                                           | Example                                               |
| ----------------------------------------------------- | ----------------------------------------------------- | ----------------------------------------------------- | ----------------------------------------------------- | ----------------------------------------------------- |
| `ProcessId`                                           | *string*                                              | :heavy_check_mark:                                    | N/A                                                   | cbfcddd2-0006-43ae-996c-995fff7bed2e                  |
| `AppId`                                               | *string*                                              | :heavy_minus_sign:                                    | N/A                                                   | app-af469a92-5b45-4565-b3c4-b79878de67d2              |
| `End`                                                 | *double*                                              | :heavy_minus_sign:                                    | Unix timestamp. Default is current time.              |                                                       |
| `Metrics`                                             | List<[MetricName](../../models/shared/MetricName.md)> | :heavy_minus_sign:                                    | Available metrics to query over time.                 |                                                       |
| `Start`                                               | *double*                                              | :heavy_minus_sign:                                    | Unix timestamp. Default is -1 hour from `end`.        |                                                       |
| `Step`                                                | *int*                                                 | :heavy_minus_sign:                                    | N/A                                                   |                                                       |