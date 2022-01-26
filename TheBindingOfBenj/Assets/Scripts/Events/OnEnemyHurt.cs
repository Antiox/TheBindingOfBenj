using UnityEngine;

namespace GameLibrary
{
    public class OnEnemyHurt : IGameEvent
    {
        public GameObject Enemy { get; set; }

        public OnEnemyHurt(GameObject enemy)
        {
            Enemy = enemy;
        }
    }
}
