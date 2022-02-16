using GameLibrary;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapManagerScript : MonoBehaviour
{
    private RoomScript _currentRoom;

    private void Awake()
    {
        EventManager.Instance.AddListener<OnPlayerSpawnRequested>(SpawnPlayer);
        EventManager.Instance.AddListener<OnPlayerRoomChanged>(ChangeRoom);
        EventManager.Instance.AddListener<OnAllEnemiesKilled>(OpenDoors);
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
            if (!_currentRoom.SpawnedEnemies && _currentRoom.Type != RoomType.Boss && _currentRoom.Type != RoomType.Spawn)
            {
                // spawn des nouveaux ennemis
                foreach (var pos in _currentRoom.MonstersPositions)
                {
                    EventManager.Instance.Dispatch(new OnEnemySpawnRequested(pos, Utility.RandomEnum<EnemyType>()));
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
