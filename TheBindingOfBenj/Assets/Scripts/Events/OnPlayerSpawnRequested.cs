using UnityEngine;

namespace GameLibrary
{
    class OnPlayerSpawnRequested : IGameEvent
    {
        public Vector3 Position { get; set; }

        public OnPlayerSpawnRequested(Vector3 position)
        {
            Position = position;
        }
    }
}
