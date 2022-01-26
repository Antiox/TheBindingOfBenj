using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace GameLibrary
{
    public abstract class Pattern : IPattern
    {
        protected readonly GameObject _enemy;
        protected readonly GameObject _player;

        protected Pattern(GameObject enemy, GameObject player)
        {
            _enemy = enemy;
            _player = player;
        }

        public virtual IEnumerator Execute()
        {
            yield return new WaitForEndOfFrame();
        }
    }
}
