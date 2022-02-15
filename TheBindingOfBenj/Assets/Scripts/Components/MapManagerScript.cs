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
        var worldPosRoom = new Vector2(e.Room.Coordinates.x * e.Room.CellSize.x, e.Room.Coordinates.y * e.Room.CellSize.y);

        // mouvement caméra
        _currentRoom = e.Room;


        if (!_currentRoom.SpawnedEnemies && _currentRoom.Type != RoomType.Boss && _currentRoom.Type != RoomType.Spawn)
        {
            // spawn des nouveaux ennemis
            

            // blocage des portes
            if (_currentRoom.SpawnedEnemies)
                _currentRoom.OpenCloseDoors(e.Room.OpenedDoors, false);
        }
    }

    public void OpenDoors(OnAllEnemiesKilled e)
    {
        _currentRoom.OpenCloseDoors(_currentRoom.OpenedDoors, true);
    }
}
