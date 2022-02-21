using Newtonsoft.Json;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;

namespace GameLibrary
{
    public class MapManager
    {
        private int _columnCount, _rowCount;
        private Dictionary<string, RoomScript> _specialRooms;
        private RoomScript[,] _map;

        private List<RoomScript> _paths;

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
            _columnCount = 40;
            _rowCount = 30;
            _specialRooms = new Dictionary<string, RoomScript>();
            _map = new RoomScript[_columnCount, _rowCount];
            _paths = new List<RoomScript>();
        }

        #endregion


        public void Start()
        {
            GenerateMap();
            /*
            try
            {
            }
            catch (System.Exception e)
            {
                Debug.LogError(e);
                ClearMap();
                Start();
            }
            */
        }

        public void Update()
        {
        }

        private void GenerateMap()
        {
            // genere une grille vierge
            GenerateBlankMap();

            // instancie les salles voisines de chaque salle
            AddRoomNeighbours();

            // place un point aléatoire pour le spawn du joueur
            // uniquement sur les aretes ou case à 1 à 2 d'une arete
            AddSpecialRoom("SpawnRoom", IsOnEdge, RoomType.Spawn);

            var worldPos = _specialRooms["SpawnRoom_0_0"].Coordinates * _specialRooms["SpawnRoom_0_0"].CellSize;
            EventManager.Instance.Dispatch(new OnPlayerSpawnRequested(worldPos));

            // place un point aléatoire pour la salle de boss
            // uniquement sur le coté opposé au spawn
            AddSpecialRoom("BossRoom", OnOppositeSide, RoomType.Boss);

            for (int i = 0; i < 9; i++)
            {
                AddSpecialRoom("OtherRoom" + i, AwayFromOtherRooms, RoomType.Other);
            }

            /// debug
            foreach (var item in _specialRooms)
            {
                if (item.Key.StartsWith("SpawnRoom")) _map.Get(item.Value.Coordinates).ChangeColor(Color.green);
                if (item.Key.StartsWith("BossRoom")) _map.Get(item.Value.Coordinates).ChangeColor(Color.red);
                if (item.Key.StartsWith("OtherRoom")) _map.Get(item.Value.Coordinates).ChangeColor(Color.blue);
            }

            // ajout des éléments à l'intérieur d'une salle
            LoadRoomsConfiguration();

            // création des chemins entre les salles
            ConnectAllRooms();

            CleanUselessRooms();
        }

        /// <summary>
        /// retire les portes des salles inutilisées
        /// </summary>
        private void CleanUselessRooms()
        {
            foreach (var room in _map)
            {
                if (!_paths.Contains(room) && !_specialRooms.ContainsValue(room))
                    room.OpenCloseDoors(DoorType.Up | DoorType.Right | DoorType.Down | DoorType.Left, true);
            }
        }

        private void LoadRoomsConfiguration()
        {
            var rooms = Resources.LoadAll("Rooms");
            var obstacles = Resources.LoadAll("Prefabs/Obstacles/Rocks");
            var weapons = Resources.LoadAll<Weapon>("Scriptables");
            
            foreach (var room in _specialRooms.Where(room => room.Value.Type == RoomType.Other && room.Key.EndsWith("_0_0")))
            {
                var tiles = JsonConvert.DeserializeObject<List<Tile>>(File.ReadAllText("Assets/Resources/Rooms/" + rooms[Random.Range(0, rooms.Length)].name + ".json"));
                foreach (var tile in tiles)
                {
                    var pos = new Vector2(room.Value.Coordinates.x * room.Value.CellSize.x + tile.X * 7 - 5, 
                                          room.Value.Coordinates.y * room.Value.CellSize.y + tile.Y * 7 - 5);
                    switch (tile.Type)
                    {
                        case TileType.Obstacle:
                            Object.Instantiate(obstacles[Random.Range(0, obstacles.Length)], pos, Quaternion.identity, GameObject.Find("Obstacles").transform);
                            break;
                        case TileType.Weapon:
                            var weapon = Object.Instantiate(Resources.Load("Prefabs/Weapon"), pos, Quaternion.identity, GameObject.Find("Weapons").transform) as GameObject;
                            weapon.GetComponent<WeaponGeneratorScript>().Weapon = weapons[Random.Range(0, weapons.Length)];
                            break;
                        case TileType.Loot:
                            break;
                        case TileType.Monster:
                            room.Value.MonstersPositions.Add(pos);
                            break;
                        case TileType.Hole:
                            Object.Instantiate(Resources.Load("Prefabs/Obstacles/Hole/Hole"), pos, Quaternion.identity, GameObject.Find("Holes").transform);
                            break;
                        default:
                            break;
                    }
                }
            }
        }

