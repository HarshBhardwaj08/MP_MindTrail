using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class InvincibilityCells : MonoBehaviour
{
    public GameObject cellIcon;

    public static event Action<bool> cellPicked;
    public float invincibleTime = 10.0f;
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            // Get the SpriteRenderer component of the player
            cellIcon.SetActive(true);
            cellPicked?.Invoke(true);
            StartCoroutine(ApplyInvincibility(collision.gameObject,invincibleTime));
            this.gameObject.GetComponent<SpriteRenderer>().enabled = false;
            this.gameObject.GetComponent<BoxCollider2D>().enabled = false;
        }
    }
    private IEnumerator ApplyInvincibility(GameObject player,float invincibletime)
    {
        // Get the SpriteRenderer of the player
        SpriteRenderer playerSprite = player.GetComponent<SpriteRenderer>();
        if (playerSprite != null)
        {
            // Set alpha to 160 (160/255)
            Color color = playerSprite.color;
            color.a = 160f / 255f;
            playerSprite.color = color;
            // Wait for 10 seconds
            yield return new WaitForSeconds(invincibletime);
            // Reset alpha to normal (1f for full opacity)
            color.a = 1f;
            cellIcon.SetActive(false);
            cellPicked?.Invoke(false);
            playerSprite.color = color;
            this.gameObject.GetComponent<SpriteRenderer>().enabled = true;
            this.gameObject.GetComponent<BoxCollider2D>().enabled = true;
        }
    }
}
