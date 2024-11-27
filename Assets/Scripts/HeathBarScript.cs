using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HeathBarScript : MonoBehaviour
{
    [Header("Health Settings")]
    public float maxHealth = 100f; // Maximum health
    public float currentHealth;    // Current health

    [Header("UI Reference")]
    public Slider healthSlider;    // Reference to the Slider UI

    void Start()
    {
        // Initialize health
        currentHealth = maxHealth;

        // Set up the slider
        healthSlider.maxValue = maxHealth;
        healthSlider.value = currentHealth;
    }

    // Call this method to take damage
    public void TakeDamage(float damageAmount)
    {
        currentHealth -= damageAmount;

        // Clamp health to avoid negative values
        currentHealth = Mathf.Clamp(currentHealth, 0, maxHealth);

        // Update the slider
        healthSlider.value = currentHealth;

        // Optional: Handle death
        if (currentHealth <= 0)
        {
            Debug.Log("Player is Dead!");
            // Add additional logic for death
        }
    }

    // Call this method to heal
    public void Heal(float healAmount)
    {
        currentHealth += healAmount;

        // Clamp health to avoid exceeding maxHealth
        currentHealth = Mathf.Clamp(currentHealth, 0, maxHealth);

        // Update the slider
        healthSlider.value = currentHealth;
    }
}
