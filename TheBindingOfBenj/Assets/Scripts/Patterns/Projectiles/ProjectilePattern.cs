using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace GameLibrary
{
    public abstract class ProjectilePattern : IPattern
    {
        protected readonly GameObject _source;
        protected readonly Vector3 _target;
        protected readonly float _speed;

        protected ProjectilePattern(GameObject source, Vector3 target, float speed)
        {
            _source = source;
            _target = target;
            _speed = speed;
        }

        public virtual IEnumerator Execute()
        {
            yield return new WaitForEndOfFrame();
        }
    }
}
