using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace GameLibrary
{
    public class RoomScript : MonoBehaviour {
        // coordonn�es sur la map
        public Vector2 Coordinates { get; set; }

        // type de salle
        public RoomType Type { get; set; }

        // pond�ration
        public float Weight { get; set; }

        // meilleur cout actuel
        public float GCost { get; set; }

        // heuristique
        public float HCost { get; set; }

        // salles voisines
        public List<RoomScript> Neighbours { get; private set; }

        /// <summary>
        /// Utilis� pour le backtracking de l'algo A* pour connecter les salles
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
            Neighbours = new List<RoomScript>();
        }

        private void Update()
        {
            GetComponentInChildren<Text>().text = GCost.ToString() + " " + HCost;
        }

        public void OpenDoors(DoorType type)
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

        public void AddNeighbour(RoomScript room)
        {
            Neighbours.Add(room);
        }

    }
}