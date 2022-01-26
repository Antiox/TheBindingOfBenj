using UnityEngine;

namespace GameLibrary
{
    class OnPlayerMoved : IGameEvent
    {
        public Vector2 Direction { get; set; }

        public OnPlayerMoved(Vector2 direction)
        {
            Direction = direction;
        }
    }
}
