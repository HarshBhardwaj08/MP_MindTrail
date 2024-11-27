using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    public static event Action onBulletHit;
    public  float speed = 10;
    private void Update()
    {
        transform.Translate(Vector2.right*Time.deltaTime*speed);
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        // Add logic here for hitting the player or other objects
        Debug.Log("Bullet hit: " + collision.tag);
        if (collision.gameObject.tag == "Enemy")
        {
            onBulletHit?.Invoke();
            collision.gameObject.GetComponent<EnemyBase>().OnDamage();
         //   collision.gameObject.SetActive(false); 
        }
          Destroy(gameObject); // Destroy the fireball on collision
    }
}
