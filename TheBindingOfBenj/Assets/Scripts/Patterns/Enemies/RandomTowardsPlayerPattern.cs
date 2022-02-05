using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace GameLibrary
{
    public class RandomTowardsPlayerPattern : Pattern
    {
        public RandomTowardsPlayerPattern(GameObject enemy, GameObject player) : base(enemy, player) { }

        public override IEnumerator Execute()
        {
            while (true)
            {
                var t = 0f;
                var rand = Random.insideUnitCircle * 10;
                var targetPosition = _player.transform.position + new Vector3(rand.x, rand.y, 0);
                while (t < 1f)
                {
                    _enemy.transform.position = Vector3.MoveTowards(_enemy.transform.position, targetPosition, Time.deltaTime * 14f);
                    t += Time.deltaTime;
                    yield return new WaitForEndOfFrame();
                }

                yield return new WaitForSeconds(0.3f);
            }
        }
    }
}
