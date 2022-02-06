﻿using System;
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
            var template = Resources.Load<Enemy>($"Scriptables/{type}");
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
                default: return new GenericPattern(enemy, player);
            }
        }



        private void EnemySpawnRequested(OnEnemySpawnRequested e)
        {
            SpawnEnemy(e.Position, e.Type);
        }

        private void EnemyDied(OnEnemyDied e)
        {
            var particles = Object.Instantiate(Resources.Load("Prefabs/Particles/BloodExplosion2D"), e.GameObject.transform.position, Quaternion.identity) as GameObject;
            AudioSource.PlayClipAtPoint(e.Enemy.DeathSound.GetRandom(), Camera.main.transform.position);

            _enemies.Remove(e.GameObject);
            Object.Destroy(e.GameObject);
            Object.Destroy(particles, 2f);

            if (_enemies.Count == 0) EventManager.Instance.Dispatch(new OnAllEnemiesKilled());
        }
    }
}
