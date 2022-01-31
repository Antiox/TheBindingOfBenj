using GameLibrary;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapManagerScript : MonoBehaviour
{
    private RoomScript _currentRoom;

    private void Start()
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
        var worldPosRoom = new Vector2(e.Room.Coordinates.x * e.Room.Size.x, e.Room.Coordinates.y * e.Room.Size.y);

        // mouvement caméra
        StartCoroutine(MoveCameraRoutine(worldPosRoom));
        _currentRoom = e.Room;


        if (!e.Room.SpawnedEnemies && e.Room.Type != RoomType.Boss && e.Room.Type != RoomType.Spawn)
        {
            // spawn des nouveaux ennemis
            EventManager.Instance.Dispatch(new OnEnemySpawnRequested(worldPosRoom, EnemyType.BasicEnemy1));
            EventManager.Instance.Dispatch(new OnEnemySpawnRequested(worldPosRoom, EnemyType.BasicEnemy2));
            EventManager.Instance.Dispatch(new OnEnemySpawnRequested(worldPosRoom, EnemyType.BasicEnemy3));
            e.Room.SpawnedEnemies = true;

            // blocage des portes
            e.Room.OpenCloseDoors(e.Room.OpenedDoors, false);
        }
    }

    private IEnumerator MoveCameraRoutine(Vector2 targetRoom)
    {
        var camera = Camera.main;
        var startPos = camera.transform.position;
        var targetPos = new Vector3(targetRoom.x, targetRoom.y, startPos.z);
        var t = 0f;
        while (camera.transform.position != targetPos)
        {
            camera.transform.position = new Vector3(
                Mathf.SmoothStep(startPos.x, targetPos.x, t * 3),
                Mathf.SmoothStep(startPos.y, targetPos.y, t * 3),
                camera.transform.position.z);

            t += Time.deltaTime;
            yield return new WaitForEndOfFrame();
        }
    }

    public void OpenDoors(OnAllEnemiesKilled e)
    {
        _currentRoom.OpenCloseDoors(_currentRoom.OpenedDoors, true);
    }
}
