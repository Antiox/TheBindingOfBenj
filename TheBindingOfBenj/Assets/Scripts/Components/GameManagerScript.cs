using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GameLibrary;

public class GameManagerScript : MonoBehaviour
{
    void Awake() => GameManager.Instance.Awake();
    void Start() => GameManager.Instance.Start();
    void Update() => GameManager.Instance.Update();
}
