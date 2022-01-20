using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomScript : MonoBehaviour
{
    /// <summary>
    /// Indique la largeur et hauteur de la salle
    /// </summary>
    public Vector2 Size
    {
        get
        {
            // 10.6f taille de la prefab de base
            return new Vector2(10.6f * transform.localScale.x, 10.6f * transform.localScale.y);
        }
    }


    public GameObject UpDoor { get; private set; }
    public GameObject RightDoor { get; private set; }
    public GameObject DownDoor { get; private set; }
    public GameObject LeftDoor { get; private set; }

    // Start is called before the first frame update
    void Start()
    {
        UpDoor = GameObject.Find("UW_Door");
        RightDoor = GameObject.Find("RW_Door");
        DownDoor = GameObject.Find("DW_Door");
        LeftDoor = GameObject.Find("LW_Door");
    }

    public void OpenDoors(DoorType type)
    {
        UpDoor.SetActive(!type.HasFlag(DoorType.Up));
        RightDoor.SetActive(!type.HasFlag(DoorType.Right));
        DownDoor.SetActive(!type.HasFlag(DoorType.Down));
        LeftDoor.SetActive(!type.HasFlag(DoorType.Left));
    }

    public void ChangeColor(Color color)
    {
        var sprites = GetComponentsInChildren<SpriteRenderer>();
        foreach (var sprite in sprites)
        {
            sprite.color = color;
        }
    }

}
