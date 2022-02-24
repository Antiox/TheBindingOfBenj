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
            yield return new WaitForSeconds(1f);
            while (true)
            {

                _enemy.transform.position = _player.transform.position + (_enemy.transform.position - _player.transform.position).normalized * 10f;
                _enemy.transform.RotateAround(_player.transform.position, Vector3.forward, 90f * Time.deltaTime);
                var q = _enemy.transform.rotation;
                q.eulerAngles = new Vector3(q.eulerAngles.x, q.eulerAngles.y, 0);
                _enemy.transform.rotation = q;

                yield return new WaitForEndOfFrame();
            }
        }
    }
}
