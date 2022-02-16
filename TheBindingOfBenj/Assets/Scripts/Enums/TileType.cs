using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameLibrary
{
    [Serializable]
    public enum TileType
    {
        Empty = 0,
        Obstacle = 1,
        Monster = 2,
        Loot = 3,
        Weapon = 4,
        Hole = 5
    }
}
