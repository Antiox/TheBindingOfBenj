using GameLibrary;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBehaviorScript : MonoBehaviour
{
    private int _health;
    private CircleCollider2D _collider;

    private void Start()
    {
        _collider = GetComponent<CircleCollider2D>();
        var generatorScript = GetComponent<EnemyGeneratorScript>();
        _health = generatorScript.Enemy.Health;

        EventManager.Instance.AddListener<OnEnemyHurt>(EnemyHurt);

        StartCoroutine(generatorScript.Enemy.pattern.Execute());
    }

    private void Update()
    {
        
    }


    private void EnemyHurt(OnEnemyHurt e)
    {
        if (e.Enemy == gameObject && --_health == 0)
        {
            EventManager.Instance.Dispatch(new OnEnemyDied(gameObject));
        }
    }
}
