using GameLibrary;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyGeneratorScript : MonoBehaviour
{
    [HideInInspector] public Enemy Enemy;
    private SpriteRenderer _spriteRenderer;


    public void LoadEnemy()
    {
        _spriteRenderer = GetComponent<SpriteRenderer>();
        _spriteRenderer.sprite = Enemy.artwork;
        _spriteRenderer.color = Color.white;
    }
}
