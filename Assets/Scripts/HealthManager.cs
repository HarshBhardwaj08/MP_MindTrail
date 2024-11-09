using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class HealthManager : MonoBehaviour
{  
    
    public GameObject[] health;
    public int healthCount = 3;
    [SerializeField] GameObject heartDecrease;
    [SerializeField] AudioManager audioManager;
    [SerializeField] ApiCaller apiCaller;
    private void OnEnable()
    {
        QuestionIntilizers.healthDelegate += DecreaseHealth;
    }
    private void OnDisable()
    {
        QuestionIntilizers.healthDelegate -= DecreaseHealth;
    }
    public  void DecreaseHealth()
    {  
        healthCount--;
        if (healthCount <= 0)
        {
            apiCaller.SaveDataToApi();
            GameManager.Instance.gameOverPanel.SetActive(true);
            GameManager.Instance.musicSound.Stop();

        }
        health[healthCount].SetActive(false);
        StartCoroutine(HeartDecrease());
    }

    public IEnumerator HeartDecrease()
    {
        audioManager.PlaySoundEffects(0);
        heartDecrease.SetActive(true);
        yield return new WaitForSeconds(1f);
        heartDecrease.SetActive(false);


    }
}