        private void ConnectAllRooms()
        {
            // chaque salle différente
            var roots = new Dictionary<string, RoomScript>();
            foreach (var room in _specialRooms)
            {
                // radical de la salle
                var roomString = GetRoomRoot(room.Value).Key;

                if (!roots.ContainsKey(roomString + "_0_0"))
                    roots.Add(roomString + "_0_0", _specialRooms[roomString + "_0_0"]);
            }

            foreach (var room in roots)
            {
                // récupere les salles les plus proches
                var closestRooms = roots.Where(x => x.Value != room.Value && !x.Value.RoomsConnected.Contains(room.Value) && Vector2.Distance(x.Value.Coordinates, room.Value.Coordinates) <= _columnCount / 2).OrderBy(x => Vector2.Distance(x.Value.Coordinates, room.Value.Coordinates));

                // connexion aux trois salles les plus proches
                for (int i = 0; i < closestRooms.Count() - room.Value.RoomsConnected.Count; i++)
                {
                    var fromString = GetRoomRoot(room.Value).Key;
                    var from = room.Value;

                    var to = closestRooms.ElementAt(i).Value;

                    from.RoomsConnected.Add(to);
                    to.RoomsConnected.Add(from);

                    var toString = GetRoomRoot(to).Key;

                    // calcul des meilleures portes
                    string[] doors = { "_Up", "_Down", "_Left", "_Right" };
                    var lowestDistance = 10000f;

                    for (int j = 0; j < doors.Length; j++)
                    {
                        for (int k = 0; k < doors.Length; k++)
                        {
                            var distance = Vector2.Distance(_specialRooms[fromString + doors[j]].Coordinates, _specialRooms[toString + doors[k]].Coordinates);
                            if (lowestDistance > distance)
                            {
                                lowestDistance = distance;
                                from = _specialRooms[fromString + doors[j]];
                                to = _specialRooms[toString + doors[k]];
                            }
                        }
                    }
                    ConnectTwoRooms(from, to);
                }
            }
        }

        private void ClearMap()
        {
            foreach (Transform roomObject in _roomContainer.transform)
            {
                Object.Destroy(roomObject.gameObject);
            }
        }

        private void AddSpecialRoom(string name, System.Predicate<Vector2[]> predicate, RoomType type)
        {
            //var size = new Vector2(Random.Range(1, 3) * 2 + 1, Random.Range(1, 3) * 2 + 1);

            var size = new Vector2(5, 5);

            var startCoord = RandomPossibleCoordinates(predicate, size);

            for (int j = 0; j < size.y; j++)
            {
                for (int i = 0; i < size.x; i++)
                {
                    var posRoom = new Vector2(i + startCoord.x, j + startCoord.y);
                    var room = _map.Get(posRoom);

                    var roomName = "";

                    void InstanciateWallTrigger(int index, float xOffset, float yOffset)
                    {
                        var res = (Object.Instantiate(Resources.Load("Prefabs/WallTrigger"), room.transform.GetChild(index).transform.position, Quaternion.identity, room.transform.GetChild(index)) as GameObject).transform;
                        res.position += new Vector3(xOffset, yOffset, 0);
                    }

                    if ((int) size.x / 2 == i)
                    {
                        if (j == size.y - 1)
                        {
                            roomName = name + "_Up";
                            InstanciateWallTrigger(0, 0, -3f);
                        }
                        else if (j == 0)
                        {
                            roomName = name + "_Down";
                            InstanciateWallTrigger(2, 0, 3f);
                        }
                    }
                    else if ((int) size.y / 2 == j)
                    {
                        if (i == size.x - 1)
                        {
                            roomName = name + "_Right";
                            InstanciateWallTrigger(1, -3f, 0);
                        }
                        else if (i == 0)
                        {
                            roomName = name + "_Left";
                            InstanciateWallTrigger(3, 3f, 0);
                        }
                    }
                    
                    if (roomName == "") roomName = name + "_" + i + "_" + j;

                    _specialRooms.Add(roomName, room);
                    _specialRooms[roomName].Type = type;
                    _specialRooms[roomName].Weight = 300;
                    _specialRooms[roomName].Size = size;

                    // ouverture des portes intérieures à la salle
                    if (i < size.x - 1) Object.Destroy(room.RightDoor);
                    if (i > 0) Object.Destroy(room.LeftDoor);
                    if (j < size.y - 1) Object.Destroy(room.UpDoor);
                    if (j > 0) Object.Destroy(room.DownDoor);
                }
            }
        }

