using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace GameLibrary
{
    [System.Flags]
    public enum RoomType {
        Normal = 0,
        Boss = 1,
        Spawn = 2,
        Other = 4
    }
}
