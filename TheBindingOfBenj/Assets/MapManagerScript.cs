using GameLibrary;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapManagerScript : MonoBehaviour
{
    private void Start()
    {
        EventManager.Instance.AddListener<OnPlayerSpawnRequested>(SpawnPlayer);
    }

    private void SpawnPlayer(OnPlayerSpawnRequested e)
    {
        Instantiate(Resources.Load("Prefabs/Player"), e.Position, Quaternion.identity);
    }
}
