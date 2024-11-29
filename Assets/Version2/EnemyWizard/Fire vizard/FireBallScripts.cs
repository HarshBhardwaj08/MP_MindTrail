using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireBallScripts : MonoBehaviour
{
    public static event Action<int> onfireBallHit;
    private void OnTriggerEnter2D(Collider2D collision)
    {
        // Add logic here for hitting the player or other objects
        Debug.Log("Fireball hit: " + collision.name);
        if (collision.gameObject.tag == "Player") ;
        {
            onfireBallHit?.Invoke(1);
            Destroy(gameObject); // Destroy the fireball on collision
        }
    }
}
