using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GameLibrary
{
    public class MapManager
    {
        private int _columnCount, _rowCount;
        private Dictionary<string, Vector2> _specialRooms;
        private readonly RoomScript[,] _map;

        private GameObject _roomContainer;

        #region Singleton

        private static MapManager instance;
        public static MapManager Instance
        {
            get
            {
                return instance ??= new MapManager();
            }
        }

        private MapManager()
        {
            _roomContainer = GameObject.Find("Map");
            _columnCount = 5;
            _rowCount = 5;
            _map = new RoomScript[_columnCount, _rowCount];
            _specialRooms = new Dictionary<string, Vector2>();
        }

        #endregion


        public void Start()
        {
            GenerateMap();
        }

        public void Update()
        {

        }

        private void GenerateMap()
        {
            // genere une grille vierge
            for (int i = 0; i < _columnCount; i++)
            {
                for (int j = 0; j < _rowCount; j++)
                {
                    GameObject roomPrefab = Resources.Load<GameObject>("Prefabs/Room");
                    RoomScript roomScript = roomPrefab.GetComponent<RoomScript>();

                    GameObject room = Object.Instantiate(roomPrefab, new Vector3(i * roomScript.Size.x, j * roomScript.Size.y, 1), Quaternion.identity, _roomContainer.transform);

                    _map[i, j] = room.GetComponent<RoomScript>();
                    room.name = i + " " + j;
                }
            }

            // place un point aléatoire pour le spawn du joueur
            // uniquement sur les aretes
            _specialRooms.Add("SpawnRoom", RandomPossibleCoordinates(IsOnEdge));

            // place un point aléatoire pour la salle de boss
            // uniquement sur le coté opposé au spawn
            _specialRooms.Add("BossRoom", RandomPossibleCoordinates(OnOppositeSide));


            _specialRooms.Add("OtherRoom1", RandomPossibleCoordinates(AwayFromBossRoom));

            _specialRooms.Add("OtherRoom2", RandomPossibleCoordinates(AwayFromBossRoom));

            /// debug
            foreach (KeyValuePair<string, Vector2> item in _specialRooms)
            {
                if (item.Key == "SpawnRoom") _map[(int)item.Value.x, (int)item.Value.y].ChangeColor(Color.green);
                if (item.Key == "BossRoom") _map[(int)item.Value.x, (int)item.Value.y].ChangeColor(Color.red);
                if (item.Key.StartsWith("OtherRoom")) _map[(int)item.Value.x, (int)item.Value.y].ChangeColor(Color.blue);
            }
        }

        /// <summary>
        /// Genere un vector2 aléatoire selon le prédicat
        /// </summary>
        /// <param name="predicate"></param>
        /// <returns></returns>
        private Vector2 RandomPossibleCoordinates(System.Predicate<Vector2> predicate)
        {
            List<Vector2> possibleValues = new List<Vector2>();
            for (int i = 0; i < _columnCount; i++)
            {
                for (int j = 0; j < _rowCount; j++)
                {
                    Vector2 res = new Vector2(i, j);
                    if (predicate(res)) possibleValues.Add(res);
                }
            }
            return possibleValues[Random.Range(0, possibleValues.Count)];
        }

        /// <summary>
        /// Utilisé comme prédicat pour dire si le vector2 en paramètre se trouve sur les aretes de la grille
        /// </summary>
        /// <param name="vector"></param>
        /// <returns></returns>
        private bool IsOnEdge(Vector2 vector)
        {
            return vector.x == 0 || vector.x == _columnCount - 1 || vector.y == 0 || vector.y == _rowCount - 1;
        }

        /// <summary>
        /// Utilisé comme prédicat pour dire si le vector2 en paramètre est sur l'arete opposé à celle du spawn
        /// </summary>
        /// <param name="vector"></param>
        /// <returns></returns>
        private bool OnOppositeSide(Vector2 vector)
        {
            Vector2 spawnRoom = _specialRooms["SpawnRoom"];
            if (spawnRoom.x == 0)
            {
                return vector.x == _columnCount - 1;
            }
            else if (spawnRoom.x == _columnCount - 1)
            {
                return vector.x == 0;
            }

            if (spawnRoom.y == 0)
            {
                return vector.y == _rowCount - 1;
            }
            else if (spawnRoom.y == _rowCount - 1)
            {
                return vector.y == 0;
            }

            return false;
        }

        /// <summary>
        /// Utilisé comme prédicat pour dire si le vector2 en paramètre est à au moins une salle de celle du boss
        /// et on ne la place pas sur un salle déjà utilisée
        /// </summary>
        /// <param name="vector"></param>
        /// <returns></returns>
        private bool AwayFromBossRoom(Vector2 vector)
        {
            return Vector2.Distance(vector, _specialRooms["BossRoom"]) > 1 && !_specialRooms.ContainsValue(vector);
        }

    }
}
