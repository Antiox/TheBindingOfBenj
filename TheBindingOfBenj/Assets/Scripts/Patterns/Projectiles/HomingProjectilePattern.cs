using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace GameLibrary
{
    public class HomingProjectilePattern : ProjectilePattern
    {
        private float _radius;
        public HomingProjectilePattern(GameObject source, Vector3 target, float speed, float radius) : base(source, target, speed)
        {
            _radius = radius;
        }

        public override IEnumerator Execute()
        {
            var direction = (_target - _source.transform.position).normalized;
            while (true)
            {
                var targetEnemies = Physics2D.OverlapCircleAll(new Vector2(_source.transform.position.x, _source.transform.position.y), _radius, LayerMask.GetMask("Enemy"));

                if (targetEnemies.Length > 0)
                {
                    direction = (targetEnemies.OrderBy(enemy => (_source.transform.position - enemy.transform.position).magnitude).First().transform.position - _source.transform.position).normalized;
                    var angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg - 90f;
                    _source.transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
                }

                _source.GetComponent<Rigidbody2D>().MovePosition(Vector3.MoveTowards(_source.transform.position, _source.transform.position + direction, Time.fixedDeltaTime * _speed));
                yield return new WaitForFixedUpdate();
            }
        }
    }
}