        private void GenerateBlankMap()
        {
            for (int i = 0; i < _columnCount; i++)
            {
                for (int j = 0; j < _rowCount; j++)
                {
                    var roomPrefab = Resources.Load<GameObject>("Prefabs/Room");
                    var roomScript = roomPrefab.GetComponent<RoomScript>();

                    var room = Object.Instantiate(roomPrefab, new Vector3(i * roomScript.CellSize.x, j * roomScript.CellSize.y, roomPrefab.transform.position.z), Quaternion.identity, _roomContainer.transform);

                    _map[i, j] = room.GetComponent<RoomScript>();
                    _map[i, j].Coordinates = new Vector2(i, j);
                    room.name = i + " " + j;
                }
            }
        }

        /// <summary>
        /// renvoie un string équivalant au radical de la salle (ex : OtherRoom5)
        /// et le roomScript associé à la base de la salle (ex : OtherRoom5_0_0)
        /// </summary>
        /// <param name="room"></param>
        /// <returns></returns>
        public KeyValuePair<string, RoomScript> GetRoomRoot(RoomScript room)
        {
            string keyRoom = _specialRooms.FirstOrDefault(x => x.Value == room).Key;
            if (keyRoom == null) return default(KeyValuePair<string, RoomScript>);
            keyRoom = keyRoom.Substring(0, keyRoom.IndexOf('_'));
            return new KeyValuePair<string, RoomScript>(keyRoom, _specialRooms[keyRoom + "_0_0"]);
        }

        /// <summary>
        /// Genere un vector2 aléatoire selon le prédicat
        /// </summary>
        /// <param name="predicate"></param>
        /// <returns></returns>
        private Vector2 RandomPossibleCoordinates(System.Predicate<Vector2[]> predicate, Vector2 size)
        {
            List<Vector2> possibleValues = new List<Vector2>();
            for (int i = 0; i < _columnCount; i++)
            {
                for (int j = 0; j < _rowCount; j++)
                {
                    Vector2 res = new Vector2(i, j);
                    if (predicate(new Vector2[] { res, size })) possibleValues.Add(res);
                }
            }

            if (possibleValues.Count == 0)
                throw new System.Exception("No possible coordinates found for " + predicate.Method);

            var res2 = Random.Range(0, possibleValues.Count);

            return possibleValues[res2];
        }

        /// <summary>
        /// Utilisé comme prédicat pour dire si le vector2 en paramètre se trouve à 2 cases d'une arete de la grille
        /// </summary>
        /// <param name="vector"></param>
        /// <returns></returns>
        private bool IsOnEdge(Vector2[] vectors)
        {
            var vector = vectors[0];
            var size = vectors[1];

            var res = vector.x <= _columnCount - size.x && vector.y <= _rowCount - size.y && vector.x != 0 && vector.y != 0 && (vector.x <= 2 || vector.y <= 2 || vector.x >= _columnCount - 2 - size.x && vector.x != _columnCount - size.x || vector.y >= _rowCount - 2 - size.y && vector.y != _rowCount - size.y);
            return res;
        }

