using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.SceneManagement;

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

        private GameStatus _status;


        public void Awake()
        {
            Inputs.Awake();
            MapManager.Instance.Awake();
            EnemyManager.Instance.Awake();
            ProjectileManager.Instance.Awake();

            EventManager.Instance.AddListener<OnPlayerKilled>(PlayerKilled);
        }

        public void Start()
        {
            Inputs.Start();
			MapManager.Instance.Start();
            EnemyManager.Instance.Start();
            ProjectileManager.Instance.Start();

            InitializeGameStatus();

            EventManager.Instance.Dispatch(new OnGameStarted(_status));
        }

        public void Update()
        {
            EnemyManager.Instance.Update();
            MapManager.Instance.Update();
        }


        private void InitializeGameStatus()
        {
            _status = new GameStatus();
            _status.MaxHealth = 10;
            _status.CurrentHealth = 10;
        }


        private void PlayerKilled(OnPlayerKilled e)
        {
            SceneManager.LoadScene(0);
        }
    }
}
