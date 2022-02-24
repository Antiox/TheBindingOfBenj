using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace GameLibrary
{
    public class GenericPattern : Pattern
    {
        public GenericPattern(GameObject enemy, GameObject player) : base(enemy, player) { }

        public override IEnumerator Execute()
        {
            yield return new WaitForSeconds(1f);
            while (true)
            {

                var t = 0f;
                while (t < 1f)
                {
                    _enemy.transform.position = Vector3.MoveTowards(_enemy.transform.position, _player.transform.position, Time.deltaTime * 7f);
                    t += Time.deltaTime;
                    yield return new WaitForEndOfFrame();
                }

                yield return new WaitForSeconds(1f);
            }
        }
    }
}
