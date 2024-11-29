using System;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System.Collections;
using UnityEngine.Networking;

public class QuestionIntilizers : MonoBehaviour
{
    [SerializeField] private int questionNumber;  // Use this to access the specific question
    public TextMeshProUGUI QuestionNo;
    public TextMeshProUGUI QuestionText;
    public List<GameObject> options;
    [HideInInspector]
    public List<TextMeshProUGUI> optionText;
   [SerializeField] private string correctAnswer;
    public Reward reward;
    public delegate void RewardDelegate(Reward reward);
    public static event RewardDelegate RewardIntiliazer;
    public delegate void HealthDelegate(int val);
    public static event HealthDelegate healthDelegate;
    public static event Action DisableObject;
    string[] questionData;
    bool isCorrect;
    [SerializeField] Image questionImage;
    [SerializeField] string imagepath;
    public bool completed; 
    private void Awake()
    {
        // Initialize the optionText list
        optionText = new List<TextMeshProUGUI>();
        for (int i = 0; i < options.Count; i++)
        {
            optionText.Add(options[i].GetComponentInChildren<TextMeshProUGUI>());
        }
    }

  public  void Start()
    {
        // Access the API data directly
        if (ApiCaller.apiData != null && ApiCaller.apiData.Length > questionNumber)
        {
            // Retrieve question data from the apiData array
            questionData = ApiCaller.apiData[questionNumber];

            // Extract the values from the apiData
            //   QuestionNo.text = Convert.ToString(questionData[0]).Replace('"', ' ');// Question ID
            QuestionNo.text = (questionNumber + 1).ToString();// Question ID
            QuestionText.text = Convert.ToString(questionData[1]).Replace('"', ' '); ; // Question text
            correctAnswer = Convert.ToString(questionData[6]).Replace('"', ' '); // Correct answer ID

            // Set options and correct answer
            for (int j = 0; j < 4; j++) // Assuming there are always 4 options
            {
                optionText[j].text = Convert.ToString(questionData[j + 2]).Replace('"', ' ');  // Options start from index 2
            }
           // Debug.Log("sd " + ApiCaller.apiData.Length);
            imagepath = questionData[7];
            imagepath = imagepath.Trim('"');
            if (string.IsNullOrEmpty(imagepath))
            {
                questionImage.gameObject.SetActive(false);
            }
            else
            {
         
               
                questionImage.gameObject.SetActive(true);

                StartCoroutine(LoadImageCoroutine(imagepath));
            }
            
        }
        else
        {
            Debug.LogError("API data is not available or question number is out of range.");
        }

    }
    private IEnumerator LoadImageCoroutine(string filePath)
    {
        // Load the image using UnityWebRequest
        using (UnityWebRequest request = UnityWebRequestTexture.GetTexture(filePath))
        {
            yield return request.SendWebRequest();

            if (request.result == UnityWebRequest.Result.Success)
            {
                // Get the texture from the downloaded data
                Texture2D texture = DownloadHandlerTexture.GetContent(request);

                // Apply the texture to the UI Image component
                questionImage.sprite = Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height), Vector2.zero);
                questionImage.gameObject.SetActive(true);
            }
            else
            {
                Debug.LogError("Failed to load image: " + request.error);
            }
        }
    }
    public void DisableGameObject()
    {
        DisableObject?.Invoke();
        gameObject.SetActive(false);
    }

    public async void CorrectAns(Button clickedButton)
    {
       
        // Get the selected answer text from the clicked button
       string  selectedAnswer = clickedButton.GetComponentInChildren<TextMeshProUGUI>().text;
        // Compare the selected answer with the correct answer
        if (selectedAnswer == correctAnswer)
        {
            isCorrect = true;


            this.gameObject.SetActive(false);
            RewardIntiliazer?.Invoke(reward);
        }
        else
        {
        //    Debug.Log("Incorrect Answer!");
            isCorrect =false;
            healthDelegate?.Invoke(1);
        }


        GameManager.totalQuestionAttempted++;
        int timeTaken = Convert.ToInt32(GameManager.timeElapsed);
        AnswerData answerData = new AnswerData
        {
            question_id = Convert.ToInt32(ApiCaller.apiData[questionNumber][0]),
            answer = selectedAnswer,
            is_correct = isCorrect,
            time_taken = timeTaken.ToString(),
            submission_date_time = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")

        };

        // Convert the answer data to JSON and save it to PlayerPrefs or a file, or send it to a server, etc.
        string jsonData = JsonUtility.ToJson(answerData);
        //PlayerPrefs.SetString("AnswerData", jsonData);
       await DataManager.SaveAnswerDataAsync(answerData);
    }
}




public enum Reward
{
    Platfomer,
    Bridge,
    GateUnlock,
    ClearPasssage,
    TelePort,
    Elevator
}
