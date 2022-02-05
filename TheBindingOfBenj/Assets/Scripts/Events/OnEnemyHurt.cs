using UnityEngine;

namespace GameLibrary
{
    public class OnEnemyHurt : IGameEvent
    {
        public GameObject GameObject { get; set; }
        public Enemy Enemy { get; set; }
        public float Damage { get; set; }

        public OnEnemyHurt(GameObject gameObject, Enemy enemy, float damage)
        {
            GameObject = gameObject;
            Damage = damage;
            Enemy = enemy;
        }
    }
}
