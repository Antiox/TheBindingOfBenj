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
        var startPos = Camera.main.transform.position;

        var t = 0f;
        while (true)
        {
            Camera.main.transform.position = new Vector3(
                Mathf.SmoothStep(startPos.x, e.Position.x, t * 3),
                Mathf.SmoothStep(startPos.y, e.Position.y, t * 3),
                Camera.main.transform.position.z);

            t += Time.deltaTime;
            yield return new WaitForEndOfFrame();
        }
    }
}
