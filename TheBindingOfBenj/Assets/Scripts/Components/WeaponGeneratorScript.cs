using GameLibrary;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponGeneratorScript : MonoBehaviour
{
    [HideInInspector] public Weapon Weapon;

    void Start()
    {
        var renderer = GetComponent<SpriteRenderer>();
        renderer.sprite = Weapon.WeaponSprite;
    }
}
