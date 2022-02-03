using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GameLibrary
{
    public class Entity : MonoBehaviour
    {
        [SerializeField] protected Weapon _equipedWeapon;
        [SerializeField] private LayerMask _enemyMask;

        private float _timeSinceLastAttack;

        protected virtual void Awake()
        {
            _timeSinceLastAttack = _equipedWeapon.Cooldown;
        }

        protected virtual void Update()
        {
            _timeSinceLastAttack += Time.deltaTime;
        }

        protected void Attack(Vector2 pos)
        {
            if (_timeSinceLastAttack >= _equipedWeapon.Cooldown)
            {
                if (_equipedWeapon.WeaponType == WeaponType.Melee)
                {
                    Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position, _equipedWeapon.Range, _enemyMask);

                    for (int i = 0; i < colliders.Length; i++)
                    {
                        Debug.Log(colliders[i]);
                        float angle = Vector2.SignedAngle(
                            new Vector2(colliders[i].transform.position.x, colliders[i].transform.position.y)
                                - new Vector2(transform.position.x, transform.position.y),
                            new Vector2(transform.up.x, transform.up.y));
                        // then checks within the sphere if collider is in front of the attacker (120°)
                        if (angle > -60 && angle < 60)
                        {
                            colliders[i].gameObject.GetComponent<Entity>().Hurt(_equipedWeapon.Damage);
                        }
                    }
                }
                else
                {
                    var posScreen = Camera.main.ScreenToWorldPoint(pos);
                    posScreen.z = transform.position.z;
                    EventManager.Instance.Dispatch(new OnProjectileSpawnRequested(_equipedWeapon, transform.position, posScreen));
                }
                _timeSinceLastAttack = 0;
            }
        }

        protected void Hurt(float damage)
        {
            Debug.Log(gameObject + " recois " + damage);
        }

    }
}
