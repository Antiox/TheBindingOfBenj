using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace GameLibrary
{
    public class RoomScript : MonoBehaviour {
        // coordonnées sur la map
        public Vector2 Coordinates { get; set; }

        // type de salle
        public RoomType Type { get; set; }

        // pondération
        public float Weight { get; set; }

        // meilleur cout actuel
        public float GCost { get; set; }

        // heuristique
        public float HCost { get; set; }

        // salles voisines
        public Dictionary<string, RoomScript> Neighbours { get; private set; }

        /// <summary>
        /// Utilisé pour le backtracking de l'algo A* pour connecter les salles
        /// </summary>
        public RoomScript Parent { get; set; }


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


        void Awake()
        {
            UpDoor = transform.GetChild(0).GetChild(2).gameObject;
            RightDoor = transform.GetChild(1).GetChild(2).gameObject;
            DownDoor = transform.GetChild(2).GetChild(2).gameObject;
            LeftDoor = transform.GetChild(3).GetChild(2).gameObject;
            GCost = 10000;
            HCost = 0;
            Weight = 1;
            Type = RoomType.Normal;
            Neighbours = new Dictionary<string, RoomScript>();
        }

        private void Update()
        {
        }

        public void OpenDoors(DoorType type)
        {
            if (type.HasFlag(DoorType.Up)) OpenCloseDoor(UpDoor, true);

            if (type.HasFlag(DoorType.Right)) OpenCloseDoor(RightDoor, true);

            if (type.HasFlag(DoorType.Down)) OpenCloseDoor(DownDoor, true);

            if (type.HasFlag(DoorType.Left)) OpenCloseDoor(LeftDoor, true);
        }
        
        public void CloseDoors(DoorType type)
        {
            if (type.HasFlag(DoorType.Up))
                UpDoor.SetActive(false);

            if (type.HasFlag(DoorType.Right))
                RightDoor.SetActive(false);

            if (type.HasFlag(DoorType.Down))
                DownDoor.SetActive(false);

            if (type.HasFlag(DoorType.Left))
                LeftDoor.SetActive(false);
        }

        public void ChangeColor(Color color)
        {
            var sprites = GetComponentsInChildren<SpriteRenderer>();
            foreach (var sprite in sprites)
            {
                sprite.color = color;
            }
        }

        public void AddNeighbour(string name, RoomScript room)
        {
            Neighbours.Add(name, room);
        }

        private void OpenCloseDoor(GameObject door, bool openClose)
        {
            door.GetComponent<BoxCollider2D>().isTrigger = true && openClose;
            door.GetComponent<SpriteRenderer>().enabled = false && openClose;
            if (openClose)
            {

            }
                //door.GetComponent<SpriteRenderer>().material = ;
            else
                door.GetComponent<SpriteRenderer>().material = Resources.Load("Materials/Obstacle", typeof(Material)) as Material;

        }

    }
}