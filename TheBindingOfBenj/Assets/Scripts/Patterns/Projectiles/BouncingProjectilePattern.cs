using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace GameLibrary
{
    public class BouncingProjectilePattern : ProjectilePattern
    {
        public BouncingProjectilePattern(GameObject source, Vector3 target, float speed) : base(source, target, speed) { }

        public override IEnumerator Execute()
        {
            var direction = (_target - _source.transform.position).normalized;
            while (true)
            {
                var hit = Physics2D.CircleCast(_source.transform.position, 0.5f, _source.transform.up, 0.25f, ~(LayerMask.GetMask("Player") | LayerMask.GetMask("DeactivatedWall") | LayerMask.GetMask("Enemy") | LayerMask.GetMask("Weapon") | LayerMask.GetMask("SummoningEnemy")));

                // si collision on fait rebondir selon la normale
                if (hit.collider != null) direction = Vector3.Reflect(direction, hit.normal.normalized);
                
                _source.GetComponent<Rigidbody2D>().MovePosition(Vector3.MoveTowards(_source.transform.position, _source.transform.position + direction, Time.deltaTime * _speed));

                // rotation vers la direction
                _source.transform.up = direction;

                yield return new WaitForFixedUpdate();
            }
        }
    }
}
