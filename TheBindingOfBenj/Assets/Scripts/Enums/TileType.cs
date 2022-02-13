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
        Weapon = 2,
        Loot = 3,
        Monster = 4,
        Hole = 5
    }
}
