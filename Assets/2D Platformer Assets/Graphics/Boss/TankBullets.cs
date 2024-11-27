using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
public class TankBullets : MonoBehaviour
{
    public static event Action onBulletHit;
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            onBulletHit?.Invoke();
        }
        Destroy(this.gameObject);
    }
}
