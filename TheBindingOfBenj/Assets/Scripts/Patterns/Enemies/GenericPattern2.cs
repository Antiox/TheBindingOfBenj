using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace GameLibrary
{
    public class GenericPattern2 : Pattern
    {
        public GenericPattern2(GameObject enemy, GameObject player) : base(enemy, player) { }


        public override IEnumerator Execute()
        {
            yield return new WaitForSeconds(1f);
            while (true)
            {
                var targetPosition = _player.transform.position;
                var t = 0f;
                while (t < 1.5f)
                {
                    _enemy.transform.position = Vector3.MoveTowards(_enemy.transform.position, targetPosition, Time.deltaTime * 13f);
                    t += Time.deltaTime;
                    yield return new WaitForEndOfFrame();
                }

                yield return new WaitForSeconds(1.5f);
            }
        }
    }
}
