using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// classe utilis�e pour appeller des coroutines � partir d'autres classes non-monoBehaviour
/// </summary>
public class CoroutineInterface : MonoBehaviour
{
    public static CoroutineInterface Instance = null;

    private void Start()
    {
        Instance = this;
    }
}
