using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Flags]
public enum DoorType 
{
    None = 0,
    Up = 1,
    Right = 2,
    Down = 4,
    Left = 8
}
