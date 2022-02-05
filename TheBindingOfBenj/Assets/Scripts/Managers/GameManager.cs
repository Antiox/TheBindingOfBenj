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

            EventManager.Instance.AddListener<OnPlayerKilled>(PlayerKilled);
        }

        public void Start()
        {
            Inputs.Start();
			MapManager.Instance.Start();
            EnemyManager.Instance.Start();
<<<<<<< HEAD

            InitializeGameStatus();

            EventManager.Instance.Dispatch(new OnGameStarted(_status));
=======
            ProjectileManager.Instance.Start();
>>>>>>> 7f5c9c73fb2c120d512e99888d8d229917b21ea2
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
