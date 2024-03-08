# RoomStatus

The allocation status of a room.

`scheduling`: a process is not allocated yet and the room is waiting to be scheduled

`active`: ready to accept connections

`suspended`: room is unallocated from the process but can be rescheduled later with the same `roomId`

`destroyed`: all associated metadata is deleted


## Values

| Name         | Value        |
| ------------ | ------------ |
| `Scheduling` | scheduling   |
| `Active`     | active       |
| `Suspended`  | suspended    |
| `Destroyed`  | destroyed    |