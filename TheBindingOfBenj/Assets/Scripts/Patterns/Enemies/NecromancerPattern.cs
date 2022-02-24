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

                var rand = Random.insideUnitCircle * 10;
                var targetPosition = _player.transform.position + new Vector3(rand.x, rand.y, 0);

                if (state)
                {
                    state = false;
                    // mouvement : teleporte vers le joueur

                    _enemy.transform.position = targetPosition;
                
                    yield return new WaitForSeconds(1f);

                    _enemy.GetComponent<EnemyBehaviorScript>().Attack(_player.transform.position);

                    yield return new WaitForSeconds(0.2f);
                }
                else
                {
                    state = true;

                    var enemy = Utility.GetEnumValues<EnemyType>().Where(x => !MapManagerScript.AllBosses.Contains(x)).First();

                    // spawn de deux ennemis identiques
                    EventManager.Instance.Dispatch(new OnEnemySpawnRequested(targetPosition, enemy));
                    EventManager.Instance.Dispatch(new OnEnemySpawnRequested(targetPosition, enemy));

                    yield return new WaitForSeconds(2.5f);
                }
            }
        }
    }
}
