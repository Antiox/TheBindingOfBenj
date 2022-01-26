using UnityEngine;

namespace GameLibrary
{
    public class OnEnemySpawned : IGameEvent
    {
        public GameObject Enemy { get; set; }

        public OnEnemySpawned(GameObject enemy)
        {
            Enemy = enemy;
        }
    }
}
