using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace GameLibrary
{
    public class GenericProjectilePattern : ProjectilePattern
    {
        public GenericProjectilePattern(GameObject source, Vector3 target, float speed) : base(source, target, speed) { }

        public override IEnumerator Execute()
        {
            var direction = (_target - _source.transform.position).normalized;
            while (true)
            {
                _source.GetComponent<Rigidbody2D>().MovePosition(Vector3.MoveTowards(_source.transform.position, _source.transform.position + direction, Time.fixedDeltaTime * _speed));
                yield return new WaitForFixedUpdate();
            }
        }
    }
}
