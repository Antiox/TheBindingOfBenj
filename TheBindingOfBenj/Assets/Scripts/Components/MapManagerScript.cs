using GameLibrary;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapManagerScript : MonoBehaviour
{
    private void Start()
    {
        EventManager.Instance.AddListener<OnPlayerSpawnRequested>(SpawnPlayer);
        EventManager.Instance.AddListener<OnPlayerRoomChanged>(MoveCamera);
    }

    private void SpawnPlayer(OnPlayerSpawnRequested e)
    {
        Instantiate(Resources.Load("Prefabs/Player"), e.Position, Quaternion.identity);
    }

    private void MoveCamera(OnPlayerRoomChanged e)
    {
        StartCoroutine(MoveCameraRoutine(e));
    }

    private IEnumerator MoveCameraRoutine(OnPlayerRoomChanged e)
    {
        var camera = Camera.main;
        var startPos = Camera.main.transform.position;
        var targetPos = new Vector3(e.Position.x, e.Position.y, startPos.z);
        var t = 0f;
        while (camera.transform.position != targetPos)
        {
            camera.transform.position = new Vector3(
                Mathf.SmoothStep(startPos.x, e.Position.x, t * 3),
                Mathf.SmoothStep(startPos.y, e.Position.y, t * 3),
                camera.transform.position.z);

            t += Time.deltaTime;
            yield return new WaitForEndOfFrame();
        }
    }
}
