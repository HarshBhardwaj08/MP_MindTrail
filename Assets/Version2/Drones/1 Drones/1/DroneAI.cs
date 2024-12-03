using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DroneAI : EnemyBase
{
    public Transform player;
    public float speed = 3f;
    public float detectionRadius = 5f;
    public float destroyRadius = 1f;
    [SerializeField] private Explosion explosion;
    public static event Action<int> onDroneHit;
    private bool isPlayerHit;
    private void Start()
    {
        healthPickup.SetActive(false);
    }
    private void Explode()
    {
        explosion.Explode();
    }

   
    void Update()
    {
        float distanceToPlayer = Vector2.Distance(transform.position, player.position);

        if (distanceToPlayer <= detectionRadius)
        {
            // Move toward the player
            Vector2 direction = (player.position - transform.position).normalized;
            transform.position += (Vector3)direction * speed * Time.deltaTime;

            // Check if close enough to destroy
            if (distanceToPlayer <= destroyRadius && isPlayerHit == false)
            {
                onDroneHit?.Invoke(2);
                Explode();
                isPlayerHit = true;
                EnableHealthPickup();
                //DestroySelf();
            }
        }
    }
    
    void DestroySelf()
    {
        // Add explosion effect or damage logic here
        Debug.Log("Drone destroyed near player!");
        Destroy(gameObject);
    }
    public override void OnDamage()
    {
        Debug.Log(this.gameObject.name + "Damage Deducted");
        EnableHealthPickup();
        Explode();
    }
    void OnDrawGizmos()
    {
        // Draw detection radius
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, detectionRadius);

        // Draw destruction radius
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, destroyRadius);
    }
}