        /// <summary>
        /// Utilisé comme prédicat pour dire si le vector2 en paramètre est sur l'arete opposé à celle du spawn
        /// </summary>
        /// <param name="vector"></param>
        /// <returns></returns>
        private bool OnOppositeSide(Vector2[] vectors)
        {
            var vector = vectors[0];
            var size = vectors[1];
            Vector2 spawnRoom = _specialRooms["SpawnRoom_0_0"].Coordinates;


            if (IsOnEdge(vectors) && Vector2.Distance(vector, spawnRoom) >= 10)
            {
                if (spawnRoom.x <= _columnCount / 2) return vector.x >= _columnCount / 2;
                if (spawnRoom.x >= _columnCount / 2) return vector.x <= _columnCount / 2;
                if (spawnRoom.y <= _rowCount / 2) return vector.y >= _rowCount / 2;
                if (spawnRoom.y >= _rowCount / 2) return vector.y <= _rowCount / 2;
            }

            return false;
        }

        /// <summary>
        /// Utilisé comme prédicat pour dire si le vector2 en paramètre est à au moins une salle d'une autre
        /// et on ne la place pas sur un salle déjà utilisée
        /// </summary>
        /// <param name="vector"></param>
        /// <returns></returns>
        private bool AwayFromOtherRooms(Vector2[] vectors)
        {
            var vector = vectors[0];
            var size = vectors[1];
            var res = true;

            foreach (var room in _specialRooms)
            {
                for (int j = 0; j < size.y; j++)
                {
                    for (int i = 0; i < size.x; i++)
                    {
                        if (Vector2.Distance(room.Value.Coordinates, new Vector2(i + vector.x, j + vector.y)) <= 2) res = false;
                    }
                }
            }

            return res && !_specialRooms.ContainsValue(_map.Get(vector)) && vector.x <= _columnCount - 2 - size.x && vector.y <= _rowCount - 2 - size.y;
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

        public void OpenCloseDoorFromRoot(RoomScript root, bool openClose)
        {
            // à revoir
            if (root.OpenedDoors.HasFlag(DoorType.Up)) _specialRooms[GetRoomRoot(root).Key + "_Up"].OpenCloseDoors(DoorType.Up, openClose);
            if (root.OpenedDoors.HasFlag(DoorType.Down)) _specialRooms[GetRoomRoot(root).Key + "_Down"].OpenCloseDoors(DoorType.Down, openClose); ;
            if (root.OpenedDoors.HasFlag(DoorType.Right)) _specialRooms[GetRoomRoot(root).Key + "_Right"].OpenCloseDoors(DoorType.Right, openClose); ;
            if (root.OpenedDoors.HasFlag(DoorType.Left)) _specialRooms[GetRoomRoot(root).Key + "_Left"].OpenCloseDoors(DoorType.Left, openClose); ;
        }

        /// <summary>
        /// ouvre les portes entre la salle from et la salle to
        /// </summary>
        /// <param name="from"></param>
        /// <param name="to"></param>
        private void ConnectTwoRooms(RoomScript from, RoomScript to)
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
                            _paths.Add(currentNode);

                            DoorType doorsToOpen = DoorType.None;
                            DoorType doorsToOpenParent = DoorType.None; // à optimiser (car ça ouvre la porte que d'un coté de la salle)

                            if (currentNode.Parent != null) // pour la dernière salle (qui n'a pas de parent)
                            {
                                // augmentation de la pondération des salles où on est déjà passé et les salles adjacentes
                                currentNode.Weight += 2;
                                foreach (var currentNeighbour in currentNode.Neighbours)
                                {
                                    if (!currentNode.Parent.Type.HasFlag(RoomType.Boss | RoomType.Spawn))
                                    {
                                        currentNeighbour.Value.Weight += 10;
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

                                var parentRoomRoot = GetRoomRoot(currentNode.Parent).Value;
                                var currentRoomRoot = GetRoomRoot(currentNode).Value;

                                currentNode.Parent.OpenCloseDoors(doorsToOpenParent, true, parentRoomRoot == null);
                                currentNode.OpenCloseDoors(doorsToOpen, true, currentRoomRoot == null);


                                if (parentRoomRoot != null) parentRoomRoot.OpenedDoors |= doorsToOpenParent;
                                if (currentRoomRoot != null) currentRoomRoot.OpenedDoors |= doorsToOpen;
                            }

                            currentNode = currentNode.Parent;
                        }
                        return;
                    }
                }
            }

        }
    }
}
