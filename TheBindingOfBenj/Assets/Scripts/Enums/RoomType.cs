using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Flags]
public enum RoomType
{
    Normal = 0,
    Boss = 1,
    Spawn  = 2,
    Other = 4
}
