using UnityEngine;

namespace GameLibrary
{
    class OnPlayerRoomChanged : IGameEvent
    {
        public Vector2 Position { get; set; }

        public OnPlayerRoomChanged(Vector2 position)
        {
            Position = position;
        }
    }
}
