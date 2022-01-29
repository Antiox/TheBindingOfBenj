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
            StartCoroutine(AutoDestroy());
        }

        private void Update()
        {
            transform.position = Vector2.MoveTowards(transform.position, transform.position + transform.up, Projectile.Speed * Time.deltaTime);
        }


        private IEnumerator AutoDestroy()
        {
            yield return new WaitForSeconds(Projectile.Duration);
            Destroy(gameObject);
        }


        private void OnTriggerEnter2D(Collider2D other)
        {
            if(other.tag == Tags.Enemy)
                EventManager.Instance.Dispatch(new OnEnemyHurt(other.gameObject, 1));

            if(other.tag != Tags.Player)
                Destroy(gameObject);
        }
    }
}
