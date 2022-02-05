using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace GameLibrary
{
    public class MapManager
    {
        private int _columnCount, _rowCount;
        private Dictionary<string, RoomScript> _specialRooms;
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
            _specialRooms = new Dictionary<string, RoomScript>();
        }

        #endregion

        public void Awake()
        {
        }

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
                    var roomPrefab = Resources.Load<GameObject>("Prefabs/Room");
                    var roomScript = roomPrefab.GetComponent<RoomScript>();

                    var room = Object.Instantiate(roomPrefab, new Vector3(i * roomScript.Size.x, j * roomScript.Size.y, roomPrefab.transform.position.z), Quaternion.identity, _roomContainer.transform);

                    _map[i, j] = room.GetComponent<RoomScript>();
                    _map[i, j].Coordinates = new Vector2(i, j);
                    room.name = i + " " + j;
                }
            }

            // place un point aléatoire pour le spawn du joueur
            // uniquement sur les aretes
            _specialRooms.Add("SpawnRoom", _map.Get(RandomPossibleCoordinates(IsOnEdge)));
            _specialRooms["SpawnRoom"].Type = RoomType.Spawn;

            var worldPos = _specialRooms["SpawnRoom"].Coordinates * _specialRooms["SpawnRoom"].Size;
            EventManager.Instance.Dispatch(new OnPlayerSpawnRequested(worldPos));
            EventManager.Instance.Dispatch(new OnPlayerRoomChanged(_specialRooms["SpawnRoom"]));

            // place un point aléatoire pour la salle de boss
            // uniquement sur le coté opposé au spawn
            _specialRooms.Add("BossRoom", _map.Get(RandomPossibleCoordinates(OnOppositeSide)));
            _specialRooms["BossRoom"].Type = RoomType.Boss;

            _specialRooms.Add("OtherRoom1", _map.Get(RandomPossibleCoordinates(AwayFromOtherRooms)));
            _specialRooms["OtherRoom1"].Type = RoomType.Other;

            _specialRooms.Add("OtherRoom2", _map.Get(RandomPossibleCoordinates(AwayFromOtherRooms)));
            _specialRooms["OtherRoom2"].Type = RoomType.Other;

            /// debug
            foreach (KeyValuePair<string, RoomScript> item in _specialRooms)
            {
                if (item.Key == "SpawnRoom") _map.Get(item.Value.Coordinates).ChangeColor(Color.green);
                if (item.Key == "BossRoom") _map.Get(item.Value.Coordinates).ChangeColor(Color.red);
                if (item.Key.StartsWith("OtherRoom")) _map.Get(item.Value.Coordinates).ChangeColor(Color.blue);
            }

            AddRoomNeighbours();

            ConnectRoomsAStar(_specialRooms["SpawnRoom"], _specialRooms["BossRoom"]);
            ConnectRoomsAStar(_specialRooms["OtherRoom1"], _specialRooms["SpawnRoom"]);
            ConnectRoomsAStar(_specialRooms["OtherRoom2"], _specialRooms["SpawnRoom"]);


            
            var obstacles = Resources.LoadAll("Prefabs/Obstacles");
            var weapons = Resources.LoadAll<Weapon>("Scriptables");

            foreach (var room in _map)
            {
                var middlePointRoom = new Vector3(room.Coordinates.x * room.Size.x, room.Coordinates.y * room.Size.y, 0);
                if (room.Type != RoomType.Spawn && room.Type != RoomType.Boss && room.Type != RoomType.Other)
                {
                    if (Random.Range(0, 3) == 0) // 1/3 d'avoir un obstacle dans la salle
                        Object.Instantiate(obstacles[Random.Range(0, obstacles.Length)], middlePointRoom, Quaternion.identity, GameObject.Find("Obstacles").transform);
                }
                if (room.Type == RoomType.Other)
                {
                    var weapon = Object.Instantiate(Resources.Load("Prefabs/Weapon"), middlePointRoom, Quaternion.identity) as GameObject;
                    weapon.GetComponent<WeaponGeneratorScript>().Weapon = weapons[Random.Range(0, weapons.Length)];
                }
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
            Vector2 spawnRoom = _specialRooms["SpawnRoom"].Coordinates;
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
        /// Utilisé comme prédicat pour dire si le vector2 en paramètre est à au moins une salle d'une autre
        /// et on ne la place pas sur un salle déjà utilisée
        /// </summary>
        /// <param name="vector"></param>
        /// <returns></returns>
        private bool AwayFromOtherRooms(Vector2 vector)
        {
            var res = true;

            foreach (var room in _specialRooms)
            {
                if (Vector2.Distance(vector, room.Value.Coordinates) <= 2) res = false;
            }

            return res && !_specialRooms.ContainsValue(_map.Get(vector));
        }

        /// <summary>
        /// parcours toutes les salles et actualise leurs voisins
        /// </summary>
        private void AddRoomNeighbours()
        {
            for (int i = 0; i < _columnCount; i++)
            {
                for (int j = 0; j < _rowCount; j++)
                {
                    if (i > 0) _map[i, j].AddNeighbour("Left", _map[i - 1, j]);
                    if (i < _columnCount - 1) _map[i, j].AddNeighbour("Right", _map[i + 1, j]);
                    if (j > 0) _map[i, j].AddNeighbour("Down", _map[i, j - 1]);
                    if (j < _rowCount - 1) _map[i, j].AddNeighbour("Up", _map[i, j + 1]);
                }
            }
        }

        /// <summary>
        /// ouvre les portes entre la salle from et la salle to
        /// </summary>
        /// <param name="from"></param>
        /// <param name="to"></param>
        private void ConnectRoomsAStar(RoomScript from, RoomScript to)
        {
            // on réinitialise à chaque fois la grille : l'heuristique, meilleure cout et parent;
            for (int i = 0; i < _columnCount; i++)
            {
                for (int j = 0; j < _rowCount; j++)
                {
                    _map[i, j].HCost = Vector2.Distance(new Vector2(i, j), to.Coordinates);
                    _map[i, j].GCost = 10000;
                    _map[i, j].Parent = null;
                }
            }
            // met le cout du from à 0
            from.GCost = 0;

            List<RoomScript> openSet = new List<RoomScript>();
            HashSet<RoomScript> closedSet = new HashSet<RoomScript>();

            // mettre le nœud de départ dans OUVERT
            openSet.Add(from);

            // si OUVERT est vide, sortir avec Echec, sinon continuer
            while (openSet.Count != 0)
            {
                // ordonner OUVERT suivant la fonction f(n)
                openSet = openSet.OrderBy(room => room.GCost + room.HCost).ToList<RoomScript>();

                // enlever le nœud de la tête de OUVERT et le mettre dans FERME. Appeler ce nœud n
                var n = openSet[0];
                openSet.RemoveAt(0);
                closedSet.Add(n);

                // développer n en générant tous ses successeurs
                foreach (var keyValue in n.Neighbours)
                {
                    var neighbour = keyValue.Value;
                    // si le noeud est le noeud but
                    if (n != to)
                    {
                        // pour tout successeur n’ de n : a.calculer f(n’)
                        float cost = n.GCost + neighbour.Weight;

                        // si n’ n’est ni dans OUVERT ni dans FERME l’ajouter à OUVERT
                        if (!openSet.Contains(neighbour) && !closedSet.Contains(neighbour)) openSet.Add(neighbour);

                        // si l’ancien cout est supérieur, nouvelle route 
                        if (neighbour.GCost > cost)
                        {
                            neighbour.GCost = cost;
                            neighbour.Parent = n;
                        }
                    }
                    else
                    {
                        // backtracking  pour ouvrir les portes du chemin
                        RoomScript currentNode = to;

                        while (currentNode != null)
                        {
                            DoorType doorsToOpen = DoorType.None;
                            DoorType doorsToOpenParent = DoorType.None; // à optimiser (car ça ouvre la porte que d'un coté de la salle)

                            if (currentNode.Parent != null) // pour la dernière salle (qui n'a pas de parent)
                            {
                                // augmentation de la pondération des salles où on est déjà passé et les salles adjacentes
                                currentNode.Weight += 10;
                                foreach (var currentNeighbour in currentNode.Neighbours)
                                {
                                    if (!currentNode.Parent.Type.HasFlag(RoomType.Boss | RoomType.Spawn))
                                    {
                                        currentNeighbour.Value.Weight += 5;
                                    }
                                }


                                // ouvertures des portes
                                var doorDirection = (currentNode.Parent.Coordinates - currentNode.Coordinates);
                                if (doorDirection.x == -1)
                                {
                                    doorsToOpen |= DoorType.Left;
                                    doorsToOpenParent |= DoorType.Right;
                                }
                                else if (doorDirection.x == 1)
                                {
                                    doorsToOpen |= DoorType.Right;
                                    doorsToOpenParent |= DoorType.Left;
                                }
                                if (doorDirection.y == -1)
                                {
                                    doorsToOpen |= DoorType.Down;
                                    doorsToOpenParent |= DoorType.Up;
                                }
                                else if (doorDirection.y == 1)
                                {
                                    doorsToOpen |= DoorType.Up;
                                    doorsToOpenParent |= DoorType.Down;
                                }

                                currentNode.Parent.OpenCloseDoors(doorsToOpenParent, true);
                                currentNode.Parent.OpenedDoors |= doorsToOpenParent;
                            }

                            currentNode.OpenCloseDoors(doorsToOpen, true);
                            currentNode.OpenedDoors |= doorsToOpen;

                            currentNode = currentNode.Parent;
                        }

                        return;
                    }
                }
            }

        }
    }
}
