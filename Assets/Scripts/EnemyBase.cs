using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBase : MonoBehaviour
{
    public GameObject healthPickup;
    private void Start()
    {
        healthPickup.SetActive(false);
    }
    public void EnableHealthPickup()
    {
        healthPickup.SetActive(true);
        healthPickup.transform.parent = null;
    }
   public  virtual void OnDamage()
    {

    }
}
