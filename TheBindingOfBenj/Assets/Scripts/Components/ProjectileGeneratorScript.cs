using GameLibrary;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileGeneratorScript : MonoBehaviour
{
    public Weapon Weapon { get; set; }

    public LayerMask EnemyLayer { get; set; }

    private SpriteRenderer _spriteRenderer;

    public void LoadProjectile()
    {
        _spriteRenderer = GetComponent<SpriteRenderer>();
        _spriteRenderer.sprite = Weapon.ProjectileSprite;
        _spriteRenderer.color = Color.white;
    }
}
