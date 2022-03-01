using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using Object = UnityEngine.Object;

namespace GameLibrary
{
    public class EnemyManager
    {
        #region Singleton
        private static EnemyManager instance;
        public static EnemyManager Instance
        {
            get
            {
                instance ??= new EnemyManager();
                return instance;
            }
        }
        private EnemyManager() { }
        #endregion

        private readonly List<GameObject> _enemies = new List<GameObject>();
        private GameObject _enemiesContainer;

        public void Awake()
        {
            EventManager.Instance.AddListener<OnEnemySpawnRequested>(EnemySpawnRequested);
            EventManager.Instance.AddListener<OnEnemyDied>(EnemyDied);
        }

        public void Start()
        {
            _enemiesContainer = GameObject.FindGameObjectWithTag(Tags.EnemiesContainer);
        }

        public void Update()
        {
        }

        private void SpawnEnemy(Vector2 position, EnemyType type)
        {
            var player = GameObject.FindGameObjectWithTag(Tags.Player);
            var enemy = Object.Instantiate(Resources.Load<GameObject>("Prefabs/Enemy"), position, Quaternion.identity, _enemiesContainer.transform);

            var template = Resources.LoadAll<Enemy>("Scriptables/Enemies").Where(x => x.name == type.ToString()).First();
            var rendererScript = enemy.GetComponent<EnemyGeneratorScript>();
            rendererScript.Enemy = template.Clone();
            rendererScript.Enemy.pattern = GetPattern(type, enemy, player);

            rendererScript.LoadEnemy();
            _enemies.Add(enemy);

            EventManager.Instance.Dispatch(new OnEnemySpawned(enemy));
        }

        private IPattern GetPattern(EnemyType type, GameObject enemy, GameObject player)
        {
            switch (type)
            {
                case EnemyType.BasicEnemy1: return new GenericPattern(enemy, player);
                case EnemyType.BasicEnemy2: return new GenericPattern2(enemy, player);
                case EnemyType.BasicEnemy3: return new SpiningPattern(enemy, player);
                case EnemyType.RandomTowards: return new RandomTowardsPlayerPattern(enemy, player);
                case EnemyType.Teleport: return new TeleportPattern(enemy, player);
                case EnemyType.Necromancer: return new NecromancerPattern(enemy, player);
                default: return new GenericPattern(enemy, player);
            }
        }


        private void EnemySpawnRequested(OnEnemySpawnRequested e)
        {
            if (e.Summon)
            {
                // lors du spawn d'un ennemi (non-boss) il y a d'abord une phase d'invocation, cette invocation peut être stoppée si on regarde l'ennemi avec la lampe torche
                var summoningEnemy = Object.Instantiate(Resources.Load("Prefabs/SummoningEnemy"), e.Position, Quaternion.identity, _enemiesContainer.transform) as GameObject;
                _enemies.Add(summoningEnemy);
                CoroutineInterface.Instance.StartCoroutine(FlashLightOnEnemy(summoningEnemy));
                CoroutineInterface.Instance.StartCoroutine(SummonEnemySpawn(e.Position, e.Type, summoningEnemy));
            }
            else SpawnEnemy(e.Position, e.Type);
        }

        private IEnumerator FlashLightOnEnemy(GameObject summoningEnemy)
        {
            var health = 20;
            while (true)
            {
                if (PlayerVision2DScript.IsGameObjectInsideFlashLight(summoningEnemy))
                {
                    if (health-- < 0)
                    {
                        EventManager.Instance.Dispatch(new OnEnemyDied(summoningEnemy, Resources.Load<Enemy>("Scriptables/Enemies/SummoningEnemy")));
                        yield break;
                    }
                }
                yield return new WaitForSeconds(0.1f);
            }
        }

        private IEnumerator SummonEnemySpawn(Vector2 position, EnemyType type, GameObject summoningEnemy)
        {
            var t = 0f;
            while (true)
            {
                t += Time.deltaTime;
                if (summoningEnemy == null) yield break;
                // spawn de l'ennemi si il s'est passé 3 secondes
                if (t > 3)
                {
                    SpawnEnemy(position, type);
                    Object.Destroy(summoningEnemy);
                    _enemies.Remove(summoningEnemy);
                    yield break;
                }
                yield return new WaitForEndOfFrame();
            }
        }

        private void EnemyDied(OnEnemyDied e)
        {
            ParticleManager.Instance.InstanciateParticle("BloodExplosion2D", e.GameObject.transform.position, Quaternion.identity, 2f);

            AudioSource.PlayClipAtPoint(e.Enemy.DeathSound.GetRandom(), Camera.main.transform.position);

            _enemies.Remove(e.GameObject);
            Object.Destroy(e.GameObject);

            if (_enemies.Count == 0) EventManager.Instance.Dispatch(new OnAllEnemiesKilled());
        }
    }
}
