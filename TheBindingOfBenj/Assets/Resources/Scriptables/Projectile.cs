using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace GameLibrary
{
    [CreateAssetMenu(fileName = "New projectile", menuName = "Scriptables/Projectile")]
    public class Projectile : ScriptableObject
    {
        /// <summary>
        /// Vitesse du projectile
        /// </summary>
        public float Speed;

        /// <summary>
        /// rayon du projectile
        /// </summary>
        public float Radius;

        /// <summary>
        /// longueur du projectile
        /// </summary>
        public float Length;

        public float Duration;
    }
}
