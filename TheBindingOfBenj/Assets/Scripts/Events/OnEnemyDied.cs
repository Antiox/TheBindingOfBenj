using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace GameLibrary
{
    public class OnEnemyDied : IGameEvent
    {
        public GameObject GameObject { get; set; }
        public Enemy Enemy{ get; set; }

        public OnEnemyDied(GameObject gameObject, Enemy enemy)
        {
            GameObject = gameObject;
            Enemy = enemy;
        }
    }
}
