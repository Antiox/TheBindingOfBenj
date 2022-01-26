using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace GameLibrary
{
    public class SpiningPattern : Pattern
    {
        public SpiningPattern(GameObject enemy, GameObject player) : base(enemy, player) { }

        public override IEnumerator Execute()
        {
            while (true)
            {
                var playerPosition = 3f * Vector3.Normalize(_player.transform.position - _enemy.transform.position) + _enemy.transform.position;
                _enemy.transform.RotateAround(playerPosition, Vector3.forward, 180 * Time.deltaTime);
                var q = _enemy.transform.rotation;
                q.eulerAngles = new Vector3(q.eulerAngles.x, q.eulerAngles.y, 0);
                _enemy.transform.rotation = q;

                yield return new WaitForEndOfFrame();
            }
        }
    }
}
