using GameLibrary;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileBehaviourScript : MonoBehaviour
{
    public Weapon Weapon { get; set; }

    private void Start()
    {
    }

    public void Init()
    {
        transform.localScale = new Vector3(Weapon.Radius, Weapon.Length, 1);
        //Destroy(gameObject, Weapon.Duration);
        var generatorScript = GetComponent<ProjectileGeneratorScript>();
        StartCoroutine(generatorScript.Weapon.Pattern.Execute());
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        // A rendre plus propre
        if (other.tag == Tags.Enemy)
        {
            EventManager.Instance.Dispatch(new OnEnemyHurt(other.gameObject, Weapon.Damage));
            var angle = transform.rotation.eulerAngles.z + 180f;
            var particles = Instantiate(Resources.Load("Prefabs/Particles/BloodSplatDirectional2D"), transform.position, Quaternion.Euler(0, 0, angle)) as GameObject;
            AudioSource.PlayClipAtPoint(Weapon.EnemyImpact.GetRandom(), Camera.main.transform.position);
            Destroy(gameObject);
            Destroy(particles, 2f);
        }
        else if (other.tag != Tags.Player && other.tag != Tags.Projectile)
        {
            var particles = Instantiate(Resources.Load("Prefabs/Particles/MetalHit2D"), transform.position, Quaternion.identity) as GameObject;
            AudioSource.PlayClipAtPoint(Weapon.WallImpact.GetRandom(), Camera.main.transform.position);
            Destroy(gameObject);
            Destroy(particles, 2f);
        }
    }

}
