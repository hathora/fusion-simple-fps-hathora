# Room

A room object represents a game session or match.


## Fields

| Field                                                                                                                                                                                                                                                                                                                 | Type                                                                                                                                                                                                                                                                                                                  | Required                                                                                                                                                                                                                                                                                                              | Description                                                                                                                                                                                                                                                                                                           | Example                                                                                                                                                                                                                                                                                                               |
| --------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------- | --------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------- | --------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------- | --------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------- | --------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------- |
| `Allocations`                                                                                                                                                                                                                                                                                                         | List<[RoomAllocation](../../models/shared/RoomAllocation.md)>                                                                                                                                                                                                                                                         | :heavy_check_mark:                                                                                                                                                                                                                                                                                                    | N/A                                                                                                                                                                                                                                                                                                                   |                                                                                                                                                                                                                                                                                                                       |
| `AppId`                                                                                                                                                                                                                                                                                                               | *string*                                                                                                                                                                                                                                                                                                              | :heavy_check_mark:                                                                                                                                                                                                                                                                                                    | System generated unique identifier for an application.                                                                                                                                                                                                                                                                | app-af469a92-5b45-4565-b3c4-b79878de67d2                                                                                                                                                                                                                                                                              |
| `CurrentAllocation`                                                                                                                                                                                                                                                                                                   | [CurrentAllocation](../../models/shared/CurrentAllocation.md)                                                                                                                                                                                                                                                         | :heavy_check_mark:                                                                                                                                                                                                                                                                                                    | N/A                                                                                                                                                                                                                                                                                                                   |                                                                                                                                                                                                                                                                                                                       |
| `RoomConfig`                                                                                                                                                                                                                                                                                                          | *string*                                                                                                                                                                                                                                                                                                              | :heavy_check_mark:                                                                                                                                                                                                                                                                                                    | N/A                                                                                                                                                                                                                                                                                                                   | {"name":"my-room"}                                                                                                                                                                                                                                                                                                    |
| `RoomId`                                                                                                                                                                                                                                                                                                              | *string*                                                                                                                                                                                                                                                                                                              | :heavy_check_mark:                                                                                                                                                                                                                                                                                                    | Unique identifier to a game session or match. Use the default system generated ID or overwrite it with your own.<br/>Note: error will be returned if `roomId` is not globally unique.                                                                                                                                 | 2swovpy1fnunu                                                                                                                                                                                                                                                                                                         |
| `Status`                                                                                                                                                                                                                                                                                                              | [RoomStatus](../../models/shared/RoomStatus.md)                                                                                                                                                                                                                                                                       | :heavy_check_mark:                                                                                                                                                                                                                                                                                                    | The allocation status of a room.<br/><br/>`scheduling`: a process is not allocated yet and the room is waiting to be scheduled<br/><br/>`active`: ready to accept connections<br/><br/>`suspended`: room is unallocated from the process but can be rescheduled later with the same `roomId`<br/><br/>`destroyed`: all associated metadata is deleted | active                                                                                                                                                                                                                                                                                                                |