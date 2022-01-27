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
        }

        private void Update()
        {
            transform.position = Vector2.MoveTowards(transform.position, transform.position + transform.up, Projectile.Speed * Time.deltaTime);
        }
    }
}
