using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tank : MonoBehaviour
{
   
    [Header("Shooting Settings")]
    public GameObject projectilePrefab; // The projectile to shoot
    public Transform firePoint; // The point from which the projectile is fired
    public float fireRate = 1f; // Time between shots
    public float projectileSpeed = 10f;

    private float fireCooldown;

    void Update()
    {
        ShootAtTarget();
    }

    void ShootAtTarget()
    {
        // Fire projectiles at intervals
        if (fireCooldown <= 0f)
        {
            FireProjectile();
            fireCooldown = 1f / fireRate;
        }

        fireCooldown -= Time.deltaTime;
    }

    void FireProjectile()
    {
        if (projectilePrefab != null && firePoint != null)
        {
            GameObject projectile = Instantiate(projectilePrefab, firePoint.position, firePoint.rotation);
            
            Rigidbody2D rb = projectile.GetComponent<Rigidbody2D>();
            if (rb != null)
            {
                rb.velocity = firePoint.transform.right * -projectileSpeed;
            }
            Destroy(projectile,3.0f);
        }
    }
}
