using GameLibrary;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBehaviorScript : Entity
{
    private float _health;
    private float _maxHealth;
    private SpriteRenderer _renderer;

    protected override void Awake()
    {
        base.Awake();
        EventManager.Instance.AddListener<OnEnemyHurt>(EnemyHurt);
    }

    private void Start()
    {
        _renderer = GetComponent<SpriteRenderer>();
        var generatorScript = GetComponent<EnemyGeneratorScript>();
        _equipedWeapon = generatorScript.Enemy.Weapon;
        _health = generatorScript.Enemy.Health;
        _maxHealth = generatorScript.Enemy.Health;

        StartCoroutine(generatorScript.Enemy.pattern.Execute());
    }

    private void OnDestroy()
    {
        EventManager.Instance.RemoveListener<OnEnemyHurt>(EnemyHurt);
    }


    private void EnemyHurt(OnEnemyHurt e)
    {
        if (e.GameObject == gameObject)
        {
            _health -= e.Damage;
            StartCoroutine(PlayHurtAnimation());

            transform.Find("Canvas").Find("HealthBarShrink").GetComponent<EnemyHealthBarScript>().SetHealth(_health / _maxHealth);

            if (_health <= 0)
                EventManager.Instance.Dispatch(new OnEnemyDied(e.GameObject, e.Enemy));
        }
    }

    private IEnumerator PlayHurtAnimation()
    {
        _renderer.color = Color.red;
        yield return new WaitForSeconds(0.06f);
        _renderer.color = Color.white;
    }
}
