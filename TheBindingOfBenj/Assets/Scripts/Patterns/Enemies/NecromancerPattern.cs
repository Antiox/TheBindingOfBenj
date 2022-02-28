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

        private bool state = true;

        public override IEnumerator Execute()
        {
            yield return new WaitForSeconds(1f);
            while (true)
            {
                if (state)
                {
                    state = false;
                    // mouvement : teleporte vers le joueur

                    _enemy.transform.position = Utility.RandomPointInAnnulus(_player.transform.position, 3f, 6f);
                
                    yield return new WaitForSeconds(1f);

                    _enemy.GetComponent<EnemyBehaviorScript>().Attack(_player.transform.position);

                    yield return new WaitForSeconds(0.2f);
                }
                else
                {
                    state = true;

                    var enemy = Utility.GetEnumValues<EnemyType>().Where(x => !MapManagerScript.AllBosses.Contains(x)).First();

                    // spawn de trois ennemis identiques
                    for (int i = 0; i < 3; i++)
                    {
                        EventManager.Instance.Dispatch(
                            new OnEnemySpawnRequested(Utility.RandomPointInAnnulus(_player.transform.position, 5f, 8f), enemy, true));
                    }

                    yield return new WaitForSeconds(2f);
                }
            }
        }
    }
}
