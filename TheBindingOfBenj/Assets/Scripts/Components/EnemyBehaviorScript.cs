using GameLibrary;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBehaviorScript : MonoBehaviour
{
    private float _health;
    private CircleCollider2D _collider;
    private SpriteRenderer _renderer;

    private void Start()
    {
        _renderer = GetComponent<SpriteRenderer>();
        _collider = GetComponent<CircleCollider2D>();
        var generatorScript = GetComponent<EnemyGeneratorScript>();
        _health = generatorScript.Enemy.Health;

        EventManager.Instance.AddListener<OnEnemyHurt>(EnemyHurt);

        StartCoroutine(generatorScript.Enemy.pattern.Execute());
    }

    private void Update()
    {
        
    }

    private void OnDestroy()
    {
        EventManager.Instance.RemoveListener<OnEnemyHurt>(EnemyHurt);
    }


    private void EnemyHurt(OnEnemyHurt e)
    {
        if (e.Enemy == gameObject)
        {
            _health -= e.Damage;
            StartCoroutine(PlayHurtAnimation());

            if (_health <= 0)
                EventManager.Instance.Dispatch(new OnEnemyDied(gameObject));
        }
    }

    private IEnumerator PlayHurtAnimation()
    {
        _renderer.color = Color.red;
        yield return new WaitForSeconds(0.06f);
        _renderer.color = Color.white;
    }
}
