using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Restart : MonoBehaviour
{
    [SerializeField] private ApiCaller apiCaller;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            int timeTaken = Convert.ToInt32(GameManager.timeElapsed);
            if (GameManager.totalQuestionAttempted == 0)
            {
                AnswerData answerData = new AnswerData
                {
                    question_id = 0,
                    answer = "",
                    is_correct = false,
                    time_taken = timeTaken.ToString(),
                    submission_date_time = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")


                };
                // Convert the answer data to JSON and save it to PlayerPrefs or a file, or send it to a server, etc.
                string jsonData = JsonUtility.ToJson(answerData);

                 DataManager.SaveAnswerDataAsync(answerData);

                apiCaller.SaveDataToApi();
            }
            else
            {
                apiCaller.SaveDataToApi();

            }
            GameManager.Instance.gameOverPanel.SetActive(true);
            GameManager.Instance.musicSound.Stop();

        }
    }
}
