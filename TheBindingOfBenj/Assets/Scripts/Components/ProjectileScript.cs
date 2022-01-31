using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GameLibrary
{
    public class ProjectileScript : MonoBehaviour
    {
        public Projectile Projectile { get; set; }

        private void Start()
        {
            transform.localScale = new Vector3(Projectile.Radius, Projectile.Length, 1);
            Destroy(gameObject, Projectile.Duration);
        }

        private void Update()
        {
            transform.position = Vector2.MoveTowards(transform.position, transform.position + transform.up, Projectile.Speed * Time.deltaTime);
        }

        private void OnTriggerEnter2D(Collider2D other)
        {
            // A rendre plus propre
            if(other.tag == Tags.Enemy)
            {
                EventManager.Instance.Dispatch(new OnEnemyHurt(other.gameObject, 1));
                var angle = transform.rotation.eulerAngles.z + 180f;
                var particles = Instantiate(Resources.Load("Prefabs/BloodSplatDirectional2D"), transform.position, Quaternion.Euler(0, 0, angle)) as GameObject;
                AudioSource.PlayClipAtPoint(Projectile.EnemyImpact.GetRandom(), Camera.main.transform.position);
                Destroy(gameObject);
                Destroy(particles, 2f);
            }
            else if (other.tag != Tags.Player && other.tag != Tags.Projectile)
            {
                var particles = Instantiate(Resources.Load("Prefabs/MetalHit2D"), transform.position, Quaternion.identity) as GameObject;
                AudioSource.PlayClipAtPoint(Projectile.WallImpact.GetRandom(), Camera.main.transform.position);
                Destroy(gameObject);
                Destroy(particles, 2f);
            }
        }
    }
}
