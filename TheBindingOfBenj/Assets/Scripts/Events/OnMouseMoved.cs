using UnityEngine;

namespace GameLibrary
{
    class OnMouseMoved : IGameEvent
    {
        public Vector2 Position { get; set; }

        public OnMouseMoved(Vector2 position)
        {
            Position = position;
        }
    }
}
