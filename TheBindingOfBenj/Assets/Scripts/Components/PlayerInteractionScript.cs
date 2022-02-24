using GameLibrary;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInteractionScript : MonoBehaviour
{
    private PlayerCombatScript _playerCombat;

    void Start()
    {
        _playerCombat = GetComponent<PlayerCombatScript>();
    }


    void Update()
    {
        var collider = Physics2D.OverlapCircle(transform.position, 3, LayerMask.GetMask("Weapon"));
        if (collider != null && collider.GetComponent<WeaponGeneratorScript>() != null)
        {
            if (Inputs.WeaponChange)
            {
                _playerCombat.ChangeWeapon(collider.GetComponent<WeaponGeneratorScript>().Weapon);
                Destroy(collider.gameObject);
                Inputs.WeaponChange = false; // moche mais pas d'autre solution pour l'instant
            }
        }
    }
}
