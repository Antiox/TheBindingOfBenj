using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace GameLibrary
{
    public class RoomScript : MonoBehaviour
    {
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
        /// Indique si la salle a déjà spawn des ennemis
        /// </summary>
        public bool SpawnedEnemies { get; set; }

        /// <summary>
        /// Indique les portes à bloquer lors du spawn d'ennemis et les réouvre ensuite
        /// </summary>
        public DoorType OpenedDoors { get; set; }


        /// <summary>
        /// Indique la largeur et hauteur d'une cellule de salle
        /// </summary>
        public Vector2 CellSize
        {
            get
            {
                // 11 taille de la prefab de base
                return new Vector2(11 * transform.localScale.x, 11 * transform.localScale.y);
            }
        }

        /// <summary>
        /// Largeur et hauteur d'une salle spéciale
        /// </summary>
        public Vector2 Size { get; set; }

        /// <summary>
        /// les salles connectées à celle-ci
        /// </summary>
        public List<RoomScript> RoomsConnected { get; private set; }


        public GameObject UpDoor { get; private set; }
        public GameObject RightDoor { get; private set; }
        public GameObject DownDoor { get; private set; }
        public GameObject LeftDoor { get; private set; }


        void Awake()
        {
            Type = RoomType.Normal;
            Weight = 1;
            GCost = 10000;
            HCost = 0;
            Neighbours = new Dictionary<string, RoomScript>();
            SpawnedEnemies = false;
            OpenedDoors = DoorType.None;
            RoomsConnected = new List<RoomScript>();
            UpDoor = transform.GetChild(0).gameObject;
            RightDoor = transform.GetChild(1).gameObject;
            DownDoor = transform.GetChild(2).gameObject;
            LeftDoor = transform.GetChild(3).gameObject;
        }

        private void Update()
        {
        }

        /// <summary>
        /// ouvre ou ferme les portes de la salle selon le bool
        /// false = ferme, true = ouvre
        /// </summary>
        /// <param name="type"></param>
        /// <param name="openOrClose"></param>
        public void OpenCloseDoors(DoorType type, bool openOrClose)
        {
            if (type.HasFlag(DoorType.Up)) OpenCloseDoor(UpDoor);
            if (type.HasFlag(DoorType.Right)) OpenCloseDoor(RightDoor);
            if (type.HasFlag(DoorType.Down)) OpenCloseDoor(DownDoor);
            if (type.HasFlag(DoorType.Left)) OpenCloseDoor(LeftDoor);

            void OpenCloseDoor(GameObject door)
            {
                door.GetComponent<BoxCollider2D>().isTrigger = openOrClose;
                door.GetComponent<SpriteRenderer>().enabled = !openOrClose;

                if (!openOrClose)
                    door.GetComponent<SpriteRenderer>().material = Resources.Load("Materials/Obstacle", typeof(Material)) as Material;
            }
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
    }
}