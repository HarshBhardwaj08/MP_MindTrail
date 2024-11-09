using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class WinManager : MonoBehaviour
{
    [SerializeField] Text timerText;
    [SerializeField] ApiCaller apiCaller;
    void Start()
    {
        apiCaller.SaveDataToApi();
        timerText.text = string.Format("Time : {0:00}:{1:00}:{2:00}", GameManager.hours, GameManager.minutes, GameManager.seconds); // Format the text

    }

   public void RestartButton()
    {
        SceneManager.LoadScene(1);
    }
}
