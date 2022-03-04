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
            yield return new WaitForSeconds(1f);
            while (true)
            {

                var teleportPoint = Utility.RandomPointInAnnulus(_player.transform.position, 3f, 6f);

                ParticleManager.Instance.InstanciateParticle("TeleportGlow", teleportPoint, Quaternion.identity, 2f);

                // temps du tp
                yield return new WaitForSeconds(2f);

                _enemy.transform.position = teleportPoint;

                yield return new WaitForSeconds(1f);

                _enemy.GetComponent<EnemyBehaviorScript>().Attack(_player.transform.position);

                yield return new WaitForSeconds(0.2f);
            }
        }
    }
}
