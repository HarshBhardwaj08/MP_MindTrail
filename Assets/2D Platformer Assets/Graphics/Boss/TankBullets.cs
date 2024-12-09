using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
public class TankBullets : MonoBehaviour
{
    public static event Action<int> onBulletHit;
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            onBulletHit?.Invoke(1);
        }
            Destroy(this.gameObject);
    }
}
