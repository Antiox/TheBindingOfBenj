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
            Inputs.Start();
			MapManager.Instance.Start();
            EnemyManager.Instance.Start();
        }

        public void Update()
        {
            EnemyManager.Instance.Update();
            MapManager.Instance.Update();
        }
    }
}
