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
        public GameObject Enemy { get; set; }

        public OnEnemyDied(GameObject enemy)
        {
            Enemy = enemy;
        }
    }
}
