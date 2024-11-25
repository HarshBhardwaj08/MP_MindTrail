using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    public static event Action onBulletHit;
    private void OnTriggerEnter2D(Collider2D collision)
    {
        // Add logic here for hitting the player or other objects
        Debug.Log("Bullet hit: " + collision.name);
        if (collision.gameObject.tag == "Enemy") ;
        {
            onBulletHit?.Invoke();
        }
        Destroy(gameObject); // Destroy the fireball on collision
    }
}
