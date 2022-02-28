using GameLibrary;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class MapManagerScript : MonoBehaviour
{
    private RoomScript _currentRoom;
    public static List<EnemyType> AllBosses { get; private set; }

    private void Awake()
    {
        EventManager.Instance.AddListener<OnPlayerSpawnRequested>(SpawnPlayer);
        EventManager.Instance.AddListener<OnPlayerRoomChanged>(ChangeRoom);
        EventManager.Instance.AddListener<OnAllEnemiesKilled>(OpenDoors);
        AllBosses = new List<EnemyType>();
        AllBosses.Add(EnemyType.Necromancer);
    }

    private void SpawnPlayer(OnPlayerSpawnRequested e)
    {
        Instantiate(Resources.Load("Prefabs/Player"), e.Position, Quaternion.identity);
    }

    private void ChangeRoom(OnPlayerRoomChanged e)
    {
        _currentRoom = MapManager.Instance.GetRoomRoot(e.Room).Value;
        if (_currentRoom != null)
        {
            if (!_currentRoom.SpawnedEnemies && _currentRoom.Type != RoomType.Spawn)
            {
                // spawn des nouveaux ennemis
                foreach (var pos in _currentRoom.MonstersPositions)
                {
                    var allTypes = System.Enum.GetValues(typeof(EnemyType)).Cast<EnemyType>();
                    IEnumerable<EnemyType> types;
                    bool summon = false;

                    if (_currentRoom.Type == RoomType.Boss) types = allTypes.Where(x => AllBosses.Contains(x));
                    else
                    {
                        types = allTypes.Where(x => !AllBosses.Contains(x));
                        summon = true;
                    }

                    EventManager.Instance.Dispatch(new OnEnemySpawnRequested(pos, types.ElementAt(Random.Range(0, types.Count())), summon));

                    _currentRoom.SpawnedEnemies = true;
                }

                // blocage des portes
                if (_currentRoom.SpawnedEnemies)
                {
                    MapManager.Instance.OpenCloseDoorFromRoot(_currentRoom, false);
                }
            }
        }
    }

    public void OpenDoors(OnAllEnemiesKilled e)
    {
        MapManager.Instance.OpenCloseDoorFromRoot(_currentRoom, true);
    }
}
