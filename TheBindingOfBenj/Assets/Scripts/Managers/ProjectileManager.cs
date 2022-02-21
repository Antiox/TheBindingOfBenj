using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using Object = UnityEngine.Object;

namespace GameLibrary
{
    public class ProjectileManager
    {
        #region Singleton
        private static ProjectileManager instance;
        public static ProjectileManager Instance
        {
            get
            {
                instance ??= new ProjectileManager();
                return instance;
            }
        }
        private ProjectileManager() { }
        #endregion

        public void Awake()
        {
            EventManager.Instance.AddListener<OnProjectileSpawnRequested>(ProjectileSpawnRequested);
        }

        public void Start()
        {
        }

        public void Update()
        {
        }

        private void SpawnProjectiles(Vector3 position, Vector3 target, Weapon weapon)
        {
            switch (weapon.RangedType)
            {
                case RangedType.Basic:
                    SpawnProjectile(position, target, weapon);
                    break;
                case RangedType.Spread:
                    var angleBetweenProjectiles = weapon.Spread / weapon.ProjectileCount;
                    for (int i = 0; i < weapon.ProjectileCount; i++)
                    {
                        var rotatedTarget = Utility.RotatePointAroundPivot(Utility.RotatePointAroundPivot(target, position, new Vector3(0, 0, i * angleBetweenProjectiles)), position, new Vector3(0, 0, -(weapon.Spread - angleBetweenProjectiles) / 2));
                        SpawnProjectile(position, rotatedTarget, weapon);
                    }
                    break;
            }
        }

        /// <summary>
        /// instancie un projectile 
        /// </summary>
        /// <param name="position"></param>
        /// <param name="target"></param>
        /// <param name="weapon">permet de savoir la projectile à instancier</param>
        private void SpawnProjectile(Vector3 position, Vector3 target, Weapon weapon)
        {
            position += Vector3.back;
            target += Vector3.back;

            var direction = target - position;
            var angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg - 90f;

            var projectile = Object.Instantiate(Resources.Load<GameObject>("Prefabs/GenericProjectile"), position + direction.normalized, Quaternion.AngleAxis(angle, Vector3.forward), GameObject.Find("Projectiles").transform);

            var projectileTemplate = Resources.Load<Weapon>($"Scriptables/{weapon.name}");

            var rendererScript = projectile.GetComponent<ProjectileGeneratorScript>();

            rendererScript.Weapon = projectileTemplate.Clone();
            rendererScript.Weapon.Pattern = GetPattern(projectileTemplate.ProjectileType, projectile, target, projectileTemplate.Speed, projectileTemplate.HomingRadius);

            rendererScript.LoadProjectile();
        }

        private IPattern GetPattern(ProjectileType type, GameObject source, Vector3 target, float speed, float homingRadius)
        {
            switch (type)
            {
                case ProjectileType.BasicProjectile: return new GenericProjectilePattern(source, target, speed);
                case ProjectileType.HomingProjectile: return new HomingProjectilePattern(source, target, speed, homingRadius);
                case ProjectileType.BouncingProjectile: return new BouncingProjectilePattern(source, target, speed);
                default: return new GenericProjectilePattern(source, target, speed);
            }
        }

        private void ProjectileSpawnRequested(OnProjectileSpawnRequested e)
        {
            SpawnProjectiles(e.Position, e.Target, e.Weapon);
        }
    }
}
