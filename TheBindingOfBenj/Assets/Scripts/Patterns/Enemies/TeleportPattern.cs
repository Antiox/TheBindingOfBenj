using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace GameLibrary
{
    public class TeleportPattern : Pattern
    {
        public TeleportPattern(GameObject enemy, GameObject player) : base(enemy, player) { }

        public override IEnumerator Execute()
        {
            while (true)
            {
                var rand = Random.insideUnitCircle * 10;
                var targetPosition = _player.transform.position + new Vector3(rand.x, rand.y, 0);
                
                _enemy.transform.position = targetPosition;

                yield return new WaitForSeconds(2f);
            }
        }
    }
}
