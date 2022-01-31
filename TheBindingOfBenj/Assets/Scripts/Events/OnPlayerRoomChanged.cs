using UnityEngine;

namespace GameLibrary
{
    class OnPlayerRoomChanged : IGameEvent
    {
        public RoomScript Room { get; set; }

        public OnPlayerRoomChanged(RoomScript room)
        {
            Room = room;
        }
    }
}
