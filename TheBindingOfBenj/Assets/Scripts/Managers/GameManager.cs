using GameLibrary;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GameLibrary
{
    public class GameManager : MonoBehaviour {
        #region Singleton
        private static GameManager instance;
        public static GameManager Instance
        {
            get
            {
                return instance ??= new GameManager();
            }
        }
        private GameManager() { }
        #endregion


        public void Start()
        {
            MapManager.Instance.Start();
        }

        public void Update()
        {
            MapManager.Instance.Update();
        }
    }
}
