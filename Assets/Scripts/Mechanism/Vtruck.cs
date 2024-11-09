using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using static FlowChanger;

public class Vtruck : MonoBehaviour
{
    public GameObject player;
    public Animator animator;
    public GameObject solveQuizes;
    public BoxCollider2D boxCollider;
    public GameObject Fire;
    public bool AllQuestionSolved = false;
    private void Awake()
    {   
        Fire.SetActive(false);
        boxCollider.isTrigger = false;
        animator.enabled = false;
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player" && AllQuestionSolved == true)
        {
            player.SetActive(false);
            AudioManager.Instance.PlaySoundEffects(3);
            animator.enabled = true;
        }
    }
    public void FireEnable()
    {
        AudioManager.Instance.PlaySoundEffects(4);
        Fire.SetActive(true);
    }

    public void WinnningScene()
    {
        GameManager.timerStart = false;
        SceneManager.LoadScene(2);

    }
}
