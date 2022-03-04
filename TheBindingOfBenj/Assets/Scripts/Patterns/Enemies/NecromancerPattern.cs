using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace GameLibrary
{
    public class NecromancerPattern : Pattern
    {
        public NecromancerPattern(GameObject enemy, GameObject player) : base(enemy, player) { }

        // permet d'alterner entre deux modes (ratio : 2 mvts et atq / 1 invoc) : mouvements et attaque / invocation
        private int state = 0;

        public override IEnumerator Execute()
        {
            // temps avant de commencer le pattern
            yield return new WaitForSeconds(1f);
            while (true)
            {
                if (state < 3)
                {
                    state++;

                    var teleportPoint = Utility.RandomPointInAnnulus(_player.transform.position, 3f, 6f);

                    ParticleManager.Instance.InstanciateParticle("TeleportGlow", teleportPoint, Quaternion.identity, 1f);

                    // temps du tp
                    yield return new WaitForSeconds(1f);

                    // mouvement : teleporte vers le joueur
                    _enemy.transform.position = teleportPoint;

                    // temps entre le tp et l'attaque
                    yield return new WaitForSeconds(0.2f);

                    _enemy.GetComponent<EnemyBehaviorScript>().Attack(_player.transform.position);

                    yield return new WaitForSeconds(0.2f);
                }
                else
                {
                    yield return new WaitForSeconds(1f);

                    state = 0;
                    var enemy = Utility.GetEnumValues<EnemyType>().Where(x => !MapManagerScript.AllBosses.Contains(x)).First();

                    // spawn des ennemis selon un triangle équilatéral
                    var triangle = Utility.EquilateralTriangleFromCenter(_player.transform.position, 15, -22.5f);

                    // spawn de trois ennemis
                    for (int i = 0; i < triangle.Length; i++)
                    {
                        EventManager.Instance.Dispatch(new OnEnemySpawnRequested(triangle[i], enemy, true));
                    }

                    // temps d'invocation
                    yield return new WaitForSeconds(2f);
                }
            }
        }
    }
}
