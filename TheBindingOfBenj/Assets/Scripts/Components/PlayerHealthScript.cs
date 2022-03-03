using GameLibrary;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHealthScript : MonoBehaviour
{
    [SerializeField] private float _currentHealth = 10f;
    [SerializeField] private float _maxHealth = 10f;
    [SerializeField, Range(0f, 5f)] private float _immunityFrames = 1f;
    private bool _isImmune = false;

    private SpriteRenderer _spriteRenderer;

    void Start()
    {
        _spriteRenderer = transform.Find("playerSprite").GetComponent<SpriteRenderer>();
    }

    private void OnCollisionStay2D(Collision2D collision)
    {
        if (!_isImmune && (collision.gameObject.CompareTag(Tags.Enemy))) Hurt();
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        var projectileBehaviour = collision.gameObject.GetComponent<ProjectileBehaviourScript>();
        if (projectileBehaviour != null)
        {
            if (!_isImmune)
            {
                if (projectileBehaviour.EnemyLayer == (projectileBehaviour.EnemyLayer | (1 << gameObject.layer)))
                {
                    Hurt();
                    Destroy(collision.gameObject);
                }
            }
            else Physics2D.IgnoreCollision(collision.collider, GetComponent<Collider2D>());
        }
    }

    private void Hurt()
    {
        _currentHealth--;

        if (_currentHealth <= 0)
            EventManager.Instance.Dispatch(new OnPlayerKilled());
        else
        {
            EventManager.Instance.Dispatch(new OnPlayerHurted(_maxHealth, _currentHealth));
            StartCoroutine(WaitForImmunityFrames());
        }
    }

    private IEnumerator WaitForImmunityFrames()
    {
        var originalColor = _spriteRenderer.color;
        _spriteRenderer.color = Color.red;

        _isImmune = true;
        yield return new WaitForSeconds(_immunityFrames);
        _isImmune = false;

        _spriteRenderer.color = originalColor;
    }
}
