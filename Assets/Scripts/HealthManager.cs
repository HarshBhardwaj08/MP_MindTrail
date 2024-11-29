using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class HealthManager : MonoBehaviour
{

    public GameObject[] health; // Array of health GameObjects
    public Sprite fullheart, halfhealth, emptyHealth; // Sprites for health states
    public int healthCount = 6; // Total health
    [SerializeField] private AudioManager audioManager; // Audio manager for sound effects
    [SerializeField] private ApiCaller apiCaller; // API caller for saving data
    public static event Action OnHealthZero;
    private void OnEnable()
    {
        QuestionIntilizers.healthDelegate += DecreaseHealth;
        HeroMovementScript.healthDelegate += DecreaseHealth;
        HeaalthBooster.OnHealthPicked += IncreseHealth;
    }

    private void OnDisable()
    {
        QuestionIntilizers.healthDelegate -= DecreaseHealth;
        HeroMovementScript.healthDelegate -= DecreaseHealth;
        HeaalthBooster.OnHealthPicked -= IncreseHealth;
    }

    public void DecreaseHealth(int val)
    {
        if (healthCount <= 0)
        {
            OnHealthZero?.Invoke();
            return;
        }

        healthCount = healthCount - val;

        UpdateHealthDisplay(healthCount);

        if (healthCount <= 0)
        {
            // Game over logic
            apiCaller.SaveDataToApi();
            GameManager.Instance.gameOverPanel.SetActive(true);
            GameManager.Instance.musicSound.Stop();
        }

        audioManager.PlaySoundEffects(0);
    }
    public void IncreseHealth(int val)
    {
        if(healthCount >= 6)
        {
            return;
        }
        healthCount += val;
        UpdateHealthDisplay(healthCount);
    }

    private void UpdateHealthDisplay(int healthcount)
    {
        for (int i = 0; i < health.Length; i++)
        {
            if (i < healthCount / 2) // Fully active hearts
            {
                health[i].GetComponent<Image>().sprite = fullheart;
                health[i].SetActive(true);
            }
            else if (i == healthCount / 2 && healthCount % 2 != 0) // Half heart
            {
                health[i].GetComponent<Image>().sprite = halfhealth;
                health[i].SetActive(true);
            }
            else // Empty hearts
            {
                health[i].GetComponent<Image>().sprite = emptyHealth;
                health[i].SetActive(true);
            }
        }
    }
}
