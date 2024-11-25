using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TankBullets : MonoBehaviour
{
    private void Start()
    {
        Destroy(gameObject,2.5f);
    }
}
