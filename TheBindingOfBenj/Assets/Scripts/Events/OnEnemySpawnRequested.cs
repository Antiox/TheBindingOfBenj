using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace GameLibrary
{
    class OnEnemySpawnRequested : IGameEvent
    {
        public Vector2 Position { get; set; }
        public EnemyType Type { get; set; }
        public bool Summon { get; set; }

        public OnEnemySpawnRequested(Vector2 position, EnemyType type, bool summon)
        {
            Position = position;
            Type = type;
            Summon = summon;
        }
    }
}
