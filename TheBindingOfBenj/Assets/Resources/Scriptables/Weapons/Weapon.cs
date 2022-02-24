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
        public WeaponType WeaponType;

        /// <summary>
        /// sprite de l'arme
        /// </summary>
        public Sprite WeaponSprite;

        /// <summary>
        /// d�gats qu'inflige l'arme
        /// </summary>
        public float Damage;

        /// <summary>
        /// temps entre chaque attaque
        /// </summary>
        public float Cooldown;

        /// <summary>
        /// port�e de l'arme
        /// </summary>
        public float Range;

        /// <summary>
        /// type d'arme � distance
        /// </summary>
        public RangedType RangedType;

        public float Spread;

        public int ProjectileCount;


        [Header("Projectile")]

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

        /// <summary>
        /// dur�e max de vie du projectile
        /// </summary>
        public float Duration;

        /// <summary>
        /// sprite du projectile
        /// </summary>
        public Sprite ProjectileSprite;

        /// <summary>
        /// pattern de mouvement du projectile
        /// </summary>
        public IPattern Pattern;

        /// <summary>
        /// type du projectile
        /// </summary>
        public ProjectileType ProjectileType;

        public float HomingRadius;

        public List<AudioClip> EnemyImpact;
        public List<AudioClip> WallImpact;
    }
}
