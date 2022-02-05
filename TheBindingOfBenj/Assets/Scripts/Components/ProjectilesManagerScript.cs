using GameLibrary;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectilesManagerScript : MonoBehaviour
{
    void Awake()
    {
        EventManager.Instance.AddListener<OnProjectileSpawnRequested>(ProjectileSpawnRequested);
    }

    // Update is called once per frame
    void Update()
    {
        
    }


    void ProjectileSpawnRequested(OnProjectileSpawnRequested e)
    {
        var direction = e.Target - e.Position;
        var angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg - 90f;
        var projectile = Instantiate(Resources.Load("Prefabs/GenericProjectile"), e.Position + direction.normalized, Quaternion.AngleAxis(angle, Vector3.forward), GameObject.Find("Projectiles").transform) as GameObject;
        projectile.GetComponent<ProjectileScript>().Projectile = e.Projectile;
    }
}
