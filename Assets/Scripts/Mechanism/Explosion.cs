using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Explosion : MonoBehaviour
{
    public float explosionRadius = 5f;      
    public float explosionForce = 700f;     
    public float upwardsModifier = 0.5f;   
    public LayerMask affectedLayers;
    public Animator explosionAnimator;
    public void Explode()
    {
      
        Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position, explosionRadius, affectedLayers);

        explosionAnimator.SetTrigger("Explode");
        
        foreach (Collider2D nearbyObject in colliders)
        {
            Rigidbody2D rb = nearbyObject.GetComponent<Rigidbody2D>();
            if (rb != null)
            {
               
                Vector2 explosionDirection = (rb.transform.position - transform.position).normalized;
                rb.AddForce(explosionDirection * explosionForce + Vector2.up * upwardsModifier, ForceMode2D.Impulse);
            }
        }

        AudioManager.Instance.PlaySoundEffects(7);
    }
    public void DesTroyObj()
    {
        Destroy(gameObject);
        this.gameObject.transform.parent.gameObject.SetActive(false);
    }
  
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, explosionRadius);
    }
}
