using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Elevator : MonoBehaviour
{
    public Transform spawnPoint;
    public GameObject player;
    public bool spawn;
    private Animator animator;
    public float time;
    void Start()
    {
        animator = GetComponent<Animator>();
        if(spawn == true)
        {
            animator.SetTrigger("Open");
        }
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player" && spawn == false)
        {
            animator.SetTrigger("Close");
            Invoke("DisplacePlayer", 2.5f);
        }
    }
    public void DisplacePlayer()
    {
        player.transform.position = spawnPoint.position;
    }
}
