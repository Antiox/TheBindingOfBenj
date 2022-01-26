using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;


namespace GameLibrary
{
    public class GameManager
    {
        #region Singleton
        private static GameManager instance;
        public static GameManager Instance
        {
            get
            {
                instance ??= new GameManager();
                return instance;
            }
        }
        private GameManager() { }
        #endregion

        public void Start()
        {
			MapManager.Instance.Start();
            EnemyManager.Instance.Start();

            EventManager.Instance.Dispatch(new OnEnemySpawnRequested(new Vector2(-10, 0), EnemyType.BasicEnemy1));
            EventManager.Instance.Dispatch(new OnEnemySpawnRequested(new Vector2(10, 0), EnemyType.BasicEnemy2));
            EventManager.Instance.Dispatch(new OnEnemySpawnRequested(new Vector2(0, 10), EnemyType.BasicEnemy3));
        }

        public void Update()
        {
            EnemyManager.Instance.Update();
            MapManager.Instance.Update();
        }
    }
}
