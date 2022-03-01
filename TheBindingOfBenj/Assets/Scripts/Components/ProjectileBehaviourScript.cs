using GameLibrary;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileBehaviourScript : MonoBehaviour
{
    private Weapon _weapon;

    public LayerMask EnemyLayer { get; private set; }

    private void Start()
    {
        var generatorScript = GetComponent<ProjectileGeneratorScript>();
        _weapon = generatorScript.Weapon;
        gameObject.layer = generatorScript.EnemyLayer == LayerMask.GetMask("Enemy") ? LayerMask.NameToLayer("Weapon") : LayerMask.NameToLayer("EnemyWeapon");
        EnemyLayer = generatorScript.EnemyLayer;
        transform.localScale = new Vector3(_weapon.Radius, _weapon.Length, 1);
        Destroy(gameObject, _weapon.Duration);

        StartCoroutine(_weapon.Pattern.Execute());
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        // A rendre plus propre
        if (collision.gameObject.tag == Tags.Enemy)
        {
            var enemy = collision.gameObject.GetComponent<EnemyGeneratorScript>().Enemy;
            EventManager.Instance.Dispatch(new OnEnemyHurt(collision.gameObject, enemy, _weapon.Damage));
            
            var angle = transform.rotation.eulerAngles.z + 180f;

            ParticleManager.Instance.InstanciateParticle("BloodSplatDirectional2D", transform.position, Quaternion.Euler(0, 0, angle), 2f);

            AudioSource.PlayClipAtPoint(_weapon.EnemyImpact.GetRandom(), Camera.main.transform.position);
            Destroy(gameObject);
        }
        else if (collision.gameObject.tag != Tags.Player && collision.gameObject.tag != Tags.Projectile && collision.gameObject.tag != Tags.DeactivatedDoor && collision.gameObject.tag != Tags.Hole)
        {
            ParticleManager.Instance.InstanciateParticle("MetalHit2D", transform.position, Quaternion.identity, 2f);

            AudioSource.PlayClipAtPoint(_weapon.WallImpact.GetRandom(), Camera.main.transform.position);
            if (_weapon.ProjectileType != ProjectileType.BouncingProjectile) Destroy(gameObject);
        }
    }

}
