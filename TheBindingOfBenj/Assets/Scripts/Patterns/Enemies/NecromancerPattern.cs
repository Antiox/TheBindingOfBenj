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
                if (state < 2)
                {
                    state++;

                    // mouvement : teleporte vers le joueur
                    _enemy.transform.position = Utility.RandomPointInAnnulus(_player.transform.position, 3f, 6f);
                
                    // temps entre le tp et l'attaque
                    yield return new WaitForSeconds(1f);

                    _enemy.GetComponent<EnemyBehaviorScript>().Attack(_player.transform.position);
                    Debug.Log("attaque");

                    yield return new WaitForSeconds(0.2f);
                }
                else
                {
                    state = 0;
                    var enemy = Utility.GetEnumValues<EnemyType>().Where(x => !MapManagerScript.AllBosses.Contains(x)).First();

                    // spawn de trois ennemis identiques
                    for (int i = 0; i < 3; i++)
                    {
                        EventManager.Instance.Dispatch(
                            new OnEnemySpawnRequested(Utility.RandomPointInAnnulus(_player.transform.position, 5f, 8f), enemy, true));
                    }

                    // temps d'invocation
                    yield return new WaitForSeconds(2f);
                }
            }
        }
    }
}
