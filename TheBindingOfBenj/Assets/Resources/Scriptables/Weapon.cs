using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace GameLibrary
{
    [CreateAssetMenu(fileName = "New weapon", menuName = "Scriptables/Weapon")]
    public class Weapon : ScriptableObject
    {
        /// <summary>
        /// type de l'arme (melee, ranged)
        /// </summary>
        public WeaponType Type;

        /// <summary>
        /// dégats qu'inflige l'arme
        /// </summary>
        public float Damage;

        /// <summary>
        /// temps entre chaque attaque
        /// </summary>
        public float Cooldown;

        /// <summary>
        /// portée de l'arme
        /// </summary>
        public float Range;


        [Header("Only for ranged weapons")]

        /// <summary>
        /// prefab du projectile à lancer
        /// </summary>
        public Projectile Projectile;
    }
}
