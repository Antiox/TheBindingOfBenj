using UnityEngine;

namespace GameLibrary
{
    public class OnEnemyHurt : IGameEvent
    {
        public GameObject Enemy { get; set; }
        public float Damage { get; set; }

        public OnEnemyHurt(GameObject enemy, float damage)
        {
            Enemy = enemy;
            Damage = damage;
        }
    }
}
