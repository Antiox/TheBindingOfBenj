using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GameLibrary
{
    public class ParticleManager
    {
        #region Singleton
        private static ParticleManager instance;
        public static ParticleManager Instance
        {
            get
            {
                instance ??= new ParticleManager();
                return instance;
            }
        }
        private ParticleManager() { }
        #endregion

        private GameObject _particlesContainer;

        public void Awake()
        {
            _particlesContainer = GameObject.Find("Particles");
        }

        public void InstanciateParticle(string name, Vector3 position, Quaternion rotation, float destroyTime, GameObject parent = null)
        {
            if (parent == null) parent = _particlesContainer;

            var particle = Object.Instantiate(Resources.Load("Prefabs/Particles/" + name), position, rotation, parent.transform);
            Object.Destroy(particle, destroyTime);
        }
    }
}
