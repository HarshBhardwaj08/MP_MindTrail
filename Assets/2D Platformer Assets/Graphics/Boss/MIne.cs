using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MIne : MonoBehaviour
{
    [SerializeField] private Explosion explosion;
    public static event Action<int> onExplosion;
    private void OnEnable()
    {
      
    }

    private void Explode()
    {
        explosion.Explode();
    }

    private void OnDisable()
    {
       
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            // Get the SpriteRenderer component of the player
            Explode(); 
            onExplosion?.Invoke(2);
        }
    }
}
