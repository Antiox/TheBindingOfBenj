using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace GameLibrary
{
    class OnProjectileSpawnRequested : IGameEvent
    {
        public Projectile Projectile { get; set; }
        public Vector3 Position { get; set; }
        public Vector3 Target { get; set; }


        public OnProjectileSpawnRequested(Projectile projectile, Vector3 position, Vector3 target)
        {
            Position = position;
            Target = target;
            Projectile = projectile;
        }
    }
}
