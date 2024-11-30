using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeaalthBooster : MonoBehaviour
{
    public static event Action<int> OnHealthPicked;
    
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            OnHealthPicked?.Invoke(1);
            Destroy(this.gameObject);
        }
    }
}
